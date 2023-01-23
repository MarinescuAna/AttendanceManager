import { CreateUserParameters, UserInformationViewModule, UserViewModule } from "@/modules/user";
import { ResponseModule } from "@/shared/modules";
import { Store } from "vuex";
import { namespace as userNamespace } from "../modules/user";

export class UserStore {
    private store: Store<any>;

    constructor(store: Store<any>) {
        this.store = store;
    }

    /**
     * Getter for fetching all users from the store, not from the API
     */
    public get users(): UserViewModule[] {
        return this.store.getters[`${userNamespace}/users`];
    }

    /**
     * Load all the users from the API
     */
    public loadUsers(): Promise<UserViewModule[]> {
        return this.store.dispatch(`${userNamespace}/loadUsers`);
    }

    /**
     * Load all the users from the API
     * @todo remove this
     */
    public loadCurrentUserInfo(payload: string): Promise<UserInformationViewModule> {
        return this.store.dispatch(`${userNamespace}/loadCurrentUserInfo`, payload);
    }

    /**
     * Add a new user only
     */
    public addUser(payload: CreateUserParameters): Promise<ResponseModule> {
        return this.store.dispatch(`${userNamespace}/addUser`, payload);
    }

    /**
     * Reset the state with the initial values
     */
    public reset(): void {
        this.store.dispatch(`${userNamespace}/resetStore`);
    }

}