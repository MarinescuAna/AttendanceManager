import { CourseType } from "@/shared/enums";

export interface InvolvementViewModule{
    involvementId: number;
    collectionId: number;
    bonusPoints: number;
    student: string;
    activityType: CourseType;
    isPresent: boolean;
    updateOn:string;
    email: string;
}

export interface InvolvementUpdateViewModule{
    involvementId: number;
    bonusPoints: number;
    isPresent: boolean;
    userId:string;
    collectionId: number;
}

export interface InvolvementsUpdateViewModule{
    involvements: InvolvementUpdateViewModule[];
} 

export interface TotalInvolvementViewModule{
    userId: string;
    userName: string;
    code: string;
    bonusPoints: number;
    courseAttendances: number;
    laboratoryAttendances: number;
    seminaryAttendances: number;
}