/* eslint-disable @typescript-eslint/no-explicit-any */
import { DocumentViewModule } from "@/modules/document";
import axios from "axios";

//state type
export interface DocumentState {
    createdDocuments: DocumentViewModule[]
}

//initialize the state with an empty array
 export function initialize(): DocumentState {
    return {
        createdDocuments: []
    };
} 

// initial state
const state: DocumentState = initialize();

// getters for this store
const getters = {
    /**
     * Gets documents from the store
     * @todo delete this if it's not needed anymore
     
    departments(state): DepartmentViewModel[] {
        return state.departments;
    }*/
};

// mutations for this store
const mutations = {
    /**
     * Update the entire list of documents existed into the store
     */
    _createdDocuments(state, payload: DocumentViewModule[]): void {
        state.createdDocuments = payload;
    },
    /**
     * Remove department from the store
     * @todo reimplemente this
    
    _removeDepartment(state, payload: string): void {
        state.departments = state.departments.filter(cr => cr.id != payload);
    }, */
    /**
     * Update department name
     
    _updateDepartmentName(state, payload: DepartmentUpdateModule): void {
        state.departments.foreach(cr =>{
            if(cr.id == payload.id){
                cr.name = payload.name;
            }
        });
    }
*/
};

// actions for this store
const actions = {
    /**
     * Load all the documents from the API and initialize the store
     */
    async loadCreatedDocuments({ commit }, payload: string): Promise<DocumentViewModule[]> {
 /*        if(state.departments.length != 0){
            return state.departments;
        } */
        const documents: DocumentViewModule[] = (await axios.get('document/created_documents_by_email?email='+payload)).data;
        commit("_createdDocuments", documents);
        return documents;
    }
};

// export the namespace
export const namespace = 'document';

// export the store 'module'
export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};
