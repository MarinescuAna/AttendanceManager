/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { DocumentFullViewModule, DocumentViewModule } from "@/modules/document";
import { AttendanceCollectionInsertModule, AttendanceCollectionViewModule } from "@/modules/document/attendance-collection";
import https from "@/plugins/axios";
import { ATTENDANCE_COLLECTION_CONTROLLER, DOCUMENT_CONTROLLER } from "@/shared/constants";
import { CourseType } from "@/shared/enums";
import { AxiosResponse } from "axios";

//state type
export interface DocumentState {
    createdDocuments: DocumentViewModule[]
    currentDocument: {
        documentDetails: DocumentFullViewModule,
        documentFiles: AttendanceCollectionViewModule[]
    }
}

//initialize the state with an empty array
function initialize(): DocumentState {
    return {
        createdDocuments: [],
        currentDocument: null!
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
     * Gets created documents from the store
    */
    documentDetails(state): DocumentFullViewModule {
        return state.currentDocument.documentDetails;
    },
    /**
     * Gets created documents from the store
    */
    documentFiles(state): AttendanceCollectionViewModule[] {
        return state.currentDocument.documentFiles;
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
    _documentDetails(state, payload: DocumentFullViewModule): void {
        if (state.currentDocument == null) {
            state.currentDocument = {
                documentDetails: Object as () => DocumentFullViewModule,
                documentFiles: []
            }
        }
        state.currentDocument.documentDetails = payload;
    },
    /**
     * Update the entire list of documents existed into the store
     */
    _documentCollections(state, payload: AttendanceCollectionViewModule[]): void {
        if (state.currentDocument == null) {
            state.currentDocument = {
                documentDetails: Object as () => DocumentFullViewModule,
                documentFiles: []
            }
        }
        state.currentDocument.documentFiles = payload;
    },
    /**
     * Add a documentFile in the current document
    */
    _addAttendanceCollection(state, payload: AttendanceCollectionViewModule): void {
        state.currentDocument.documentFiles.push(payload);
        
        if(payload.courseType == CourseType[CourseType.Laboratory]){
            state.currentDocument.documentDetails.noLaboratories++;
        }else {
            if(payload.courseType == CourseType[CourseType.Lesson]){
                state.currentDocument.documentDetails.noLessons++;
            }else{
                state.currentDocument.documentDetails.noSeminaries++;
            }
        }
    },
    /**
     * Reset the state with the initial values
     */
    _resetStore(state): void{
        Object.assign(state, initialize());
    }
};

// actions for this store
const actions = {
    /**
     * Load all the documents from the API and initialize the store
     */
    async loadCreatedDocuments({ commit }): Promise<void> {
        if (state.createdDocuments.length == 0) {
            const documents: DocumentViewModule[] = (await https.get(`${DOCUMENT_CONTROLLER}/created_documents_by_email`)).data;
            commit("_createdDocuments", documents);
        }
    },
    /**
     * Update the currentDocument from the state only if the currentDocument is null or if the new documentID is different from the current one
     * @param payload documentId
     */
    async loadCurrentDocument({ commit, state }, payload: string): Promise<void> {
        if (payload && (state.currentDocument == null || state.currentDocument.documentDetails.documentId != payload)) {
            //load the document details and update the store
            const documentDetails: DocumentFullViewModule = 
                (await https.get(`${DOCUMENT_CONTROLLER}/document_by_id?id=${payload}`)).data;
            commit("_documentDetails", documentDetails);

            // load the document files and update the store
            const documentCollections: AttendanceCollectionViewModule[] = 
                (await https.get(`${ATTENDANCE_COLLECTION_CONTROLLER}/attendance_collection_by_documentId?documentId=${payload}`)).data;
            commit("_documentCollections", documentCollections);
        }
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
