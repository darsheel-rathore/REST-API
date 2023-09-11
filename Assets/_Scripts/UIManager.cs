using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    // Panels
    public GameObject mainMenuPanel;
    public GameObject publicAPIPanel;

    // Prefabs
    public GameObject patternPrefab;

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

    #region Main-Menu-BTN
    public void BTN_PublicAPIs()
    {
        REST_Manager.instance.PublicAPIs();
    }
    #endregion

    private void CloseAllPanel()
    {
        mainMenuPanel.SetActive(false);
        publicAPIPanel.SetActive(false);
    }

    public void ShowPubliAPIPanel(PublicAPIResponse response)
    {
        CloseAllPanel();
        publicAPIPanel.SetActive(true);

        List<PublicAPIInfo> publicAPIInfo = response.entries;

        int totalIteration = 10;

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
}
