import { Store } from "vuex";
import { namespace as authNamespace, LoginParameters } from "../modules/auth";

export class AuthStore {
    private store: Store<any>;

    constructor(store: Store<any>) {
        this.store = store;
    }

    /** do login */
    public async login(user: LoginParameters): Promise<boolean> {
        return this.store.dispatch(`${authNamespace}/login`, user);
    }
}