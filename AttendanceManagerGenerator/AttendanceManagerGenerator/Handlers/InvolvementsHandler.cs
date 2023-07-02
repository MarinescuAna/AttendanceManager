using AttendanceManagerGenerator.Modules;
using AttendanceManagerGenerator.Utils;

namespace AttendanceManagerGenerator.Handlers
{
    public sealed class InvolvementsHandler
    {
        private readonly RequestHandler _handler;
        public InvolvementsHandler(HttpClient http)
        {
            _handler = new RequestHandler(http, "involvement");
        }

        private async Task<List<InvolvementGetResponseVm>?> GetAttendancesAsync(int collectionId) 
            => await _handler.GetAsync<List<InvolvementGetResponseVm>>($"involvements?email=&collection_id={collectionId}&use_code=false&current_user=false&only_present=false");

        public async Task InsertInvolvementsAsync(int collectionId)
        {
            var involvements = await GetAttendancesAsync(collectionId) ?? throw new Exception("Involvements not found!");
            var newDate = new InvolvementsPostVm
            {
                Involvements = new InvolvementDto[involvements.Count]
            };

            for (var i = 0; i < involvements.Count; i++)
            {
                var isPresent = RandomGenerator.Next(0, 2) == 0;
                newDate.Involvements[i] = new InvolvementDto()
                {
                    CollectionId = collectionId,
                    UserId = involvements[i].Email,
                    InvolvementId = involvements[i].InvolvementId,
                    BonusPoints = isPresent ? RandomGenerator.Next(0, 3) : 0,
                    IsPresent = isPresent
                };
            }

            newDate.Involvements = newDate.Involvements.Where(u => u.IsPresent).ToArray();
            if (newDate.Involvements.Length > 0)
            { 
                var result = await _handler.PatchAsync<InvolvementsPostVm, bool>("update_student_involvement", newDate);
                if (result)
                {
                    await Console.Out.WriteLineAsync($"Involvements added for collection {collectionId}");
                }
            }
        }
    }
}
