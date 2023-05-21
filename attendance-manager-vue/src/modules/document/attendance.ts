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
    userId:string;
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
    reportId: number;
    involvements: StudentAttendanceInsertModule[];
}

export interface StudentAttendanceModule{
    bonusPoints: number;
    courseType: string;
    isPresent: boolean;
    userId: string;
    attendanceId: number;
    updatedOn:string;
}