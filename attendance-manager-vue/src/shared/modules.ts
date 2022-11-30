import { Role } from "./enums";

/**
 * Used to filter the response from the API and send the response to components
 */
export interface ResponseModule{
    error: string;
    isSuccess: boolean;
}
/**
 * Token date after decoding
 */
 export interface TokenData{
    email:string;
    role: Role;
    name: string;
    code: string;
}










