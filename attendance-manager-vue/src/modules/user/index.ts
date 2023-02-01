import { TableModule } from "../shared";
import { SpecializationViewModule,SpecializationModule } from "../specialization";

/**
 * Use this module in order to create a new user's account
 */
 export interface CreateUserParameters {
    email: string;
    role: string;
    fullname: string;
    year: number;
    code: string;
    specializationIds: number[];
}
/**
 * Use this module in order to display the users for admin
 */
export interface UserViewModule extends TableModule {
   fullname: string;
   role:string;
   enrollmentYear: number;
   code: string;
   accountConfirmed: boolean;
   updated: string;
   created: string;
   departmentId: number;
   departmentName: string;
   userSpecializations: SpecializationViewModule[];
}
/**
 * Use this module to get additional information about the user
 */
export interface UserInformationViewModule{
   departmentId: number;
   departmentName: string;
   specializations: SpecializationModule[];
}
export interface StudentForCourseViewModule{
   email: string;
   fullname: string;
}