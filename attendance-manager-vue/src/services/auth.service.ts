/* eslint-disable @typescript-eslint/no-explicit-any */
import { ACCESS_TOKEN, REFRESH_TOKEN } from "@/shared/constants";
import { ResponseLogin, LoginParameters } from "@/shared/modules";
import axios from "axios";

export default class AuthService{
    static isLogged(): boolean{
        return (<any>window).$cookies.isKey(ACCESS_TOKEN);
    }

    static getDataFromToken(): any{
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

    static logout(): void {
        (<any>window).$cookies.remove(ACCESS_TOKEN);
        (<any>window).$cookies.remove(REFRESH_TOKEN);
    }

    static async login(user: LoginParameters): Promise<boolean> {
        const response = await axios.post('account/authenticate', {
            email: user.email,
            password: user.password
        }) as ResponseLogin;
        console.log(response);
        (<any>window).$cookies.set(ACCESS_TOKEN, response.token);
        (<any>window).$cookies.set(REFRESH_TOKEN, response.refreshToken);
        return true;
    }

}
