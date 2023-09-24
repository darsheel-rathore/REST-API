using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Newtonsoft.Json.Linq;

public class UIAnimationManager : MonoBehaviour
{
    public static UIAnimationManager instance;
    public float canvasAnimationDuration = 0.5f;

    [SerializeField] RectTransform mainMenuCanvas;
    [SerializeField] RectTransform publicApiCanvas;
    [SerializeField] RectTransform catFactCanvas;
    [SerializeField] RectTransform nationalityCanvas;
    [SerializeField] RectTransform knowYourIPCanvas;
    [SerializeField] RectTransform randomDogImageCanvas;
    [SerializeField] RectTransform zipcodeCanvas;
    [SerializeField] GameObject LoadingPanel;
    [SerializeField] Button[] mainMenuButtons;

    private Dictionary<string, RectTransform> canvasRectDictionary;

    private Vector2 xStartingPos = new Vector2(-1600f, 0);
    private Vector2 yStartingPos = new Vector2(0f, -900f);
    private Vector2 renderArea = Vector2.zero;

    private CanvasGroup mainMenuCanvasGroup;

    private void Awake()
    {
        instance = this;
        canvasRectDictionary = new Dictionary<string, RectTransform>();
        DOTween.Init();
    }

    void Start()
    {
        canvasRectDictionary.Add(publicApiCanvas.gameObject.name, publicApiCanvas);
        canvasRectDictionary.Add(catFactCanvas.gameObject.name, catFactCanvas);
        canvasRectDictionary.Add(nationalityCanvas.gameObject.name, nationalityCanvas);
        canvasRectDictionary.Add(knowYourIPCanvas.gameObject.name, knowYourIPCanvas);
        canvasRectDictionary.Add(randomDogImageCanvas.gameObject.name, randomDogImageCanvas);
        canvasRectDictionary.Add(zipcodeCanvas.gameObject.name, zipcodeCanvas);

        foreach(var canvas in canvasRectDictionary)
        {
            canvas.Value.anchoredPosition = yStartingPos;
        }

        // Set Initial transform position
        mainMenuCanvas.anchoredPosition = xStartingPos;

        // Set the initial canvas alpha value 0
        mainMenuCanvasGroup = mainMenuCanvas.GetComponent<CanvasGroup>();
        mainMenuCanvasGroup.alpha = 0;

        MainMenuStartUpAnimation();
    }

    public void MainMenuStartUpAnimation()
    {
        ToggleButtonInteractions(false);

        mainMenuCanvas.DOAnchorPos(renderArea, canvasAnimationDuration).OnComplete(() => {
            
            ToggleButtonInteractions(true);
        });

        mainMenuCanvasGroup.DOFade(1f, canvasAnimationDuration);
    }

    public void MainMenuCanvasMoveOutSideRenderArea()
    {
        ToggleButtonInteractions(false);

        mainMenuCanvas.DOAnchorPos(yStartingPos * -1, canvasAnimationDuration).OnComplete(() => {

            ToggleButtonInteractions(true);
            mainMenuCanvas.gameObject.SetActive(false);
        });
    }
    public void MainMenuCanvasMoveToRenderArea() 
    {
        ToggleButtonInteractions(false);
        mainMenuCanvas.gameObject.SetActive(true);

        mainMenuCanvas.DOAnchorPos(renderArea, canvasAnimationDuration).OnComplete(() => {
            mainMenuCanvas.gameObject.SetActive(true);
            ToggleButtonInteractions(true);
        });
    }

    public void OnClickCloseButton()
    {
        foreach (var canvas in canvasRectDictionary)
        {
            if(canvas.Value.gameObject.activeInHierarchy)
            {
                MainMenuCanvasMoveToRenderArea();
                canvas.Value.DOAnchorPos(yStartingPos, canvasAnimationDuration).OnComplete(() => {
                    CloseAllCanvas();
                });
            }
        }
    }
    public void MoveCanvasUp(string name)
    {
        foreach (var canvas in canvasRectDictionary)
        {
            if (string.Equals(name, canvas.Key))
            {
                canvas.Value.DOAnchorPos(renderArea, canvasAnimationDuration);
            }
        }
        MainMenuCanvasMoveOutSideRenderArea();
    }

    public void ToggleButtonInteractions(bool interactionValue = true)
    {
        for(int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].interactable = interactionValue;
        }
    }

    public void CloseAllCanvas()
    {
        LoadingPanel.SetActive(false);
        publicApiCanvas.gameObject.SetActive(false);
        catFactCanvas.gameObject.SetActive(false);
        nationalityCanvas.gameObject.SetActive(false);
        knowYourIPCanvas.gameObject.SetActive(false);
        randomDogImageCanvas.gameObject.SetActive(false);
        zipcodeCanvas.gameObject.SetActive(false);
    }

    // Animation Effects
    public void DoTweenPunchPosition(GameObject gameObject)
    {
        gameObject.GetComponent<RectTransform>().DOPunchAnchorPos(new Vector2(10f, 10f), canvasAnimationDuration);
    }

    public void DoTweenScale(GameObject gameObject)
    {
        gameObject.GetComponent<RectTransform>().DOScale(new Vector2(0f, 0f), canvasAnimationDuration).OnComplete(() => {
            gameObject.SetActive(false);
            gameObject.transform.localScale = Vector3.one;
        });
    }
}
