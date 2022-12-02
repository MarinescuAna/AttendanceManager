import ResponseHandler from "@/error-handler/error-handler";
import { CreateUserParameters } from "@/modules/user/index";
import { ResponseModule } from "@/shared/modules";
import axios from "axios";

export default class UserService{
    /**
     * Use this method in order to register a new user
     */
    static async CreateUser(playload: CreateUserParameters): Promise<ResponseModule>{
        let response: ResponseModule = {
            error: '',
            isSuccess: true
        };

        await axios.post('user/create_user', playload).catch(error => {
            response = ResponseHandler.errorResponseHandler(error);
        });

        return response;
    }
}