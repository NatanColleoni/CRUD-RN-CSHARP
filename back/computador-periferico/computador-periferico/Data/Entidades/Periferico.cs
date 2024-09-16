namespace computador_periferico.Data.Entidades;

public class Periferico
{
    public int Id { get; set; }
    public string Nome { get; set; }


    //configuração de relacionamento
    public int ComputadorId { get; set; }
    public Computador Computador { get; set; }

    public Periferico()
    {

    }
}
