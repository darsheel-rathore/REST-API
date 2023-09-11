using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CloseAllCanvas();
        mainMenuCanvas.SetActive(true);
    }

    #region BTN
    public void BTN_PublicAPIs() => REST_Manager.instance.PublicAPIs();
    public void BTN_CatFactsAPI() => REST_Manager.instance.CatFactAPI();
    public void BTN_MainMenu()
    {
        CloseAllCanvas();
        mainMenuCanvas.SetActive(true);
    }
    public void BTN_GuessNationalityMainMenu()
    {
        CloseAllCanvas();
        guessNationalityCanvas.SetActive(true);
    }
    public void BTN_GuessNationality() => REST_Manager.instance.GuessNationality(nameInputField);
    public void BTN_KnowYourIP() => REST_Manager.instance.KnowYourIP();

    public void BTN_RandomDogImage() => REST_Manager.instance.RandomDogImage();
    #endregion

    private void CloseAllCanvas()
    {
        mainMenuCanvas.SetActive(false);
        publicAPICanvas.SetActive(false);
        catFactAPICanvas.SetActive(false);
        guessNationalityCanvas.SetActive(false);
        knowYourIPCanvas.SetActive(false);
        randomDogImageAPICanvas.SetActive(false);
    }

    #region Main-Menu-Btn-Instruction
    public void ShowPubliAPICanvas(PublicAPIResponse response)
    {
        CloseAllCanvas();
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
        CloseAllCanvas();
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

    public void ShowYourIP(KnowYourIP knowYourIP)
    {
        CloseAllCanvas();
        knowYourIPCanvas.SetActive(true);

        knowYourIPText.text = knowYourIP.ip;
    }

    public void ShowRandomDogImage()
    {
        // Show the image
    }
    #endregion
}
