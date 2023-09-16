using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler
{
    private Vector3 originalPosition;
    private float shakeDuration = 0.3f;
    private float shakeStrength = 10f;

    private void Start()
    {
        // Store the button's original position
        originalPosition = transform.position;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Shake the button's position
        transform.DOShakePosition(shakeDuration, shakeStrength);
    }
}
