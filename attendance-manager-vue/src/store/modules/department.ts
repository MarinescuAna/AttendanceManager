/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { DepartmentUpdateModule, DepartmentViewModel } from "@/modules/department";
import { ResponseModule } from "@/shared/modules";
import axios, { AxiosResponse } from "axios";

//state type
export interface DepartmentState {
    departments: DepartmentViewModel[]
}

//initialize the state with an empty array
export function initialize(): DepartmentState {
    return {
        departments: []
    };
}

// initial state
const state: DepartmentState = initialize();

// getters for this store
const getters = {
    /**
     * Gets departments from the store
     * @todo delete this if it's not needed anymore
     */
    departments(state): DepartmentViewModel[] {
        return state.departments;
    }
};

// mutations for this store
const mutations = {
    /**
     * Update the entire list of departments existed into the store
     */
    _departments(state, payload: DepartmentViewModel): void {
        state.departments = payload;
    },
    /**
     * Add a new department into the store 
     */
    _addDepartment(state, payload: DepartmentViewModel): void {
        state.departments.push(payload);
    },
    /**
     * Remove department from the store
     * @todo reimplemente this
     */
    _removeDepartment(state, payload: string): void {
        state.departments = state.departments.filter(cr => cr.id != payload);
    },
    /**
     * Update department name
     */
    _updateDepartmentName(state, payload: DepartmentUpdateModule): void {
        state.departments.foreach(cr =>{
            if(cr.id == payload.id){
                cr.name = payload.name;
            }
        });
    }

};

// actions for this store
const actions = {
    /**
     * Load all the departments and the specializations from the API and initialize the store
     */
    async loadDepartments({ commit, state }): Promise<DepartmentViewModel[]> {
        if(state.departments.length != 0){
            return state.departments;
        }

        const departments: DepartmentViewModel[] = (await axios.get('department/departments')).data;
        commit("_departments", departments);
        return departments;
    },
    /**
     * Add a new department into the database and initialize the store
     */
    async addDepartment({ commit }, payload: string): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };
        const result = await axios.post(`department/create_department?name=${payload}`)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });

        if (response.isSuccess) {
            commit("_addDepartment", {
                id: (result as AxiosResponse).data,
                name: payload
            } as DepartmentViewModel);
        }
        return response;
    },
    /**
     * Remove a department from the db and store
     */
    async removeDepartment({ commit }, payload: string): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };

        await axios.patch(`department/delete_department?id=`, payload)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });

        if (response.isSuccess) {
            commit("_removeDepartment", payload);
        }
        return response;
    },
    /**
     * Update department name in db and store
     */
     async updateDepartmentName({ commit }, payload: DepartmentUpdateModule): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };

        await axios.patch(`department/update_department`, payload)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });

        if (response.isSuccess) {
            commit("_updateDepartmentName", payload);
        }
        return response;
    }
};

// export the namespace
export const namespace = 'department';

// export the store 'module'
export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};
