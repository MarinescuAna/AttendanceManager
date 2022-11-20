/**
 * Use this module to send the parameters used for login
 */
export interface LoginParameters {
    email: string;
    password: string;
}

/**
 * Use this module to receive the date from the api when the login is done
 */
export interface ResponseLogin {
    token: string;
    refreshToken: string;
}