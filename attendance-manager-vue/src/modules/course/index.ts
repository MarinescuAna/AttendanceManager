import { TableModule } from "../shared";

/**
 * Use this module in order to display a course
 */
export interface CourseViewModule extends TableModule {
    name: string;
    specializationId:number;
    specializationName:string;
 }
 /**
 * Used for saving a new course
 */
export interface CreateCourseModule {
    name: string;
    specializationId: number;
    specializationName: string;
}
 /**
 * Used for updating the course name
 */
 export interface UpdateCourseModule {
    name: string;
    courseId: number;
}