using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication9.Models;

namespace WebApplication9.Controllers
{
    public class PedidoesController : Controller
    {
        private PedidoContext db = new PedidoContext();

        // GET: Pedidoes
        public ActionResult Index(int? id, string pid, int? clienteid, int? numero, DateTime? ini, DateTime? fim)
        {
            int max = 10;
            int pg = 1;
            pg = id.HasValue ? Convert.ToInt32(id) : 1;
            string order = pid;
            var pedido = db.Pedido.Include(p => p.cliente).Include(p => p.itens);
            if (order != null)
                order = order.ToLower();

            if (clienteid.HasValue)
                pedido = pedido.Where(x => x.ClienteID == clienteid);
            if (numero.HasValue)
                pedido = pedido.Where(x => x.PedidoID == numero);
            if (ini != null && fim != null)
                if (fim >= ini)
                  pedido = pedido.Where(x => x.dataEntrega >= ini && x.dataEntrega <= fim);

            switch (order)
            {   
                case "cliente":
                    pedido = pedido.OrderBy(x => x.cliente.Nome);
                    break;
                case "cliente_desc":
                    pedido = pedido.OrderByDescending(x => x.cliente.Nome);
                    break;
                case "data":
                    pedido = pedido.OrderBy(x => x.dataEntrega);
                    break;
                case "data_desc":
                    pedido = pedido.OrderByDescending(x => x.dataEntrega);
                    break;
                case "num_desc":
                    pedido = pedido.OrderByDescending(x => x.PedidoID);
                    break;
                default :
                    pedido = pedido.OrderBy(x => x.PedidoID);
                    break;
            }

            

            double pageCount = (double)((decimal)pedido.Count() / Convert.ToDecimal(max));
            var pedidolist = pedido.Skip((pg - 1) * max).Take(max).ToList();
            ViewBag.pagCount = (int)Math.Ceiling(pageCount);
            ViewBag.cPag = pg;
            ViewBag.order = order;
            Cliente vazio = new Cliente();
            vazio.Nome = "Selecione";
            db.Cliente.Add(vazio);
            ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "Nome");

            return View(pedidolist);
        }

        // GET: Pedidoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // GET: Pedidoes/Create
        public ActionResult Create()
        {
            //var itensPedido = db.ItensPedido.Include(i => i.pedido).Include(i => i.produto).ToList();
            
            ViewBag.ProdutoID = new SelectList(db.Produto, "ProdutoID", "Descricao");
            ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "Nome");
            ViewBag.listItens = null;
            

            return View();
        }

        // POST: Pedidoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PedidoID,dataEntrega,ClienteID")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Pedido.Add(pedido);
                db.SaveChanges();
                return Json(new { Resultado = pedido.PedidoID }, JsonRequestBehavior.AllowGet);
            }

            ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "Nome", pedido.ClienteID);
            return View(pedido);
        }

        // GET: Pedidoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "Nome", pedido.ClienteID);
            return View(pedido);
        }

        // POST: Pedidoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PedidoID,dataEntrega,ClienteID")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "Nome", pedido.ClienteID);
            return View(pedido);
        }

        // GET: Pedidoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedido.Find(id);
            db.Pedido.Remove(pedido);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
