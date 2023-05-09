namespace MoogleEngine;
public static class Moogle
{
    public static bool Iniciado = false;
    static Motor motor { get; set; }
    public static void Iniciar()
    {
        motor = new Motor();
        Iniciado = true;
    }
    public static SearchResult Query(string query) {
        return motor.Busca(query);
    }
}
