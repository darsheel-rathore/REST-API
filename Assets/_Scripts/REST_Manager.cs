using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class REST_Manager : MonoBehaviour
{
    public static REST_Manager instance;

    [SerializeField] public PublicAPIResponse publicAPIResponse;
    [SerializeField] public CatFactsAPIResponse catfactAPIResponse;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PublicAPIs()
    {
        StartCoroutine(PubliAPIsCoroutine());
    }

    public void CatFactAPI()
    {
        StartCoroutine(CatFactAPICoroutine());
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
    #endregion
}
