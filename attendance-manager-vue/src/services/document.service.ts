import ResponseHandler from "@/error-handler/error-handler";
import { DocumentFullViewModule, DocumentInsertModule } from "@/modules/document";
import https from "@/plugins/axios";
import { DOCUMENT_CONTROLLER } from "@/shared/constants";

export default class DocumentService {

    /**
     * Add a new document
     * @param payload DocumentInsertModule
     * @returns 
     */
    static async addDocument(payload: DocumentInsertModule): Promise<boolean> {
        let isSuccess =  true;

        await https.post(`${DOCUMENT_CONTROLLER}/create_document?`,payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        return isSuccess;
    }

    static async getDocumentById(payload: string): Promise<DocumentFullViewModule>{
        return (await https.get(`${DOCUMENT_CONTROLLER}/document_by_id?id=${payload}`)).data;
    }
}