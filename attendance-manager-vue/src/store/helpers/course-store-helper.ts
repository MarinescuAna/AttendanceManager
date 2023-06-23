
import { Store } from "vuex";
import { namespace as courseNamespace } from "../modules/course";
import { CreateCourseParameters, UpdateCourseParameters } from "@/modules/commands-parameters";
import { CourseViewModule } from "@/modules/view-modules";

export class CourseStore {
    private store: Store<any>;

    constructor(store: Store<any>) {
        this.store = store;
    }

    /**
     * Getter for fetching all the courses from the store, not from the API
     */
    public get courses(): CourseViewModule[] {
        return this.store.getters[`${courseNamespace}/courses`];
    }

    /**
     * Reset the state with the initial values
     */
    public reset(): void {
        this.store.dispatch(`${courseNamespace}/resetStore`);
    }


    /**
     * Load all the courses from the API
     */
    public async loadCourses(reload:boolean): Promise<void> {
        return await this.store.dispatch(`${courseNamespace}/loadCourses`,reload);
    }

    /**
     * Add a new course
     */
    public async addCourse(payload: CreateCourseParameters): Promise<boolean> {
        return await this.store.dispatch(`${courseNamespace}/addCourse`, payload);
    }

    /**
     * Remove course
     */
    public async removeCourse(courseId: number): Promise<boolean> {
        return await this.store.dispatch(`${courseNamespace}/removeCourse`, courseId);
    }

    /**
    * Change course name
    */
    public async updateCourse(payload: UpdateCourseParameters, specializationName: string): Promise<boolean> {
        return await this.store.dispatch(`${courseNamespace}/updateCourse`, { specializationName: specializationName, parameter: payload });
    }
}