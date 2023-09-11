using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class REST_Manager : MonoBehaviour
{
    public static REST_Manager instance;

    [SerializeField] public PublicAPIResponse publicAPIResponse;
    [SerializeField] public CatFactsAPIResponse catfactAPIResponse;
    [SerializeField] public NationalityAPIResponse nationalityAPIResponse;
    [SerializeField] public KnowYourIP knowYourIP;
    [SerializeField] public SearchZipCodeResponse searchZipCodeResponse;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        publicAPIResponse = null;
        catfactAPIResponse = null;
        nationalityAPIResponse = null;
    }
    private void Start()
    {
        
    }

    public void PublicAPIs()
    {
        StartCoroutine(PubliAPIsCoroutine());
    }
    public void CatFactAPI()
    {
        StartCoroutine(CatFactAPICoroutine());
    }
    public void GuessNationality(TMP_InputField nameInputField)
    {
        string nameText = nameInputField.text.Trim();
        StartCoroutine(GuessNationalityCoroutine(nameText));
        
        //CheckString(nameText);
    }
    public void KnowYourIP()
    {
        StartCoroutine(FindYourIPCoroutine());
    }
    public void RandomDogImage()
    {
        StartCoroutine(RandomDogImageCoroutine());
    }
    public void SearchZipCode()
    {
        string zipcode = "33162";
        StartCoroutine(SearchZipCodeCoroutine(zipcode));
    }

    #region Coroutines
    IEnumerator PubliAPIsCoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get(APIs._PublicAPIs);
        UIManager.instance.LoadingPanel(true);
        yield return www.SendWebRequest();
        
        if(www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;
            // Deserialize the JSON response into a C# object
            publicAPIResponse = JsonUtility.FromJson<PublicAPIResponse>(response);

            // UI to enable the panel
            UIManager.instance.ShowPubliAPICanvas(publicAPIResponse);
            UIManager.instance.LoadingPanel(false);
        }
        else
        {
            // Show some UI Element
            Debug.LogWarning("PUBLIC API Request Failed!!");
            UIManager.instance.LoadingPanel(message: "PUBLIC API Request Failed!!");
        }
    }
    IEnumerator CatFactAPICoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get(APIs._CatFacts);
        UIManager.instance.LoadingPanel(true);
        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success )
        {
            string response = www.downloadHandler.text;
            // Deserialize the JSON response into a C# object
            catfactAPIResponse = JsonUtility.FromJson<CatFactsAPIResponse>(response);

            // UI to enable the panel
            UIManager.instance.ShowCatFactAPICanvas(catfactAPIResponse);
            UIManager.instance.LoadingPanel(false);
        }
        else
        {
            // Show some UI Element
            Debug.LogWarning("Cat Fact API Request Failed!!");
            UIManager.instance.LoadingPanel(message: "Cat Fact API Request Failed!!");
        }
    }
    IEnumerator GuessNationalityCoroutine(string nameText)
    {
        string URL = APIs._NationalizeIO + nameText;
        UnityWebRequest www = UnityWebRequest.Get(URL.ToString());
        UIManager.instance.LoadingPanel(true);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;
            // Deserialize the JSON response into a C# object
            nationalityAPIResponse = JsonUtility.FromJson<NationalityAPIResponse>(response);

            UIManager.instance.ShowNationality(nationalityAPIResponse);

            UIManager.instance.LoadingPanel(false);
        }
        else
        {
            // Show some UI Element
            Debug.LogWarning("Nationality API Request Failed!!");
        }
    }   
    IEnumerator FindYourIPCoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get(APIs._IP);
        UIManager.instance.LoadingPanel(true);
        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;
            // Deserialize the JSON response into a C# object
            KnowYourIP knowYourIP = JsonUtility.FromJson<KnowYourIP>(response);
            UIManager.instance.ShowYourIP(knowYourIP);
            UIManager.instance.LoadingPanel(false);
        }
        else
        {
            // Show some UI Element
            UIManager.instance.LoadingPanel(message:"IP API Request Failed!");
            Debug.LogWarning("IP API Request Failed!!");
        }
    }
    IEnumerator RandomDogImageCoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get(APIs._Dogs);
        UIManager.instance.LoadingPanel(true);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;
            // Deserialize the JSON response into a C# object
            RandomDogImageResponse randomDogImageResponse = JsonUtility.FromJson<RandomDogImageResponse>(response);
            
            StartCoroutine(DownloadImage(randomDogImageResponse.message));
        }
        else
        {
            // Show some UI Element
            UIManager.instance.LoadingPanel(message: "Failed to load dog image.");
        }
    }
    IEnumerator DownloadImage(string urlOfDogRandomPicture)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(urlOfDogRandomPicture);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            //var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            var imageSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0, 1f));

            // send this image to the UI
            UIManager.instance.ShowRandomDogImage(imageSprite);

            UIManager.instance.LoadingPanel(false);
        }
        else
        {
            UIManager.instance.LoadingPanel(message: "Failed to download Image");
            Debug.Log("Failed to download Image");
        }
    }
    IEnumerator SearchZipCodeCoroutine(string zipCode)
    {
        string url = APIs._Zippopotam + zipCode;
        UnityWebRequest www = UnityWebRequest.Get(url);

        UIManager.instance.LoadingPanel(true);

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;
            // Deserialize the JSON response into a C# object
            searchZipCodeResponse = JsonUtility.FromJson<SearchZipCodeResponse>(response);

            Debug.Log(response);

            UIManager.instance.LoadingPanel(false);
        }
        else
        {
            UIManager.instance.LoadingPanel(message: "Failed to load the zipcode.");
        }
    }
    #endregion
}
