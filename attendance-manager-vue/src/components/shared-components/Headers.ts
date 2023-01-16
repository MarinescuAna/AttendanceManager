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
        value: "email",
    },
    {
        text: "Role",
        value: "role",
    },
];
/**
 * Headers for tabel used to display departments and specializations
 */
export const DepartmentsHeader: HeaderModule[] = [
    {
        text: "Department Name",
        value: "name",
    }
];
/**
 * Headers for tabel used to display courses
 */
export const CoursesHeader: HeaderModule[] = [
    {
        text: "Course Name",
        value: "name",
    },
    {
        text: "Specialization Name",
        value: "specializationName",
    }
];