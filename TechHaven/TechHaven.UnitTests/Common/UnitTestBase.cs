
//Class cơ sở chứa Mock Setup
using Moq;
using AutoMapper;
using TechHaven.Domain.Interfaces;
using TechHaven.Application.Mappings; // Namespace chứa MappingProfile của bạn

//Thêm vào để lấp đầy khoảng trống ở hàm constructor MapperConfiguration
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TechHaven.UnitTests.Common;

public abstract class UnitTestBase
{
    protected readonly Mock<IUnitOfWork> MockUow;
    protected readonly IMapper Mapper;
    protected readonly CancellationToken CancellationToken;

    protected UnitTestBase()
    {
        // 1. Setup Mock Unit of Work
        MockUow = new Mock<IUnitOfWork>();

        // 2. Setup Real AutoMapper (nên dùng mapper thật thay vì mock)
        var configuration = new MapperConfiguration(cfg =>
        {
            // Thay vì AddProfile<MappingProfile>() (không tồn tại),
            // ta dùng AddMaps để quét toàn bộ Assembly chứa class AppSettingProfile.
            // Nó sẽ tự động load OrderProfile, UserProfile, v.v.
            cfg.AddMaps(typeof(AppSettingProfile).Assembly);
        }, NullLoggerFactory.Instance); //Tạo null Logger để tránh lỗi null trong constructor của MapperConfiguration 

        Mapper = configuration.CreateMapper();

        CancellationToken = CancellationToken.None;
    }

    // Helper: Mock một Repository cụ thể khi cần
    protected void SetupRepo<TRepo>(Func<IUnitOfWork, TRepo> repoSelector, TRepo mockRepo) where TRepo : class
    {
        MockUow.Setup(u => repoSelector(u)).Returns(mockRepo);
    }
}