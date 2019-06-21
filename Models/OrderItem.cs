using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DM106_TF.Models {
    public class OrderItem {
        public int Id { get; set; }

        public int quantidade { get; set; }

        public int ProductId { get; set; } //Foreign Key

        public virtual Product Product { get; set; } // Navigation property

        public int OrderId { get; set; }

    }
}