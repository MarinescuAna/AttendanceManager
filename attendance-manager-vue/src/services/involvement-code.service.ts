import ResponseHandler from "@/error-handler/error-handler";
import { InsertInvolvementCodeParameters } from "@/modules/commands-parameters";
import https from "@/plugins/axios";
import { INVOLVEMENT_CODE_CONTROLLER } from "@/shared/constants";
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
    static async createInvolvementCodeAsync(payload: InsertInvolvementCodeParameters): Promise<undefined | InvolvementCodeInsertModule> {
        let isSuccess = true;

        const result = await https.post(`${INVOLVEMENT_CODE_CONTROLLER}/create_code`,payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });
            
        if (isSuccess) {
            return (result as AxiosResponse).data;
        }

        return undefined;
    }
}