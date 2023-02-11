
import { CourseViewModule, CreateCourseModule, UpdateCourseModule } from "@/modules/course";
import { ResponseModule } from "@/shared/modules";
import { Store } from "vuex";
import { namespace as courseNamespace } from "../modules/course";

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
    public loadCourses(): Promise<void> {
        return this.store.dispatch(`${courseNamespace}/loadCourses`);
    }

    /**
     * Add a new course
     */
    public addCourse(payload: CreateCourseModule): Promise<ResponseModule> {
        return this.store.dispatch(`${courseNamespace}/addCourse`, payload);
    }

    /**
     * Remove course
     */
    public removeCourse(courseId: number): Promise<ResponseModule> {
        return this.store.dispatch(`${courseNamespace}/removeCourse`, courseId);
    }

    /**
    * Change course name
    */
    public updateCourse(payload: UpdateCourseModule): Promise<ResponseModule> {
        return this.store.dispatch(`${courseNamespace}/updateCourse`, payload);
    }
}