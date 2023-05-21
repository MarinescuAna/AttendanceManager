
import https from "@/plugins/axios";
import { ATTENDANCE_CONTROLLER } from "@/shared/constants";
import { StudentAttendanceInsertModule, StudentAttendanceModule, StudentAttendancesInsertModule } from "@/modules/document/attendance";
import ResponseHandler from "@/error-handler/error-handler";

interface UseAttendanceCodeUpdateModule{
    code: string;
    attendanceId: number;
    attendanceCollectionId: number;
}

export default class AttendanceService {

    static async getStudentsAttendancesByCollectionIdAsync(payload: number): Promise<StudentAttendanceModule[]> {
        return (await https.get(`${ATTENDANCE_CONTROLLER}/attendances_by_attendance_collection_id?attendanceCollectionId=${payload}`)).data;
    }

    /**
     * Update the students attendances (attendance or bonus points)
     * @param payload StudentAttendanceInsertModule
     * @returns true if the attendance is added or false if something occured
     */
    static async addStudentsAttendances(payload: StudentAttendancesInsertModule): Promise<boolean> {
        let isSuccess = true;

        await https.patch(`${ATTENDANCE_CONTROLLER}/update_student_involvement`,payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        return isSuccess;
    }

    /**
     * Update the students attendances by code and attendance id
     */
    static async updateAttendanceByCode(payload: UseAttendanceCodeUpdateModule): Promise<boolean> {
        let isSuccess = true;
        
        await https.patch(`${ATTENDANCE_CONTROLLER}/update_involvement_by_code`,payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        return isSuccess;
    }
}