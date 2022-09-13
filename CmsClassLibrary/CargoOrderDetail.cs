using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmsClassLibrary
{
    public class CargoOrderDetail
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustId { get; set; }

        [ForeignKey("Cargo")]
        public  int CargoId { get; set; }
        



    }
}
