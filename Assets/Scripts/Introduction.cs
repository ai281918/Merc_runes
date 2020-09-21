using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Introduction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject introductionText;

    public void OnPointerEnter(PointerEventData pointerEventData){
        introductionText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData){
        introductionText.SetActive(false);
    }
}
