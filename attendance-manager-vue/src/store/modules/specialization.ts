/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import https from "@/plugins/axios";
import { SPECIALIZATION_CONTROLLER } from "@/shared/constants";
import { AxiosResponse } from "axios";
import { ActionTree, GetterTree, Module, MutationTree } from "vuex";
import { RootState } from "..";
import { InsertSpecializationParameters } from "@/modules/commands-parameters";
import { SpecializationViewModule } from "@/modules/view-modules";

//state type
interface SpecializationState {
    specializations: SpecializationViewModule[]
}

//initialize the state with an empty array
function initialize(): SpecializationState {
    return {
        specializations: []
    };
}

// initial state
const state: SpecializationState = initialize();

// getters for this store
const getters: GetterTree<SpecializationState, RootState> = {
    /**
     * Gets specializations from the store
     */
    specializations(state:SpecializationState): SpecializationViewModule[] {
        return state.specializations;
    }
};

// mutations for this store
const mutations: MutationTree<SpecializationState> = {
    /**
     * Update the entire list of specializations existed into the store
     */
    _specializations(state, payload: SpecializationViewModule[]): void {
        state.specializations = payload;
    },
    /**
     * Add a new specialziation into the store 
     */
    _addSpecialization(state, payload: SpecializationViewModule): void {
        state.specializations.push(payload);
    },
    _deleteSpecialization(state, payload: number): void{
        state.specializations = state.specializations.filter(s=>s.id!=payload);
    },
    /**
     * Reset the state with the initial values
     */
    _resetStore(state): void{
        Object.assign(state, initialize());
    }
};

// actions for this store
const actions: ActionTree<SpecializationState, RootState> = {
    /**
     * Load all the specializations
     */
    async loadSpecializationsAsync({ commit, state }): Promise<void> {
        if (state.specializations.length == 0) {
            const specializations: SpecializationViewModule[] = (await https.get(`${SPECIALIZATION_CONTROLLER}/specializations`)).data;
            commit("_specializations", specializations);
        }
    },
    async deleteSpecializationAsync({ commit }, payload: number): Promise<boolean> {
        let isSuccess = true;

        await https.delete(`${SPECIALIZATION_CONTROLLER}/delete/${payload}`)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_deleteSpecialization",payload);
        }

        return isSuccess;
    },
    /**
     * Add a new specialization into the database and initialize the store
     */
    async addSpecializationAsync({ commit }, payload: InsertSpecializationParameters): Promise<boolean> {

        let isSuccess = true;

        // this result represents the id of the specialization
        const result = await https.post(`${SPECIALIZATION_CONTROLLER}/create_specialization`,payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_addSpecialization", {
                id: (result as AxiosResponse).data,
                name: payload.name,
                departmentId: payload.departmentId,
                usersLinked: 0,
                updatedOn: (new Date()).toString()
            } as SpecializationViewModule);
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
export const namespace = 'specialization';

// export the store 'module'
export default <Module<SpecializationState, RootState>>{
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};
