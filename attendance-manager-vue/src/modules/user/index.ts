/**
 * Use this model in order to create a new user's account
 */
 export interface CreateUserParameters{
    email: string;
    role: string;
    fullname: string;
    year: string;
    code: string;
}