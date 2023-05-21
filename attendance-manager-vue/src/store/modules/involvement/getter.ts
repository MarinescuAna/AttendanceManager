import { InvolvementViewModule } from "@/modules/document/involvement";

// getters for this store
export const involvementGetter = {
    /**
     * Get involvements from the store
    */
    involvements(state): InvolvementViewModule[] {
        return state.involvements;
    },
};