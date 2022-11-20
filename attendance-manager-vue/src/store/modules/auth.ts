/* eslint-disable @typescript-eslint/no-explicit-any */
import { ACCESS_TOKEN, REFRESH_TOKEN } from "@/shared/constants";
import { Role } from "@/shared/enums";
import axios from "axios";

//state type
export interface AuthState {
    token: string;
    refreshToken: string;
    tokenExpiration: string;
    email: string;
    fullname: string;
    code: string;
    role: Role;
}

export function initialize(): AuthState {
    return {
        code: '',
        email: '',
        fullname: '',
        refreshToken: '',
        role: Role.NoRole,
        token: '',
        tokenExpiration: ''
    };
}

// initial state
const state: AuthState = initialize();

// getters for this store
const getters = {
    /**
 * Gets current user
 * @param state
 */
    getUserName(state): string {
        return state.fullname;
    },
    /**
* Gets current user
* @param state
*/
    getUserRole(state): Role {
        return state.role;
    },
};

// mutations for this store
const mutations = {
    setAuthData(state, response: ResponseLogin): void {
        (<any>window).$cookies.set(ACCESS_TOKEN, response.token);
        (<any>window).$cookies.set(REFRESH_TOKEN, response.refreshToken);

        const jwtDecodedValue = jwtDecrypt(response.token);

        state.code = jwtDecodedValue.code;
        state.email = jwtDecodedValue.email;
        state.fullname = jwtDecodedValue.name;
        state.token = response.token;
        state.refreshToken = response.refreshToken;
        state.tokenExpiration = jwtDecodedValue.exp;
    },
    removeData(state): void {
        (<any>window).$cookies.remove(ACCESS_TOKEN);
        (<any>window).$cookies.remove(REFRESH_TOKEN);
        state = initialize();
    }
};

// actions for this store
const actions = {
    async login({ commit }, user: LoginParameters): Promise<boolean> {
        const response = await axios.post('account/authenticate', {
            email: user.email,
            password: user.password
        });
        console.log(response);

        commit("setAuthData", response.data);
        return true;
    },

    logout({ commit }): void {
        commit("removeData");
    }
};

export interface LoginParameters {
    email: string;
    password: string;
}

interface ResponseLogin {
    token: string;
    refreshToken: string;
}

function jwtDecrypt(token) {
    const base64Url = token.split(".")[1];
    const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
    const jsonPayload = decodeURIComponent(
        atob(base64)
            .split("")
            .map(function (c) {
                return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
            })
            .join("")
    );

    return JSON.parse(jsonPayload);
}

// export the namespace
export const namespace = 'auth';

// export the store 'module'
export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};
