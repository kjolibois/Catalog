namespace Catalog.Api.Dtos
{
    public record TotalWeekInfoDto
    {
        public Guid Id { get; init;}
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset  CreatedDate { get; set; }
    } 
}