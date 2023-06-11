/**
 * Use this to display a course
 */
export interface CourseViewModule {
    courseId:number;
    name: string;
    specializationId:number;
    specializationName:string;
    reportsLinked: number;
 }