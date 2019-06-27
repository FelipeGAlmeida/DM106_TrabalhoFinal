using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DM106_TF.Models;
using DM106_TF.br.com.correios.ws;
using DM106_TF.CRMClient;

namespace DM106_TF.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private DM106_TFContext db = new DM106_TFContext();

        // GET DO WEB SERVICE CRM
        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("cep")]
        public IHttpActionResult ObtemCEP() {
            CRMRestClient crmClient = new CRMRestClient();
            Customer customer = crmClient.GetCustomerByEmail(User.Identity.Name);
            if (customer != null) {
                return Ok(customer.zip);
            } else {
                return BadRequest("Falha ao consultar o CRM");
            }
        }

        // GET DO WEB SERVICE WSDL DOS CORREIOS
        [Authorize]
        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("frete")]
        public IHttpActionResult CalculaFrete(int id) {

            Order order = db.Orders.Find(id);
            if (order == null) {
                return NotFound();
            }

            if (!order.userName.Equals(User.Identity.Name) &&
                User.IsInRole("USER")) {
                return Unauthorized();
            }

            if(order.OrderItems.Count == 0) {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            if(order.status != "Novo") {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            //Busca o cep pelo e-mail
            CRMRestClient crmClient = new CRMRestClient();
            Customer customer = crmClient.GetCustomerByEmail(order.userName);
            if (customer == null) {
                return BadRequest("Falha ao consultar o CRM");
            }
            
            order.setPesoPedido(); //Função que efetua a soma dos pesos dos produtos contidos no pedido
            decimal comp = order.getComprimento();
            decimal larg = order.getLargura();
            decimal alt = order.getAltura();
            decimal diam = (decimal) Math.Sqrt(Math.Pow(Decimal.ToDouble(comp), 2) + Math.Pow(Decimal.ToDouble(larg), 2));
            order.setPrecoPedido(); //Função que efetua a soma dos preços dos produtos contidos no pedido

            string frete = "0", prazo = "0";  // O frete será calculado tendo como base o cep de origem sendo do Paraná (59950-000)
            CalcPrecoPrazoWS correios = new CalcPrecoPrazoWS();
            cResultado resultado = correios.CalcPrecoPrazo("", "", "40010", "59950-000", customer.zip, order.peso_pedido.ToString(), 3, comp, alt, larg, diam, "N", order.preco_pedido, "S");

            if (!resultado.Servicos[0].Erro.Equals("0")) {
                return BadRequest("Código do erro: " + resultado.Servicos[0].Erro + " - " + resultado.Servicos[0].MsgErro);
            }

            frete = resultado.Servicos[0].Valor;
            prazo = resultado.Servicos[0].PrazoEntrega;

            // Atualiza o frete recebido e o prazo de entrega previsto supondo que o pedido seja fechado hoje
            order.preco_frete = Decimal.Parse(frete.Replace(",","."));
            order.data_entrega = TimeZoneInfo.ConvertTime(DateTime.Now.AddDays(Double.Parse(prazo)), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString("dd-MM-yyyy HH:mm:ss");

            db.Entry(order).State = EntityState.Modified;

            try {
                db.SaveChanges();
            } catch (DbUpdateConcurrencyException) {
                if (!OrderExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return Ok("Frete calculado no valor de: " + frete + " e entrega estimada em " + prazo + " dia(s)");
        }

        // GET: api/Orders
        [Authorize(Roles = "ADMIN")]
        public List<Order> GetOrders() {
            return db.Orders.Include(order => order.OrderItems).ToList();
        }

        // GET: api/Orders/byuser?user=user5
        [Authorize]
        [ResponseType(typeof(List<Order>))]
        [HttpGet]
        [Route("byuser")]
        public IHttpActionResult GetOrderByUser(string user) {

            if (!user.Equals(User.Identity.Name) &&
                User.IsInRole("USER")) {
                return Unauthorized();
            }

            var orders = db.Orders.Where(o => o.userName == user).ToList();

            if (orders == null) {
                return NotFound();
            }

            return Ok(orders);
        }

        // GET: api/Orders/5
        [Authorize]
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            if (!order.userName.Equals(User.Identity.Name) &&
                User.IsInRole("USER")) {
                return Unauthorized();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        [Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (id != order.Id)
            //{
            //    return BadRequest();
            //}

            Order order = db.Orders.Find(id); //Busca a ordem para ser fechada

            if (order == null) {
                return NotFound();
            }

            if (!order.userName.Equals(User.Identity.Name) &&
                User.IsInRole("USER")) {
                return Unauthorized();
            }

            if(order.preco_frete <= 0) {
                return BadRequest("Calcule o frete antes !");
            }

            order.status = "Fechado";

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Orders
        [Authorize]
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            order.status = "Novo";
            order.peso_pedido = 0;
            order.preco_frete = 0;
            order.preco_pedido = 0;
            order.data_pedido = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString("dd-MM-yyyy HH:mm:ss");
            order.data_entrega = "<Aguardando cálculo do frete>";

            db.Orders.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [Authorize]
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            if (!order.userName.Equals(User.Identity.Name) &&
                User.IsInRole("USER")) {
                return Unauthorized();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}