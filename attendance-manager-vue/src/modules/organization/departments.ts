/**
 * This module is used for v-select components to map only the departments
 */
 export interface DepartmentModule{
    id: string;
    name: string;
}
/**
 * This module is used to delete a department or to change the name
 */
export interface UpdateDepartmentModule{
    departmentId:string;
    name:string;
}