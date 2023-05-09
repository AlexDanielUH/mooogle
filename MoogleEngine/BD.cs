namespace MoogleEngine;
public class Carpeta
 {  
    public Archivo[] Archivos { get; private set; }
    public Carpeta(string Ruta)
    {
        string[] libros = Directory.EnumerateFiles(Ruta).ToArray();
        Archivos = new Archivo[libros.Length];
        for(int i = 0; i < Archivos.Length; i++)
            Archivos[i] = new Archivo(libros[i]);
    }
    public static Dictionary<string, float> IDF()
{
    Dictionary<string, Dictionary<string, float>> diccionario = Archivo.Control();
    Dictionary<string, float> IDF = new Dictionary<string, float>();
    foreach (var libro in diccionario)
    {
        foreach (var palabra in libro.Value)
        {
            if (IDF.ContainsKey(palabra.Key))
            {
                IDF[palabra.Key]++;
            }
            else
            {
                IDF[palabra.Key] = 1;
            }
        }
    }
     foreach(var palabra in IDF.Keys)
        IDF[palabra] = (float)Math.Log10(diccionario.Count / IDF[palabra]);

    return IDF;
}
     public static float Idf(string palabra)
    {
        Dictionary<string , float> IDF =Carpeta.IDF();
        //si la palabra existe en el conjunto de documentos, devolvemos su idf
        if(IDF.Keys.Contains(palabra.ToLower()))
            return IDF[palabra.ToLower()];
        //sino devolvemos -1
        return -1f;
    }
 }
