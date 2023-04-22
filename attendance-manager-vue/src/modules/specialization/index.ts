/**
 * Used for store and it contains all the specializations
 */
export interface SpecializationModule{
    id: number;
    name: string;
    departmentId: number;
}

export interface SpecializationInsertParameter{
    name: string;
    departmentId: number;
}
