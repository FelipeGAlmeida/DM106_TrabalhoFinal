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
    public class ProductsController : ApiController {
        private DM106_TFContext db = new DM106_TFContext();

        // GET: api/Products
        [Authorize]
        public IQueryable<Product> GetProducts() {
            return db.Products;
        }

        // GET: api/Products/5
        [Authorize]
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id) {
            Product product = db.Products.Find(id);
            if (product == null) {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Produto não encontrado!"));
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != product.Id) {
                return BadRequest("Id fornecido e do produto diferentes!");
            }

            if (db.Products.Where(p => (p.codigo == product.codigo) && (p.Id != id)).Count() > 0) {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Código já existe!"));
                //return StatusCode(HttpStatusCode.Forbidden);
            }

            if (db.Products.Where(p => (p.modelo == product.modelo) && (p.Id != id)).Count() > 0) {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Modelo já existe!"));
                //return StatusCode(HttpStatusCode.Forbidden);
            }

            db.Entry(product).State = EntityState.Modified;

            try {
                db.SaveChanges();
            } catch (DbUpdateConcurrencyException) {
                if (!ProductExists(id)) {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Produto não encontrado!"));
                } else {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (db.Products.Where(p => p.codigo == product.codigo).Count() > 0) {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Código já existe!"));
                //return StatusCode(HttpStatusCode.Forbidden);
            }

            if (db.Products.Where(p => p.modelo == product.modelo).Count() > 0) {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Modelo já existe!"));
                //return StatusCode(HttpStatusCode.Forbidden);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id) {
            Product product = db.Products.Find(id);
            if (product == null) {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Produto não encontrado!"));
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id) {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}