import { ReportViewModule } from "@/modules/view-modules";


// getters for this store
export const documentGetter = {
    /**
     * Gets created documents from the store
    */
    report(state): ReportViewModule {
        return state.currentReport;
    }
};