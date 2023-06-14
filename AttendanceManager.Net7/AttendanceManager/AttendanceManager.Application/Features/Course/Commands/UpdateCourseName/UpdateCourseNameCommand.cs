using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using MediatR;

namespace AttendanceManager.Application.Features.Course.Commands.UpdateCourseName
{
    /// <summary>
    /// Update only the course name and the UpdateOn field
    /// </summary>
    public sealed class UpdateCourseNameCommand:IRequest<bool>
    {
        public required int CourseId { get; init; }
        public required int SpecializationId { get; init; }
        public required string Name { get; init; }
        public string? UserEmail { get; set; }
    }
    public sealed class UpdateCourseNameCommandHandler : IRequestHandler<UpdateCourseNameCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCourseNameCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateCourseNameCommand request, CancellationToken cancellationToken)
        {
            var course = await _unitOfWork.CourseRepository.GetAsync(c => c.CourseID == request.CourseId)
                ?? throw new NotFoundException("Course", request.CourseId);
            var userSpecialization = await 
                _unitOfWork.UserSpecializationRepository.GetAsync(c => c.SpecializationID == request.SpecializationId && c.UserID.Equals(request.UserEmail))
                ?? throw new NotFoundException("UserSpecialization", request.CourseId);

            course.Name = request.Name;
            course.UpdatedOn = DateTime.Now;
            course.UserSpecializationID = userSpecialization!.UserSpecializationID;

            _unitOfWork.CourseRepository.Update(course);

            return await _unitOfWork.CommitAsync();
        }
    }
}
