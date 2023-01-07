/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { OrganizationViewModel } from "@/modules/organization";
import { DepartmentModule, UpdateDepartmentModule } from "@/modules/organization/departments";
import { SpecializationCreateParamsModule, SpecializationModule, SpecializationViewModule } from "@/modules/organization/specializations";
import { ResponseModule } from "@/shared/modules";
import axios, { AxiosResponse } from "axios";

//state type
export interface OrganizationState {
    organizations: OrganizationViewModel[]
}

//initialize the state with an empty array
export function initialize(): OrganizationState {
    return {
        organizations: []
    };
}

// initial state
const state: OrganizationState = initialize();

// getters for this store
const getters = {
    /**
     * Gets departments and specializations from the store
     */
    organizations(state): OrganizationViewModel[] {

        if (state.organizations.length == 0) {
            actions.loadOrganizations;
        }

        return state.organizations;
    },
    /**
     * Get the list with all the departments without specializations
     */
    departments(state): DepartmentModule[] {
        return state.organizations.map(cr => ({
            id: cr.id,
            name: cr.name
        }) as DepartmentModule)
    },
    /**
     * Get the list with all the specializations by departmentId
     */
    specializations: (state) => {
        return (departmentId: string): SpecializationModule[] => {
            return state.organizations.find(cr => cr.id == departmentId).children;
        }
    }
};

// mutations for this store
const mutations = {
    /**
     * Update the entire list of departments and specializations existed into the store
     */
    _organizations(state, organizations: OrganizationViewModel): void {
        state.organizations = organizations;
    },
    /**
     * Add a new department into the store 
     */
    _addDepartment(state, department: OrganizationViewModel): void {
        department.children = [];
        state.organizations.push(department);
    },
    /**
     * Add a new specialziation for a specific department into the store 
     */
    _addSpecialization(state, specialization: SpecializationModule): void {
        state.organizations.forEach(element => {
            if (element.id == specialization.departmentId) {
                element.children.push({
                    id: specialization.id,
                    name: specialization.name
                } as SpecializationViewModule)
            }
        });
    },
    /**
     * Remove department and the all the specialisations defined under it
     */
    _removeDepartment(state, departmentId: string): void {
        state.organizations = state.organizations.filter(cr => cr.id != departmentId);
    },
    /**
     * Update department name
     */
    _updateDepartmentName(state, departmentId: string, name: string): void {
        state.organizations.foreach(cr =>{
            if(cr.id == departmentId){
                cr.name = name;
            }
        });
    }

};

// actions for this store
const actions = {
    /**
     * Load all the departments and the specializations from the API and initialize the store
     */
    async loadOrganizations({ commit }): Promise<OrganizationViewModel[]> {
        const organizations: OrganizationViewModel[] = (await axios.get('department/departments')).data;
        commit("_organizations", organizations);
        return organizations;
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
            commit("_addDepartment", (result as AxiosResponse).data);
        }
        return response;
    },
    /**
     * Add a new specialization into the database and initialize the store
     */
    async addSpecialization({ commit }, payload: SpecializationCreateParamsModule): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };

        const result = await axios.post(`specialization/create_specialization`, payload)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });

        if (response.isSuccess) {
            commit("_addSpecialization", (result as AxiosResponse).data);
        }
        return response;
    },
    /**
     * Remove a department from the db and store
     */
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
    },
    /**
     * Update department name in db and store
     */
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
    }
};

// export the namespace
export const namespace = 'organization';

// export the store 'module'
export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};
