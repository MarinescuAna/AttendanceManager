import { CourseType } from "@/shared/enums";
import { initializeDocumentState } from ".";
import { UpdateCollectionParameters, UpdateReportParameters } from "@/modules/commands-parameters";
import { CollectionDto, MembersDto, ReportViewModule } from "@/modules/view-modules";

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
    _partialCurrentDocumentUpdate(state, payload: { module: UpdateReportParameters, newCourseName: string }): void {
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
    _addCollection(state, payload: CollectionDto): void {
        state.currentReport.collections.unshift(payload);

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

    _partialCollectionUpdate(state, payload: UpdateCollectionParameters): void {
        state.currentReport.collections.forEach(c => {
            if (c.collectionId == payload.collectionId) {
                c.activityTime = payload.activityDateTime;
                c.courseType = payload.courseType;
                c.title = payload.title;
            }
        })
    },
    /** Delete document from the store */
    _deleteDocument(state): void {
        state.currentReport = {};
    },
    _deleteCollection(state, collectionId: number): void {
        const removedCollection = state.currentReport.collections.find(c => c.collectionId == collectionId);
        state.currentReport.collections = state.currentReport.collections.filter(c => c.collectionId != collectionId);

        if (removedCollection.courseType == CourseType[CourseType.Laboratory]) {
            state.currentReport.noLaboratories--;
        } else if (removedCollection.courseType == CourseType[CourseType.Lecture]) {
            state.currentReport.noLessons--;
        } else if (removedCollection.courseType == CourseType[CourseType.Seminary]) {
            state.currentReport.noSeminaries--;
        }
    },
    _addCollaborator(state, payload: MembersDto): void {
        state.currentReport.members.push(payload);
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