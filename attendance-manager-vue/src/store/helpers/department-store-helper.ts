
import { DepartmentModule } from "@/modules/department";
import { Store } from "vuex";
import { namespace as departmentNamespace } from "../modules/department";

export class DepartmentStore {
    private store: Store<any>;

    constructor(store: Store<any>) {
        this.store = store;
    }

    /**
     * Getter for fetching all the departments from the store, not from the API
     */
    public get departments(): DepartmentModule[] {
        return this.store.getters[`${departmentNamespace}/departments`];
    }

    /**
     * Load all the departments from the API
     */
    public loadDepartments(): void {
        this.store.dispatch(`${departmentNamespace}/loadDepartments`);
    }

    /**
     * Add a new departments in db and store
     */
    public addDepartment(payload: string): Promise<boolean> {
        return this.store.dispatch(`${departmentNamespace}/addDepartment`, payload);
    }

    /**
    * Change department name from db and store
    */
    public updateDepartmentName(payload: DepartmentModule): Promise<boolean> {
        return this.store.dispatch(`${departmentNamespace}/updateDepartmentName`, payload);
    }

    /**
     * Reset the state with the initial values
     */
    public reset(): void {
        this.store.dispatch(`${departmentNamespace}/resetStore`);
    }

}