/**
 * Used for v-selector component to map only the specialziations
 */
 export interface SpecializationModule{
    id: string;
    name: string;
    departmentId:string;
}
/**
 * Used for transfering data from store to components
 */
export interface SpecializationViewModule {
    id: string;
    name: string;
}
/**
 * Used for data transfer from component to store in order to create a new specialization
 */
export interface SpecializationCreateParamsModule {
    name: string;
    departmentId: string;
}
