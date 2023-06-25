import { BadgeType, CourseType, NotificationPriority, Role } from "@/shared/enums";

/**
 * Use this to display a course
 */
export interface CourseViewModule {
    courseId:number;
    name: string;
    specializationId:number;
    specializationName:string;
    reportsLinked: number;
    updatedOn: string;
 }

 /**Used to display the badges percentage for teachers */
 export interface BadgePercentageViewModule{   
    badgeId: number;
    imagePath: string;
    badgeType: BadgeType;
    title: string;
    description: string;
    isCustom: boolean;
    percentage:number;
    studentsId: string[];
}

/**Used to display bagdes for students and teachers */
export interface BadgeViewModule{
    imagePath: string;
    type: BadgeType;
    title: string;
    rewardId: number;
    badgeType: BadgeType;
    activityType: CourseType;
    maxNumber: number;
    badgeId: number;
    isActive: boolean;
    description: string;
    isCustom: boolean;
}

/** This module is used in the document store to keep all the informations related to a document */
export interface ReportViewModule{
    reportId:number;
    title: string;
    courseId: number;
    courseName: string;
    specializationId: number;
    specializationName: string;
    enrollmentYear: number;
    maxNoLessons: number;
    maxNoLaboratories: number;
    maxNoSeminaries: number;
    noLessons: number;
    noLaboratories: number;
    noSeminaries: number;
    creationDate: string;
    updatedOn: string;
    createdBy: string;
    collections: CollectionDto[];
    members: MembersDto[];
    attendanceImportance: number;
    bonusPointsImportance: number;
    numberOfStudents: number;
    isCreator: boolean;
}
export interface MembersDto{
    email: string;
    name: string;
}
/**Use this interface to display collection in timeline */
export interface CollectionDto{
    collectionId: number;
    activityTime: string;
    courseType: string;
    title: string;
}

/**
 * Used for store and it contains all the departments
 * This module is used for v-select component, update and insert
 */
export interface DepartmentViewModule
{
    id: number
    name: string;
    updatedOn: string;
    linkedSpecializations: number;
}

/** Use this view in order to get the documents for the cards */
export interface ReportCardViewModule{
    title: string;
    courseName: string;
    specializationName: string;
    enrollmentYear: number;
    documentId: number;
    isCreator: boolean;
    updatedOn: string;
}

export interface InvolvementViewModule {
    involvementId: number;
    collectionId: number;
    bonusPoints: number;
    student: string;
    activityType: CourseType;
    isPresent: boolean;
    updateOn: string;
    email: string;
    updateBy: string;
    title: string;
    heldOn: string;
}

export interface TotalInvolvementViewModule {
    userId: string;
    userName: string;
    code: string;
    bonusPoints: number;
    courseAttendances: number;
    laboratoryAttendances: number;
    seminaryAttendances: number;
}

export interface NotificationViewModel{
    notificationId: number;
    message: string;
    createdOn: string;
    image: string;
    isRead: boolean;
    priority: NotificationPriority;
}

/**
 * Used for store and it contains all the specializations
 */
export interface SpecializationViewModule{
    id: number;
    name: string;
    departmentId: number;
    usersLinked: number;
    updatedOn: string;
}

/**
 * Use this module to get additional information about the user
 */
export interface UserInformationViewModule{
    departmentId: number;
    departmentName: string;
    specializations: SpecializationViewModule[];
 }

 /**
 * Use this module in order to display the users for admin
 */
export interface UserViewModule {
    id: string;
    accountConfirmed: boolean;
    departmentId: number;
    departmentName: string;
    role: string;
    fullname: string;
    year: number;
    code: string;
    specializationIds: number[];
    created: string;
    updated: string;
 }

 /**Used when create the report */
 export interface StudentForCourseViewModule{
    email: string;
    fullname: string;
 }

 /**Used for big dashboard */
 export interface CourseDashboardViewModule{
    courseId: number;
    courseName: string;
    reports: ReportDashboardDto[];
 }
 export interface ReportDashboardDto{
    reportId: number;
    reportName: string;
    percentageAttendances: number[];
    noAttendancesAchieved: number[];
    noPointsAchieved: number[];
    creationYear: number;
    badges: BadgeDashboardDto[];
 }
 export interface BadgeDashboardDto{
    title: string;
    description: string;
    noAchievements: number;
    percentageAchievements: number;
    role: Role;
    imagePath: string;
    badgeType: number;
    maxAchievements: number;
 }