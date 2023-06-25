/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { InsertMultipleUsersParameters, InsertUserParameters } from "@/modules/commands-parameters";
import { DepartmentViewModule, UserInformationViewModule, UserViewModule } from "@/modules/view-modules";
import https from "@/plugins/axios";
import { Toastification } from "@/plugins/vue-toastification";
import { USER_CONTROLLER } from "@/shared/constants";
import { Role } from "@/shared/enums";
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
        Object.assign(state, initialize());
    }
};

// actions for this store
const actions = {
    /**
     * Load all the users from the API and initialize the store
     */
    async loadUsersAsync({ commit, state }): Promise<void> {
        if (state.users.length == 0) {
            const users: UserViewModule[] = (await https.get(`${USER_CONTROLLER}/users`)).data;
            commit("_users", users);
        }
    },
    /**
     * Load current user information
     * @todo remove the id
     */
    async loadCurrentUserInfoAsync({ commit, state }): Promise<void> {
        if (state.currentUser != null) {
            return state.currentUser;
        }

        const result: UserInformationViewModule = (await https.get(`${USER_CONTROLLER}/current_user_info`)).data;
        commit("_addCurrentUser", result);
    },
    /**
     * Add a new user into the database and initialize the store
     */
    async addUserAsync({ commit }, payload: {newUser: InsertUserParameters, department: DepartmentViewModule}): Promise<boolean> {
        let isSuccess = true;

        await https.post(`${USER_CONTROLLER}/create_user`, payload.newUser).catch(error => {
            isSuccess = ResponseHandler.errorResponseHandler(error);
        });

        if (isSuccess) {
            commit("_addUser", {
                departmentId: payload.department.id,
                departmentName: payload.department.name,
                accountConfirmed: false,
                code: payload.newUser.code,
                year: payload.newUser.year,
                fullname: payload.newUser.fullname,
                id: payload.newUser.email,
                role: Role[payload.newUser.role],
                specializationIds: payload.newUser.specializationIds
            } as UserViewModule);
        }
        return isSuccess;
    },
    async addMultipleUsersAsync({ commit }, payload: {users: InsertMultipleUsersParameters, department: DepartmentViewModule}): Promise<boolean> {
        let isSuccess = true;

        const result = await https.post(`${USER_CONTROLLER}/create_users`, payload.users).catch(error => {
            isSuccess = ResponseHandler.errorResponseHandler(error);
        });

        if (isSuccess) {
            payload.users.users.forEach(user =>{
                commit("_addUser", {
                    departmentId: payload.department.id,
                    departmentName: payload.department.name,
                    accountConfirmed: false,
                    code: user.code,
                    year: user.year,
                    fullname: user.fullname,
                    id: user.email,
                    role: Role[user.role],
                    specializationIds: user.specializationIds
                } as UserViewModule);
            });

            if((result as AxiosResponse).data!=0){
                Toastification.info("Not all the user received the email, but they were inserted into the system, successfully!");
            }
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
export const namespace = 'user';

// export the store 'module'
export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};
