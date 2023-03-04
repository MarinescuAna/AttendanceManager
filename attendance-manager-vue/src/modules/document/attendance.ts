import { CourseType } from "@/shared/enums";

export interface AttendanceFileInsertModule{
    dateTime: string;
    type: CourseType;
    documentId: number;
}

export interface StudentAttendanceModule{
    attendanceID: number;
    userID: string;
    bonusPoints: number;
    isPresent: boolean;
    updateOn:string;
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