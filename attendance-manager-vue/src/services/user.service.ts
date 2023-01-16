import { StudentForCourseViewModule } from "@/modules/user";
import axios from "axios";

export default class UserService {
    private static controllerName = "user";

    static async getStudentsBySpecializationIdEnrollmentYear(year: string, specializationId: string): Promise<StudentForCourseViewModule[]> {
        return (await axios.get(`${this.controllerName}/students_by_specializationId_enrollmentYear?year=${year}&specializationId=${specializationId}`)).data;
    }
}