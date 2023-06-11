/**
 * Use this interface to define the headers for future tabels that use the component ManagementTableComponent
 */
export interface HeaderModule {
    text: string;
    value: string;
}
/**
 * Headers for tabel used to display users
 */
export const UserHeader: HeaderModule[] = [
    {
        text: "Code",
        value: "code",
    },
    {
        text: "Fullname",
        value: "fullname",
    },
    {
        text: "Email",
        value: "id",
    },
    {
        text: "Role",
        value: "role",
    },
];
