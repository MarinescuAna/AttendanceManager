import ResponseHandler from "@/error-handler/error-handler";
import { DocumentFullViewModule, DocumentInsertModule } from "@/modules/document";
import { ResponseModule } from "@/shared/modules";
import axios from "axios";

export default class DocumentService {
    private static controllerName = "document";

    static async addDocument(payload: DocumentInsertModule): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };
        await axios.post(`${this.controllerName}/create_document?`,payload)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });
        return response;
    }

    static async getDocumentById(payload: string): Promise<DocumentFullViewModule>{
        return (await axios.get(`${this.controllerName}/document_by_id?id=${payload}`)).data;
    }
}