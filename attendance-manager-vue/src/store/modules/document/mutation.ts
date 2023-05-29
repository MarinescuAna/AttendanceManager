import { DocumentFullViewModule, DocumentMembersViewModule, DocumentUpdateModule } from "@/modules/document";
import { AttendanceCollectionViewModule } from "@/modules/document/attendance-collection";
import { CourseType } from "@/shared/enums";
import { initializeDocumentState } from ".";

// mutations for this store
export const documentMutations = {
    /**
     * Update the entire list of documents existed into the store
     */
    _documentDetails(state, payload: DocumentFullViewModule): void {
        state.currentDocument = payload;
    },
    /**
 * Update some information related to the current document
 */
    _partialCurrentDocumentUpdate(state, payload: { module: DocumentUpdateModule, newCourseName: string }): void {
        state.currentDocument.title = payload.module.title;
        state.currentDocument.courseId = payload.module.courseId;
        state.currentDocument.maxNoLaboratories = payload.module.noLaboratories;
        state.currentDocument.maxNoLessons = payload.module.noLessons;
        state.currentDocument.maxNoSeminaries = payload.module.noSeminaries;
        state.currentDocument.attendanceImportance = payload.module.attendanceImportance;
        state.currentDocument.bonusPointsImportance = payload.module.bonusPointsImportance;
        state.currentDocument.courseName = payload.newCourseName;
    },
    /**
     * Add a documentFile in the current document
    */
    _addAttendanceCollection(state, payload: AttendanceCollectionViewModule): void {
        state.currentDocument.attendanceCollections.unshift(payload);

        if (payload.courseType == CourseType[CourseType.Laboratory]) {
            state.currentDocument.noLaboratories++;
        } else {
            if (payload.courseType == CourseType[CourseType.Lecture]) {
                state.currentDocument.noLessons++;
            } else {
                state.currentDocument.noSeminaries++;
            }
        }
    },
    /** Delete document from the store */
    _deleteDocument(state): void {
        state.currentDocument = {};
    },
    _addCollaborator(state, payload: DocumentMembersViewModule): void {
        state.currentDocument.documentMembers.push(payload);
    },
    /**
     * Reset the state with the initial values
     */
    _resetStore(state): void {
        Object.assign(state, initializeDocumentState());
    },

    /**
     * Reset current document state with the initial values
     */
    _resetCurrentDocumentStore(state): void {
        state.currentDocument = {};
    }
};