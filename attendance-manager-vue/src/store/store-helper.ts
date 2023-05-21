/* eslint-disable @typescript-eslint/no-explicit-any */
import { Store } from "vuex";
import { CourseStore } from "./helpers/course-store-helper";
import { DepartmentStore } from "./helpers/department-store-helper";
import { DocumentStore } from "./helpers/document-store-helper";
import { SpecializationStore } from "./helpers/specialization-store-helper";
import { UserStore } from "./helpers/user-store-helper";
import { InvolvementStore } from "./helpers/involvement-store-helper";

export default class {

    private static department: DepartmentStore;
    private static document: DocumentStore;
    private static user: UserStore;
    private static course: CourseStore;
    private static specialization: SpecializationStore;
    private static involvement: InvolvementStore;

    public static init(store: Store<any>): void {
        this.department = new DepartmentStore(store);
        this.user = new UserStore(store);
        this.course = new CourseStore(store);
        this.specialization = new SpecializationStore(store);
        this.document = new DocumentStore(store);
        this.involvement = new InvolvementStore(store);
    }

    public static get departmentStore(): DepartmentStore {
        return this.department;
    }

    public static get documentStore(): DocumentStore {
        return this.document;
    }

    public static get userStore(): UserStore {
        return this.user;
    }

    public static get courseStore(): CourseStore {
        return this.course;
    }

    public static get specializationStore(): SpecializationStore {
        return this.specialization;
    }

    public static get involvementStore(): InvolvementStore {
        return this.involvement;
    }
}