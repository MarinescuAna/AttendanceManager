namespace AttendanceManager.Domain.Enums
{
    public enum BadgeType
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
        FullClass=13,
        GoodTeacher = 16,
        SayByeLecture = 17,
        SayByeSeminary = 18,
        SayByeLaboratory = 19,
        BestTeacher = 20,

        //WARNING: those are mapped in vue
        CustomAttendanceAchieved=14,
        CustomBonusPointAchieved=15,
    }
}
