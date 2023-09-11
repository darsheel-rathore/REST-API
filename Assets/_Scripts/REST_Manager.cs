using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class REST_Manager : MonoBehaviour
{
    public static REST_Manager instance;

    [SerializeField] public PublicAPIResponse publicAPIResponse;

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
            UIManager.instance.ShowPubliAPIPanel(publicAPIResponse);
        }
        else
        {
            // Show some UI Element
            Debug.LogWarning("PUBLIC API Request Failed!!");
        }
    }
    #endregion
}
