using System;
using System.Collections.Generic;

[System.Serializable]
public class NationalityInfo
{
    public string country_id;
    public float probability;
}

[System.Serializable]
public class NationalityAPIResponse
{
    public int count;
    public string name;
    public List<NationalityInfo> country;
}

