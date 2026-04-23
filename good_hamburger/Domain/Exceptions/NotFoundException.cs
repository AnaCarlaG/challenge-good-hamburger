using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NotFoundException :Exception
    {
        private string v;

        public NotFoundException(string entidade, Guid id) 
            :base($"{entidade} com id '{id}' não foi encontrado.") { }

        public NotFoundException(string? message, string v) : base(message)
        {
            this.v = v;
        }
    }
}
