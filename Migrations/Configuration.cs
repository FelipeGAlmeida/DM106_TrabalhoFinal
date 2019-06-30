namespace DM106_TF.Migrations
{
    using DM106_TF.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DM106_TF.Models.DM106_TFContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DM106_TF.Models.DM106_TFContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Products.AddOrUpdate(
                p => p.Id,
                new Product {
                    Id = 1,
                    nome = "produto 1",
                    descricao = "descrição produto 1",
                    cor = "Verde",
                    modelo = "Modelo1",
                    codigo = "COD1",
                    preco = 10,
                    peso = 1,
                    altura = 18,
                    largura = 20,
                    comprimento = 20,
                    diametro = 10,
                    Url = "www.siecolasystems.com/produto1"
                },
                new Product {
                    Id = 2,
                    nome = "produto 2",
                    codigo = "COD2",
                    descricao = "descrição produto 2",
                    cor = "Vermelho",
                    modelo = "Modelo2",
                    preco = 22,
                    peso = 2,
                    altura = 30,
                    largura = 20,
                    comprimento = 16,
                    diametro = 12,
                    Url = "www.siecolasystems.com/produto2"
                }
            );
        }
    }
}
