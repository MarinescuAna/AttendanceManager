
import { UpdateCourseParameters } from "@/modules/commands-parameters";
import { Store } from "vuex";
import { namespace as courseNamespace } from "../modules/course";
import { CreateCourseParameters } from "@/modules/commands-parameters";
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
    public loadCourses(reload:boolean): Promise<void> {
        return this.store.dispatch(`${courseNamespace}/loadCourses`,reload);
    }

    /**
     * Add a new course
     */
    public addCourse(payload: CreateCourseParameters): Promise<boolean> {
        return this.store.dispatch(`${courseNamespace}/addCourse`, payload);
    }

    /**
     * Remove course
     */
    public removeCourse(courseId: number): Promise<boolean> {
        return this.store.dispatch(`${courseNamespace}/removeCourse`, courseId);
    }

    /**
    * Change course name
    */
    public updateCourse(payload: UpdateCourseParameters, specializationName: string): Promise<boolean> {
        return this.store.dispatch(`${courseNamespace}/updateCourse`, { specializationName: specializationName, parameter: payload });
    }
}