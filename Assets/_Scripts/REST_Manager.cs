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

    #region Coroutines
    IEnumerator PubliAPIsCoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get(APIs._PublicAPIs);
        yield return www.SendWebRequest();
        
        if(www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;
            
            publicAPIResponse = JsonUtility.FromJson<PublicAPIResponse>(response);

            // UI to enable the panel
            UIManager.instance.ShowPubliAPICanvas(publicAPIResponse);
        }
        else
        {
            // Show some UI Element
            Debug.LogWarning("PUBLIC API Request Failed!!");
        }
    }

    IEnumerator CatFactAPICoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get(APIs._CatFacts);
        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success )
        {
            string response = www.downloadHandler.text;
            catfactAPIResponse = JsonUtility.FromJson<CatFactsAPIResponse>(response);

            // UI to enable the panel
            UIManager.instance.ShowCatFactAPICanvas(catfactAPIResponse);
        }
        else
        {
            // Show some UI Element
            Debug.LogWarning("Cat Fact API Request Failed!!");
        }
    }

    IEnumerator GuessNationalityCoroutine(string nameText)
    {
        string URL = APIs._NationalizeIO + nameText;

        UnityWebRequest www = UnityWebRequest.Get(URL.ToString());
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;
            nationalityAPIResponse = JsonUtility.FromJson<NationalityAPIResponse>(response);

            UIManager.instance.ShowNationality(nationalityAPIResponse);

            //Debug.Log(response);
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
        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;
            KnowYourIP knowYourIP = JsonUtility.FromJson<KnowYourIP>(response);
            UIManager.instance.ShowYourIP(knowYourIP);
        }
        else
        {
            // Show some UI Element
            Debug.LogWarning("IP API Request Failed!!");
        }
    }

    IEnumerator RandomDogImageCoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get(APIs._Dogs);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;
            RandomDogImageResponse randomDogImageResponse = JsonUtility.FromJson<RandomDogImageResponse>(response);
            
            StartCoroutine(DownloadImage(randomDogImageResponse.message));
        }
        else
        {
            // Show some UI Element
            Debug.LogWarning("DOG API Request Failed!!");
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
        }
        else
        {
            Debug.Log("Failed to download Image");
        }
    }

    #endregion
}
