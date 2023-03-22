/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { DocumentFullViewModule, DocumentMembersViewModule, DocumentViewModule } from "@/modules/document";
import { AttendanceCollectionInsertModule, AttendanceCollectionViewModule } from "@/modules/document/attendance-collection";
import https from "@/plugins/axios";
import { ATTENDANCE_COLLECTION_CONTROLLER, DOCUMENT_CONTROLLER } from "@/shared/constants";
import { CourseType } from "@/shared/enums";
import { AxiosResponse } from "axios";

//state type
export interface DocumentState {
    // array with all the documents created by the teacher
    createdDocuments: DocumentViewModule[];
    // array with all the documents where the teacher is just collaborator
    collaboratorDocuments: DocumentViewModule[];
    // current document
    currentDocument: DocumentFullViewModule;
}

//initialize the state with an empty state
function initialize(): DocumentState {
    return {
        createdDocuments: [],
        collaboratorDocuments: [],
        currentDocument: {} as DocumentFullViewModule
    };
}

// initial state
const state: DocumentState = initialize();

// getters for this store
const getters = {
    /**
     * Gets created documents from the store
    */
    createdDocuments(state): DocumentViewModule[] {
        return state.createdDocuments;
    },
    /**
    * Gets documents the teacher is collaborator from the store
    */
    collaboratorDocuments(state): DocumentViewModule[] {
        return state.collaboratorDocuments;
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
    _createdDocuments(state, payload: DocumentViewModule[]): void {
        state.createdDocuments = payload;
    },
    /**
     * Update the entire list of documents existed into the store
     */
    _collaboratorDocuments(state, payload: DocumentViewModule[]): void {
        state.collaboratorDocuments = payload;
    },
    /**
     * Update the entire list of documents existed into the store
     */
    _documentDetails(state, payload: DocumentFullViewModule): void {
        state.currentDocument = payload;
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
     * Load the documents where the user is collaborator or the created documents
     * @param payload true for created documents, false for documents where the user is collaborator
     */
    async loadDocuments({ commit, state }, payload: boolean): Promise<void> {
        if (state.createdDocuments.length == 0) {
            let isSuccess = true;

            const result = await https.get(`${DOCUMENT_CONTROLLER}/documents?loadCreatedDocuments=${payload}`)
                        .catch(error => {
                            isSuccess = ResponseHandler.errorResponseHandler(error);
                        });
            
            if(isSuccess){
                const documents: DocumentViewModule[] = (result as AxiosResponse).data as DocumentViewModule[];
                commit(payload ? "_createdDocuments" : "_collaboratorDocuments", documents);
            }
        }
    },
    /**
     * Update the currentDocument from the state only if the currentDocument is null or if the new documentID is different from the current one
     * @param payload documentId
     */
    async loadCurrentDocument({ commit, state }, payload: string): Promise<void> {

        if (typeof (payload) != "undefined" && Object.keys(state.currentDocument).length === 0) {
            let isFail = false;

            //load the document details and update the store
            const response = await https.get(`${DOCUMENT_CONTROLLER}/document_by_id?id=${payload}`)
                .catch(error => isFail = ResponseHandler.errorResponseHandler(error));

            if (!isFail) {
                commit("_documentDetails", (response as AxiosResponse).data);
            }
        }
    },
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
