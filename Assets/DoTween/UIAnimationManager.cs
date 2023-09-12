using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnimationManager : MonoBehaviour
{
    public static UIAnimationManager instance;

    public RectTransform mainMenuCanvas;
    Dictionary<string, RectTransform> canvasRectDictionary;

    private Vector2 xStartingPos = new Vector2(-1600f, 0);
    private Vector2 yStartingPos = new Vector2(0f, -900f);
    private Vector2 renderArea = Vector2.zero;
    public float canvasAnimationDuration = 2f;

    public CanvasGroup mainMenuCanvasGroup;

    Button[] mainMenuButtons;

    private void Awake()
    {
        instance = this;
        canvasRectDictionary = new Dictionary<string, RectTransform>();
    }

    void Start()
    {
        mainMenuCanvas = UIManager.instance.mainMenuCanvas.GetComponent<RectTransform>();

        RectTransform publicApiCanvas = UIManager.instance.publicAPICanvas.GetComponent<RectTransform>();
        RectTransform catFactCanvas = UIManager.instance.catFactAPICanvas.GetComponent<RectTransform>();
        RectTransform nationalityCanvas = UIManager.instance.guessNationalityCanvas.GetComponent<RectTransform>();
        RectTransform knowYourIPCanvas = UIManager.instance.knowYourIPCanvas.GetComponent<RectTransform>();
        RectTransform randomDogImageCanvas = UIManager.instance.randomDogImageAPICanvas.GetComponent<RectTransform>();

        canvasRectDictionary.Add(publicApiCanvas.gameObject.name, publicApiCanvas);
        canvasRectDictionary.Add(catFactCanvas.gameObject.name, catFactCanvas);
        canvasRectDictionary.Add(nationalityCanvas.gameObject.name, nationalityCanvas);
        canvasRectDictionary.Add(knowYourIPCanvas.gameObject.name, knowYourIPCanvas);
        canvasRectDictionary.Add(randomDogImageCanvas.gameObject.name, randomDogImageCanvas);

        foreach(var canvas in canvasRectDictionary)
        {
            canvas.Value.anchoredPosition = yStartingPos;
        }

        // Grab all the buttons
        mainMenuButtons = UIManager.instance.gameObject.GetComponentsInChildren<Button>();

        // Set Initial transform position
        mainMenuCanvas.anchoredPosition = xStartingPos;

        // Set the initial canvas alpha value 0
        mainMenuCanvasGroup = mainMenuCanvas.GetComponent<CanvasGroup>();
        mainMenuCanvasGroup.alpha = 0;

        // Starup Animation
        StartCoroutine(MainMenuStartUpAnimationCoroutine());
    }

    IEnumerator MainMenuStartUpAnimationCoroutine()
    {
        mainMenuCanvas.DOAnchorPos(renderArea, canvasAnimationDuration);
        mainMenuCanvasGroup.DOFade(1f, canvasAnimationDuration);

        ToggleButtonInteractions(false);

        yield return new WaitForSeconds(canvasAnimationDuration);

        ToggleButtonInteractions(true);
    }

    public void MainMenuCanvasMoveOutSideRenderArea()
    {
        StartCoroutine(MainMenuCanvasMoveOutSideRenderAreaCoroutine());
    }
    public void MainMenuCanvasMoveToRenderArea() 
    { 
        StartCoroutine(MainMenuCanvasMoveToRenderAreaCoroutine());
    }

    public void OnClickCloseButton()
    {
        foreach (var canvas in canvasRectDictionary)
        {
            if(canvas.Value.gameObject.activeInHierarchy)
            {
                StartCoroutine(MoveCanvasDown(canvas.Value));
            }
        }
    }

    IEnumerator MoveCanvasDown(RectTransform value)
    {
        value.DOAnchorPos(yStartingPos, canvasAnimationDuration);
        MainMenuCanvasMoveToRenderArea();

        yield return new WaitForSeconds(canvasAnimationDuration);
        
        UIManager.instance.CloseAllCanvas();
    }

    public void MoveCanvasUp(string name)
    {
        foreach(var canvas in canvasRectDictionary)
        {
            if(string.Equals(name, canvas.Key))
            {
                canvas.Value.DOAnchorPos(renderArea, canvasAnimationDuration);
            }
        }
        MainMenuCanvasMoveOutSideRenderArea();
    }

    IEnumerator MainMenuCanvasMoveOutSideRenderAreaCoroutine()
    {
        mainMenuCanvas.DOAnchorPos(new Vector2(0, 900f), canvasAnimationDuration);
        ToggleButtonInteractions(false);
        
        yield return new WaitForSeconds(canvasAnimationDuration);
        
        ToggleButtonInteractions(true);
        mainMenuCanvas.gameObject.SetActive(false);
    }

    IEnumerator MainMenuCanvasMoveToRenderAreaCoroutine()
    {
        mainMenuCanvas.DOAnchorPos(renderArea, canvasAnimationDuration);
        ToggleButtonInteractions(false);
        mainMenuCanvas.gameObject.SetActive(true);

        yield return new WaitForSeconds(canvasAnimationDuration);
        
        mainMenuCanvas.gameObject.SetActive(true);
        ToggleButtonInteractions(true);
    }

    public void ToggleButtonInteractions(bool interactionValue = true)
    {
        for(int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].interactable = interactionValue;
        }
    }
}
