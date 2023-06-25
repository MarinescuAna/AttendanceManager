import { CourseType } from "@/shared/enums";

export interface CreateBadgeParameters {
    maxNumber: number;
    title: string;
    badgeType: string;
    type: CourseType;
}
/**
* Used for saving a new course
*/
export interface CreateCourseParameters {
    name: string;
    specializationId: number;
    specializationName: string;
}

/**
* Used for updating the course
*/
export interface UpdateCourseParameters {
    name: string;
    courseId: number;
    specializationId: number;
}

/**Used to update involvements */
export interface UpdateInvolvementDto {
    involvementId: number;
    bonusPoints: number;
    isPresent: boolean;
    userId: string;
    collectionId: number;
}

export interface UpdateInvolvementsParameters {
    involvements: UpdateInvolvementDto[];
}

export interface UpdateInvolvementByCodeParameters {
    code: string;
    attendanceId: number;
    collectionId: number;
}

/**Used to insert department */
export interface InsertDepartmentParameters {
    name: string;
}

/**Used to update department */
export interface UpdateDepartmentParameters {
    departmentId: number;
    name: string;
}

/**Used to insert collaborator */
export interface InsertCollaboratorParameters {
    email: string;
}

/** Use this to update the document*/
export interface UpdateReportParameters {
    title: string;
    courseId: number;
    noLessons: number;
    noLaboratories: number;
    noSeminaries: number;
    attendanceImportance: number;
    bonusPointsImportance: number;
    documentId: number;
}

/**Used to create a new involvement code */
export interface InsertInvolvementCodeParameters {
    minutes: number;
    collectionId: number;
}

/**Create specialization */
export interface InsertSpecializationParameters {
    departmentId: number;
    name: string;
}

/** Use this view only for sending the collected data from the form used for creating a new document, to the API */
export interface InsertReportParameters {
    title: string;
    courseId: number;
    specializationId: number;
    enrollmentYear: number;
    maxNoLessons: number;
    maxNoLaboratories: number;
    maxNoSeminaries: number;
    studentIds: string[];
    attendanceImportance: number;
    bonusPointsImportance: number;
}

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
export interface LoginResponse {
    accessToken: string;
    refreshToken: string;
    expirationDateAccessToken: string;
    expirationDateRefreshToken: string;
}

/**
 * Use this module in order to create a new user's account
 */
export interface InsertUserParameters {
    email: string;
    role: string;
    fullname: string;
    year: number;
    code: string;
    specializationIds: number[];
 }

export interface InsertMultipleUsersParameters{
    users: InsertUserParameters[];
}

/**Used to create a new collection */
export interface InsertCollectionParameters{
    activityDateTime: string;
    courseType: string;
    title: string;
}

/**Used to updaet a collection */
export interface UpdateCollectionParameters{
    activityDateTime: string;
    courseType: string;
    title: string;
    collectionId: number;
}