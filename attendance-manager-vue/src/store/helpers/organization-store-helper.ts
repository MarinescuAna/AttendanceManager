
import { OrganizationViewModel } from "@/modules/organization";
import { DepartmentModule, UpdateDepartmentModule } from "@/modules/organization/departments";
import { SpecializationCreateParamsModule, SpecializationModule } from "@/modules/organization/specializations";
import { ResponseModule } from "@/shared/modules";
import { Store } from "vuex";
import { namespace as organizationNamespace } from "../modules/organization";

export class OrganizationStore {
    private store: Store<any>;

    constructor(store: Store<any>) {
        this.store = store;
    }

    /**
     * Getter for fetching all the departments and specializations from the store, not from the API
     */
    public get organizations(): OrganizationViewModel[] {
        return this.store.getters[`${organizationNamespace}/organizations`];
    }

    /**
     * Getter for taking the number of organisations
     */
    public get organisationsExists(): number {
        return this.store.getters[`${organizationNamespace}/organisationsExists`];
    }

    /**
     * Getter for fetching only the departments, without no information about the specializations from the store
     */
    public get departments(): DepartmentModule[] {
        return this.store.getters[`${organizationNamespace}/departments`];
    }

    /**
     * Getter for fetching the specializations by departmentId
     */
    public specializations(departmentId: string): SpecializationModule[] {
        return this.store.getters[`${organizationNamespace}/specializations`](departmentId);
    }

    /**
     * Load all the departments and specializations from the API
     */
    public loadOrganizations(): Promise<OrganizationViewModel[]> {
        return this.store.dispatch(`${organizationNamespace}/loadOrganizations`);
    }

    /**
     * Add a new departments only
     */
    public addDepartment(name: string): Promise<ResponseModule> {
        return this.store.dispatch(`${organizationNamespace}/addDepartment`, name);
    }

    /**
     * Add a new specialziation only
     */
    public addSpecialization(specialization: SpecializationCreateParamsModule): Promise<ResponseModule> {
        return this.store.dispatch(`${organizationNamespace}/addSpecialization`, specialization);
    }

    /**
     * Remove department
     */
    public removeDepartment(department: UpdateDepartmentModule): Promise<ResponseModule> {
        return this.store.dispatch(`${organizationNamespace}/removeDepartment`, department);
    }

    /**
    * Change department name
    */
    public updateDepartmentName(department: UpdateDepartmentModule): Promise<ResponseModule> {
        return this.store.dispatch(`${organizationNamespace}/updateDepartmentName`, department);
    }
}