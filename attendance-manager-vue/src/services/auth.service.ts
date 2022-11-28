/* eslint-disable @typescript-eslint/no-explicit-any */
import { ACCESS_TOKEN, REFRESH_TOKEN } from "@/shared/constants";
import { ResponseLogin, LoginParameters, TokenData } from "@/shared/modules";
import ResponseHandler, { Response } from "@/error-handler/error-handler";
import axios from "axios";

export default class AuthService {
    /**
     * Check if the access token was set
     * @returns true if the access token exists in the cookies
     */
    static isLogged(): boolean {
        return (<any>window).$cookies.isKey(ACCESS_TOKEN);
    }

    /**
     * Get access token
     * @returns access token
     */
    static getAccessToken(): string {
        return (<any>window).$cookies.get(ACCESS_TOKEN);
    }

    /**
     * Decode the token and get the data
     * @returns data about the user
     */
    static getDataFromToken(): TokenData {
        const base64Url = (<any>window).$cookies.get(ACCESS_TOKEN).split(".")[1];
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

    /**
     * Remove the tokens from the cookies
     */
    static logout(): void {
        (<any>window).$cookies.remove(ACCESS_TOKEN);
        (<any>window).$cookies.remove(REFRESH_TOKEN);
    }

    /**
     * Do loging
     * @param user email and password
     * @returns true if the logging was successfully done, or false if something happen
     */
    static async login(user: LoginParameters): Promise<boolean> {
        let success = false;
        await axios.post('account/authenticate', {
            email: user.email,
            password: user.password
        }).then(response => {
            const result = response.data as unknown as ResponseLogin;
            (<any>window).$cookies.set(ACCESS_TOKEN, result.token);
            (<any>window).$cookies.set(REFRESH_TOKEN, result.refreshToken);
            success = true;
        }).catch(error => {
            const result = error as unknown as Response;
            ResponseHandler.errorResponseHandler(result)
            success = false;
        });
        return Promise.resolve(success);
    }

}
