import { StudentForCourseViewModule } from "@/modules/view-modules";
import https from "@/plugins/axios";
import { USER_CONTROLLER } from "@/shared/constants";

export default class UserService {

    static async getStudentsBySpecializationIdEnrollmentYear(year: number, specializationId: number): Promise<StudentForCourseViewModule[]> {
        return (await https.get(`${USER_CONTROLLER}/students?year=${year}&specializationId=${specializationId}`)).data;
    }
}