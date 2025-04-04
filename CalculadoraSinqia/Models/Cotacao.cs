public class Cotacao
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public required string Indexador { get; set; }
    public decimal Valor { get; set; }
}