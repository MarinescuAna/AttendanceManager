
import { Store } from "vuex";
import { namespace as specializationNamespace } from "../modules/specialization";
import { InsertSpecializationParameters } from "@/modules/commands-parameters";
import { SpecializationViewModule } from "@/modules/view-modules";

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
    public async loadSpecializationsAsync(): Promise<void>{
       await this.store.dispatch(`${specializationNamespace}/loadSpecializationsAsync`);
    }
    public async deleteSpecializationAsync(id: number): Promise<boolean>{
        return this.store.dispatch(`${specializationNamespace}/deleteSpecializationAsync`,id);
    }
    /**
     * Add a new specialziation only
     * @test
     */
    public async addSpecializationAsync(payload: InsertSpecializationParameters): Promise<boolean> {
        return await this.store.dispatch(`${specializationNamespace}/addSpecializationAsync`,payload);
    }

    /**
     * Reset the state with the initial values
     */
    public reset(): void {
        this.store.dispatch(`${specializationNamespace}/resetStore`);
    }
}