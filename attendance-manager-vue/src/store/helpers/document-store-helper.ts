
import { DocumentViewModule } from "@/modules/document";
import { Store } from "vuex";
import { namespace as documentNamespace } from "../modules/document";

export class DocumentStore {
    private store: Store<any>;

    constructor(store: Store<any>) {
        this.store = store;
    }

    /**
     * Getter for fetching all the departments from the store, not from the API
     
    public get departments(): DepartmentViewModel[] {
        return this.store.getters[`${departmentNamespace}/departments`];
    }*/

    /**
     * Load all the created documents from the API
    */
    public loadCreatedDocuments(payload: string): Promise<DocumentViewModule[]> {
        return this.store.dispatch(`${documentNamespace}/loadCreatedDocuments`,payload);
    }   
}