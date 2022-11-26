/* eslint-disable @typescript-eslint/no-explicit-any */
import { DepartmentModule, SpecializationModule } from "@/shared/modules";
import axios from "axios";

//state type
export interface OrganizationState {
    departments: DepartmentViewDTO[]
}

//initialize the state with an empty array
export function initialize(): OrganizationState {
    return {
        departments: []
    };
}

// initial state
const state: OrganizationState = initialize();

// getters for this store
const getters = {
    /**
     * Gets departments froom the store
     */
    departments(state): string {
        return state.departments;
    },

    /**
     * Get the list with all the departments only
     */
    departmentsOnly(state): DepartmentModule[] {
        return state.departments.map(cr => ({
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
    _departments(state, departments: DepartmentViewDTO): void {
        state.departments = departments;
    },
    /**
     * Add a new department into the store 
     */
    _addDepartment(state, department: DepartmentViewDTO): void {
        state.departments.add(department);
    },
    /**
 * Add a new specialziation for a specific department into the store 
 */
    _addSpecialization(state, specialization: SpecializationModule): void {
        state.departments.forEach(element => {
            if (element.id == specialization.departmentId) {
                element.children.add({
                    id: specialization.id,
                    name: specialization.name
                } as SpecializationViewDTO)
            }
        });
    }
};

// actions for this store
const actions = {
    /**
     * Load all the departments and the specializations from the API and initialize the store
     */
    async loadDepartments({ commit }): Promise<DepartmentViewDTO[]> {
        const departments: DepartmentViewDTO[] = (await axios.get('department/departments')).data;
        commit("_departments", departments);
        return departments;
    },
    /**
     * Add a new department into the database and initialize the store
     */
    async addDepartment({ dispatch, commit }, name: string): Promise<boolean> {
        const result = (await axios.post(`department/create_department?name=${name}`)).data;

        if (result || result == null) {
            return false;
        }

        commit("_addDepartment", result);
        //TODO update the tree
        await dispatch("_departments");
        return true;
    },
    /**
     * Add a new specialization into the database and initialize the store
     */
    async addSpecialization({ dispatch, commit }, specialization: SpecializationCreateDTO): Promise<boolean> {
        const result = (await axios.post(`specialization/create_specialization`, specialization)).data;

        if (result || result == null) {
            return false;
        }

        commit("_addSpecialization", result);
        await dispatch("_departments");        //TODO update the tree
        return true;
    }
};

export interface DepartmentViewDTO {
    id: string;
    name: string;
    children: SpecializationViewDTO[];
}
export interface SpecializationViewDTO {
    id: string;
    name: string;
}
export interface SpecializationCreateDTO {
    name: string;
    departmentId: string;
}


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
