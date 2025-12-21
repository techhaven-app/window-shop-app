using FluentAssertions;
using Moq;
using TechHaven.Application.Features.Order.Commands.UpdateOrder;
using TechHaven.Domain.Common;
using TechHaven.Domain.Entities;
using TechHaven.Domain.Enums;
using TechHaven.Shared.DTOs.Orders;
using TechHaven.UnitTests.Common;
using Xunit;

namespace TechHaven.UnitTests.Features.Order;

public class UpdateOrderCommandHandlerTests : UnitTestBase
{
    private readonly UpdateOrderCommandHandler _handler;

    public UpdateOrderCommandHandlerTests()
    {
        _handler = new UpdateOrderCommandHandler(MockUow.Object, Mapper);
    }

    [Fact] // Kịch bản: Hoàn hàng (Completed -> Returned)
    public async Task Handle_StatusChange_ToReturned_ShouldRefundCustomer_And_RestockProducts()
    {
        // --- ARRANGE ---
        var productId = 1;
        var customerId = 10;

        // 1. Giả lập đơn hàng cũ đã Completed
        var oldOrder = new Domain.Entities.Order
        {
            OrderId = 1,
            Status = Domain.Enums.OrderStatus.Completed,
            CustomerId = customerId,
            TotalAmount = 1000000, // 1 triệu
            OrderDetails = new List<OrderDetail>
            {
                new() { ProductId = productId, Quantity = 2 }
            }
        };

        var customer = new Customer { CustomerId = customerId, TotalPurchased = 2000000 };
        var product = new Product { ProductId = productId, StockQuantity = 10 };

        var command = new UpdateOrderCommand
        {
            OrderId = 1,
            Status = Domain.Enums.OrderStatus.Returned // Yêu cầu trả hàng
        };

        MockOrderRepo.Setup(x => x.GetWithDetailsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(oldOrder);
        MockCustomerRepo.Setup(x => x.GetByIdAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        MockProductRepo.Setup(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>())).ReturnsAsync(product);

        // --- ACT ---
        var result = await _handler.Handle(command, CancellationToken);

        // --- ASSERT ---
        result.IsSuccess.Should().BeTrue();
        oldOrder.Status.Should().Be(Domain.Enums.OrderStatus.Returned);

        // Kiểm tra hoàn tiền: 2tr - 1tr = 1tr
        customer.TotalPurchased.Should().Be(1000000);

        // Kiểm tra hoàn kho: 10 + 2 = 12
        product.StockQuantity.Should().Be(12);

        MockUow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact] // Kịch bản: Cập nhật số lượng (Bù trừ kho)
    public async Task Handle_UpdateQuantity_ShouldAdjustStockCorrectly()
    {
        // --- ARRANGE ---
        var productId = 1;
        var oldOrder = new Domain.Entities.Order
        {
            OrderId = 1,
            Status = Domain.Enums.OrderStatus.Pending,
            TotalAmount = 500000,
            OrderDetails = new List<OrderDetail>
            {
                new() { ProductId = productId, Quantity = 2, UnitPrice = 250000 }
            }
        };

        var product = new Product { ProductId = productId, StockQuantity = 10, SellPrice = 250000 };

        var command = new UpdateOrderCommand
        {
            OrderId = 1,
            Status = Domain.Enums.OrderStatus.Pending,
            Details = new List<OrderUpsertItemDto>
            {
                new() { ProductId = productId, Quantity = 5 } // Mua thêm 3 cái (tổng 5)
            }
        };

        MockOrderRepo.Setup(x => x.GetWithDetailsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(oldOrder);
        MockProductRepo.Setup(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>())).ReturnsAsync(product);

        // --- ACT ---
        var result = await _handler.Handle(command, CancellationToken);

        // --- ASSERT ---
        // Ban đầu mua 2, giờ mua 5 -> Phải trừ thêm 3 cái trong kho.
        // Kho: 10 - 3 = 7
        product.StockQuantity.Should().Be(7);
        oldOrder.OrderDetails.First().Quantity.Should().Be(5);
    }

    [Fact] // Kịch bản: Đổi khách hàng
    public async Task Handle_ChangeCustomer_ShouldAdjustBothCustomersBalances()
    {
        // --- ARRANGE ---
        var oldCustId = 1;
        var newCustId = 2;
        var productId = 99; // ID sản phẩm giả lập

        // 1. Tạo Product giả để Handler lấy giá
        var product = new Product { ProductId = productId, SellPrice = 100000, StockQuantity = 10 };

        // 2. Tạo Order cũ (đang có 1 sản phẩm trị giá 100k)
        var oldOrder = new Domain.Entities.Order
        {
            OrderId = 1,
            CustomerId = oldCustId,
            TotalAmount = 100000,
            Status = Domain.Enums.OrderStatus.Pending,
            OrderDetails = new List<OrderDetail> {
                new() { ProductId = productId, Quantity = 1, UnitPrice = 100000 }
            }
        };

        var oldCustomer = new Customer { CustomerId = oldCustId, TotalPurchased = 100000 };
        var newCustomer = new Customer { CustomerId = newCustId, TotalPurchased = 0 };

        // 3. Tạo Command Update (QUAN TRỌNG: Phải gửi kèm item để giữ nguyên giá trị đơn hàng)
        var command = new UpdateOrderCommand
        {
            OrderId = 1,
            CustomerId = newCustId,
            Status = Domain.Enums.OrderStatus.Pending,
            // Gửi kèm item cũ lên để Handler tính ra tiền (1 * 100k = 100k)
            Details = new List<OrderUpsertItemDto> {
                new() { ProductId = productId, Quantity = 1 }
            }
        };

        MockOrderRepo.Setup(x => x.GetWithDetailsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(oldOrder);
        MockCustomerRepo.Setup(x => x.GetByIdAsync(oldCustId, It.IsAny<CancellationToken>())).ReturnsAsync(oldCustomer);
        MockCustomerRepo.Setup(x => x.GetByIdAsync(newCustId, It.IsAny<CancellationToken>())).ReturnsAsync(newCustomer);

        // MOCK THÊM: Handler cần lấy Product để tính giá lại
        MockProductRepo.Setup(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>())).ReturnsAsync(product);

        // --- ACT ---
        await _handler.Handle(command, CancellationToken);

        // --- ASSERT ---
        // Khách cũ bị trừ tiền hóa đơn này: 100k - 100k = 0
        oldCustomer.TotalPurchased.Should().Be(0);

        // Khách mới bị cộng tiền hóa đơn này: 0 + 100k = 100k
        newCustomer.TotalPurchased.Should().Be(100000);
    }

    [Fact] // Kịch bản: Bảo mật - Không cho sửa đơn đã Cancel
    public async Task Handle_CancelledOrder_ShouldReturnFailure()
    {
        // --- ARRANGE ---
        var oldOrder = new Domain.Entities.Order { OrderId = 1, Status = Domain.Enums.OrderStatus.Cancelled };
        var command = new UpdateOrderCommand { OrderId = 1 };

        MockOrderRepo.Setup(x => x.GetWithDetailsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(oldOrder);

        // --- ACT ---
        var result = await _handler.Handle(command, CancellationToken);

        // --- ASSERT ---
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be(ErrorType.Validation); //sửa result.ErrorType thành result.ErrorCode
        result.ErrorMessage.Should().Contain("Only Pending or Processing orders can be updated");
    }
}