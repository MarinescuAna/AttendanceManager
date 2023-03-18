import { ERR_BAD_RESPONSE, ERR_NOT_FOUND } from "./constants";
import { ResponseModule } from "@/shared/modules";
import { Toastification } from "@/plugins/vue-toastification";

interface Response{
    status: number;
    error: string;
}

export default class ResponseHandler {
    public static errorResponseHandler(error): ResponseModule {
        const response = error.response.data as Response;
        switch (response.status) {
            case ERR_NOT_FOUND:
                Toastification.error(response.error, `${response.status} Not found:`);
                break;
            case ERR_BAD_RESPONSE:
                Toastification.error(response.error, `${response.status} Bad request:`)
                break;
            default:
                Toastification.simpleError("API doesn't respond!");
        }

        // TODO this should be removed
        return {
            isSuccess:false,
            error:''
        };
    }
}



