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