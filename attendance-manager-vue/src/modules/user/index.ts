import { Role } from "@/shared/enums";

/**
 * 
 */
 export interface CreateUserParameters{
    email: string;
    code: string;
    role: Role;
    fullname: string;
}