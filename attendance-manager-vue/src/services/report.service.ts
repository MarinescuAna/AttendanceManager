import ResponseHandler from "@/error-handler/error-handler";
import { InsertReportParameters } from "@/modules/commands-parameters";
import { ReportCardViewModule } from "@/modules/view-modules";
import https from "@/plugins/axios";
import { REPORT_CONTROLLER } from "@/shared/constants";

export default class ReportService {
    static async getReportsAsync(): Promise<ReportCardViewModule[]> {
        return (await https.get(`${REPORT_CONTROLLER}/reports`)).data;
    }

    static async addReportAsync(parameters: InsertReportParameters): Promise<boolean> {
        let isSuccess = true;
        await https.post(`${REPORT_CONTROLLER}/create_report`, parameters)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });
        return isSuccess;
    }
}