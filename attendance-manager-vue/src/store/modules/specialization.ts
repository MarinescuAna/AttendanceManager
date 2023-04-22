/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { SpecializationInsertParameter, SpecializationModule } from "@/modules/specialization";
import https from "@/plugins/axios";
import { SPECIALIZATION_CONTROLLER } from "@/shared/constants";
import { AxiosResponse } from "axios";
import { ActionTree, GetterTree, Module, MutationTree } from "vuex";
import { RootState } from "..";

//state type
interface SpecializationState {
    specializations: SpecializationModule[]
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
    specializations(state:SpecializationState): SpecializationModule[] {
        return state.specializations;
    }
};

// mutations for this store
const mutations: MutationTree<SpecializationState> = {
    /**
     * Update the entire list of specializations existed into the store
     */
    _specializations(state, payload: SpecializationModule[]): void {
        state.specializations = payload;
    },
    /**
     * Add a new specialziation into the store 
     */
    _addSpecialization(state, payload: SpecializationModule): void {
        state.specializations.push(payload);
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
    async loadSpecializations({ commit, state }): Promise<void> {
        if (state.specializations.length == 0) {
            const specializations: SpecializationModule[] = (await https.get(`${SPECIALIZATION_CONTROLLER}/specializations`)).data;
            commit("_specializations", specializations);
        }
    },
    /**
     * Add a new specialization into the database and initialize the store
     */
    async addSpecialization({ commit }, payload: SpecializationInsertParameter): Promise<boolean> {
        let isSuccess = true;

        // this result represents the id of the specialization
        const result = await https.post(`${SPECIALIZATION_CONTROLLER}/create_specialization`, payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_addSpecialization", {
                id: (result as AxiosResponse).data,
                name: payload.name,
                departmentId: payload.departmentId,
            } as SpecializationModule);
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
