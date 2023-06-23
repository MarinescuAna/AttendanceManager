
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
    public async loadCoursesAsync(reload: boolean): Promise<void> {
        return await this.store.dispatch(`${courseNamespace}/loadCoursesAsync`, reload);
    }

    /**
     * Add a new course
     */
    public async addCourseAsync(payload: CreateCourseParameters): Promise<boolean> {
        return await this.store.dispatch(`${courseNamespace}/addCourseAsync`, payload);
    }

    /**
     * Remove course
     */
    public async removeCourseAsync(courseId: number): Promise<boolean> {
        return await this.store.dispatch(`${courseNamespace}/removeCourseAsync`, courseId);
    }

    /**
    * Change course name
    */
    public async updateCourseAsync(payload: UpdateCourseParameters, specializationName: string): Promise<boolean> {
        return await this.store.dispatch(`${courseNamespace}/updateCourseAsync`, { specializationName: specializationName, parameter: payload });
    }
}