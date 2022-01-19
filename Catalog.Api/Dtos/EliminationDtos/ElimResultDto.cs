namespace Catalog.Api.Dtos
{
    public record ElimResultDto
    {
        public Guid Id { get; init;}
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset  CreatedDate { get; set; }
    } 
        public class CreateElimsBySeason
    {
        public List<List<ElimResultDto>> ElimResults { get; set; }
    }
}