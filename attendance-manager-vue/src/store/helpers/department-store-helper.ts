
import { DepartmentUpdateModule, DepartmentViewModel } from "@/modules/department";
import { ResponseModule } from "@/shared/modules";
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
    public get departments(): DepartmentViewModel[] {
        return this.store.getters[`${departmentNamespace}/departments`];
    }

    /**
     * Load all the departments from the API
     */
    public loadDepartments(): Promise<DepartmentViewModel[]> {
        return this.store.dispatch(`${departmentNamespace}/loadDepartments`);
    }

    /**
     * Add a new departments in db and store
     */
    public addDepartment(payload: string): Promise<ResponseModule> {
        return this.store.dispatch(`${departmentNamespace}/addDepartment`, payload);
    }

    /**
     * Remove department from db and store
     */
    public removeDepartment(payload: string): Promise<ResponseModule> {
        return this.store.dispatch(`${departmentNamespace}/removeDepartment`, payload);
    }

    /**
    * Change department name from db and store
    */
    public updateDepartmentName(payload: DepartmentUpdateModule): Promise<ResponseModule> {
        return this.store.dispatch(`${departmentNamespace}/updateDepartmentName`, payload);
    }

    /**
     * Reset the state with the initial values
     */
    public reset(): void {
        this.store.dispatch(`${departmentNamespace}/resetStore`);
    }

}