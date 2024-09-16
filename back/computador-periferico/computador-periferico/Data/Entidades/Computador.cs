namespace computador_periferico.Data.Entidades;

public class Computador
{
    public int Id {  get; set; }
    public string Nome { get; set; }
    public string Cor {  get; set; }
    public DateTime DataFabricacao { get; set; }

    public ICollection<Periferico> Perifericos { get; set; } = [];

    public Computador()
    {

    }

    public Computador AddPeriferico(Periferico periferico)
    {
        Perifericos.Add(periferico);
        return this;
    }

    public Computador RemovePeriferico(Periferico periferico)
    {
        Perifericos.Remove(periferico);
        return this;
    }
}
