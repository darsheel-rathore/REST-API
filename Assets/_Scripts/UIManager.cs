using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    // Canvas Reference
    public GameObject mainMenuCanvas;
    public GameObject publicAPICanvas;
    public GameObject catFactAPICanvas;

    // PUblic API Prefabs
    public GameObject patternPrefab;

    // Cat Fact Ref
    public TextMeshProUGUI catFactText;

    // Instantiate Location
    public GameObject publicAPIPatternPrefabInstantiateLocation;

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
    public void BTN_PublicAPIs()
    {
        REST_Manager.instance.PublicAPIs();
    }

    public void BTN_CatFactsAPI()
    {
        REST_Manager.instance.CatFactAPI();
    }

    public void BTN_MainMenu()
    {
        CloseAllCanvas();
        mainMenuCanvas.SetActive(true);
    }
    #endregion

    private void CloseAllCanvas()
    {
        mainMenuCanvas.SetActive(false);
        publicAPICanvas.SetActive(false);
        catFactAPICanvas.SetActive(false);
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
    #endregion
}
