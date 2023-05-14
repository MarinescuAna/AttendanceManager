import { Role } from '@/shared/enums';

export class MenuItems{
    // List with all the buttons that appear into the navigation drawer
    private static links: MenuChildModel[] = [
        {
            title: "Create new involvement report",
            icon: "mdi-plus-box",
            route: "create-document",
            role: Role.Teacher,
            description: "Create a new involvement report for logging the activities for a specific course."
        },
        {
            title: "View involvement reports",
            icon: "mdi-file-multiple",
            route: "documents",
            role: Role.Teacher,
            description: "View all the involvement reports that you created or the involvement reports where you was added as collaborator."
        },
        {
            title: "Add single user",
            icon: "mdi-account-plus-outline",
            route: "create-user",
            role: Role.Admin,
            description: "Add a new user (student or teacher) into the system."
        },
        {
            title: "Upload users",
            icon: "mdi-account-arrow-up",
            route: "upload-users",
            role: Role.Admin,
            description: "Upload a group of users from an excel file."
        },
        {
            title: "View users",
            icon: "mdi-account-group-outline",
            route: "users",
            role: Role.Admin,
            description: "View all the users that are defined into the system."
        },
        {
            title: "Subscribed involvement reports",
            icon: "mdi-file-multiple",
            route: "documents",
            role: Role.Student,
            description: "View all the involvement reports where you are member."
        },
        {
            icon: "mdi-folder",
            title: "Departments and specializations",
            route: "department",
            role: Role.Admin,
            description: "View all the available departments and specializations, or create a new department or specialization."
        },
        {
            icon: "mdi-bookshelf",
            title: "Manage courses",
            route: "courses",
            role: Role.Teacher,
            description: "View all the courses that you created, or create a new course."
        },
        {
            icon: "mdi-home",
            title: "Home",
            route: "home",
            role: Role.All,
            description: "Home page"
        },
        {
            icon: "mdi-information-variant",
            title: "About",
            route: "about",
            role: Role.All,
            description: "View the details about this site."
        }
    ];
    static getLinkListByRole(role: Role): MenuChildModel[]{
        return this.links.filter(cr =>  cr.role == role || cr.role == Role.All);
    }
}

/**
 * Model for menu buttons
 */
export interface MenuChildModel {
    icon: string;
    title: string;
    route: string;
    role: Role;
    description: string;
}