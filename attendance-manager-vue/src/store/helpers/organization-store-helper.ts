import { DepartmentModule } from "@/shared/modules";
import { Store } from "vuex";
import { namespace as organizationNamespace, SpecializationCreateDTO } from "../modules/organization";
import { DepartmentViewDTO } from "../modules/organization";

export class OrganizationStore {
    private store: Store<any>;

    constructor(store: Store<any>) {
        this.store = store;
    }

    /**
     * Getter for fetching all the departments and specializations
     * Those departments are the ones that exist into the store
     */
    public get departments(): DepartmentViewDTO[] {
        return this.store.getters[`${organizationNamespace}/departments`];
    }

    /**
     * Getter for fetching only the departments, without no information about the specializations
     */
    public get departmentsOnly(): DepartmentModule[]{
        return this.store.getters[`${organizationNamespace}/departmentsOnly`];
    }

    /**
     * Load all the departments from the API
     */
    public loadDepartments(): Promise<DepartmentViewDTO[]>{
        return this.store.dispatch(`${organizationNamespace}/loadDepartments`);
    }

    /**
     * Add a new departments only
     */
    public addDepartment(name: string): Promise<boolean>{
        return this.store.dispatch(`${organizationNamespace}/addDepartment`,name);
    }

    /**
     * Add a new specialziation only
     */
    public addSpecialization(specialization: SpecializationCreateDTO): Promise<boolean>{
        return this.store.dispatch(`${organizationNamespace}/addSpecialization`,specialization);
    }

}