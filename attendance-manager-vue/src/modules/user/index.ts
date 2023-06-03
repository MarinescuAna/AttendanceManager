import { SpecializationViewModule } from "../specialization";

/**
 * Use this module in order to create a new user's account
 */
 export interface CreateUserParameters extends UserBaseModule {
   email: string;
}

/**
 * Use this module in order to display the users for admin
 */
export interface UserViewModule extends UserBaseModule {
   id: string;
   accountConfirmed: boolean;
   departmentId: number;
   departmentName: string;
}

interface UserBaseModule{
   role: string;
   fullname: string;
   year: number;
   code: string;
   specializationIds: number[];
}
/**
 * Use this module to get additional information about the user
 */
export interface UserInformationViewModule{
   departmentId: number;
   departmentName: string;
   specializations: SpecializationViewModule[];
}
export interface StudentForCourseViewModule{
   email: string;
   fullname: string;
}