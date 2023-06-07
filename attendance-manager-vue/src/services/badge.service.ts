import ResponseHandler from "@/error-handler/error-handler";
import { CreateBadgeParameters } from "@/modules/commands-parameters";
import { BadgePercentageViewModule, BadgeViewModule } from "@/modules/document";
import https from "@/plugins/axios";
import { BADGE_CONTROLLER } from "@/shared/constants";
import { AxiosResponse } from "axios";

export default class BadgeService {
    static async createBadge(parameters: CreateBadgeParameters): Promise<BadgeViewModule> {
        let isSuccess = true;
        const response = await https.post(`${BADGE_CONTROLLER}/create_badge`, parameters)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });
        if (isSuccess) {
            return (response as AxiosResponse).data as BadgeViewModule;
        }

        return null!;
    }

    static async getBadgesPercentageAsync(): Promise<BadgePercentageViewModule[]> {
        return (await https.get(`${BADGE_CONTROLLER}/badges`)).data;
    }
}