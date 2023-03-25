using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Dtos;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.DocumentMember.Commands.InsertCollaboratorByDocumentId
{
    public sealed class InsertCollaboratorByDocumentIdCommandHandler : BaseFeature, IRequestHandler<InsertCollaboratorByDocumentIdCommand, DocumentMembersDto>
    {
        public InsertCollaboratorByDocumentIdCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<DocumentMembersDto> Handle(InsertCollaboratorByDocumentIdCommand request, CancellationToken cancellationToken)
        {
            //check if the user exists and if the user is teacher
            var user = await unitOfWork.UserRepository.GetAsync(u => u.Email == request.Email) ?? throw new NotFoundException("Teacher", request.Email);

            if (user.Role != Domain.Enums.Role.Teacher)
            {
                throw new BadRequestException($"You can add only teachers as collaborators! This user ({request.Email}) is a student.");
            }

            //get the document teachers
            var documentTeachers = await unitOfWork.DocumentMemberRepository.GetDocumentMembersByDocumentIdAndRoleAsync(request.DocumentId, Domain.Enums.Role.Teacher);

            if (documentTeachers.Any(dt => dt.UserID == request.Email))
            {
                throw new AlreadyExistsException("The teacher is already a member of this document");
            }

            // add the new teacher as collaborator
            unitOfWork.DocumentMemberRepository.AddAsync(new Domain.Entities.DocumentMember { 
                DocumentID = request.DocumentId,
                Role = Domain.Enums.DocumentRole.Collaborator,
                UserID = request.Email
            });

            if(await unitOfWork.CommitAsync())
            {
                return new()
                {
                    Email = request.Email,
                    Name = user.FullName,
                    Role = Domain.Enums.DocumentRole.Collaborator.ToString()
                };
            }

            return null;
        }
    }
}
