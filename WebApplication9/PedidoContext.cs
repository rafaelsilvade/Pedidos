using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;


namespace WebApplication9.Models
{
    public class PedidoContext : DbContext
    {
     
        public PedidoContext()
            : base("name=PedidoContext")
        {

        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<ItensPedido> ItensPedido { get; set; }

    }
}