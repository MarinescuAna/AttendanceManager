
import https from "@/plugins/axios";
import { ATTENDANCE_CONTROLLER } from "@/shared/constants";
import { StudentAttendanceInsertModule, StudentAttendanceModule, TotalAttendanceModule } from "@/modules/document/attendance";
import ResponseHandler from "@/error-handler/error-handler";

export default class AttendanceService {

    static async getStudentsAttendancesByCollectionIdAsync(payload: number): Promise<StudentAttendanceModule[]> {
        return (await https.get(`${ATTENDANCE_CONTROLLER}/attendances_by_attendance_collection_id?attendanceCollectionId=${payload}`)).data;
    }

    /**
     * Update the students attendances (attendance or bonus points)
     * @param payload StudentAttendanceInsertModule
     * @returns true if the attendance is added or false if something occured
     */
    static async addStudentsAttendances(payload: StudentAttendanceInsertModule[]): Promise<boolean> {
        let isSuccess = true;

        await https.patch(`${ATTENDANCE_CONTROLLER}/update_attendances`, { students: payload })
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        return isSuccess;
    }

    
    static async getTotalAttendancesByDocumentId(payload: number): Promise<TotalAttendanceModule[]> {
        return (await https.get(`${ATTENDANCE_CONTROLLER}/total_attendances_by_document_id?documentId=${payload}`)).data;
    }

    static async getStudentAttendancesByDocumentIdAndUserId(payload1: number,payload2:string): Promise<StudentAttendanceModule[]> {
        return (await https.get(`${ATTENDANCE_CONTROLLER}/student_attendances_by_document_id_and_user_id?documentId=${payload1}&userId=${payload2}`)).data;
    }
}