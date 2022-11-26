import { Role } from "./enums";
//TODO refactor

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

/**
 * 
 */
export interface CreateUserParameters{
    email: string;
    code: string;
    role: Role;
    fullname: string;
}

/**
 * /**
 * This module is used for selector components to map the departments
 */
export interface DepartmentModule{
    id: string;
    name: string;
}

/**
 * This module is used for selector components to map the specialziations
 */
 export interface SpecializationModule{
    id: string;
    name: string;
    departmentId:string;
}