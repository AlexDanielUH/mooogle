namespace MoogleEngine;
public static class Tools
{
    public static string Direccion()
    {
        //obteneoms la ruta a la carpeta con los archivos sobre los que se realizara la busqueda
        string resultado = Directory.GetCurrentDirectory();
        resultado = resultado.Substring(0,resultado.LastIndexOf('\\') + 1) + "Content";
        return resultado;
    }
    public static string[] ExtraePalabras(string consulta)
    {
        consulta = consulta.ToLower();
        string[] resultado = consulta.Split(' ');
        List<string> final = new List<string>();
        foreach (var palabra in resultado)
            if (!String.IsNullOrEmpty(palabra) && !String.IsNullOrWhiteSpace(palabra))
                final.Add(palabra);
        return final.ToArray();
    }

    public static string[] PalabrasSinRepetir(string[] palabras)
    {
        List<string> resultado = new List<string>();
        foreach (var palabra in palabras)
            if (!resultado.Contains(palabra))
                resultado.Add(palabra);
        return resultado.ToArray();
    }
    public static int IndexOf(string palabra, string[] palabras)
    {
        for (int i = 0; i < palabras.Length; i++)
            if (palabras[i] == palabra)
                return i;
        return -1;
    }
    public static SearchItem[] Ordena(SearchItem[] elementos)
    {
        for(int i = 0; i < elementos.Length; i++)
        {
            for(int j = i; j < elementos.Length; j++)
            {
                if(elementos[j].Score > elementos[i].Score)
                {
                    SearchItem temp = elementos[j];
                    elementos[j] = elementos[i];
                    elementos[i] = temp;
                }
            }
        }
        return elementos;
    }
}