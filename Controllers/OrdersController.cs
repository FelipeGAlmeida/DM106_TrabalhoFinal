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

namespace DM106_TF.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private DM106_TFContext db = new DM106_TFContext();

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