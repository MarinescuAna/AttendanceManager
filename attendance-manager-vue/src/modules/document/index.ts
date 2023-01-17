export interface DocumentInsertModule{
    title: string;
    courseId: string;
    specializationId: string;
    enrollmentYear: string;
    maxNoLessons: string;
    maxNoLaboratories: string;
    maxNoSeminaries: string;
    studentIds: string[];
    email: string;
}
export interface DocumentViewModule{
    title: string;
    courseName: string;
    specializationName: string;
    enrollmentYear: string;
    documentId: string;
}
export interface DocumentFullViewModule{
    documentId:string;
    title: string;
    courseId: string;
    courseName: string;
    specializationId: string;
    specializationName: string;
    enrollmentYear: string;
    maxNoLessons: string;
    maxNoLaboratories: string;
    maxNoSeminaries: string;
    creationDate: string;
    updateDate: string;
}