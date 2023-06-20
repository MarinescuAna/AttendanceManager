import ResponseHandler from "@/error-handler/error-handler";
import { InsertReportParameters } from "@/modules/commands-parameters";
import { ReportCardViewModule } from "@/modules/view-modules";
import https from "@/plugins/axios";
import { DOCUMENT_CONTROLLER } from "@/shared/constants";

export default class ReportService {
    static async getReports(): Promise<ReportCardViewModule[]> {
        return (await https.get(`${DOCUMENT_CONTROLLER}/reports`)).data;
    }

    static async addReport(parameters: InsertReportParameters): Promise<boolean> {
        let isSuccess = true;
        await https.post(`${DOCUMENT_CONTROLLER}/create_report`, parameters)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });
        return isSuccess;
    }
}