using AttendanceManagerGenerator.Modules;
using AttendanceManagerGenerator.Utils;

namespace AttendanceManagerGenerator.Handlers
{
    public sealed class CollectionHandler
    {
        private readonly RequestHandler _handler;
        private static Dictionary<ActivityType,DateTime> dates;
        public CollectionHandler(HttpClient http,bool isFirstSemester)
        {
            _handler = new RequestHandler(http, "collection");

            dates = new Dictionary<ActivityType, DateTime>
            {
                { ActivityType.Lecture, isFirstSemester ? Constants.StartSemester1 : Constants.StartSemester2 },
                { ActivityType.Laboratory, isFirstSemester ? Constants.StartSemester1.AddDays(1) : Constants.StartSemester2.AddDays(1) },
                { ActivityType.Seminary, isFirstSemester ? Constants.StartSemester1.AddDays(2) : Constants.StartSemester2.AddDays(2) }
            };
        }
        public async Task<int?> InsertCollectionAsync(ActivityType courseType)
        {
            dates[courseType] = dates[courseType].AddDays(7);
            var newCollection = new CollectionPostVm() {
                Title = $"G_{courseType}",
                ActivityDateTime = dates[courseType].ToString(Constants.DateFormat),
                CourseType = courseType.ToString()
            };

            return await _handler.PostAsync<CollectionPostVm,int>("create_collection",newCollection);
        }
    }
}
