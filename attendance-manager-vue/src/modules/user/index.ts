import { SpecializationViewModule } from "../organization/specializations";
import { TableModule } from "../shared";

/**
 * Use this module in order to create a new user's account
 */
 export interface CreateUserParameters{
    email: string;
    role: string;
    fullname: string;
    year: string;
    code: string;
    specializations: string[];
}
/**
 * Use this module in order to display the users for admin
 */
export interface UserViewModule extends TableModule {
   fullname: string;
   email: string;
   role:string;
   enrollmentYear: string;
   code: string;
   accountConfirmed: boolean;
   updated: string;
   created: string;
   departmentId: string;
   departmentName: string;
   userSpecializations: SpecializationViewModule[];
}