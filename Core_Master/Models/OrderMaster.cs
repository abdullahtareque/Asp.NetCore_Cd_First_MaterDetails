using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core_Master.Models
{
    public class OrderMaster
    {
        [Key]
        public int OrderId { get; set; }

        [Display(Name = "Order Note")]
        public string OrderNote { get; set; }

        public DateTime OrderDate { get; set; }

        public string ImagePath { get; set; }

        public bool Terms { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

}
