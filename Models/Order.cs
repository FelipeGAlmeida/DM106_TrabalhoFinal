using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DM106_TF.Models {
    public class Order {

        public Order() {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }

        public string userName { get; set; }

        public string data_pedido { get; set; }

        public string data_entrega { get; set; }

        public string status { get; set; } //Novo, Fechado, Cancelado, Entregue

        public decimal preco_pedido { get; set; }

        public decimal peso_pedido { get; set; }

        public decimal preco_frete { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public void setPesoPedido() { //Seta o peso do pedido
            peso_pedido = 0;
            foreach (OrderItem oi in OrderItems) {
                peso_pedido += oi.Product.peso;
            }
        }

        public void setPreçoPedido() { //Seta o preço do pedido
            preco_pedido = 0;
            foreach (OrderItem oi in OrderItems) {
                preco_pedido += oi.Product.preco;
            }
        }

    }
}