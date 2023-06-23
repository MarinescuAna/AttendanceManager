/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { InsertCollaboratorParameters, InsertCollectionParameters, UpdateCollectionParameters, UpdateReportParameters } from "@/modules/commands-parameters";
import { CollectionDto } from "@/modules/view-modules";
import https from "@/plugins/axios";
import { COLLECTION_CONTROLLER, REPORT_CONTROLLER } from "@/shared/constants";
import { AxiosResponse } from "axios";

// actions for this store
export const documentActions = {
    /**
     * Update the currentDocument from the state only if the currentDocument is null or if the new documentID is different from the current one
     * @param payload documentId
     */
    async loadCurrentReport({ commit, state }, payload: string): Promise<boolean> {

        if (Object.keys(state.currentReport).length > 0) {
            return true;
        }
        let isFail = false;

        //load the document details and update the store
        const response = await https.get(`${REPORT_CONTROLLER}/${payload}`)
            .catch(error => isFail = ResponseHandler.errorResponseHandler(error));

        if (!isFail) {
            commit("_currentReport", (response as AxiosResponse).data);
        }
        return isFail;
    },
    /** Add new collaborator teacher */
    async addCollaborator({ commit }, payload: InsertCollaboratorParameters): Promise<boolean> {
        let isSuccess = true;

        const result = await https.post(`${REPORT_CONTROLLER}/add_collaborator`,payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_addCollaborator", (result as AxiosResponse).data);
        }

        return isSuccess;
    },
    /** Update some information related to the document */
    async updateDocument({ commit }, payload: { module: UpdateReportParameters, newCourseName: string }): Promise<boolean> {
        let isSuccess = true;

        await https.patch(`${REPORT_CONTROLLER}/update_report`, payload.module)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_partialCurrentDocumentUpdate", payload);
        }

        return isSuccess;
    },

    async deleteDocument({ commit }): Promise<boolean> {
        let isSuccess = true;

        await https.delete(`${REPORT_CONTROLLER}/delete_current_report`)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_deleteDocument");
        }

        return isSuccess;
    },
    /**
     * 1.Call api
     * 2.remove the collection from store
     * 3.update document details regarding the courses held by now
     */
    async deleteCollection({ commit }, collectionId: number): Promise<boolean> {
        let isSuccess = true;

        await https.delete(`${COLLECTION_CONTROLLER}/delete/${collectionId}`)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_deleteCollection", collectionId);
        }

        return isSuccess;
    },
    async updateCollection({ commit }, payload: UpdateCollectionParameters): Promise<boolean> {
        let isSuccess = true;

        await https.patch(`${COLLECTION_CONTROLLER}/update_collection`, payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_partialCollectionUpdate", payload);
        }

        return isSuccess;
    },
    /** Add new attendance collection */
    async addCollection({ commit }, payload: InsertCollectionParameters): Promise<boolean> {
        let isSuccess = true;

        const result = await https.post(`${COLLECTION_CONTROLLER}/create_collection`, payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_addCollection", {
                collectionId: (result as AxiosResponse).data,
                activityTime: payload.activityDateTime,
                courseType: payload.courseType,
                title: payload.title
            } as CollectionDto);
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

