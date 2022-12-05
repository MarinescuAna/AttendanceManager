using AttendanceManager.Application.SharedDtos;

namespace AttendanceManager.Application.Features.User.Queries.GetAllUsers
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int EnrollmentYear { get; set; }
        public string Code { get; set; }
        public string Updated { get; set; }
        public string Created { get; set; }
        public bool AccountConfirmed { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }  
        public SpecializationDto[] UserSpecializations { get; set; }
    }
}