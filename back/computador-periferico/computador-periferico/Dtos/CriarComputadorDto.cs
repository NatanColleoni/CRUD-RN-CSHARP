namespace computador_periferico.Dtos;

public class CriarComputadorDto
{
    public string nome { get; set; }
    public string cor { get; set; }
    public DateTime dataFabricacao { get; set; }
    public List<PerifericoDto> perifericos { get; set; } = [];
}
