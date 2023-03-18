/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { SpecializationInsertModule, SpecializationViewModule } from "@/modules/specialization";
import https from "@/plugins/axios";
import { SPECIALIZATION_CONTROLLER } from "@/shared/constants";
import { AxiosResponse } from "axios";
import { ActionTree, GetterTree, Module, MutationTree } from "vuex";
import { RootState } from "..";

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
    /**
     * Reset the state with the initial values
     */
    _resetStore(state): void{
        Object.assign(state, initialize());
    }
    /**
     * Remove specialisation
     * @todo Implement the delete method for specializations
    _removeSpecialization(state, payload: string): void {
        state.specializations = state.specializations.filter(cr => cr.id != payload);
    }, */
    /**
     * Update department name
     * @todo Implement the update method for specializations: update the name and department
    _updateSpecialization(state, departmentId: string, name: string): void {
        state.organizations.foreach(cr =>{
            if(cr.id == departmentId){
                cr.name = name;
            }
        });
    }
*/
};

// actions for this store
const actions: ActionTree<SpecializationState, RootState> = {
    /**
     * Load all the specializations
     */
    async loadSpecializations({ commit, state }): Promise<void> {
        if (state.specializations.length == 0) {
            const specializations: SpecializationViewModule[] = (await https.get(`${SPECIALIZATION_CONTROLLER}/specializations`)).data;
            commit("_specializations", specializations);
        }
    },
    /**
     * Add a new specialization into the database and initialize the store
     */
    async addSpecialization({ commit }, payload: SpecializationInsertModule): Promise<boolean> {
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
                departmentId: payload.departmentId
            } as SpecializationViewModule);
        }
        return isSuccess;
    },
    /**
     * Update department name in db and store
     * @todo implement the update entierly
     async updateDepartmentName({ commit }, payload: UpdateDepartmentModule): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };

        await axios.patch(`department/update_department`, payload)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });

        if (response.isSuccess) {
            commit("_updateDepartmentName", payload.departmentId, payload.name);
        }
        return response;
    }*/
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
