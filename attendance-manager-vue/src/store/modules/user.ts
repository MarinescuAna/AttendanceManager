/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { CreateUserParameters, UserViewModule } from "@/modules/user";
import { ResponseModule } from "@/shared/modules";
import axios, { AxiosResponse } from "axios";


//state type
export interface UserState {
    users: UserViewModule[]
}

//initialize the state with an empty array
export function initialize(): UserState {
    return {
        users: []
    };
}

// initial state
const state: UserState = initialize();

// getters for this store
const getters = {
    /**
     * Gets users from store
     */
    users(state): UserViewModule[] {
        return state.users;
    }
};

// mutations for this store
const mutations = {
    /**
     * Update the entire list of users from store
     */
    _users(state, payload: UserViewModule[]): void {
        state.users = payload;
    },
    /**
     * Add a new user into the store 
     */
    _addUser(state, payload: UserViewModule): void {
        state.users.push(payload);
    },
};

// actions for this store
const actions = {
    /**
     * Load all the users from the API and initialize the store
     */
    async loadUsers({ commit }): Promise<UserViewModule[]> {
        const users: UserViewModule[] = (await axios.get('user/users')).data;
        commit("_users", users);
        return users;
    },
    /**
     * Add a new user into the database and initialize the store
     */
    async addUser({ commit }, payload: CreateUserParameters): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: '',
            isSuccess: true
        };

        const result = await axios.post('user/create_user', payload).catch(error => {
            response = ResponseHandler.errorResponseHandler(error);
        });

        if (response.isSuccess) {
            commit("_addUser", (result as AxiosResponse).data);
        }
        return response;
    }
};

// export the namespace
export const namespace = 'user';

// export the store 'module'
export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};
