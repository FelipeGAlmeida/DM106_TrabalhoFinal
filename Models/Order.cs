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
                peso_pedido += oi.Product.peso * oi.quantidade;
            }
        }

        public void setPrecoPedido() { //Seta o preço do pedido
            preco_pedido = 0;
            foreach (OrderItem oi in OrderItems) {
                preco_pedido += oi.Product.preco * oi.quantidade;
            }
        }

        // O cálculo do tamanho de espaço ocupado pelo pedido será calculado com os pedidos lado a lado em comprimento.
        // Por esse motivo será feita a soma dos comprimentos.
        public decimal getComprimento() {
            decimal comp_total = 0;
            foreach (OrderItem oi in OrderItems) {
                comp_total += oi.Product.comprimento * oi.quantidade;
            }
            return comp_total;
        }

        // A largura será pega apenas a maior, portanto.
        public decimal getLargura() {
            decimal larg_total = 0;
            foreach (OrderItem oi in OrderItems) {
                if (larg_total < oi.Product.altura) larg_total = oi.Product.largura;
            }
            return larg_total;
        }

        // A altura tmabém será pega apenas a maior, portanto.
        public decimal getAltura() {
            decimal alt_total = 0;
            foreach (OrderItem oi in OrderItems) {
                if (alt_total < oi.Product.altura) alt_total = oi.Product.altura;
            }
            return alt_total;
        }

    }
}