
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models
{
    [NotMapped]
    public class MappingQty
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class UserModel
    {
        public int Quantity { get; set; }
        public List<ProductModel>? Products { get; set; } = null;
    }

    public class CartModel
    {
        [Key]
        public int Id { get; set; }  

        public int ItemCount { get; set; } = 0;
        public decimal TotalPrice { get; set; } = 0;
        public decimal Discount { get; set; } = 0m;
        public decimal Tax { get; set; } = 0;
        public decimal DeliveryCharges { get; set; } = 100;

        [NotMapped]
        public List<UserModel>? ItemsSelected { get; set; } = new List<UserModel>();
        [NotMapped]
        public List<MappingQty>? Mapping { get; set; } = new List<MappingQty>();

        [Column(TypeName = "varchar(max)")]
        public string? ItemsSelectedJson { get; set; }  

        
    }
}
