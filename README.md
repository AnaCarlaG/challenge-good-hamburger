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
```

#### Executando a API

```bash
cd good_hamburger
dotnet run
```

A API estará disponível em `https://localhost:63305`.  
O Swagger estará disponível em `https://localhost:63305/swagger`.

> As migrations são aplicadas automaticamente na inicialização — não é necessário rodar `dotnet ef` manualmente.

### ⚠️ Caso as migrations não rodem automaticamente
 
Se o banco não for criado ou as migrations não forem aplicadas na inicialização, execute os comandos abaixo na **raiz da solution** via Developer PowerShell:
 
```powershell
# Aplicar as migrations pendentes
dotnet ef database update --project GoodHamburger.Infrastructure --startup-project GoodHamburger.Api
```
 
Se o comando `dotnet ef` não for reconhecido, instale a ferramenta globalmente primeiro:
 
```powershell
dotnet tool install --global dotnet-ef
```
 
Para criar uma nova migration:
 
```powershell
dotnet ef migrations add NomeDaMigration --project GoodHamburger.Infrastructure --startup-project GoodHamburger.Api
```
 
E se precisar recriar o banco do zero:
 
```powershell
# Remove o banco existente (opcional)
del good_hamburger.db
 
# Aplica todas as migrations novamente
dotnet ef database update --project GoodHamburger.Infrastructure --startup-project GoodHamburger.Api
```
 
> O arquivo `good_hamburger.db` será criado na raiz do projeto `GoodHamburger.Api` automaticamente.

#### Executando o Frontend (Blazor WebAssembly)

```bash
cd GoodHamburger.Web
dotnet run
```

O frontend estará disponível em `https://localhost:7143`.

> Certifique-se de rodar a API antes de abrir o frontend.

#### Executando os Testes

