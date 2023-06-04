import ResponseHandler from "@/error-handler/error-handler";
import { CreateBadgeParameters } from "@/modules/commands-parameters";
import { BadgeViewModule } from "@/modules/document";
import https from "@/plugins/axios";
import { REWARD_CONTROLLER } from "@/shared/constants";
import { AxiosResponse } from "axios";

export default class RewardService {
    static async getRewardsByReportIdAsync(id: number): Promise<BadgeViewModule[]> {
        return (await https.get(`${REWARD_CONTROLLER}/${id}`)).data;
    }

    static async createBadge(parameters: CreateBadgeParameters): Promise<BadgeViewModule>{
        let isSuccess = true;
        const response = await https.post(`${REWARD_CONTROLLER}/create_badge`, parameters)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });
        if(isSuccess){
            return (response as AxiosResponse).data as BadgeViewModule;
        }

        return null!;
    }
}