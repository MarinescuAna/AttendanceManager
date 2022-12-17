namespace AttendanceManager.Application.SharedDtos
{
    public sealed class OrganizationDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required ICollection<SpecializationDto> Children { get; set; }
    }
}