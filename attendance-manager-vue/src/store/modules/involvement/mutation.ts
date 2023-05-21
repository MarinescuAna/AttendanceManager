import { InvolvementViewModule } from "@/modules/document/involvement";
import { initializeInvolvementState } from ".";

// mutations for this store
export const involvementMutations = {
    /**
     * Update the entire list of involvements
     */
    _involvements(state, payload: InvolvementViewModule[]): void {
        state.involvements = payload;
    },
    /**
     * Reset the state with the initial values
     */
    _resetStore(state): void {
        Object.assign(state, initializeInvolvementState());
    },
};