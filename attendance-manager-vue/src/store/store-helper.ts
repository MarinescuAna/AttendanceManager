/* eslint-disable @typescript-eslint/no-explicit-any */
import { Store } from "vuex";
import { OrganizationStore } from "./helpers/organization-store-helper";
import { UserStore } from "./helpers/user-store-helper";

export default class {
    /** 
     * Private pointers to the stores 
     * */
    private static organization: OrganizationStore;
    private static user: UserStore;

    public static init(store: Store<any>): void {
        this.organization = new OrganizationStore(store);
        this.user = new UserStore(store);
    }

    /** Gets the site module */
    public static get organizationStore(): OrganizationStore {
        return this.organization;
    }

    /** Gets the site module */
    public static get userStore(): UserStore {
        return this.user;
    }

}