using Application.DTOs.Reponse;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public static class ItemCardapioMapper
    {
        public static ItemCardapioResponse ToResponse(this ItemCardapio item) => new()
        {
            Id = item.Id,
            TipoItem = item.TipoItem,
            CategoriaItem = item.CategoriaItem,
            Nome = item.Nome,
            Preco = item.Preco,
            Ativo = item.Ativo,
        };
    }
}
