
import { SpecializationViewModule } from "@/modules/specialization";
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
     * Load all the specializations by the departmentId from the API(if it's needed) and update the store
     * @test
     */
    public loadSpecializations(): void{
        this.store.dispatch(`${specializationNamespace}/loadSpecializations`);
    }

    /**
     * Add a new specialziation only
     * @test
     */
    public addSpecialization(name: string,departmentId: number): Promise<boolean> {
        return this.store.dispatch(`${specializationNamespace}/addSpecialization`, {
            name: name,
            departmentId: departmentId
        });
    }

    /**
     * Reset the state with the initial values
     */
    public reset(): void {
        this.store.dispatch(`${specializationNamespace}/resetStore`);
    }
}