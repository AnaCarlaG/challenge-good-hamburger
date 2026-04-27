using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {}

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItemPedidos { get; set; }
        public DbSet<ItemCardapio> ItemCardapios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pedido>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Id).ValueGeneratedOnAdd();
                e.Property(p => p.Subtotal).HasColumnType("decimal(18,2)");
                e.Property(p => p.PercentualDesconto).HasColumnType("decimal(18,2)"); ;
                e.Property(p => p.Desconto).HasColumnType("decimal(18,2)"); ;
                e.Property(p=> p.Total).HasColumnType("decimal(18,2)"); ;
                e.HasMany(p => p.ItemPedidos)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
                e.HasQueryFilter(p => p.DeletedAt == null);
            });
            modelBuilder.Entity<ItemPedido>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Id).ValueGeneratedOnAdd();
                e.Property(p => p.Nome).IsRequired().HasMaxLength(100);
                e.Property(p => p.Preco).HasColumnType("decimal(18,2)");
                e.HasQueryFilter(p => p.DeletedAt == null);
            });
            modelBuilder.Entity<ItemCardapio>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Id).ValueGeneratedOnAdd();
                e.Property(p => p.Nome).IsRequired().HasMaxLength(100);
                e.Property(p => p.Preco).HasColumnType("decimal(18,2)");
                e.HasQueryFilter(p => p.DeletedAt == null);
                e.HasData(
                    new ItemCardapio
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        Preco = 5.00m,
                        TipoItem = TipoItem.XBurger,
                        CategoriaItem = CategoriaItem.Hamburguer,
                        Nome = "X Burger",
                        Ativo = true,
                        CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    },
                     new ItemCardapio
                     {
                         Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                         Preco = 4.50m,
                         TipoItem = TipoItem.XEgg,
                         CategoriaItem = CategoriaItem.Hamburguer,
                         Nome = "X Egg",
                         Ativo = true,
                         CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                     },
                      new ItemCardapio
                      {
                          Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                          Preco = 7.00m,
                          TipoItem = TipoItem.XBacon,
                          CategoriaItem = CategoriaItem.Hamburguer,
                          Nome = "X Bacon",
                          Ativo = true,
                          CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                      },
                      new ItemCardapio
                      {
                          Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                          Preco = 2.00m,
                          TipoItem = TipoItem.BatatFrita,
                          CategoriaItem = CategoriaItem.Acompanhamento,
                          Nome = "Batata frita",
                          Ativo = true,
                          CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                      },
                      new ItemCardapio
                      {
                          Id = Guid.Parse("00000000-0000-0000-0000-000000000005"),
                          Preco = 2.50m,
                          TipoItem = TipoItem.Refrigerante,
                          CategoriaItem = CategoriaItem.Bebida,
                          Nome = "Refrigerante",
                          Ativo = true,
                          CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                      }
                    );
            });
        }
    }
}
