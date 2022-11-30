/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { OrganizationViewModel } from "@/modules/organization";
import { DepartmentModule } from "@/modules/organization/departments";
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
    organizations(state): string {
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
    async addDepartment({ commit }, playload: string): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };
        const result = await axios.post(`department/create_department?name=${playload}`)
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
    async addSpecialization({ commit }, playload: SpecializationCreateParamsModule): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };

        const result = await axios.post(`specialization/create_specialization`, playload)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });

        if (response.isSuccess) {
            commit("_addSpecialization", (result as AxiosResponse).data);
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
