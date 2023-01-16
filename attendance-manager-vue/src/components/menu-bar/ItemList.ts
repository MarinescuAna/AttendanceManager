import { Role } from '@/shared/enums';

// List with all the buttons that appear into the navigation drawer
export const links: MenuItemListModel[] = [
    {
        icon: "mdi-file-cabinet",
        title: "Attendance documents",
        route: "",
        role: Role.Teacher,
        children: [
            {
                title: "Create new document",
                icon: "mdi-plus-box",
                route: "create-document",
                role: Role.Teacher,
            },
            {
                title: "Upload users",
                icon: "mdi-account-arrow-up",
                route: "/",
                role: Role.Admin,
            },
            {
                title: "View created documents",
                icon: "mdi-file-multiple",
                route: "created-documents",
                role: Role.Teacher,
            }
        ]
    },
    {
        icon: "mdi-account-multiple",
        title: "Manage user's accounts",
        route: "",
        role: Role.Admin,
        children: [
            {
                title: "Create single user",
                icon: "mdi-account-plus-outline",
                route: "create-user",
                role: Role.Admin,
            },
            {
                title: "Upload users",
                icon: "mdi-account-arrow-up",
                route: "/",
                role: Role.Admin,
            },
            {
                title: "View users",
                icon: "mdi-account-group-outline",
                route: "users",
                role: Role.Admin,
            }
        ]
    },
    {
        icon: "mdi-folder",
        title: "Manage departments and specialisations",
        route: "department",
        role: Role.Admin,
        children: []
    },
    {
        icon: "mdi-bookshelf",
        title: "Manage courses",
        route: "courses",
        role: Role.Teacher,
        children: []
    },
    {
        icon: "mdi-home",
        title: "Home",
        route: "home",
        role: Role.All,
        children: []
    },
    {
        icon: "mdi-information-variant",
        title: "About",
        route: "about",
        role: Role.All,
        children: []
    }
];

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
}