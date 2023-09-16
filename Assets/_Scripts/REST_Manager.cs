using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class REST_Manager
{
    #region Coroutines
    public static IEnumerator FetchAndDeserializeJSON_Coroutine(string url, Action<string> onSuccess, Action<string> onFailure)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);  
        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success )
        {
            string response = www.downloadHandler.text;
            onSuccess(response);
        }
        else
        {
            onFailure("API Request Failed!");
        }
    }  

    public static IEnumerator DownloadTextureFromURL_Coroutine(string URL, Action<Sprite> onSuccess, Action<string> onFailure)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            var imageSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0, 1f));
            
            onSuccess(imageSprite);
        }
        else
        {
            onFailure("Failed To Download Image From URL!");
        }
    }
    #endregion
}
