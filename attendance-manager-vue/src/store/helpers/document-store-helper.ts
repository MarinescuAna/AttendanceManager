
import { DocumentFullViewModule, DocumentViewModule } from "@/modules/document";
import { AttendanceCollectionInsertModule, AttendanceCollectionViewModule } from "@/modules/document/attendance-collection";
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
    public get collaboratorDocuments(): DocumentViewModule[] {
        return this.store.getters[`${documentNamespace}/collaboratorDocuments`];
    }

    /**
     * Getter for fetching all the documents from the store, not from the API
    */
    public get documentFiles(): AttendanceCollectionViewModule[] {
        return this.store.getters[`${documentNamespace}/documentFiles`];
    }

    /**
    * Getter for fetching all the documents from the store, not from the API
    */
    public get documentDetails(): DocumentFullViewModule {
        return this.store.getters[`${documentNamespace}/documentDetails`];
    }

    /**
     * Load the documents where the user is collaborator or the created documents
     * @param loadCreatedDocuments true for created documents, false for documents where the user is collaborator
     */
    public async loadDocuments(loadCreatedDocuments: boolean): Promise<void> {
        await this.store.dispatch(`${documentNamespace}/loadDocuments`, loadCreatedDocuments);
    }

    /**
     * Load the current document from the API and update the store
     * @payload documentId
    */
    public async loadCurrentDocument(payload: string): Promise<void> {
        await this.store.dispatch(`${documentNamespace}/loadCurrentDocument`, payload);
    }

    /**
     * Add new teacher as collaborator to a document
     * @payload teacher email
    */
    public async addCollaborator(payload: string): Promise<boolean> {
       return await this.store.dispatch(`${documentNamespace}/addCollaborator`, payload);
    }

    /**
   * Add a new specialziation only
   * @test
   */
    public addAttendanceCollection(payload: AttendanceCollectionInsertModule): Promise<boolean> {
        return this.store.dispatch(`${documentNamespace}/addAttendanceCollection`, payload);
    }

    /**
     * Reset the state with the initial values
     */
    public resetEntireStore(): void {
        this.store.dispatch(`${documentNamespace}/resetStore`);
    }

    /**
     * Reset current document state with the initial values
     */
    public resetCurrentDocumentStore(): void {
        this.store.dispatch(`${documentNamespace}/resetCurrentDocumentStore`);
    }
}