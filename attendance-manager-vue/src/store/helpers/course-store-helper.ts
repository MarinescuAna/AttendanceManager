
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
     * Load all the courses from the API
     */
    public loadCourses(payload: string): Promise<CourseViewModule[]> {
        return this.store.dispatch(`${courseNamespace}/loadCourses`, payload);
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