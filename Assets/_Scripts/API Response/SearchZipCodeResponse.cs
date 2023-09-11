using System.Collections.Generic;

[System.Serializable]
public class PlaceInfo
{
    public string place_name;
    public string longitude;
    public string state;
    public string state_abbreviation;
    public string latitude;
}

[System.Serializable]
public class SearchZipCodeResponse
{
    public string post_code;
    public string country;
    public string country_abbreviation;
    public List<PlaceInfo> places;
}

