import ResponseHandler from "@/error-handler/error-handler";
import {  DocumentInsertModule } from "@/modules/document";
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
}