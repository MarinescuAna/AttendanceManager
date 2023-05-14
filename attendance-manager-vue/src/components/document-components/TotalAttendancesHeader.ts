/**
 * Headers for total attendances tabele
 */
export const totalAttendancesHeader =[
    {
        text:"Code",
        value: "code",
        align: 'start',
        filterable: false,
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
        text: "Lecture Attendances",
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

export const studentAttendancesHeader = [
    {
        text: "Id",
        value: "attendanceId",
    },
    {
        text: "Last Update",
        value: "updatedOn",
    },
    {
        text: "Attendance",
        value: "isPresent",
    },
    {
        text: "Bonus Points",
        value: "bonusPoints",
    },
    {
        text: "Activity Type",
        value: "courseType",
    },
];