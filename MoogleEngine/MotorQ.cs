using System.IO;
namespace MoogleEngine;
public class Motor
{

    Carpeta biblioteca { get; set; }
    string Ruta = @"B:\CC\!Moogle\";
    public Motor()
    {
        biblioteca = new Carpeta(@"B:\CC\!Moogle");
    }
    public Dictionary<string, float> GetTFIDF(string[] query)
    {
        //pasamos todo el contenido a minusculas para facilitar el proceso de busqueda
        string[] array = query;
        string result = string.Join(" ", array).ToLower();
        char[] separadores = { ' ', ',', '.', '{', '}', '(', ')', ';', ':', '"', '`', '~', '_', '\n', '\r', '-', '`', '-', '=', '[', ']', '\\', ';', '\'', ',', '.', '/', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', '{', '}', '|', ':', '"', '<', '>', '?', ' ' };
        string[] contet = result.Split(separadores, StringSplitOptions.RemoveEmptyEntries);

        Dictionary<string, float> TF = new Dictionary<string, float>();
        foreach (string palabra in array)
        {
            if (TF.ContainsKey(palabra))
            {
                TF[palabra]++;
            }
            else
            {
                TF.Add(palabra, 1);
            }
        }

        // Calcular el TF-IDF de cada término en la query
        Dictionary<string, float> TFIDF_Consulta = new Dictionary<string, float>();

        foreach (string palabra in TF.Keys)
        {
            // Obtener la frecuencia del término en la query
            float frequency = TF[palabra];

            // Calcular el TF-IDF del término
            float tf = (float)frequency / query.Length;
            float idf = Carpeta.Idf(palabra);
            float TFIDF_Value = tf * idf;

            // Agregar el término y su valor TF-IDF al diccionario
            TFIDF_Consulta.Add(palabra, TFIDF_Value);
        }

        return TFIDF_Consulta;
    }
    // Definir el método que verifica si las palabras existen en el diccionario
    bool VerificarPalabras(string[] palabras)
    {
        Dictionary<string, Dictionary<string, float>> diccionario = Archivo.Control();
        // Recorrer todos los libros en el diccionario
        foreach (KeyValuePair<string, Dictionary<string, float>> libro in diccionario)
        {
            // Recorrer todas las palabras en el diccionario del libro actual
            foreach (KeyValuePair<string, float> palabra in libro.Value)
            {
                // Verificar si la palabra actual está en el arreglo de palabras a buscar
                if (palabras.Contains(palabra.Key))
                {
                    // Si la palabra está en el arreglo, retornar true
                    return true;
                }
            }
        }
        // Si no se encontró ninguna palabra en el diccionario, retornar false
        return false;
    }
    // Función para calcular el coeficiente de similitud del coseno entre dos diccionarios
    public (float, string)[] CalcularSimilitud(string[] query)
    {
        int k = 0;
        Dictionary<string, Dictionary<string, float>> TF_IDF = Archivo.TF_IDF();
        //diccionario de la query
        Dictionary<string, float> TF_IDFQuery = GetTFIDF(query);
        (float score, string titulo)[] similitud = new (float, string)[TF_IDF.Keys.Count];

        Console.WriteLine(TF_IDF.Count);
        int x = 0;

        foreach (var Interno in TF_IDF)
        {
            List<string> intersepcionQuery_Doc = new List<string>();

            foreach (var par in TF_IDFQuery)
            {
                if (Interno.Value.ContainsKey(par.Key))
                    intersepcionQuery_Doc.Add(par.Key);
            }
            // Obtener las palabras únicas de los dos diccionarios Este código de C# crea una nueva lista llamada "palabras", que es la llaves de dos objetos de tipo diccionario llamados "dict1" y "dict2".

            List<float> vector1 = new List<float>();
            List<float> vector2 = new List<float>();
            foreach (var i in intersepcionQuery_Doc)
            {
                vector1.Add(TF_IDFQuery[i]);// Crear dos vectores de valores de TF-IDF para las palabras únicas
                vector2.Add(Interno.Value[i]);
            }
            // Calcular el producto escalar de los dos vectores
            float productoEscalar = 0.0f;

            /*  */
            x += intersepcionQuery_Doc.Count;

            for (int i = 0; i < intersepcionQuery_Doc.Count; i++)
            {
                productoEscalar += vector1[i] * vector2[i];
            }

            // Calcular las magnitudes de los dos vectores
            float magnitud1 = (float)Math.Sqrt(vector1.Sum(valor => (valor * valor)));
            float magnitud2 = (float)Math.Sqrt(vector2.Sum(valor => (valor * valor)));

            // Calcular el coeficiente de similitud del coseno
            if (magnitud1 * magnitud2 != 0)
                similitud[k++] = (productoEscalar / (magnitud1 * magnitud2), Interno.Key);
            else
                similitud[k++] = (0, Interno.Key);

        }

        return similitud;

    }


    public SearchItem[] Resultados(string[] palabras)
    {
        List<SearchItem> resultado = new List<SearchItem>();

        (float score, string titulo)[] similitudes = CalcularSimilitud(palabras);
        for (int i = similitudes.Length - 1; i >= 0; i--)
        {
            if (similitudes[i].score != 0)
            {
                StreamReader lector = new StreamReader(Ruta + @"Content\\"+ similitudes[i].titulo + ".txt");
                string contenido = lector.ReadToEnd();
                contenido = contenido.Substring(0,Math.Min(500,contenido.Length));
                resultado.Add(new SearchItem(similitudes[i].titulo, contenido,similitudes[i].score));
            }
        }
        SearchItem[] resultados = resultado.ToArray();
        return Tools.Ordena(resultados);
    }


    public SearchResult Busca(string consulta)
    {
        string[] palabras = Tools.ExtraePalabras(consulta);
        
        return new SearchResult(Resultados(palabras));
    }
}