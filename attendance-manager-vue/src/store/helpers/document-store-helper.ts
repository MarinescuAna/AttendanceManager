
import { DocumentFullViewModule, DocumentViewModule } from "@/modules/document";
import { DocumentFileInsertModule, DocumentFileViewModule } from "@/modules/document/document-file";
import { ResponseModule } from "@/shared/modules";
import { Store } from "vuex";
import { namespace as documentNamespace } from "../modules/document";

export class DocumentStore {
    private store: Store<any>;

    constructor(store: Store<any>) {
        this.store = store;
    }

    /**
     * Getter for fetching all the documents from the store, not from the API
    */
    public get createdDocuments(): DocumentViewModule[] {
        return this.store.getters[`${documentNamespace}/createdDocuments`];
    }

    /**
     * Getter for fetching all the documents from the store, not from the API
    */
    public get documentFiles(): DocumentFileViewModule[] {
        return this.store.getters[`${documentNamespace}/documentFiles`];
    }

    /**
    * Getter for fetching all the documents from the store, not from the API
    */
    public get documentDetails(): DocumentFullViewModule {
        return this.store.getters[`${documentNamespace}/documentDetails`];
    }

    /**
     * Load all the created documents from the API
    */
    public loadCreatedDocuments(payload: string): Promise<void> {
        return this.store.dispatch(`${documentNamespace}/loadCreatedDocuments`, payload);
    }

    /**
     * Load the current document from the API and update the store
     * @payload documentId
    */
    public loadCurrentDocument(payload: string): Promise<void> {
        return this.store.dispatch(`${documentNamespace}/loadCurrentDocument`, payload);
    }

    /**
   * Add a new specialziation only
   * @test
   */
    public addDocumentFile(payload: DocumentFileInsertModule): Promise<ResponseModule> {
        return this.store.dispatch(`${documentNamespace}/addDocumentFile`, payload);
    }

    /**
     * Reset the state with the initial values
     */
    public reset(): void {
        this.store.dispatch(`${documentNamespace}/resetStore`);
    }

}