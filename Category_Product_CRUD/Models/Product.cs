using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Category_Product_CRUD.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set;}
        public int CategoryId{ get; set;}
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

    }
}
