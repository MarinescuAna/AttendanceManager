export interface LoginParameters {
    email: string;
    password: string;
}

export interface ResponseLogin {
    token: string;
    refreshToken: string;
}