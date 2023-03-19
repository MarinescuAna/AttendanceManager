
import { AttendanceCollectionViewModule } from "@/modules/document/attendance-collection";
import { StudentAttendanceModule, TotalAttendanceModule } from "./attendance";

export interface DocumentInsertModule{
    title: string;
    courseId: number;
    specializationId: number;
    enrollmentYear: number;
    maxNoLessons: number;
    maxNoLaboratories: number;
    maxNoSeminaries: number;
    studentIds: string[];
}
export interface DocumentViewModule{
    title: string;
    courseName: string;
    specializationName: string;
    enrollmentYear: number;
    documentId: number;
}

export interface DocumentMembersViewModule{
    email: string;
    role: string;
    name: string;
}

export interface DocumentFullViewModule{
    documentId:number;
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
    updateDate: string;
    createdBy: string;
    attendanceCollections: AttendanceCollectionViewModule[];
    documentMembers: DocumentMembersViewModule[];
    // currentStudentAttendances for current user, but only if the user is teacher
    currentStudentAttendances: StudentAttendanceModule[];
    // this contains the total attendance for each student (be aware of user's role)
    totalAttendances: TotalAttendanceModule[];
}