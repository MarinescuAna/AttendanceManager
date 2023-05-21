
import { Store } from "vuex";
import { namespace as involvementNamespace } from "../modules/involvement/index";
import { InvolvementViewModule } from "@/modules/document/involvement";

export class InvolvementStore {
    private store: Store<any>;

    constructor(store: Store<any>) {
        this.store = store;
    }

    /**
     * Getter for fetching all the involements from the store
    */
    public get involvements(): InvolvementViewModule[] {
        return this.store.getters[`${involvementNamespace}/involvements`];
    }

    /**
    * Load all the involvements
    */
    public async loadInvolvements(reload: boolean): Promise<boolean> {
        return await this.store.dispatch(`${involvementNamespace}/loadInvolvements`, reload);
    }

        /**
     * Reset involvements state with the initial values
     */
        public resetInvolvementStore(): void {
            this.store.dispatch(`${involvementNamespace}/resetStore`);
        }
}