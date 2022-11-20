/* eslint-disable @typescript-eslint/no-explicit-any */
import { Store } from "vuex";
import { AuthStore } from "./helpers/auth-store-helper";

export default class {
    /** 
     * Private pointers to the stores 
     * */
    private static user: AuthStore;

    /** Gets the site module */
    public static get userStore(): AuthStore {
        return this.user;
    }

    /**
 * Initializes the store helper (provides a pointer to the store)
 * @param store The store the helper is being initialized for
 */
    public static init(store: Store<any>): void {
        this.user = new AuthStore(store);
    }
}