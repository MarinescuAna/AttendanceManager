import { BadgeViewModule } from "@/modules/document";
import https from "@/plugins/axios";
import { REWARD_CONTROLLER } from "@/shared/constants";

export default class RewardService {
    static async getRewardsByReportIdAsync(id: number): Promise<BadgeViewModule[]> {
        return (await https.get(`${REWARD_CONTROLLER}/${id}`)).data;
    }
}