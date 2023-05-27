/**
 * Headers for total attendances tabele
 */
export const totalAttendancesHeader =[
    {
        text:"Code",
        value: "code",
        align: 'start',
        filterable: false,
        class: "text-left black--text text-h6"
    },
    {
        text: "Fullname",
        value: "userName",
        class: "text-left black--text text-h6"
    },
    {
        text: "Email",
        value: "userId",
        class: "text-left black--text text-h6"
    },
    {
        text: "Lecture Attendances",
        value: "courseAttendances",
        class: "text-left black--text text-h6"
    },
    {
        text: "Laboratory Attendances",
        value: "laboratoryAttendances",
        class: "text-left black--text text-h6"
    },
    {
        text: "Seminary Attendances",
        value: "seminaryAttendances",
        class: "text-left black--text text-h6"
    },
    {
        text: "Bonus Points",
        value: "bonusPoints",
        class: "text-left black--text text-h6"
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