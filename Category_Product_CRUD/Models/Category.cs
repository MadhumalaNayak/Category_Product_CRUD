using System.ComponentModel.DataAnnotations;

namespace Category_Product_CRUD.Models
{
    public class Category
    {
        [Key]
        public int  CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
    }
}
