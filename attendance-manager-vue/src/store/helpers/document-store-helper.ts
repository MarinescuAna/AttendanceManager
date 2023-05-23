
import { BadgeViewModule, DocumentFullViewModule, DocumentInsertModule, DocumentUpdateModule, DocumentViewModule } from "@/modules/document";
import { AttendanceCollectionInsertModule, AttendanceCollectionViewModule } from "@/modules/document/attendance-collection";
import { Store } from "vuex";
import { namespace as documentNamespace } from "../modules/document/index";
import { StudentAttendanceModule } from "@/modules/document/attendance";

export class DocumentStore {
    private store: Store<any>;

    constructor(store: Store<any>) {
        this.store = store;
    }

    /**
     * Getter for fetching all the documents from the store, not from the API
    */
    public get documents(): DocumentViewModule[] {
        return this.store.getters[`${documentNamespace}/documents`];
    }
    /**
     * Getter for fetching all the documents from the store, not from the API
    */
    public get studentsTotalAttendances(): StudentAttendanceModule[] {
        return this.store.getters[`${documentNamespace}/studentsTotalAttendances`];
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

    public async addDocument(parameters: DocumentInsertModule, courseName: string, specializationName: string): Promise<boolean>{
        return await this.store.dispatch(`${documentNamespace}/addDocument`, {parameters: parameters, courseName: courseName, specializationName: specializationName});
    }

    /**
     * Load all the document
     */
    public async loadDocuments(reload: boolean): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/loadDocuments`, reload);
    }

    /**
     * Load the current document from the API and update the store
     * @payload documentId
    */
    public async loadCurrentDocument(payload: string): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/loadCurrentDocument`, payload);
    }

    public async loadStudentTotalAttendances(payload: string|null, reload: boolean ): Promise<boolean>{
        return await this.store.dispatch(`${documentNamespace}/loadStudentTotalAttendances`, { email: payload, reload: reload})
    }

    /** Use this method to update the document information */
    public async updateDocument(payload: { module: DocumentUpdateModule, newCourseName: string }): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/updateDocument`, payload);
    }

    /** Delete document */
    public async deleteDocument(): Promise<boolean> {
        return await this.store.dispatch(`${documentNamespace}/deleteDocument`);
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