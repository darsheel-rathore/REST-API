using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformGameObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //transform.DOMove(new Vector3(10f, 0f, 0f), 2f, false);
        transform.DOPunchPosition(new Vector3(5f, 0, 0), 5, 5, 2, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
