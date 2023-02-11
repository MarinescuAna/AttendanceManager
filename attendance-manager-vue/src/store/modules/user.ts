/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { CreateUserParameters, UserInformationViewModule, UserViewModule } from "@/modules/user";
import https from "@/plugins/axios";
import { Logger } from "@/plugins/custom-plugins/logging";
import { USER_CONTROLLER } from "@/shared/constants";
import { ResponseModule } from "@/shared/modules";
import { AxiosResponse } from "axios";


//state type
export interface UserState {
    users: UserViewModule[],
    currentUser: UserInformationViewModule;
}

//initialize the state with an empty array
export function initialize(): UserState {
    return {
        users: [],
        currentUser: null!
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
    },
    /**
     * Get current user from store
     */
    currentUser(state): UserInformationViewModule {
        return state.currentUser;
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
    /**
     * Use this to update the additional information about the current user
     */
    _addCurrentUser(state, payload: UserInformationViewModule): void {
        state.currentUser = payload;
    },
    /**
     * Reset the state with the initial values
     */
    _resetStore(state): void {
        Logger.logInfo('Reset the User store to the initial state')
        Object.assign(state, initialize());
    }
};

// actions for this store
const actions = {
    /**
     * Load all the users from the API and initialize the store
     */
    async loadUsers({ commit, state }): Promise<void> {
        if (state.users.length == 0) {
            const users: UserViewModule[] = (await https.get(`${USER_CONTROLLER}/users`)).data;
            commit("_users", users);
        }
    },
    /**
     * Load current user information
     * @todo remove the id
     */
    async loadCurrentUserInfo({ commit, state }): Promise<void> {
        if (state.currentUser != null) {
            return state.currentUser;
        }

        const result: UserInformationViewModule = (await https.get(`${USER_CONTROLLER}/current_user_info`)).data;
        commit("_addCurrentUser", result);
    },
    /**
     * Add a new user into the database and initialize the store
     */
    async addUser({ commit }, payload: CreateUserParameters): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: '',
            isSuccess: true
        };

        const result = await https.post(`${USER_CONTROLLER}/create_user`, payload).catch(error => {
            response = ResponseHandler.errorResponseHandler(error);
        });

        if (response.isSuccess) {
            commit("_addUser", (result as AxiosResponse).data);
        }
        return response;
    },
    /**
     * Reset the state with the initial values
     */
    resetStore({ commit }): void {
        commit('_resetStore');
    },
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
