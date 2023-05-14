/**
 * User roles
 */
export enum Role{
    Admin=0,
    Student=1,
    Teacher=2,
    None=3,
    All=4
}

/**
 * Use this enum for showing the data in the tabel management component
 */
export enum ManagementDataType{
    Users = 0,
    Department = 1,
    Course = 2,
    TotalAttendance = 3,
}

export enum CourseType{
    Lecture = 1,
    Laboratory = 2,
    Seminary = 3
}