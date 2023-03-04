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
/**
 * Headers for total attendances tabele
 */
export const TotalAttendancesHeader: HeaderModule[] =[
    {
        text:"Code",
        value: "code"
    },
    {
        text: "Fullname",
        value: "userName",
    },
    {
        text: "Email",
        value: "userID",
    },
    {
        text: "Course Attendances",
        value: "courseAttendances",
    },
    {
        text: "Laboratory Attendances",
        value: "laboratoryAttendances",
    },
    {
        text: "Seminary Attendances",
        value: "seminaryAttendances",
    },
    {
        text: "Bonus Points",
        value: "bonusPoints",
    },
];
