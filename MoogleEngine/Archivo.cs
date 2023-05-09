namespace MoogleEngine;
//En esta clase se almacenara todo lo correspondiente al procesamiento de archivos individualmente
//La clase se definde de tipo public para poder ser visible sus metodos fuera de la clase
public class Archivo
{
    string[] palabras_a_buscar { get; set; }
    public string Ruta { get; private set; }
    public float FrInicial { get; private set; }
    public float MaximaFrecuencia { get; private set; }

    public Archivo(string ruta)
    {
        Ruta = ruta;
    }
    public static Dictionary<string,Dictionary<string,float>>  Control()
    {
        // Inicializar el diccionario que almacenará los resultados
        Dictionary<string, Dictionary<string,float>> TF = new Dictionary<string, Dictionary<string,float>>();
        //Obtenemos la ruta a todos los archivos internos a la carpeta dada en la ruta
        //La función `Directory.GetFiles` se utiliza para obtener una lista de todos los archivos de texto en la carpeta que tienen la extensión ".txt"
        string[] fileName = Directory.GetFiles(@"..\Content" ,"*.txt");
        foreach (string archivo in fileName)
        {
            // Diccionario para almacenar los resultados de este archivo
            Dictionary<string, float> ResultArchivo = new Dictionary<string, float>();
            StreamReader reader = new StreamReader(archivo);
            //extraemos el nombre a partir de la ruta
            string nombreArchivo = Path.GetFileNameWithoutExtension(archivo);
            while (!reader.EndOfStream) 
            {
                //pasamos todo el contenido a minusculas para facilitar el proceso de busqueda
                string content = reader.ReadToEnd().ToLower();
                //eliminamos signos comunes 
                char[]separadores= {' ',',','.','{','}','(',')',';',':','"','`','~','_','\n','\r','-','`', '-', '=', '[', ']', '\\', ';', '\'', ',', '.', '/', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', '{', '}', '|', ':', '"', '<', '>', '?', ' '};
                //La sobrecarga de `Split` que incluye `StringSplitOptions.RemoveEmptyEntries` elimina todas las cadenas vacías de la matriz resultante después de dividir la cadena original
                string[] palabras = content.Split(separadores,StringSplitOptions.RemoveEmptyEntries);
                int totalPalabras = palabras.Length;
                //el bucle se ejecutará hasta que se llegue al final del archivo.
                // Calcular la frecuencia de cada palabra
                Dictionary<string, float> Interno = new Dictionary<string, float>();
                foreach(var palabra in palabras)
                {
                    if (Interno.ContainsKey(palabra))
                    {
                        Interno[palabra]++;
                    }
                    else
                    {
                        Interno[palabra] = 1;
                    }
                }

                foreach (KeyValuePair<string, float> par in Interno)
                {
                    float TFValue = (float)par.Value / totalPalabras;
                    Interno[par.Key] = TFValue;
                }
                TF[nombreArchivo] = Interno;
            } 
            reader.Close();
            }           
        return TF;
    
    }
    public string Fragmento
    {
        get
        {
            StreamReader lector = new StreamReader(Ruta);
            string content = lector.ReadToEnd().ToLower();
            lector.Close();
            foreach(var palabra in palabras_a_buscar)
            {
                var indice = content.IndexOf(palabra);
                if(indice > 0)
                    return content.Substring(indice,Math.Min(500,content.Length - indice));
            }
            return content.Substring(0,Math.Min(500,content.Length));
        }
    }

     public static Dictionary<string, Dictionary<string, float>> TF_IDF()
    {
        // Inicializar el diccionario que almacenará los resultados
        Dictionary<string, Dictionary<string,float>> TF_IDF = new Dictionary<string, Dictionary<string,float>>();
        Dictionary<string, Dictionary<string,float>> TF=Control();
        Dictionary<string,float> IDF = Carpeta.IDF();
        
        foreach (string fileName in TF.Keys)
    {
        Dictionary<string, float> DiccInterno = TF[fileName];
        foreach (string word in DiccInterno.Keys)
        {
            float tf = TF[fileName][word];
            // Si la palabra está en el diccionario, la variable "idf" tomará el valor correspondiente al valor asociado con la clave "word". Si la palabra no está en el diccionario, la variable "idf" se asignará a cero.
            float idf = IDF.ContainsKey(word) ? IDF[word] : 0;

            float tfidf = tf * idf;
            
            DiccInterno[word] = tfidf;
        
        }
        TF_IDF[fileName] = DiccInterno;
    }
        return TF_IDF;
    }
}


