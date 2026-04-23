using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class PedidoInvalidoException : Exception
    {
        private string v;

        public PedidoInvalidoException(string mensagem) :base(mensagem)
        {
        }

        public PedidoInvalidoException(string mensagem, string v) : this(mensagem)
        {
            this.v = v;
        }
    }
}
