using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject loadingPanel;

    [Header("Main Menu")]
    public GameObject mainMenuCanvas;

    [Header("Public API")]
    public GameObject patternPrefab;
    public GameObject publicAPIPatternPrefabInstantiateLocation;
    public GameObject publicAPICanvas;

    [Header("Cat Facts")]
    public TextMeshProUGUI catFactText;
    public GameObject catFactAPICanvas;

    [Header("Guess Nationality")]
    public TMP_InputField nameInputField;
    public GameObject guessNationalityCanvas;
    public GameObject apiResponsePanel;

    [Header("Know Your IP")]
    public TextMeshProUGUI knowYourIPText;
    public GameObject knowYourIPCanvas;

    [Header("Dog Image")]
    public GameObject randomDogImageAPICanvas;
    public Image randomDogImage;

    [Header("Zipcode")]
    public GameObject zipcodeAPICanvas;
    public TMP_InputField zipcodeInputField;
    public TextMeshProUGUI zipcodeHeader;
    public GameObject zipcodeResponsePanel;
    public GameObject zipcodePatternPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //CloseAllCanvas();
        mainMenuCanvas.SetActive(true);
    }

    #region BTN

    public void BTN_MainMenu()
    {
        mainMenuCanvas.SetActive(true);
        UIAnimationManager.instance.OnClickCloseButton();
    }

    public void BTN_GuessNationalityMainMenu()
    {
        guessNationalityCanvas.SetActive(true);
        UIAnimationManager.instance.MoveCanvasUp(guessNationalityCanvas.name);
    }

    public void BTN_SearchZipCodeMainMenu()
    {
        zipcodeAPICanvas.SetActive(true);
        UIAnimationManager.instance.MoveCanvasUp(zipcodeAPICanvas.name);
    }

    public void BTN_PublicAPIs()
    {
        LoadingPanelToggle(true);
        UIAnimationManager.instance.ToggleButtonInteractions(false);

        StartCoroutine(REST_Manager.FetchAndDeserializeJSON_Coroutine(APIs._PublicAPIs, 
            (onSuccess) => { 
                PublicAPIResponse publicAPIResponse = JsonUtility.FromJson<PublicAPIResponse>(onSuccess);

                ShowPubliAPICanvas(publicAPIResponse);
                LoadingPanelToggle(false);
                UIAnimationManager.instance.MoveCanvasUp(publicAPICanvas.name);
            }, 
            (onFailure) => {
                LoadingPanelToggle(message: onFailure);
            }
            ));
    }

    public void BTN_CatFactsAPI()
    {
        // Enable Loading Panel
        LoadingPanelToggle(true);
        // Toggle Main Menu Interaction Button
        UIAnimationManager.instance.ToggleButtonInteractions(false);

        StartCoroutine(REST_Manager.FetchAndDeserializeJSON_Coroutine(APIs._CatFacts, 
            (onSuccess) => 
            { 
                CatFactsAPIResponse catFactsAPIResponse = JsonUtility.FromJson<CatFactsAPIResponse>(onSuccess);
        
                // Show UI
                ShowCatFactAPICanvas(catFactsAPIResponse);

                // Disable Loading Panel
                LoadingPanelToggle(false);

                // Enable Animation
                UIAnimationManager.instance.MoveCanvasUp(catFactAPICanvas.name);
            }, 
            (onFailure) =>
            {
                // Show Loading/Error Panel
                LoadingPanelToggle(message: onFailure);
            }));
    }

    public void BTN_GuessNationality()
    {
        string nameURL = null;
        string[] randomNames = { "Rohit", "Shubham", "Ishan", "Suraj", "Prince", "Praveen", "Ashu", "Sudarshan", "Daniel", "Gautam"};

        if (string.IsNullOrEmpty(nameInputField.text))
            nameURL = APIs._NationalizeIO + randomNames[Random.Range(0, randomNames.Length)];
        else
            nameURL = APIs._NationalizeIO + nameInputField.text;

        LoadingPanelToggle(true);
        UIAnimationManager.instance.ToggleButtonInteractions(false);

        StartCoroutine(REST_Manager.FetchAndDeserializeJSON_Coroutine(nameURL,
            (onSuccess) =>
            {
                NationalityAPIResponse nationalityAPIResponse = JsonUtility.FromJson<NationalityAPIResponse>(onSuccess);

                ShowNationality(nationalityAPIResponse);

                LoadingPanelToggle(false);
            },
            (onFailure) =>
            {
                LoadingPanelToggle(message: onFailure);
            }));   
    }

    public void BTN_KnowYourIP()
    {
        LoadingPanelToggle(true);
        UIAnimationManager.instance.ToggleButtonInteractions(false);

        StartCoroutine(REST_Manager.FetchAndDeserializeJSON_Coroutine(APIs._IP,
            (onSuccess) => 
            {
                KnowYourIP knowYourIP = JsonUtility.FromJson<KnowYourIP>(onSuccess);
                ShowYourIP(knowYourIP);
                LoadingPanelToggle(false);
                UIAnimationManager.instance.MoveCanvasUp(knowYourIPCanvas.name);
            }, 
            (onFailure) => 
            {
                LoadingPanelToggle(message: onFailure);
            }));
    }

    public void BTN_RandomDogImage()
    {
        LoadingPanelToggle(true);
        UIAnimationManager.instance.ToggleButtonInteractions(false);

        StartCoroutine(REST_Manager.FetchAndDeserializeJSON_Coroutine(APIs._Dogs,
            (onSuccess) => 
            {
                RandomDogImageResponse randomDogImageResponse = JsonUtility.FromJson<RandomDogImageResponse>(onSuccess);

                string textureURL = randomDogImageResponse.message;

                // Download Image
                StartCoroutine(REST_Manager.DownloadTextureFromURL_Coroutine(textureURL,
                    (onSuccess) => 
                    {
                        var randomDogImage = onSuccess;
                        ShowRandomDogImage(randomDogImage);

                        // Show Animation
                        UIAnimationManager.instance.MoveCanvasUp(randomDogImageAPICanvas.name);

                        // Disable loading panel
                        LoadingPanelToggle(false);

                    }, 
                    (onFailure) => 
                    {
                        LoadingPanelToggle(message: onFailure);
                    }));
            }, 
            (onFailure) => 
            {
                LoadingPanelToggle(message: onFailure);
            }));
    }

    public void BTN_SearchZipCode()
    {
        string zipcodeURL = null;
        string[] indianZipCodes = {
                "110001",
                "400001",
                "700001",
                "600001",
                "500001",
                "380001",
                "560001",
                "440001",
                "641001",
                "250001" 
        };

        if (string.IsNullOrEmpty(zipcodeInputField.text))
            zipcodeURL = APIs._Zippopotam + indianZipCodes[Random.Range(0, indianZipCodes.Length)];
        else
            zipcodeURL = APIs._Zippopotam + zipcodeInputField.text;

        LoadingPanelToggle(true);

        StartCoroutine(REST_Manager.FetchAndDeserializeJSON_Coroutine(zipcodeURL,
            onSuccess =>
            {
                SearchZipCodeResponse searchZipCodeResponse = JsonConvert.DeserializeObject<SearchZipCodeResponse>(onSuccess);
                
                
                LoadingPanelToggle(false);
                ShowZipcode(searchZipCodeResponse);
                

                Debug.Log(onSuccess);
            },
            onFailure =>
            {
                LoadingPanelToggle(message: "Zipcode Not Present In Database!");
            }));
    }  
    #endregion

    public void CloseAllCanvas()
    {
        loadingPanel.SetActive(false);
        publicAPICanvas.SetActive(false);
        catFactAPICanvas.SetActive(false);
        guessNationalityCanvas.SetActive(false);
        knowYourIPCanvas.SetActive(false);
        randomDogImageAPICanvas.SetActive(false);

        // Close the loading panel 
        LoadingPanelToggle(false);
    }

    public void LoadingPanelToggle(bool value = false, string message = "Loading...")
    {
        if(loadingPanel.GetComponentInChildren<TextMeshProUGUI>() != null)
            loadingPanel.GetComponentInChildren<TextMeshProUGUI>().text = message;
        
        loadingPanel.SetActive(false);

        if (!string.Equals("Loading...", message))
        {
            if(loadingPanel.GetComponentInChildren<Slider>() != null)
                loadingPanel.GetComponentInChildren<Slider>().gameObject.SetActive(false);
            loadingPanel.SetActive(true);
        }
    }

    #region Main-Menu-Btn-Instruction
    public void ShowPubliAPICanvas(PublicAPIResponse response)
    {
        publicAPICanvas.SetActive(true);

        List<PublicAPIInfo> publicAPIInfo = response.entries;

        int totalIteration = 20;

        for(int i = 0; i < totalIteration; i++)
        {

            // Fill the pattern prefab
            GameObject pattern = Instantiate(patternPrefab, publicAPIPatternPrefabInstantiateLocation.transform);
            pattern.SetActive(false);         

            // Cached the data
            var info = publicAPIInfo[i];

            // Grab the entries
            TextMeshProUGUI[] text = pattern.GetComponentsInChildren<TextMeshProUGUI>();

            // Fill the entries
            text[0].text = (i+1).ToString();
            text[1].text = info.API;
            text[2].text = info.Description;
            text[3].text = info.Auth;
            text[4].text = info.HTTP.ToString();
            text[5].text = info.Cors;
            text[6].text = info.Link;
            text[7].text = info.Category;

            // Enable the Game Object
            pattern.SetActive(true);
            pattern.isStatic = true;
        }
        //Instantiate(patternPrefab, patternPrefab.transform.position, Quaternion.identity, publicAPIPatternPrefabInstantiateLocation.transform);
    }

    public void ShowCatFactAPICanvas(CatFactsAPIResponse response)
    {
        catFactAPICanvas.SetActive(true);
        catFactText.text = response.fact;
    }

    public void ShowNationality(NationalityAPIResponse response)
    {
        TextMeshProUGUI[] text = apiResponsePanel.GetComponentsInChildren<TextMeshProUGUI>();

        text[0].text = $"Name: {response.name}";
        List<NationalityInfo> countryList = response.country;

        for (int i = 0; i < 3; i++) 
        {
            text[i + 2].text = $"Country: {countryList[i].country_id} | Probability: {countryList[i].probability}%";
        }
    }

    public void ShowYourIP(KnowYourIP response)
    {
        knowYourIPCanvas.SetActive(true);
        knowYourIPText.text = response.ip;
    }

    public void ShowRandomDogImage(Sprite response)
    {
        randomDogImageAPICanvas.SetActive(true);
        randomDogImage.sprite = response;
    }

    public void ShowZipcode(SearchZipCodeResponse response)
    {
        if(zipcodePatternPrefab != null && zipcodeResponsePanel != null)
        {
            // Display zipcode heading
            zipcodeHeader.text = $"Post Code: {response.post_code}";

            TextMeshProUGUI[] text = zipcodeResponsePanel.GetComponentsInChildren<TextMeshProUGUI>();

            var iteration = response.places.Count;
            if (response.places.Count > 4)
                iteration = 4;

            for(int i = 0; i < iteration; i++)
            {
                string textPattern = $"{i+1}. Place Name : <b>{response.places[i].place_name}</b> | State : {response.places[i].state}";

                text[i].text = textPattern;
            }
        }
        else
        {
            LoadingPanelToggle(message: "Error In Zipcode Response Panel");
        }
    }
    #endregion
}
