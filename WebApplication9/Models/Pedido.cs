using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebApplication9.Models
{
    [Table("Pedido")]
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PedidoID { get; set; }
        [Required]
        public DateTime dataEntrega { get; set; }
        [Required]
        public int ClienteID { get; set; }

        [ForeignKey("ClienteID")]
        public virtual Cliente cliente { get; set; }

        [ForeignKey("PedidoID")]
        public IList<ItensPedido> itens { get; set; }

        [NotMapped]
        public double? Total { get {
                return this.itens.Sum(i => i.valor * i.qtde);
            }
        }
    }

}