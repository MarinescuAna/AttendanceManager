import { CourseDashboardViewModule } from "@/modules/view-modules";
import https from "@/plugins/axios";
import { COURSE_CONTROLLER } from "@/shared/constants";

export default class CourseService {
    static async getDashboard(): Promise<CourseDashboardViewModule[]> {
        return (await https.get(`${COURSE_CONTROLLER}/dashboard`)).data;
    }
}