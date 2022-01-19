namespace Catalog.Api.Dtos
{
    public record SpecialDto
    {
        public Guid Id { get; init;}
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset  CreatedDate { get; set; }
    } 
}