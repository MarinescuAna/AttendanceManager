﻿using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Enums;
using AutoMapper;

namespace AttendanceManager.Application.Features
{
    public class BaseDocumentFeature : BaseFeature
    {
        protected static Domain.Entities.Document? currentDocument;
        /// <summary>
        /// This lsit contains a dictionary with all the collections defined under the current report, with its activity type
        /// </summary>
        protected static Dictionary<int, CourseType>? attendanceCollectionsType;
        protected static List<Domain.Entities.DocumentMember>? documentMembers;
        public BaseDocumentFeature(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        // No need to call this method in other places then where the document is initialized
        public async Task InitialiazeDocumentAsync(int documentId)
        {
            // if there is another  current document loaded, clear the data
            if (currentDocument != null)
            {
                currentDocument = null;
                attendanceCollectionsType?.Clear();
                documentMembers?.Clear();
            }

            // get current document, which includes AttendanceCollection
            currentDocument = await unitOfWork.DocumentRepository.GetDocumentByIdAsync(documentId)
                ?? throw new NotFoundException("The document cannot be found!");

            // get a dictionary of each attendance collection course type
            attendanceCollectionsType = currentDocument.AttendanceCollections!.ToDictionary(ac => ac.AttendanceCollectionID, ac => ac.CourseType);

            // get a list with all the members 
            documentMembers = await unitOfWork.DocumentMemberRepository.GetDocumentMembersByDocumentIdAndRoleAsync(documentId, null);
        }


    }
}
