/* eslint-disable @typescript-eslint/no-explicit-any */
import { ACCESS_TOKEN, REFRESH_TOKEN } from "@/shared/constants";
import { TokenData, ResponseModule } from "@/shared/modules";
import {LoginModule, LoginResponseModule} from "@/modules/user/auth";
import ResponseHandler from "@/error-handler/error-handler";
import axios, { AxiosResponse } from "axios";
import { Role } from "@/shared/enums";
import storeHelper from "@/store/store-helper";

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
        
        const token = this.getDataFromToken();
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
        (<any>window).$cookies.remove(ACCESS_TOKEN);
        (<any>window).$cookies.remove(REFRESH_TOKEN);
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

        const result = await axios.post('account/authenticate', {
            email: payload.email,
            password: payload.password
        }).catch(error => {
            response = ResponseHandler.errorResponseHandler(error);
        });

        if (response.isSuccess) {
            const apiResponse = (result as AxiosResponse).data as LoginResponseModule;
            (<any>window).$cookies.set(ACCESS_TOKEN, apiResponse.token);
            (<any>window).$cookies.set(REFRESH_TOKEN, apiResponse.refreshToken);
        }

        return response;
    }

}
