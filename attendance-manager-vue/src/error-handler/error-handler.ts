import { ERR_BAD_RESPONSE, ERR_NOT_FOUND } from "./constants";
import { ResponseModule } from "@/shared/modules";

//TODO in progress
export default class ResponseHandler {
    public static errorResponseHandler(error): ResponseModule {
        const errorData = error.response.data;
        if (errorData.status == ERR_BAD_RESPONSE || errorData.status == ERR_NOT_FOUND) {
            return {
                error: errorData.error,
                isSuccess: false
            } as ResponseModule;
        }
        return {
            error: "API doesn't respond!",
            isSuccess: false
        };
    }
}



