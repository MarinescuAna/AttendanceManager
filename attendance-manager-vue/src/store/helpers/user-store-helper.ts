import { CreateUserParameters, UserInformationViewModule, UserViewModule } from "@/modules/user";
import { Store } from "vuex";
import { namespace as userNamespace } from "../modules/user";
import { DepartmentViewModule } from "@/modules/department";

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
    public get currentUser(): UserInformationViewModule {
        return this.store.getters[`${userNamespace}/currentUser`];
    }

    /**
     * Load all the users from the API
     */
    public loadUsers(): void {
        this.store.dispatch(`${userNamespace}/loadUsers`);
    }

    /**
     * Load all the users from the API
     * @todo remove this
     */
    public loadCurrentUserInfo(): void {
        this.store.dispatch(`${userNamespace}/loadCurrentUserInfo`);
    }

    /**
     * Add a new user only
     */
    public addUser(payload: CreateUserParameters, department: DepartmentViewModule): Promise<boolean> {
        return this.store.dispatch(`${userNamespace}/addUser`, { newUser: payload, department: department });
    }

    /**
     * Reset the state with the initial values
     */
    public reset(): void {
        this.store.dispatch(`${userNamespace}/resetStore`);
    }

}