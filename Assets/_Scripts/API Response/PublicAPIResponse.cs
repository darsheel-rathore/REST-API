using System.Collections.Generic;

[System.Serializable]
public class PublicAPIInfo
{
    public string API;
    public string Description;
    public string Auth;
    public bool HTTP;
    public string Cors;
    public string Link;
    public string Category;
}

[System.Serializable]
public class PublicAPIResponse
{
    public int count;
    public List<PublicAPIInfo> entries;
}
