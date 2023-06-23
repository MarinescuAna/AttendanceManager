import { Store } from "vuex";
import { namespace as documentNamespace } from "../modules/document/index";
import { InsertCollaboratorParameters, UpdateReportParameters, InsertCollectionParameters, UpdateCollectionParameters } from "@/modules/commands-parameters";
import { ReportCardViewModule, ReportViewModule } from "@/modules/view-modules";

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
    public async loadCurrentReportAsync(payload: string): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/loadCurrentReportAsync`, payload);
    }

    /** Use this method to update the document information */
    public async updateReportAsync(payload: { module: UpdateReportParameters, newCourseName: string }): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/updateDocumentAsync`, payload);
    }

    /** Delete document */
    public async deleteReportAsync(): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/deleteDocumentAsync`);
    }
    public async deleteCollectionAsync(collectionId: number): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/deleteCollectionAsync`, collectionId);
    }

    /**
     * Add new teacher as collaborator to a document
     * @payload teacher email
    */
    public async addCollaboratorAsync(payload: InsertCollaboratorParameters): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/addCollaboratorAsync`, payload);
    }

    public async updateCollectionAsync(payload: UpdateCollectionParameters): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/updateCollectionAsync`, payload);
    }

    /**
   * Add a new specialziation only
   */
    public async addCollectionAsync(payload: InsertCollectionParameters): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/addCollectionAsync`, payload);
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