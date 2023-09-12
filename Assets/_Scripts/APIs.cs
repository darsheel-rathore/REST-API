using System;

public static class APIs
{
    public const string _PublicAPIs = "https://api.publicapis.org/entries";

    public const string _CatFacts = "https://catfact.ninja/fact";

    public const string _AgifyIO = "https://api.agify.io?name=meelad";

    public const string _GenderizeIO = "https://api.genderize.io?name=luc";

    public const string _NationalizeIO = "https://api.nationalize.io?name=";

    public const string _Dogs = "https://dog.ceo/api/breeds/image/random";

    public const string _Jokes = "https://official-joke-api.appspot.com/random_joke";

    public const string _IP = "https://api.ipify.org?format=json";

    public const string _Zippopotam = "https://api.zippopotam.us/us/";
}

public static class CanvasName
{
    public static string _MainMenuCanvas = UIManager.instance.mainMenuCanvas.name;
    public static string _PubliAPICanvas = UIManager.instance.publicAPICanvas.name;
    public static string _CatFactCanvas = UIManager.instance.catFactAPICanvas.name;
    public static string _NationalityCanvas = UIManager.instance.guessNationalityCanvas.name;
    public static string _KnowYourIPCanvas = UIManager.instance.knowYourIPCanvas.name;
    public static string _RandomDogImageCanvas = UIManager.instance.randomDogImageAPICanvas.name;
}