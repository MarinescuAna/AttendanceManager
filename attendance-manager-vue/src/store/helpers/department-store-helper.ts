import { Store } from "vuex";
import { namespace as departmentNamespace } from "../modules/department";
import { InsertDepartmentParameters, UpdateDepartmentParameters } from "@/modules/commands-parameters";
import { DepartmentViewModule } from "@/modules/view-modules";

export class DepartmentStore {
    private store: Store<any>;

    constructor(store: Store<any>) {
        this.store = store;
    }

    /**
     * Getter for fetching all the departments from the store, not from the API
     */
    public get departments(): DepartmentViewModule[] {
        return this.store.getters[`${departmentNamespace}/departments`];
    }

    /**
     * Load all the departments from the API
     */
    public async loadDepartments(): Promise<void> {
        await this.store.dispatch(`${departmentNamespace}/loadDepartments`);
    }
    public async deleteDepartment(id: number): Promise<boolean>{
        return this.store.dispatch(`${departmentNamespace}/deleteDepartment`,id);
    }

    /**
     * Add a new departments in db and store
     */
    public async addDepartment(payload: InsertDepartmentParameters): Promise<boolean> {
        return await this.store.dispatch(`${departmentNamespace}/addDepartment`, payload);
    }

    /**
    * Change department name from db and store
    */
    public async updateDepartment(payload: UpdateDepartmentParameters): Promise<boolean> {
        return await this.store.dispatch(`${departmentNamespace}/updateDepartment`, payload);
    }

    /**
     * Reset the state with the initial values
     */
    public reset(): void {
        this.store.dispatch(`${departmentNamespace}/resetStore`);
    }

}