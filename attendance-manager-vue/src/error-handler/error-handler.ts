import { ERR_BAD_RESPONSE, ERR_NOT_FOUND, ERR_UNSUPORTED_MEDIA_TYPE } from "./constants";
import { Toastification } from "@/plugins/vue-toastification";

interface Response{
    status: number;
    error: string;
}

export default class ResponseHandler {
    public static errorResponseHandler(error): boolean {
        const response = error.response.data as Response;
        switch (response.status) {
            case ERR_NOT_FOUND:
                Toastification.error(response.error, `${response.status} Not found:`);
                break;
            case ERR_BAD_RESPONSE:
                Toastification.error(response.error, `${response.status} Bad request:`);
                break;
            case ERR_UNSUPORTED_MEDIA_TYPE:
                Toastification.error("The provided parameters are not passed correctly!", "415 Unsupported Media Type:");
                break;
            default:
                Toastification.simpleError("API doesn't respond!");
        }

        return false;
    }
}



