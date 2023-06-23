/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { InsertDepartmentParameters, UpdateDepartmentParameters } from "@/modules/commands-parameters";
import { DepartmentViewModule } from "@/modules/view-modules";
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
    _updateDepartmentName(state, payload: UpdateDepartmentParameters): void {
        state.departments.map(cr => {
            if (cr.id == payload.departmentId) {
                cr.name = payload.name;
            }
        });
    },
    _deleteDepartment(state, payload: number): void{
        state.departments = state.departments.filter(s=>s.id!=payload);  
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
    async loadDepartmentsAsync({ commit, state }): Promise<void> {
        if (state.departments.length == 0) {
            const departments: DepartmentViewModule[] = (await https.get(`${DEPARTMENT_CONTROLLER}/departments`)).data;
            commit("_departments", departments);
        }
    },
    /**
     * Add a new department into the database and initialize the store
     */
    async addDepartmentAsync({ commit }, payload: InsertDepartmentParameters): Promise<boolean> {
        let isSuccess = true;

        // get the id of the new department
        const result = await https.post(`${DEPARTMENT_CONTROLLER}/create_department`,payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_addDepartment", {
                id: (result as AxiosResponse).data,
                name: payload.name
            } as DepartmentViewModule);
        }
        return isSuccess;
    },
    /**
     * Update department name in db and store
     */
    async updateDepartmentAsync({ commit }, payload: UpdateDepartmentParameters): Promise<boolean> {
        let isSuccess = true;

        await https.patch(`${DEPARTMENT_CONTROLLER}/update_department`,payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_updateDepartmentName", payload);
        }

        return isSuccess;
    },
    async deleteDepartmentAsync({ commit }, payload: number): Promise<boolean> {
        let isSuccess = true;

        await https.delete(`${DEPARTMENT_CONTROLLER}/delete/${payload}`)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_deleteDepartment",payload);
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
