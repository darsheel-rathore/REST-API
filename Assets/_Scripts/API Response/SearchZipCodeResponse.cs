using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class PlaceInfo
{
    [JsonProperty("place name")]
    public string place_name;
    public string longitude;
    public string state;
    [JsonProperty("state abbreviation")]
    public string state_abbreviation;
    public string latitude;
}

[System.Serializable]
public class SearchZipCodeResponse
{
    [JsonProperty("post code")]
    public string post_code;
    public string country;
    [JsonProperty("country abbreviation")]
    public string country_abbreviation;
    public List<PlaceInfo> places;
}

