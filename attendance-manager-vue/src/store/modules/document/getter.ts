import { DocumentFullViewModule } from "@/modules/document";
import { AttendanceCollectionViewModule } from "@/modules/document/attendance-collection";

// getters for this store
export const documentGetter = {
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
    },
};