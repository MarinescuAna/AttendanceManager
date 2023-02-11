import ResponseHandler from "@/error-handler/error-handler";
import { DocumentFullViewModule, DocumentInsertModule } from "@/modules/document";
import https from "@/plugins/axios";
import { DOCUMENT_CONTROLLER } from "@/shared/constants";
import { ResponseModule } from "@/shared/modules";

export default class DocumentService {

    static async addDocument(payload: DocumentInsertModule): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };
        await https.post(`${DOCUMENT_CONTROLLER}/create_document?`,payload)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });
        return response;
    }

    static async getDocumentById(payload: string): Promise<DocumentFullViewModule>{
        return (await https.get(`${DOCUMENT_CONTROLLER}/document_by_id?id=${payload}`)).data;
    }
}