using AttendanceManager.Application.Features.Dashboard.Queries.GetDashboardForDocumentById;
using AttendanceManager.Core.Shared;

namespace AttendanceManager.Application.Features.Dashboard.Helpers
{
    public sealed class ComputeDailyAttendancePercentageFactory
    {
        private Domain.Entities.Document currentDocument;
        private IEnumerable<Domain.Entities.DocumentMember> studentMembers;
        public ComputeDailyAttendancePercentageFactory(Domain.Entities.Document document, IEnumerable<Domain.Entities.DocumentMember> studentMembers)
        {
            currentDocument = document;
            this.studentMembers = studentMembers;
        }
        /// <summary>
        /// Take the number of attendances for each day, divide by the number of students 
        /// that are supposed to attend that course on that day, and then multiply by 100 to get the percentage
        /// </summary>
        /// <returns>compute the percentage of students that attend a course</returns>
        public DailyActivityDto[] Compute()
        {
            var result = new List<DailyActivityDto>();
            var maxAttendancesPerDay = studentMembers.Count();

            if(maxAttendancesPerDay == 0)
            {
                return result.ToArray();
            }

            // iterate through all the collections defined in the current document for a specific type
            foreach(var collection in currentDocument.AttendanceCollections!.OrderByDescending(a=>a.HeldOn))
            {
                // get all the attendances that was marked in that day
                var attendancesReported = CountAttendancesForAttendanceCollectionId(collection.AttendanceCollectionID);

                result.Add(new()
                {
                    AttendanceCollectionId= collection.AttendanceCollectionID,
                    Datetime = collection.HeldOn.ToString(Constants.DateFormat),
                    Percentage = (float)attendancesReported/maxAttendancesPerDay * 100,
                    CourseType = collection.CourseType.ToString()
                });
            }

            return result.ToArray();
        }

        private int CountAttendancesForAttendanceCollectionId(int id)
            => studentMembers.Count(s => s.User!.Attendances!.Any(a => a.IsPresent && a.AttendanceCollectionID == id));
    }
}
