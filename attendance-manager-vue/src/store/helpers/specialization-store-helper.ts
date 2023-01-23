
import { SpecializationInsertModule, SpecializationViewModule } from "@/modules/specialization";
import { ResponseModule } from "@/shared/modules";
import { Store } from "vuex";
import { namespace as specializationNamespace } from "../modules/specialization";

export class SpecializationStore {
    private store: Store<any>;

    constructor(store: Store<any>) {
        this.store = store;
    }

    /**
     * Getter for fetching all the specializations from the store, not from the API
     */
    public get specializations(): SpecializationViewModule[] {
        return this.store.getters[`${specializationNamespace}/specializations`];
    }

    /**
     * Load all the specializations from the API(if it's needed) and update the store
     * @test
     */
    public loadSpecializations(): Promise<SpecializationViewModule[]> {
        return this.store.dispatch(`${specializationNamespace}/loadSpecializations`);
    }

    /**
     * Load all the specializations by the departmentId from the API(if it's needed) and update the store
     * @test
     */
    public loadSpecializationsByDepartmentId(payload: string): Promise<SpecializationViewModule[]> {
        return this.store.dispatch(`${specializationNamespace}/loadSpecializationsByDepartmentId`, payload);
    }

    /**
     * Add a new specialziation only
     * @test
     */
    public addSpecialization(specialization: SpecializationInsertModule): Promise<ResponseModule> {
        return this.store.dispatch(`${specializationNamespace}/addSpecialization`, specialization);
    }

    /**
     * Reset the state with the initial values
     */
    public reset(): void {
        this.store.dispatch(`${specializationNamespace}/resetStore`);
    }

    /**
     * Remove department
     * @todo implement the delete entierly
    public removeDepartment(department: UpdateDepartmentModule): Promise<ResponseModule> {
        return this.store.dispatch(`${organizationNamespace}/removeDepartment`, department);
    }
     */
    /**
    * Change department name
         * @todo implement the update entierly
    public updateDepartmentName(department: UpdateDepartmentModule): Promise<ResponseModule> {
        return this.store.dispatch(`${organizationNamespace}/updateDepartmentName`, department);
    }*/
}