/* eslint-disable @typescript-eslint/no-explicit-any */
import { Store } from "vuex";
import { OrganizationStore} from  "./helpers/organization-store-helper";

export default class {
    /** 
     * Private pointers to the stores 
     * */
    private static organization: OrganizationStore;

    /** Gets the site module */
    public static get organizationStore(): OrganizationStore {
        return this.organization;
    }

    public static init(store: Store<any>): void {
        this.organization = new OrganizationStore(store);
    }
}