/* eslint-disable @typescript-eslint/no-explicit-any */
import { ACCESS_TOKEN, ACCOUNT_CONTROLLER, EXP_ACCESS_TOKEN, EXP_REFRESH_TOKEN, REFRESH_TOKEN } from "@/shared/constants";
import { TokenData, ResponseModule } from "@/shared/modules";
import { LoginModule, LoginResponseModule, TokenModule } from "@/modules/user/auth";
import ResponseHandler from "@/error-handler/error-handler";
import { AxiosResponse } from "axios";
import storeHelper from "@/store/store-helper";
import https from "@/plugins/axios";
import jwt_decode from "jwt-decode";

export default class AuthService {
    /**
     * Check if the access token was set
     * @returns true if the access token exists in the cookies
     */
    static isLogged(): boolean {
        return (<any>window).$cookies.isKey(ACCESS_TOKEN);
    }

    /**
     * Get expire date of access token
     * @returns expire date of access token
     */
    static get expAccessToken(): Date {
        return new Date(Date.parse((<any>window).$cookies.get(EXP_ACCESS_TOKEN)));
    }

    /**
     * Get expire date of refresh token
     * @returns expire date of refresh token
     */
    static get expRefreshToken(): Date {
        return new Date(Date.parse((<any>window).$cookies.get(EXP_REFRESH_TOKEN)));
    }

    /**
     * Get access token
     * @returns access token
     */
    static get accessToken(): string {
        return (<any>window).$cookies.get(ACCESS_TOKEN);
    }

    /**
     * Get refresh token
     * @returns refresh token
     */
    static get refreshToken(): string {
        return (<any>window).$cookies.get(REFRESH_TOKEN);
    }

    /**
     * Set access token and expiration date
     */
    static set setAccessToken(token: TokenModule) {
        (<any>window).$cookies.set(ACCESS_TOKEN, token.token);
        (<any>window).$cookies.set(EXP_ACCESS_TOKEN, token.expiration);
    }

    /**
     * Decode the token and get the data
     * @returns data about the user
     */
    static getDataFromToken(): TokenData {
        const token =  (<any>window).$cookies.get(ACCESS_TOKEN);

        if(typeof(token) === 'undefined' || token === null){
            return null!;
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
                    storeHelper.documentStore.reset();
                    break;
                default:
                    break;
            }
            storeHelper.userStore.reset();
        }
        (<any>window).$cookies.remove(ACCESS_TOKEN);
        (<any>window).$cookies.remove(REFRESH_TOKEN);
        (<any>window).$cookies.remove(EXP_ACCESS_TOKEN);
        (<any>window).$cookies.remove(EXP_REFRESH_TOKEN);
    }

    /**
     * Do loging
     * @param payload email and password
     * @returns true if the logging was successfully done, or false if something happen
     */
    static async login(payload: LoginModule): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: '',
            isSuccess: true
        };

        const result = await https.post(`${ACCOUNT_CONTROLLER}/authenticate`, {
            email: payload.email,
            password: payload.password
        }).catch(error => {
            response = ResponseHandler.errorResponseHandler(error);
        });

        if (response.isSuccess) {
            const apiResponse = (result as AxiosResponse).data as LoginResponseModule;
            (<any>window).$cookies.set(ACCESS_TOKEN, apiResponse.accessToken);
            (<any>window).$cookies.set(REFRESH_TOKEN, apiResponse.refreshToken);
            (<any>window).$cookies.set(EXP_ACCESS_TOKEN, apiResponse.expirationDateAccessToken);
            (<any>window).$cookies.set(EXP_REFRESH_TOKEN, apiResponse.expirationDateRefreshToken);
        }

        return response;
    }

}
