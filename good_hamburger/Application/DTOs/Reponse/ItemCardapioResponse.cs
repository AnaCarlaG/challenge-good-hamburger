using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Reponse
{
    public class ItemCardapioResponse
    {
        public Guid Id { get; set; }
        public TipoItem TipoItem { get; set; }
        public CategoriItem CategoriItem { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public bool Ativo { get; set; }
    }
}
