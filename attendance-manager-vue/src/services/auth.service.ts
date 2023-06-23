/* eslint-disable @typescript-eslint/no-explicit-any */
import { ACCESS_TOKEN, ACCOUNT_CONTROLLER, EXP_ACCESS_TOKEN, EXP_REFRESH_TOKEN, REFRESH_TOKEN } from "@/shared/constants";
import { TokenData, TokenModule } from "@/shared/modules";
import ResponseHandler from "@/error-handler/error-handler";
import { AxiosResponse } from "axios";
import storeHelper from "@/store/store-helper";
import https from "@/plugins/axios";
import jwt_decode from "jwt-decode";
import Vue from 'vue';
import { LoginParameters, LoginResponse } from "@/modules/commands-parameters";

export default class AuthService {
    /**
     * Check if the access token was set
     * @returns true if the access token exists in the cookies
     */
    static isLogged(): boolean {
        return Vue.$cookies.isKey(ACCESS_TOKEN);
    }

    /**
     * Get expire date of access token
     * @returns expire date of access token
     */
    static get expAccessToken(): Date {
        return new Date(Date.parse(Vue.$cookies.get(EXP_ACCESS_TOKEN)));
    }

    /**
     * Get expire date of refresh token
     * @returns expire date of refresh token
     */
    static get expRefreshToken(): Date {
        return new Date(Date.parse(Vue.$cookies.get(EXP_REFRESH_TOKEN)));
    }

    /**
     * Get access token
     * @returns access token
     */
    static get accessToken(): string {
        return Vue.$cookies.get(ACCESS_TOKEN);
    }

    /**
     * Get refresh token
     * @returns refresh token
     */
    static get refreshToken(): string {
        return Vue.$cookies.get(REFRESH_TOKEN);
    }

    /**
     * Set access token and expiration date
     */
    static set setAccessToken(token: TokenModule) {
        Vue.$cookies.set(ACCESS_TOKEN, token.token);
        Vue.$cookies.set(EXP_ACCESS_TOKEN, token.expiration);
    }

    /**
     * Decode the token and get the data
     * @returns data about the user
     */
    static getDataFromToken(): TokenData|null {
        const token =  Vue.$cookies.get(ACCESS_TOKEN);

        if(typeof(token) === 'undefined' || token === null){
            return null;
        }        

        return jwt_decode(token) as TokenData;
    }

    /**
     * Remove the tokens from the cookies
     */
    static logout(): void {

        const token = this.getDataFromToken();
        if (token !== null) {
            switch (token.role.toString()) {
                case "Admin":
                    storeHelper.departmentStore.reset();
                    storeHelper.specializationStore.reset();
                    break;
                case "Teacher":
                    storeHelper.courseStore.reset();
                    storeHelper.documentStore.resetEntireStore();
                    break;
                case "Student":
                    storeHelper.documentStore.resetEntireStore();
                    break;
            }
            storeHelper.userStore.reset();
        }
        
        Vue.$cookies.remove(ACCESS_TOKEN);
        Vue.$cookies.remove(REFRESH_TOKEN);
        Vue.$cookies.remove(EXP_ACCESS_TOKEN);
        Vue.$cookies.remove(EXP_REFRESH_TOKEN);
    }

    /**
     * Do loging - update the local storage after receive the tokens
     * @param payload email and password
     * @returns true if the logging was successfully done, or false if something happen
     */
    static async loginAsync(payload: LoginParameters): Promise<boolean> {
        let isSuccess = true;

        const result = await https.post(`${ACCOUNT_CONTROLLER}/authenticate`, payload).catch(error => {
            isSuccess = ResponseHandler.errorResponseHandler(error);
        });

        if (isSuccess) {
            const apiResponse = (result as AxiosResponse).data as LoginResponse;
            Vue.$cookies.set(ACCESS_TOKEN, apiResponse.accessToken);
            Vue.$cookies.set(REFRESH_TOKEN, apiResponse.refreshToken);
            Vue.$cookies.set(EXP_ACCESS_TOKEN, apiResponse.expirationDateAccessToken);
            Vue.$cookies.set(EXP_REFRESH_TOKEN, apiResponse.expirationDateRefreshToken);
        }

        return isSuccess;
    }

}
