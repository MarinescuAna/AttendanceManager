namespace AttendanceManager.Domain.Enums
{
    public enum BadgeID
    {
        //students' badges
        FirstAttendance = 1,
        LastAttendance = 2,
        LecturesAttendances50 = 3,
        LaboratoriesAttendances50 = 4,
        SeminariesAttendances50 = 5,
        LecturesAttendancesComplete = 6,
        LaboratoriesAttendancesComplete = 7,
        SeminariesAttendancesComplete = 8,
        FirstBonus = 9,
        SmartOwl = 10,

        //teachers' badges
        FirstCodeGenerated=11,
        FirstCodeUsed=12,
        FullClass=13
    }
}
