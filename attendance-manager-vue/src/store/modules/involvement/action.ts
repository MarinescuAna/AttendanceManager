/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { InvolvementViewModule } from "@/modules/document/involvement";
import https from "@/plugins/axios";
import { ATTENDANCE_CONTROLLER } from "@/shared/constants";
import { AxiosResponse } from "axios";

// actions for this store
export const involvementActions = {
    /**
     * Load all the involvements for the current document
     */
    async loadInvolvements({ commit, state }, reload: boolean): Promise<boolean> {
        if (state.involvements.length == 0 || reload) {
            let isSuccess = true;

            const result = await https.get(`${ATTENDANCE_CONTROLLER}/involvements`)
                .catch(error => {
                    isSuccess = ResponseHandler.errorResponseHandler(error);
                });

            if (isSuccess) {
                commit("_involvements", (result as AxiosResponse).data as InvolvementViewModule[]);
            }
        }

        return true;
    },
  
    /**
     * Reset the state with the initial values
     */
    resetStore({ commit }): void {
        commit('_resetStore');
    },
};

