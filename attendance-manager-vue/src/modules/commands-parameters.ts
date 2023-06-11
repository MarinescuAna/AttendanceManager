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