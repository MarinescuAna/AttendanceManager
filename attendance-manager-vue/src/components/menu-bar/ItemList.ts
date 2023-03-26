import { Role } from '@/shared/enums';

export class MenuItems{
    // List with all the buttons that appear into the navigation drawer
    private static links: MenuItemListModel[] = [
        {
            icon: "mdi-file-cabinet",
            title: "Attendance documents",
            route: "",
            role: Role.Teacher,
            description:  "",
            children: [
                {
                    title: "Create new document",
                    icon: "mdi-plus-box",
                    route: "create-document",
                    role: Role.Teacher,
                    description: "Create a new document for logging the attendances for a specific course."
                },
                {
                    title: "View documents",
                    icon: "mdi-file-multiple",
                    route: "created-documents",
                    role: Role.Teacher,
                    description: "View all the documents that you created or the documents where you was added as collaborator."
                }
            ]
        },
        {
            icon: "mdi-account-multiple",
            title: "Manage user's accounts",
            route: "",
            role: Role.Admin,
            description: "",
            children: [
                {
                    title: "Create single user",
                    icon: "mdi-account-plus-outline",
                    route: "create-user",
                    role: Role.Admin,
                    description: "Add a new user (student or teacher) into the system."
                },
                {
                    title: "Upload users",
                    icon: "mdi-account-arrow-up",
                    route: "/",
                    role: Role.Admin,
                    description: "Upload a group of users from an excel file."
                },
                {
                    title: "View users",
                    icon: "mdi-account-group-outline",
                    route: "users",
                    role: Role.Admin,
                    description: "View all the users that are defined into the system."
                }
            ]
        },
        {
            title: "Subscribed activity",
            icon: "mdi-file-multiple",
            route: "created-documents",
            role: Role.Student,
            children: [],
            description: "View all the document where you are member."
        },
        {
            icon: "mdi-folder",
            title: "Manage departments and specialisations",
            route: "department",
            role: Role.Admin,
            children: [],
            description: "View all the available departments and specializations, or create a new department or specialization."
        },
        {
            icon: "mdi-bookshelf",
            title: "Manage courses",
            route: "courses",
            role: Role.Teacher,
            children: [],
            description: "View all the courses that you created, or create a new course."
        },
        {
            icon: "mdi-home",
            title: "Home",
            route: "home",
            role: Role.All,
            children: [],
            description: "Home page"
        },
        {
            icon: "mdi-information-variant",
            title: "About",
            route: "about",
            role: Role.All,
            children: [],
            description: "View the details about this site."
        }
    ];
    static getLinkListByRole(role: Role): MenuItemListModel[]{
        const result: MenuItemListModel[] = [];
        this.links.forEach((cr) => {
            if (cr.role == role || cr.role == Role.All) {
              const newLink = cr;
              if (cr.children.length > 0) {
                newLink.children = cr.children.filter(
                  (child) => child.role == role
                );
              }
              result.push(newLink);
            }
          });

        return result;
    }
}

/**
 * Model for the root button
 */
export interface MenuItemListModel extends MenuChildModel {
    children: MenuChildModel[];
}

/**
 * Model for the child button
 */
export interface MenuChildModel {
    icon: string;
    title: string;
    route: string;
    role: Role;
    description: string;
}