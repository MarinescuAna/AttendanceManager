
using AttendanceManager.Application.Features.Dashboard.Queries.GetDashboardForDocumentById;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Features.Dashboard.Helpers
{
    public sealed class ComputeStudentInterestPerDocumentFactory
    {
        private Dictionary<int, CourseType> attendanceCollectionsType;
        private Domain.Entities.Document currentDocument;
        private IEnumerable<Domain.Entities.DocumentMember> studentMembers;
        public ComputeStudentInterestPerDocumentFactory(Dictionary<int, CourseType> attendanceCollectionsType, Domain.Entities.Document document, IEnumerable<Domain.Entities.DocumentMember> studentMembers)
        {
            this.attendanceCollectionsType = attendanceCollectionsType;
            currentDocument = document;
            this.studentMembers = studentMembers;
        }

        public DocumentDashboardItemsDto[] Compute()
        {
            var partialyResult = new List<DocumentDashboardItemsDto>();

            // partialy compute student interest
            var maxBonusPoint = ComputePartialyStudentInterest(studentMembers, partialyResult);

            //compute the total possible score for each type of lesson
            (int lesson, int laboratory, int seminary) totalPossibleScore = (
                ComputeTotalPossibleScore(currentDocument!.MaxNoLessons, maxBonusPoint.lesson),
                ComputeTotalPossibleScore(currentDocument!.MaxNoLaboratories, maxBonusPoint.laboratory),
                ComputeTotalPossibleScore(currentDocument!.MaxNoSeminaries, maxBonusPoint.seminary));

            // compute final result
            foreach (var student in partialyResult)
            {
                student.LessonValue = student.LessonValue / totalPossibleScore.lesson * 100;
                student.LaboratoryValue = student.LaboratoryValue / totalPossibleScore.laboratory * 100;
                student.SeminaryValue = student.SeminaryValue / totalPossibleScore.seminary * 100;
            }

            return partialyResult.ToArray();
        }

        /// <summary>
        /// Use this function to compute partialy student interest and also to determine the maximum bonus point for all types of courses
        /// </summary>
        /// <returns>(max_bonus_point_lesson, max_bonus_point_laboratory, max_bonus_point_seminary)</returns>
        private (int lesson, int laboratory, int seminary) ComputePartialyStudentInterest(IEnumerable<Domain.Entities.DocumentMember> studentMembers, List<DocumentDashboardItemsDto> partialyResult)
        {
            (int lesson, int laboratory, int seminary) maxBonusPoints = (0, 0, 0);

            // iterate throught all the students
            foreach (var student in studentMembers)
            {
                // get all the attendances related to this document where the student was present
                var attendances = student.User!.Attendances!.Where(a => a.IsPresent && attendanceCollectionsType!.ContainsKey(a.AttendanceCollectionID));

                // compute the max bonus point for all type of courses in order cu compute the total_possible_score
                var lessonBonusPoints = attendances.Where(a => attendanceCollectionsType![a.AttendanceCollectionID] == CourseType.Lesson).Sum(a => a.BonusPoints);
                if (lessonBonusPoints > maxBonusPoints.lesson)
                {
                    maxBonusPoints.lesson = lessonBonusPoints;
                }

                var laboratoryBonusPoints = attendances.Where(a => attendanceCollectionsType![a.AttendanceCollectionID] == CourseType.Laboratory).Sum(a => a.BonusPoints);
                if (laboratoryBonusPoints > maxBonusPoints.laboratory)
                {
                    maxBonusPoints.laboratory = laboratoryBonusPoints;
                }

                var seminaryBonusPoints = attendances.Where(a => attendanceCollectionsType![a.AttendanceCollectionID] == CourseType.Seminary).Sum(a => a.BonusPoints);
                if (seminaryBonusPoints > maxBonusPoints.seminary)
                {
                    maxBonusPoints.seminary = seminaryBonusPoints;
                }

                // compute the weighted_average for each student to each type of course
                partialyResult.Add(new()
                {
                    Email = student.UserID,
                    StudentName = student.User!.FullName,
                    LessonValue = currentDocument!.AttendanceImportance * attendances.Count(a => attendanceCollectionsType![a.AttendanceCollectionID] == CourseType.Lesson && a.IsPresent)
                        + currentDocument!.BonusPointsImportance * lessonBonusPoints,
                    LaboratoryValue = currentDocument!.AttendanceImportance * attendances.Count(a => attendanceCollectionsType![a.AttendanceCollectionID] == CourseType.Laboratory && a.IsPresent)
                        + currentDocument!.BonusPointsImportance * laboratoryBonusPoints,
                    SeminaryValue = currentDocument!.AttendanceImportance * attendances.Count(a => attendanceCollectionsType![a.AttendanceCollectionID] == CourseType.Laboratory && a.IsPresent)
                        + currentDocument!.BonusPointsImportance * seminaryBonusPoints,
                });
            }

            return maxBonusPoints;
        }

        private int ComputeTotalPossibleScore(int noAttendances, int noBonusPoints)
            => currentDocument!.AttendanceImportance * noAttendances + currentDocument!.BonusPointsImportance * noBonusPoints;

    }
}
