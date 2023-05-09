namespace MoogleEngine;

public class SearchItem
{
    public SearchItem(string title, string snippet,float Score)
    {
        this.Title = title;
        this.Snippet = snippet;
        this.Score = Score;
    }

    public string Title { get; private set; }
    //Titulo del documento
    public string Snippet { get; private set; }
    //Porcion del documento que se encontro contenido de la query

    public float Score { get; private set; }
    //Mientras mas alto sea la similitud del coseno mayor sera el score
}
