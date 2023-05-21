import { InvolvementViewModule } from "@/modules/document/involvement";
import { involvementActions } from "./action";
import { involvementMutations } from "./mutation";
import { involvementGetter } from "./getter";

//state type
interface InvolvementState {
    // array with all the involvements for all the collections defined into the current document
    involvements: InvolvementViewModule[];
}

//initialize the state with an empty state
export function initializeInvolvementState(): InvolvementState {
    return {
        involvements: []
    };
}

// initial state
const involvementState: InvolvementState = initializeInvolvementState();

// export the namespace
export const namespace = 'involvement';

// export the store 'module'
export default {
    namespaced: true,
    state: involvementState,
    getters: involvementGetter,
    mutations: involvementMutations,
    actions: involvementActions
};