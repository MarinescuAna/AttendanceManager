/* eslint-disable @typescript-eslint/no-explicit-any */
import { Store } from "vuex";
import { CourseStore } from "./helpers/course-store-helper";
import { OrganizationStore } from "./helpers/organization-store-helper";
import { UserStore } from "./helpers/user-store-helper";

export default class {

    private static organization: OrganizationStore;
    private static user: UserStore;
    private static course: CourseStore;

    public static init(store: Store<any>): void {
        this.organization = new OrganizationStore(store);
        this.user = new UserStore(store);
        this.course = new CourseStore(store);
    }

    public static get organizationStore(): OrganizationStore {
        return this.organization;
    }

    public static get userStore(): UserStore {
        return this.user;
    }

    public static get courseStore(): CourseStore {
        return this.course;
    }
}