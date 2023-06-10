
import { ReportViewModule, DocumentUpdateModule, ReportCardViewModule } from "@/modules/document";
import { Store } from "vuex";
import { namespace as documentNamespace } from "../modules/document/index";

export class DocumentStore {
    private store: Store<any>;

    constructor(store: Store<any>) {
        this.store = store;
    }

    /**
     * Getter for fetching all the documents from the store, not from the API
    */
    public get documents(): ReportCardViewModule[] {
        return this.store.getters[`${documentNamespace}/documents`];
    }

    /**
    * Getter for fetching all the documents from the store, not from the API
    */
    public get report(): ReportViewModule {
        return this.store.getters[`${documentNamespace}/report`];
    }

    /**
     * Load the current document from the API and update the store
     * @payload documentId
    */
    public async loadCurrentReport(payload: string): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/loadCurrentReport`, payload);
    }

    /** Use this method to update the document information */
    public async updateDocument(payload: { module: DocumentUpdateModule, newCourseName: string }): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/updateDocument`, payload);
    }

    /** Delete document */
    public async deleteDocument(): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/deleteDocument`);
    }
    public async deleteCollection(collectionId: number): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/deleteCollection`, collectionId);
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
   */
    public addCollection(activityTime: string, type: string): Promise<boolean> {
        return this.store.dispatch(`${documentNamespace}/addCollection`, { activityTime: activityTime, type: type });
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