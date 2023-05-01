import { CourseType } from "@/shared/enums";

export interface AttendanceFileInsertModule{
    dateTime: string;
    type: CourseType;
    documentId: number;
}

export interface StudentAttendanceInsertModule{
    attendanceID: number;
    bonusPoints: number;
    isPresent: boolean;
}

export interface TotalAttendanceModule{
    userID: string;
    userName: string;
    code: string;
    bonusPoints: number;
    courseAttendances: number;
    laboratoryAttendances: number;
    seminaryAttendances: number;
}

export interface StudentAttendancesInsertModule{
    students: StudentAttendanceInsertModule[];
}

export interface StudentAttendanceModule{
    bonusPoints: number;
    courseType: string;
    isPresent: boolean;
    userId: string;
    attendanceId: number;
    updatedOn:string;
}

export interface UseAttendanceCodeUpdateModule{
    attendanceCode: string;
    attendanceId: number;
    attendanceCollectionId: number;
}