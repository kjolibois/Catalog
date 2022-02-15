using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Api.Entities
{
    [Table("BasketItems")]
    public class BasketItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        
        //nav props
        public int BasketId {get;set;}
        public Basket Basket {get;set;}
        
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}