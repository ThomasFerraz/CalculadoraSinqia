using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CalculadoraSinqia;

namespace CalculadoraSinqia.Test
{
    public class CalculadoraServiceTests
    {
        private CalculadoraService _service;
        private AppDbContext _context;

        public CalculadoraServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CalculadoraTestDB")
                .Options;

            _context = new AppDbContext(options);

            // Popular o banco de teste
            _context.Cotacao.AddRange(new List<Cotacao>
            {
                new Cotacao { Id = 1, Data = new DateTime(2025, 1, 1), Indexador = "SQI", Valor = 12.00m },
                new Cotacao { Id = 2, Data = new DateTime(2025, 1, 2), Indexador = "SQI", Valor = 12.00m },
                new Cotacao { Id = 3, Data = new DateTime(2025, 1, 3), Indexador = "SQI", Valor = 12.00m }
            });
            _context.SaveChanges();

            var logger = new LoggerFactory().CreateLogger<CalculadoraService>();
            _service = new CalculadoraService(_context, logger);
        }

        [Fact]
        public async Task CalcularAsync_DeveRetornarValorEsperado()
        {
            // Arrange
            var request = new InvestimentoRequest
            {
                ValorInicial = 10000m,
                DataAplicacao = new DateTime(2025, 1, 1),
                DataFinal = new DateTime(2025, 1, 3)
            };

            // Act
            var result = await _service.CalcularAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.FatorJuros > 1);  // Espera-se um fator maior que 1
            Assert.True(result.ValorFinal > request.ValorInicial);  // Espera-se que tenha rendido
        }
    }
}
