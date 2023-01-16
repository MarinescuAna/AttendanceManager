import { TableModule } from "../shared";
import { SpecializationViewModule,SpecializationModule } from "../specialization";

/**
 * Use this module in order to create a new user's account
 */
 export interface CreateUserParameters {
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
/**
 * Use this module to get additional information about the user
 */
export interface UserInformationViewModule{
   departmentId: string;
   departmentName: string;
   specializations: SpecializationModule[];
}
export interface StudentForCourseViewModule{
   email: string;
   fullname: string;
}