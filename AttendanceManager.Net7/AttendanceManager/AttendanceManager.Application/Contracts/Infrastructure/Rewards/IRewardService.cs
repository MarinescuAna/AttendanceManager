using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Contracts.Infrastructure.Rewards
{
    public interface IRewardService
    {
        Task<bool> AssignBadge(BadgeType badgeType, AttendanceCollection collection, string userId, Role userRole, bool commitChnages = true);
    }
}
