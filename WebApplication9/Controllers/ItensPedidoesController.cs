using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication9.Models;

namespace WebApplication9.Controllers
{
    public class ItensPedidoesController : Controller
    {
        private PedidoContext db = new PedidoContext();

        // GET: ItensPedidoes
        public ActionResult List(int? id)
        {
            var itensPedido = db.ItensPedido.Where(c => c.PedidoID == id).Include(i => i.pedido).Include(i => i.produto);
            return PartialView(itensPedido.ToList());
        }


        // GET: ItensPedidoes/Create
        public ActionResult Create()
        {
           // var itensPedido = db.ItensPedido.Include(i => i.pedido).Include(i => i.produto).ToList();
            ViewBag.ProdutoID = new SelectList(db.Produto, "ProdutoID", "Descricao");
            ViewBag.listItens = null;
           
            return View();
        }

        // POST: ItensPedidoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ProdutoID,PedidoID,qtde")] ItensPedido itensPedido)
        {
            if (ModelState.IsValid)
            {
                ItensPedido Item = db.ItensPedido.Where(x => x.PedidoID == itensPedido.PedidoID && x.ProdutoID == itensPedido.ProdutoID).FirstOrDefault();

                itensPedido.valor = db.Produto.Find(itensPedido.ProdutoID).Valor;
                if (Item != null)
                {
                    if ((Item.qtde + itensPedido.qtde) > 0)
                        Item.qtde += itensPedido.qtde;
                    else
                        Item.qtde = 1;
                    db.Entry(Item).State = EntityState.Modified;
                }
                else
                {
                    if (itensPedido.qtde < 0)
                        itensPedido.qtde = 1;

                    db.ItensPedido.Add(itensPedido);
                   
                }            
                
                db.SaveChanges();
                string total = db.ItensPedido.Where(x => x.PedidoID == itensPedido.PedidoID).Sum(x => (x.valor*x.qtde)).ToString("#.##");
                return Json(new { Resultado = itensPedido.PedidoID, ValorTotal =  total}, JsonRequestBehavior.AllowGet);
            }

            
            ViewBag.ProdutoID = new SelectList(db.Produto, "ProdutoID", "Descricao", itensPedido.ProdutoID);
            return View(itensPedido);
        }

        // POST: ItensPedidoes/Delete/5
        [HttpPost, ActionName("Delete")] 
        public ActionResult Delete(int id, int? pid)
        {
            ItensPedido itensPedido = db.ItensPedido.Find(id, pid);
            if(itensPedido != null) { 
                db.ItensPedido.Remove(itensPedido);
                db.SaveChanges();
            }

            string total = "0.00";
            try { 
            total = db.ItensPedido.Where(x => x.PedidoID == id).Sum(x => (x.valor * x.qtde)).ToString("#.##");
            }
            catch (Exception e) { }
            return Json(new { Resultado = id, ValorTotal = total }, JsonRequestBehavior.AllowGet);
            
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
