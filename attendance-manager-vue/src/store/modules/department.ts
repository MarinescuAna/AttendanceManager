/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { DepartmentViewModule } from "@/modules/department";
import https from "@/plugins/axios";
import { DEPARTMENT_CONTROLLER } from "@/shared/constants";
import { AxiosResponse } from "axios";

//state type
export interface DepartmentState {
    departments: DepartmentViewModule[]
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
     */
    departments(state): DepartmentViewModule[] {
        return state.departments;
    }
};

// mutations for this store
const mutations = {
    /**
     * Update the entire list of departments existed into the store
     */
    _departments(state, payload: DepartmentViewModule): void {
        state.departments = payload;
    },
    /**
     * Add a new department into the store 
     */
    _addDepartment(state, payload: DepartmentViewModule): void {
        state.departments.push(payload);
    },
    /**
     * Update department name
     */
    _updateDepartmentName(state, payload: { name: string, id: number }): void {
        state.departments.map(cr => {
            if (cr.id == payload.id) {
                cr.name = payload.name;
            }
        });
    },
    /**
     * Reset the state with the initial values
     */
    _resetStore(state): void {
        Object.assign(state, initialize());
    }
};

// actions for this store
const actions = {
    /**
     * Load all the departments and the specializations from the API and initialize the store
     */
    async loadDepartments({ commit, state }): Promise<void> {
        if (state.departments.length == 0) {
            const departments: DepartmentViewModule[] = (await https.get(`${DEPARTMENT_CONTROLLER}/departments`)).data;
            commit("_departments", departments);
        }
    },
    /**
     * Add a new department into the database and initialize the store
     */
    async addDepartment({ commit }, payload: string): Promise<boolean> {
        let isSuccess = true;

        // get the id of the new department
        const result = await https.post(`${DEPARTMENT_CONTROLLER}/create_department/${payload}`)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_addDepartment", {
                id: (result as AxiosResponse).data,
                name: payload
            } as DepartmentViewModule);
        }
        return isSuccess;
    },
    /**
     * Update department name in db and store
     */
    async updateDepartmentName({ commit }, payload: { name: string, id: number }): Promise<boolean> {
        let isSuccess = true;

        await https.patch(`${DEPARTMENT_CONTROLLER}/update_department_name?name=${payload.name}&id=${payload.id}`)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_updateDepartmentName", payload);
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
export const namespace = 'department';

// export the store 'module'
export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};
