/* eslint-disable @typescript-eslint/no-empty-interface */

import { BaseModule, TableModule } from "../shared";

/**
 * Used for store and it contains all the departments
 * This module is used for v-select component
 */
export interface DepartmentViewModel extends TableModule
{
    name: string;
}
/**
 * This module is used to update the department name
 */
export interface DepartmentUpdateModule extends BaseModule
{
    
}