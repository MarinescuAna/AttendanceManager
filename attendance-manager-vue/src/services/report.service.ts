import ResponseHandler from "@/error-handler/error-handler";
import { ReportInsertModule, ReportCardViewModule } from "@/modules/document";
import https from "@/plugins/axios";
import { DOCUMENT_CONTROLLER } from "@/shared/constants";

export default class ReportService {
    static async getReports(): Promise<ReportCardViewModule[]> {
        return (await https.get(`${DOCUMENT_CONTROLLER}/documents`)).data;
    }

    static async addReport(parameters: ReportInsertModule): Promise<boolean> {
        let isSuccess = true;
        await https.post(`${DOCUMENT_CONTROLLER}/create_document`, parameters)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });
        return isSuccess;
    }
}