# 🍔 Good Hamburger API

API REST para gerenciamento de pedidos de uma lanchonete, desenvolvida como desafio técnico para a STgenetics.

---

## 🚀 Como executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Rodando o projeto

```bash
# Clone o repositório
git clone https://github.com/AnaCarlaG/challenge-good-hamburger.git
cd challenge-good-hamburger

# Execute a API
cd good_hamburger
dotnet run
```

A API estará disponível em `https://localhost:44330` (ou a porta exibida no terminal).  
O Swagger estará disponível em `https://localhost:44330/swagger`.

> As migrations são aplicadas automaticamente na inicialização — não é necessário rodar `dotnet ef` manualmente.

---

## 📋 Endpoints

### Cardápio

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/cardapio` | Lista todos os itens do cardápio |

### Pedidos

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/pedido` | Lista todos os pedidos |
| GET | `/api/pedido/{id}` | Busca pedido por ID |
| POST | `/api/pedido` | Cria um novo pedido |
| PUT | `/api/pedido/{id}` | Atualiza os itens de um pedido |
| PATCH | `/api/pedido/{id}/status` | Atualiza o status de um pedido |
| DELETE | `/api/pedido/{id}` | Remove um pedido (soft delete) |

### Exemplo de requisição — criar pedido

```json
POST /api/pedido
{
  "itens": ["XBacon", "BatatFrita", "Refrigerante"]
}
```

### Exemplo de resposta

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "Pendente",
  "itens": [
    { "nome": "X Bacon", "categoria": "Sanduiche", "preco": 7.00 },
    { "nome": "Batata Frita", "categoria": "Acompanhamento", "preco": 2.00 },
    { "nome": "Refrigerante", "categoria": "Bebida", "preco": 2.50 }
  ],
  "subtotal": 11.50,
  "percentualDesconto": 20,
  "desconto": 2.30,
  "total": 9.20,
  "createdAt": "2025-04-23T00:00:00Z",
  "updatedAt": null
}
```

---

## 🧾 Cardápio e regras de desconto

### Sanduíches
| Item | Preço |
|------|-------|
| X Burger | R$ 5,00 |
| X Egg | R$ 4,50 |
| X Bacon | R$ 7,00 |

### Acompanhamentos
| Item | Preço |
|------|-------|
| Batata Frita | R$ 2,00 |
| Refrigerante | R$ 2,50 |

### Descontos aplicados automaticamente
| Combinação | Desconto |
|------------|----------|
| Sanduíche + Batata + Refrigerante | 20% |
| Sanduíche + Refrigerante | 15% |
| Sanduíche + Batata | 10% |

> Cada pedido aceita no máximo um item por categoria. Itens duplicados retornam erro `422 Unprocessable Entity`.

---

## 🏗️ Arquitetura

O projeto segue **Clean Architecture** com separação em 4 camadas:

```
challenge-good-hamburger/
├── Domain/              # Entidades, enums, interfaces, exceções e regras de negócio
│   ├── Entities/        # Pedido, ItemPedido, ItemCardapio (herdam de BaseEntity)
│   ├── Enum/           # TipoItem, CategoriaItem, StatusPedido
│   ├── Exceptions/      # PedidoInvalidoException, NotFoundException
│   ├── Interfaces/      # IBaseRepository, IPedidoRepository, IItemCardapioRepository
│   └── Util/           # CalculadoraDesconto
│
├── Application/         # Casos de uso, DTOs e mapeamento
│   ├── DTOs/            # Requests e Responses
│   ├── Mappers/         # PedidoMapper, ItemCardapioMapper
│   └── Services/        # PedidoService, ItemCardapioService
        ── Interfaces/      # IPedidoService, IItemCardapioService
│
├── Infrastructure/      # Implementação técnica
│   ├── Context/         # AppDbContext
│   ├── Migrations/      # Migrations do EF Core
│   └── Repositories/    # BaseRepository, PedidoRepository, ItemCardapioRepository
│
└── good_hamburger/      # API
    ├── Controllers/     # PedidoController, CardapioController
    └── Program.cs       # Configuração, DI e middleware
```

---

## 🔧 Decisões técnicas

**Minimal API vs Controllers**  
Optei por Controllers com `[ApiController]` por oferecer melhor organização e validação automática do ModelState, além de ser mais familiar para projetos em equipe.

**SQLite como banco de dados**  
Escolhido por não exigir instalação de infraestrutura externa, mantendo a execução simples com um único `dotnet run`. O arquivo `good_hamburger.db` é gerado automaticamente na inicialização.

**Soft Delete**  
Todos os registros possuem `DeletedAt` na `BaseEntity`. Deletar um pedido não remove o registro do banco — apenas preenche esse campo. O `HasQueryFilter` no EF Core garante que registros deletados são filtrados automaticamente em todas as consultas.

**Seed do cardápio no banco**  
Os itens do cardápio são persistidos no banco de dados via `HasData` com IDs e preços fixos. Isso permite que preços e itens sejam atualizados sem necessidade de redeploy da aplicação.

**Result pattern nas exceções**  
Exceções de domínio (`PedidoInvalidoException`, `NotFoundException`) são capturadas por um middleware global no `Program.cs` e convertidas em respostas HTTP padronizadas (`422` e `404` respectivamente), sem try/catch espalhado pelo código.

**DTOs de response**  
As entidades nunca são expostas diretamente na API. O mapeamento para DTOs na camada Application evita referências circulares na serialização e desacopla o contrato da API do modelo de dados interno.

---

## 🔜 O que ficou de fora

- **Frontend Blazor** — em desenvolvimento
- **Testes automatizados** — planejados para a `CalculadoraDesconto` e services
- **Autenticação** — fora do escopo do desafio

---

## 🛠️ Tecnologias

- .NET 8 / ASP.NET Core
- Entity Framework Core 8 + SQLite
- Swagger / OpenAPI (Swashbuckle)