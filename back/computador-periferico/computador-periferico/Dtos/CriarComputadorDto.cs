namespace computador_periferico.Dtos;

public class CriarComputadorDto
{
    public string Nome { get; set; }
    public string Cor { get; set; }
    public DateTime DataFabricacao { get; set; }
    public List<PerifericoDto> Perifericos { get; set; } = [];
}
