using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Commands.DeleteDocumentById
{
    public sealed class DeleteDocumentByIdCommandHandler : BaseFeature, IRequestHandler<DeleteDocumentByIdCommand, bool>
    {
        public DeleteDocumentByIdCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(DeleteDocumentByIdCommand request, CancellationToken cancellationToken)
        {
            // check if the document exists
            var document = await unitOfWork.DocumentRepository.GetAsync(d => d.DocumentId == request.DocumentId && !d.IsDeleted)
                ?? throw new NotFoundException("The document was not found, so it cannot be deleted.");

            // if the document does not any attendances logged, it can be deleted from the database, otherwise not
            if (unitOfWork.DocumentRepository.CanBeHardDeleted(request.DocumentId))
            {
                // make hard delete
                // remove all the members from the table
                unitOfWork.DocumentMemberRepository.DeleteMembersByDocumentId(request.DocumentId);

                if(!await unitOfWork.CommitAsync())
                {
                    throw new SomethingWentWrongException("Something went wrong when the members of document tried to be deleted.");
                }

                unitOfWork.DocumentRepository.Delete(document);

                return await unitOfWork.CommitAsync();
            }
            else
            {
                // do soft delete
                document.IsDeleted = true;
                document.UpdatedOn = DateTime.Now;

                return await unitOfWork.CommitAsync();  
            }
        }
    }
}
