/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { SpecializationInsertModule, SpecializationViewModule } from "@/modules/specialization";
import { ResponseModule } from "@/shared/modules";
import axios, { AxiosResponse } from "axios";
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
/**
 * @todo Currently this getter is not used anymore so delete it if it's not needed 
 * */
const getters: GetterTree<SpecializationState, RootState> = {
    /**
     * Gets specializations from the store
     */
    specializationsByDepartmentId(state): SpecializationViewModule[] {
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
     * Load all the specializations from the API and initialize the store
     * @todo test this 
     */
    async loadSpecializations({ commit, state }): Promise<SpecializationViewModule[]> {
        if (state.specializations.length != 0) {
            return state.specializations;
        }

        const specializations: SpecializationViewModule[] = (await axios.get('specialization/specializations')).data;
        commit("_specializations", specializations);
        return specializations;
    },
    /**
     * Load all the specializations by departmentId
     * @todo test this 
     */
    async loadSpecializationsByDepartmentId({ commit, state }, payload: string): Promise<SpecializationViewModule[]> {
        if (state.specializations.length == 0) {
            const specializations: SpecializationViewModule[] = (await axios.get('specialization/specializations')).data;
            commit("_specializations", specializations);
        }

        return state.specializations.filter(s => s.departmentId == payload);
    },
    /**
     * Add a new specialization into the database and initialize the store
     * @test api+vue
     * @todo change the return type -> return only the id of the specialization not the entire object
     */
    async addSpecialization({ commit }, payload: SpecializationInsertModule): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };

        // this result represents the id of the specialization
        const result = await axios.post(`specialization/create_specialization`, payload)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });

        if (response.isSuccess) {
            commit("_addSpecialization", {
                id: (result as AxiosResponse).data,
                name: payload.name,
                departmentId: payload.departmentId
            } as SpecializationViewModule);
        }
        return response;
    },
    /**
     * Remove a department from the db and store
     * @todo implement the delete entierly
  
    async removeDepartment({ commit }, payload: UpdateDepartmentModule): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };

        await axios.patch(`department/delete_department`, payload)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });

        if (response.isSuccess) {
            commit("_removeDepartment", payload.departmentId);
        }
        return response;
    },   */
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
