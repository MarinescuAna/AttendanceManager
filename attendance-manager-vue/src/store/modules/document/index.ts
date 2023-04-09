import { DocumentFullViewModule, DocumentViewModule } from "@/modules/document";
import {documentGetter} from "./getter";
import {documentMutations} from "./mutation";
import {documentActions} from "./action";

//state type
interface DocumentState {
    // array with all the documents
    documents: DocumentViewModule[];
    // current document
    currentDocument: DocumentFullViewModule;
}

//initialize the state with an empty state
export function initializeDocumentState(): DocumentState {
    return {
        documents: [],
        currentDocument: {} as DocumentFullViewModule
    };
}

// initial state
const documentState: DocumentState = initializeDocumentState();

// export the namespace
export const namespace = 'document';

// export the store 'module'
export default {
    namespaced: true,
    state: documentState,
    getters: documentGetter,
    mutuations: documentMutations,
    actions: documentActions
};