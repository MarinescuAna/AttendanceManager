import { SpecializationViewModule } from "./specializations";

/**
 * Used for store and it contains all the departments toghter with specializations
 */
export interface OrganizationViewModel {
    id: string;
    name: string;
    children: SpecializationViewModule[];
}