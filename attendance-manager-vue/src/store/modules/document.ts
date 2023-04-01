/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { DocumentFullViewModule, DocumentMembersViewModule, DocumentViewModule, DocumentUpdateModule, DocumentDashboardViewModule } from "@/modules/document";
import { AttendanceCollectionInsertModule, AttendanceCollectionViewModule } from "@/modules/document/attendance-collection";
import https from "@/plugins/axios";
import { ATTENDANCE_COLLECTION_CONTROLLER, DASHBOARD_CONTROLLER, DOCUMENT_CONTROLLER } from "@/shared/constants";
import { CourseType } from "@/shared/enums";
import { AxiosResponse } from "axios";

//state type
export interface DocumentState {
    // array with all the documents
    documents: DocumentViewModule[];
    // current document
    currentDocument: DocumentFullViewModule;
}

//initialize the state with an empty state
function initialize(): DocumentState {
    return {
        documents: [],
        currentDocument: {} as DocumentFullViewModule
    };
}

// initial state
const state: DocumentState = initialize();

// getters for this store
const getters = {
    /**
     * Get documents from the store
    */
    documents(state): DocumentViewModule[] {
        return state.documents;
    },
    /**
     * Gets created documents from the store
    */
    documentDetails(state): DocumentFullViewModule {
        return state.currentDocument;
    },
    /**
     * Gets created documents from the store
    */
    documentFiles(state): AttendanceCollectionViewModule[] {
        return typeof (state.currentDocument?.attendanceCollections) === "undefined" ? []
            : state.currentDocument?.attendanceCollections;
    }
};

// mutations for this store
const mutations = {
    /**
     * Update the entire list of documents existed into the store
     */
    _documents(state, payload: DocumentViewModule[]): void {
        state.documents = payload;
    },
    /**
     * Update the entire list of documents existed into the store
     */
    _documentDetails(state, payload: DocumentFullViewModule): void {
        state.currentDocument = payload;
    },
    /** Update document dashboard on demand */
    _documentDashboard(state, payload: DocumentDashboardViewModule): void {   
        state.currentDocument.documentDashboard = payload;
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
        state.currentDocument.attendanceCollections.push(payload);

        if (payload.courseType == CourseType[CourseType.Laboratory]) {
            state.currentDocument.noLaboratories++;
        } else {
            if (payload.courseType == CourseType[CourseType.Lesson]) {
                state.currentDocument.noLessons++;
            } else {
                state.currentDocument.noSeminaries++;
            }
        }
    },
    /** Delete document from the store */
    _deleteDocument(state, payload: number): void {
        state.currentDocument = {};
        state.documents = state.documents.filter(d => d.documentId != payload);
    },
    _addCollaborator(state, payload: DocumentMembersViewModule): void {
        state.currentDocument.documentMembers.push(payload);
    },
    /**
     * Reset the state with the initial values
     */
    _resetStore(state): void {
        Object.assign(state, initialize());
    },

    /**
     * Reset current document state with the initial values
     */
    _resetCurrentDocumentStore(state): void {
        state.currentDocument = {};
    }
};

// actions for this store
const actions = {
    /**
     * Load all the documents
     */
    async loadDocuments({ commit, state }): Promise<boolean> {
        if (state.documents.length == 0) {
            let isSuccess = true;

            const result = await https.get(`${DOCUMENT_CONTROLLER}/documents`)
                .catch(error => {
                    isSuccess = ResponseHandler.errorResponseHandler(error);
                });

            if (isSuccess) {
                commit("_documents", (result as AxiosResponse).data as DocumentViewModule[]);
            }
        }

        return true;
    },
    /**
 * Load document dashboard
 */
    async loadDocumentDashboard({ commit, state }): Promise<boolean> {
        if (typeof (state.currentDocument.documentDashboard) === "undefined") {
            let isSuccess = true;

            const result = await https.get(`${DASHBOARD_CONTROLLER}/document_dashboard/${state.currentDocument.documentId}`,)
                .catch(error => {
                    isSuccess = ResponseHandler.errorResponseHandler(error);
                });

            if (isSuccess) {
                commit("_documentDashboard", (result as AxiosResponse).data as DocumentViewModule[]);
            }
        }

        return true;
    },
    /**
     * Update the currentDocument from the state only if the currentDocument is null or if the new documentID is different from the current one
     * @param payload documentId
     */
    async loadCurrentDocument({ commit, state }, payload: string): Promise<boolean> {

        if (typeof (payload) != "undefined" && Object.keys(state.currentDocument).length === 0) {
            let isFail = false;

            //load the document details and update the store
            const response = await https.get(`${DOCUMENT_CONTROLLER}/document_by_id?id=${payload}`)
                .catch(error => isFail = ResponseHandler.errorResponseHandler(error));

            if (!isFail) {
                commit("_documentDetails", (response as AxiosResponse).data);
            }
        }
        return true;
    },
    /** Add new collaborator teacher */
    async addCollaborator({ commit, state }, payload: string): Promise<boolean> {
        let isSuccess = true;

        const result = await https.post(`${DOCUMENT_CONTROLLER}/add_collaborator`, {
            email: payload,
            documentId: state.currentDocument.documentId
        })
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_addCollaborator", (result as AxiosResponse).data);
        }

        return isSuccess;
    },
    /** Update some information related to the document */
    async updateDocument({ commit }, payload: { module: DocumentUpdateModule, newCourseName: string }): Promise<boolean> {
        let isSuccess = true;

        await https.patch(`${DOCUMENT_CONTROLLER}/update_document`, payload.module)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_partialCurrentDocumentUpdate", payload);
        }

        return isSuccess;
    },
    /** Delete document (soft or hard) */
    async deleteDocument({ commit, state }): Promise<boolean> {
        let isSuccess = true;
        const documentId = state.currentDocument.documentId;

        await https.delete(`${DOCUMENT_CONTROLLER}/delete_document/${documentId}`)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_deleteDocument", documentId);
        }

        return isSuccess;
    },
    /** Add new attendance collection */
    async addAttendanceCollection({ commit }, payload: AttendanceCollectionInsertModule): Promise<boolean> {
        let isSuccess = true;

        const result = await https.post(`${ATTENDANCE_COLLECTION_CONTROLLER}/create_attendance_collection`, payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_addAttendanceCollection", {
                attendanceCollectionId: (result as AxiosResponse).data,
                activityTime: payload.activityDateTime,
                courseType: payload.courseType
            } as AttendanceCollectionViewModule);
        }

        return isSuccess;
    },
    /**
     * Reset the state with the initial values
     */
    resetStore({ commit }): void {
        commit('_resetStore');
    },

    /**
     * Reset current document state with the initial values
     */
    resetCurrentDocumentStore({ commit }): void {
        commit('_resetCurrentDocumentStore');
    },
};

// export the namespace
export const namespace = 'document';

// export the store 'module'
export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};
