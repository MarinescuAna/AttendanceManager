/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { DocumentFullViewModule, DocumentViewModule } from "@/modules/document";
import { DocumentFileInsertModule, DocumentFileViewModule } from "@/modules/document/document-file";
import https from "@/plugins/axios";
import { Logger } from "@/plugins/custom-plugins/logging";
import { ResponseModule } from "@/shared/modules";
import { AxiosResponse } from "axios";

//state type
export interface DocumentState {
    createdDocuments: DocumentViewModule[]
    currentDocument: {
        documentDetails: DocumentFullViewModule,
        documentFiles: DocumentFileViewModule[]
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
    documentFiles(state): DocumentFileViewModule[] {
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
    _documentFiles(state, payload: DocumentFileViewModule[]): void {
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
    _addDocumentFile(state, payload: DocumentFileViewModule): void {
        state.currentDocument.documentFiles.push(payload);
    },
    /**
     * Reset the state with the initial values
     */
    _resetStore(state): void{
        Logger.logInfo('Reset the Document store to the initial state')
        Object.assign(state, initialize());
    }
};

// actions for this store
const actions = {
    /**
     * Load all the documents from the API and initialize the store
     */
    async loadCreatedDocuments({ commit, state }, payload: string): Promise<void> {
        //if (state.createdDocuments.length == 0) {
        const documents: DocumentViewModule[] = (await https.get(`document/created_documents_by_email?email=${payload}`)).data;
        commit("_createdDocuments", documents);
        //}
    },
    /**
     * Update the currentDocument from the state only if the currentDocument is null or if the new documentID is different from the current one
     * @param payload documentId
     */
    async loadCurrentDocument({ commit, state }, payload: string): Promise<void> {
        if (payload && (state.currentDocument == null || state.currentDocument.documentDetails.documentId != payload)) {
            //load the document details and update the store
            const documentDetails: DocumentFullViewModule = (await https.get('document/document_by_id?id=' + payload)).data;
            commit("_documentDetails", documentDetails);

            // load the document files and update the store
            const documentFiles: DocumentFileViewModule[] = (await https.get('document_file/document_files_by_documentId?documentId=' + payload)).data;
            commit("_documentFiles", documentFiles);

        }
    },
    async addDocumentFile({ commit }, payload: DocumentFileInsertModule): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };

        const result = await https.post(`document_file/create_document_file`, payload)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });

        if (response.isSuccess) {
            commit("_addDocumentFile", {
                documentFileId: (result as AxiosResponse).data,
                activityTime: payload.activityDateTime,
                courseType: payload.courseType
            } as DocumentFileViewModule);
        }

        return response;
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
