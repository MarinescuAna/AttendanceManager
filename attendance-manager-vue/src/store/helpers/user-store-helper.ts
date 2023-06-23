import { Store } from "vuex";
import { namespace as userNamespace } from "../modules/user";
import { DepartmentViewModule, UserInformationViewModule, UserViewModule } from "@/modules/view-modules";
import { InsertUserParameters } from "@/modules/commands-parameters";

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
     * Getter for current user from the store, not from the API
     */
    public get currentUser(): UserInformationViewModule{
        return this.store.getters[`${userNamespace}/currentUser`];
    }

    /**
     * Load all the users from the API
     */
    public async loadUsersAsync(): Promise<void> {
       await this.store.dispatch(`${userNamespace}/loadUsersAsync`);
    }

    /**
     * Load all the users from the API
     * @todo remove this
     */
    public async loadCurrentUserInfoAsync(): Promise<void> {
        await this.store.dispatch(`${userNamespace}/loadCurrentUserInfoAsync`);
    }

    /**
     * Add a new user only
     */
    public async addUserAsync(payload: InsertUserParameters, department: DepartmentViewModule): Promise<boolean> {
        return await this.store.dispatch(`${userNamespace}/addUserAsync`, { newUser: payload, department: department });
    }

    /**
     * Reset the state with the initial values
     */
    public reset(): void {
        this.store.dispatch(`${userNamespace}/resetStore`);
    }

}