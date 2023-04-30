namespace AttendanceManager.Application.Dtos
{
    public sealed class DocumentDto: DocumentBaseDto
    {
        public bool IsCreator { get; set; } = false;
    }
}