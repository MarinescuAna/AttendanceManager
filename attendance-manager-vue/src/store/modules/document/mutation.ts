import { CollectionViewModule, ReportViewModule, MembersViewModule, DocumentUpdateModule } from "@/modules/document";
import { CourseType } from "@/shared/enums";
import { initializeDocumentState } from ".";

// mutations for this store
export const documentMutations = {
    /**
     * Update the entire list of documents existed into the store
     */
    _currentReport(state, payload: ReportViewModule): void {
        state.currentReport = payload;
    },
    /**
 * Update some information related to the current document
 */
    _partialCurrentDocumentUpdate(state, payload: { module: DocumentUpdateModule, newCourseName: string }): void {
        state.currentReport.title = payload.module.title;
        state.currentReport.courseId = payload.module.courseId;
        state.currentReport.maxNoLaboratories = payload.module.noLaboratories;
        state.currentReport.maxNoLessons = payload.module.noLessons;
        state.currentReport.maxNoSeminaries = payload.module.noSeminaries;
        state.currentReport.attendanceImportance = payload.module.attendanceImportance;
        state.currentReport.bonusPointsImportance = payload.module.bonusPointsImportance;
        state.currentReport.courseName = payload.newCourseName;
    },
    /**
     * Add a documentFile in the current document
    */
    _addCollection(state, payload: CollectionViewModule): void {
        state.currentReport.attendanceCollections.unshift(payload);

        if (payload.courseType == CourseType[CourseType.Laboratory]) {
            state.currentReport.noLaboratories++;
        } else {
            if (payload.courseType == CourseType[CourseType.Lecture]) {
                state.currentReport.noLessons++;
            } else {
                state.currentReport.noSeminaries++;
            }
        }
    },
    /** Delete document from the store */
    _deleteDocument(state): void {
        state.currentReport = {};
    },
    _addCollaborator(state, payload: MembersViewModule): void {
        state.currentReport.documentMembers.push(payload);
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
        state.currentReport = {};
    }
};