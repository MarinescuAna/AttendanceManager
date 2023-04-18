/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { DocumentViewModule, DocumentUpdateModule } from "@/modules/document";
import { AttendanceCollectionInsertModule, AttendanceCollectionViewModule } from "@/modules/document/attendance-collection";
import https from "@/plugins/axios";
import { Toastification } from "@/plugins/vue-toastification";
import { ATTENDANCE_COLLECTION_CONTROLLER, DASHBOARD_CONTROLLER, DOCUMENT_CONTROLLER } from "@/shared/constants";
import { AxiosResponse } from "axios";

// actions for this store
export const documentActions = {
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

        if (Object.keys(state.currentDocument).length > 0) {
            return true;
        }

        try {
            //load the document details and update the store
            const response = await Promise.race([
                https.get(`${DOCUMENT_CONTROLLER}/document/${payload}`),
                new Promise((resolve, reject) => {
                    setTimeout(() => {
                        reject(new Error("API call timed out."))
                    }, 5000)// set timeout to 5 seconds
                })
            ]);
            commit("_documentDetails", (response as AxiosResponse).data);
            return true;

        } catch (error) {
            console.log(error)
            if(typeof(error) === "string")
            {
                Toastification.simpleError(error as string);
            }
            return false;
        }
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

