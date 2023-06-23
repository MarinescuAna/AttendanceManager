/**
 * User roles
 */
export enum Role {
    Admin = 0,
    Student = 1,
    Teacher = 2,
    None = 3,
    All = 4
}

export enum CourseType {
    None = 0,
    Lecture = 1,
    Laboratory = 2,
    Seminary = 3
}

export enum BadgeType {
    CustomAttendanceAchieved = 14,
    CustomBonusPointAchieved = 15,
}

export enum NotificationPriority {
    Info,
    Warning,
    Alert
}