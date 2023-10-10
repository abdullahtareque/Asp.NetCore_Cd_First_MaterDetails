using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core_Master.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
