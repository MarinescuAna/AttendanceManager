import { BadgeViewModule } from "@/modules/view-modules";
import https from "@/plugins/axios";
import { REWARD_CONTROLLER } from "@/shared/constants";

export default class RewardService {
    static async getRewardsByReportIdAsync(): Promise<BadgeViewModule[]> {
        return (await https.get(`${REWARD_CONTROLLER}/rewards`)).data;
    }
}