namespace Catalog.Api.Entities
{
    public class Item 
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description {get;set;}
        public Guid Id { get; set;}
        
        public DateTimeOffset  CreatedDate { get; set; }
        
    } 
}