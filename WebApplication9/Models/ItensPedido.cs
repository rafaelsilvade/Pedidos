using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication9.Models
{
    [Table("ItensPedido")]
    public class ItensPedido
    {
        [Key, Column(Order = 0)]
        [Required]
        public int PedidoID { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        public int ProdutoID { get; set; }    

        public int qtde { get; set; }
        
        public double valor { get; set; }

        [ForeignKey("PedidoID")]
        public virtual Pedido pedido { get; set; }
        [ForeignKey("ProdutoID")]
        public virtual Produto produto { get; set; }
    }


}
