import { DocumentFullViewModule, DocumentViewModule } from "@/modules/document";
import { AttendanceCollectionViewModule } from "@/modules/document/attendance-collection";

// getters for this store
export const documentGetter = {
    /**
     * Get documents from the store
    */
    documents(state): DocumentViewModule[] {
        return state.documents;
    },
    /**
     * Gets created documents from the store
    */
    documentDetails(state): DocumentFullViewModule {
        return state.currentDocument;
    },
    /**
     * Gets created documents from the store
    */
    documentFiles(state): AttendanceCollectionViewModule[] {
        return typeof (state.currentDocument?.attendanceCollections) === "undefined" ? []
            : state.currentDocument?.attendanceCollections;
    }
};