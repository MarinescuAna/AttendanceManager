import { CourseType } from "@/shared/enums";

export interface InvolvementViewModule{
    involvementId: number;
    collectionId: number;
    bonusPoints: number;
    studentEmail: string;
    activityDate: string;
    activityType: CourseType;
}