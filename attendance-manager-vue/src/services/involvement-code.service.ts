import ResponseHandler from "@/error-handler/error-handler";
import https from "@/plugins/axios";
import { INVOLVEMENT_CODE } from "@/shared/constants";
import { AxiosResponse } from "axios";

type InvolvementCodeInsertModule = {
    code: string;
    expirationDate: string;
}

export default class InvolvementCodeService {

    /**
     * Generate an involvement code
     * @param payload number of minutes
     * @returns undefinde if some errors occured or the new generated code and its expiration date
     */
    static async createInvolvementCode(minutes: number, attendanceCollectionId: number): Promise<undefined | InvolvementCodeInsertModule> {
        let isSuccess = true;

        const result = await https.post(`${INVOLVEMENT_CODE}/create_code/${minutes}/${attendanceCollectionId}`)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });
            
        if (isSuccess) {
            return (result as AxiosResponse).data;
        }

        return undefined;
    }
}