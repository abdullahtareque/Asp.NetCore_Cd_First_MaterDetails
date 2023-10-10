using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core_Master.Models
{
    public class OrderVM
    {
 
        public OrderMaster OrderMaster { get; set; }


        public IFormFile imagefile { get; set; }
    }
}
