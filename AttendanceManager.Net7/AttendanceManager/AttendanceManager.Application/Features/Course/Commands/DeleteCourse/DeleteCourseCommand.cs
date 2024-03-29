﻿using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Features.Report.Commands.DeleteReportById;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Course.Commands.DeleteCourse
{
    /// <summary>
    /// This is a soft or hard delete, depending on the collection navigation properties:
    /// if there are some data in any collection
    /// 
    /// </summary>
    public sealed class DeleteCourseCommand : IRequest<bool>
    {
        public required int Id { get; init; }
    }
    public sealed class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public DeleteCourseCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var currentCourse = await _unitOfWork.CourseRepository.GetAsync(c => c.CourseID == request.Id) ??
                throw new NotFoundException("Course", request.Id);

            //delete all those reports manualy because there are to many entities related to it
            var reportsLinked = currentCourse.Reports!;
            currentCourse.Reports = null;

            //get members
            foreach (var doc in reportsLinked)
            {
                await _mediator.Send(new DeleteReportByIdCommand() { ReportId = doc.ReportID });
            }

            //delete the courses
            _unitOfWork.CourseRepository.Delete(currentCourse);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            return true;
        }
    }
}
