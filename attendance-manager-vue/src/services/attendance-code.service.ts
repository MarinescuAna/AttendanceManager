import ResponseHandler from "@/error-handler/error-handler";
import https from "@/plugins/axios";
import { ATTENDANCE_CODE } from "@/shared/constants";
import { AxiosResponse } from "axios";
import {AttendanceCodeInsertModule} from "@/modules/document/attendance-code";

export default class AttendanceCodeService {

    /**
     * Generate an attendance code
     * @param payload number of minutes
     * @returns undefinde if some errors occured or the new generated code and its expiration date
     */
    static async createAttendanceCode(minutes: number, attendanceCollectionId: number): Promise<undefined | AttendanceCodeInsertModule> {
        let isSuccess = true;

        const result = await https.post(`${ATTENDANCE_CODE}/create_attendance_code?minutes=${minutes}&attendanceCollectionId=${attendanceCollectionId}`)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });
            
        if (isSuccess) {
            return (result as AxiosResponse).data;
        }

        return undefined;
    }
}