```bash
cd Tests.Unit
dotnet test
```

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
| PUT | `/api/pedido/{id}` | Substitui os itens de um pedido |
| PUT | `/api/pedido/{id}/acrescentar` | Acrescenta itens a um pedido existente |
| PATCH | `/api/pedido/{id}/status` | Atualiza o status de um pedido |
| DELETE | `/api/pedido/{id}/{isDeleted}` | Remove um pedido (soft delete) |

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
  "itemPedidos": [
    { "nome": "X Bacon", "tipoItem": "XBacon", "categoriaItem": "Hamburguer", "preco": 7.00 },
    { "nome": "Batata Frita", "tipoItem": "BatatFrita", "categoriaItem": "Acompanhamento", "preco": 2.00 },
    { "nome": "Refrigerante", "tipoItem": "Refrigerante", "categoriaItem": "Bebida", "preco": 2.50 }
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

### Acompanhamentos e Bebidas
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

O projeto segue **Clean Architecture** com separação em camadas:

```
challenge-good-hamburger/
├── Domain/                  # Entidades, enums, interfaces, exceções e regras de negócio
│   ├── Entities/            # Pedido, ItemPedido, ItemCardapio (herdam de BaseEntity)
│   ├── Enum/                # TipoItem, CategoriaItem, StatusPedido
│   ├── Exceptions/          # PedidoInvalidoException, NotFoundException
│   ├── Interfaces/          # IBaseRepository, IPedidoRepository, IItemCardapioRepository
│   └── Util/                # CalculadoraDesconto
│
├── Application/             # Casos de uso, DTOs e mapeamento
│   ├── DTOs/                # Requests e Responses
│   ├── Mappers/             # PedidoMapper, ItemCardapioMapper
│   └── Services/            # PedidoService, ItemCardapioService
│       └── Interfaces/      # IPedidoService, IItemCardapioService
│
├── Infrastructure/          # Implementação técnica
│   ├── Context/             # AppDbContext com seed do cardápio
│   ├── Migrations/          # Migrations do EF Core
│   └── Repositories/        # BaseRepository, PedidoRepository, ItemCardapioRepository
│
├── good_hamburger/          # API
│   ├── Controllers/         # PedidoController, CardapioController
│   └── Program.cs           # Configuração, DI, CORS e middleware de erros
│
├── GoodHamburger.Web/       # Frontend Blazor WebAssembly
│   ├── Models/              # DTOs e enums espelhados da API
│   ├── Pages/               # Index, Pedidos, NovoPedido, EditarPedido, Cardapio
│   ├── Services/            # PedidoApiService (cliente HTTP)
│   └── Shared/              # MainLayout
│
└── Tests.Unit/              # Testes automatizados
    └── PedidoServiceTests/  # Testes do PedidoService
    └── CalculadoraDescontoTests/  # CalculadoraDesconto
```

---

## 🖥️ Frontend (Blazor WebAssembly)

O frontend consome a API via HTTP e oferece as seguintes telas:

| Tela | Rota | Descrição |
|------|------|-----------|
| Início | `/` | Página inicial com atalhos |
| Cardápio | `/cardapio` | Lista itens e regras de desconto |
| Pedidos | `/pedidos` | Lista todos os pedidos com status e ações |
| Novo Pedido | `/pedidos/novo` | Cria um pedido com resumo e desconto em tempo real |
| Editar Pedido | `/pedidos/{id}/editar` | Edita os itens com formulário pré-preenchido |

---

## ✅ Testes automatizados

Testes unitários cobrem:

- **PedidoService** — criar, atualizar, acrescentar, atualizar status e remover, incluindo cenários de erro (item duplicado, recurso não encontrado, repositório retornando nulo)
- **CalculadoraDesconto** — todas as combinações de desconto (20%, 15%, 10%, 0%) e lista vazia

Utilizando **xUnit** e **Moq** para mock dos repositórios.

---

## 🔧 Decisões técnicas

**Minimal API vs Controllers**  
Optei por Controllers com `[ApiController]` por oferecer melhor organização e validação automática do ModelState, além de ser mais familiar para projetos em equipe.

**SQLite como banco de dados**  
Escolhido por não exigir instalação de infraestrutura externa, mantendo a execução simples com um único `dotnet run`. O arquivo `good_hamburger.db` é gerado automaticamente na inicialização.

**Soft Delete**  
Todos os registros possuem `DeletedAt` na `BaseEntity`. Deletar um pedido não remove o registro do banco — apenas preenche esse campo. O `HasQueryFilter` no EF Core garante que registros deletados são filtrados automaticamente em todas as consultas.

**Seed do cardápio no banco**  
Os itens do cardápio são persistidos no banco via `HasData` com IDs e preços fixos. Isso permite que preços e itens sejam atualizados sem necessidade de redeploy da aplicação.

**Exceções de domínio com middleware global**  
`PedidoInvalidoException` e `NotFoundException` são capturadas por um middleware global no `Program.cs` e convertidas em respostas HTTP padronizadas (`422` e `404` respectivamente), sem try/catch espalhado pelo código.

**DTOs de response**  
As entidades nunca são expostas diretamente na API. O mapeamento para DTOs na camada Application evita referências circulares na serialização e desacopla o contrato da API do modelo de dados interno.

**Frontend desacoplado (Blazor WebAssembly)**  
O frontend roda 100% no browser via WebAssembly, consumindo a API via HTTP. Essa abordagem reflete uma arquitetura moderna onde front e back são projetos independentes, com CORS configurado na API para permitir a comunicação local. Os enums são desserializados como string via `JsonStringEnumConverter`, com `JsonPropertyName` para compatibilidade entre os contratos.

**Endpoint `/acrescentar` no frontend**  
O endpoint existe na API e funciona corretamente. No frontend, optou-se por unificar a experiência de edição: a tela de editar pedido já carrega os itens atuais pré-selecionados, permitindo tanto adicionar quanto remover itens em uma única operação via `/atualizar`. O endpoint `/acrescentar` permanece disponível na API para uso direto ou integrações futuras.

---

## 🔜 O que ficou de fora

- **Autenticação** — fora do escopo do desafio
- **Testes de integração** — apenas testes unitários foram implementados
- **Paginação** — a listagem de pedidos retorna todos os registros sem paginação

---

## 🛠️ Tecnologias

- .NET 8 / ASP.NET Core
- Entity Framework Core 8 + SQLite
- Swagger / OpenAPI (Swashbuckle)
- Blazor WebAssembly
- Bootstrap 5 + Bootstrap Icons
- xUnit + Moq (testes)