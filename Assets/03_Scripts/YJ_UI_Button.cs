using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class UI_Button : MonoBehaviour
{
    [SerializeField]
    GameObject _car;

    public void OnButtonClick()
    {
        _car.GetComponent<SelectCar>()._onReportClicked = true;
    }
}
