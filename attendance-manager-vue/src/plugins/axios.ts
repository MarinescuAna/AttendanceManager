import router from "@/router";
import AuthService from "@/services/auth.service";
import { ACCOUNT_CONTROLLER } from "@/shared/constants";
import axios from "axios";

const https = axios.create({
    baseURL: "https://localhost:44325/api/",
    headers: {
        "Content-Type": "application/json",
        "Authorization": ""
    },
});

https.interceptors.request.use(
    async (config) => {
        // don't attach the token to request made to account controller
        if (!config.url?.includes(ACCOUNT_CONTROLLER)) {

            // check tokens
            await checkAccessToken();

            // attach the access token to the request
            config.headers!['Authorization'] = `Bearer ${AuthService.accessToken}`;

        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

/**
 * 1. get the current access and refresh token and check if they exists (if not, redirect the user to login page)
 * 2. check if the access token has expired
 *      a. check refresh token
 *      b. generate the new access token and update the one that already exists
 */
async function checkAccessToken(): Promise<void> {

    // force user to login again in case that the refresh token is not present
    if (!AuthService.accessToken || typeof (AuthService.refreshToken) === "undefined") {
        AuthService.logout();
        router.push({ name: "login" });
    }

    // check if the access token has expired
    if ((new Date()).getTime() >= AuthService.expAccessToken.getTime()) {
        // check refresh token
        await checkRefreshToken();

        // get the new access token
        const response = await https.post(`${ACCOUNT_CONTROLLER}/refresh-access-token?refreshToken=${AuthService.refreshToken}`);

        // update the new access token
        if (typeof (response.data) !== 'undefined') {
            AuthService.setAccessToken = response.data;
        }
    } else {
        // check refresh token
        await checkRefreshToken();
    }
}

/**
 *  Check if the refresh token is not expired (if yes, redirect the user to login)
  */
async function checkRefreshToken(): Promise<void> {
    // The refresh has expired
    if ((new Date()).getTime() >= AuthService.expRefreshToken.getTime()) {
        AuthService.logout();
        router.push({ name: 'login' });
    }
}

export default https;