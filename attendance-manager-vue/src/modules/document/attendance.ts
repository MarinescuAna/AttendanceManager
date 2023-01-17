import { CourseType } from "@/shared/enums";

export interface AttendanceFileInsertModule{
    dateTime: string;
    type: CourseType;
    documentId: string;
}