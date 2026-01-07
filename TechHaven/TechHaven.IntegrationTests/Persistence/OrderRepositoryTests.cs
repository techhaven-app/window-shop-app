
#if null
using TechHaven.Application.Features.Order.Commands.CreateOrder;
using TechHaven.Domain.Entities;
using TechHaven.UnitTests.Common;
using TechHaven.Shared.DTOs.Orders;
using Moq;
using FluentAssertions;
using Bogus;

namespace TechHaven.UnitTests.Features.Order;

public class CreateOrderCommandHandlerTests : UnitTestBase
{
    private readonly CreateOrderCommandHandler _handler;
    private readonly Faker _faker = new();

    public CreateOrderCommandHandlerTests()
    {
        _handler = new CreateOrderCommandHandler(MockUow.Object, Mapper);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldUseDatabasePriceNotClientPrice()
    {
        // Arrange: 
        // 1. Tạo sản phẩm trong DB có giá 100
        var dbProduct = new Product
        {
            Id = 1,
            SellPrice = 100,
            StockQuantity = 10,
            ProductName = "iPhone 15"
        };

        // 2. Client gửi yêu cầu mua với giá 999 (hòng gian lận)
        var command = new CreateOrderCommand
        {
            UserId = 1,
            CustomerId = 1,
            Details = new List<OrderUpsertItemDto> {
                new() { ProductId = 1, Quantity = 2 /* Giá Client không có trong DTO hoặc bị bỏ qua */ }
            }
        };

        MockUow.Setup(x => x.Products.GetByIdAsync(1)).ReturnsAsync(dbProduct);
        MockUow.Setup(x => x.Customers.GetByIdAsync(1)).ReturnsAsync(new Customer { Id = 1, TotalPurchased = 0 });

        // Act
        var result = await _handler.Handle(command, CancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // KIỂM TRA QUAN TRỌNG: Giá trong DB là 100, mua 2 -> Subtotal phải là 200
        result.Value.SubtotalAmount.Should().Be(200);

        // Kiểm tra kho bị trừ
        dbProduct.StockQuantity.Should().Be(8);

        // Đảm bảo SaveChanges được gọi
        MockUow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InsufficientStock_ShouldReturnFailure()
    {
        // Arrange: Kho chỉ còn 1 nhưng mua 10
        var dbProduct = new Product { Id = 1, StockQuantity = 1 };
        var command = new CreateOrderCommand
        {
            Details = new List<OrderUpsertItemDto> { new() { ProductId = 1, Quantity = 10 } }
        };

        MockUow.Setup(x => x.Products.GetByIdAsync(1)).ReturnsAsync(dbProduct);

        // Act
        var result = await _handler.Handle(command, CancellationToken);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Contain("Stock"); // Hoặc ErrorType.Validation tùy bạn định nghĩa
    }
}
#endif
//