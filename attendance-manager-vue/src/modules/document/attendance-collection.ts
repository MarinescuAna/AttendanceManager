export interface AttendanceCollectionInsertModule{
    documentId: number;
    activityDateTime: string;
    courseType: string;
}

export interface AttendanceCollectionViewModule{
    attendanceCollectionId: number;
    activityTime: string;
    courseType: string;
}