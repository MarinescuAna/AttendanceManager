/* eslint-disable @typescript-eslint/no-empty-interface */
import { BaseModule } from "../shared";

/**
 * Used for store and it contains all the specializations
 */
export interface SpecializationViewModule extends BaseModule{
    departmentId: string;
}
/**
 * Used for sending data to api in order to create a new specialization
 */
export interface SpecializationInsertModule {
    name: string;
    departmentId: string;
}
/**
 * Used for v-selector 
 */
export interface SpecializationModule extends BaseModule{
}