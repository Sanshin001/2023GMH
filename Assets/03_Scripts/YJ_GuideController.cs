using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YJ_GuideController : MonoBehaviour
{
    enum Controllers
    {
        ControllerRMove,
        ControllerRSelect,
        ControllerRSelectMove,
        ControllerRNothing,
        ControllerLSnap,
        ControllerLNothing,
    }

    FriendController _friendController;

    CrossState _crossState;

    CarEventState _carEventState;

    private void ChangeGuide()
    {
        // friend controller
        // dialog -> 진행중
        if (_friendController._isDialoging)
        {
            ActiveTwoControllers(Controllers.ControllerLNothing, Controllers.ControllerRSelect);
            return;
        }
            
        // CrossState == Playing
        if (_crossState == CrossState.Playing)
        {
            ActiveTwoControllers(Controllers.ControllerLNothing, Controllers.ControllerRSelectMove);
            return;
        }            

        // CrossState == Clear
        if (_crossState == CrossState.Clear)
        {
            ActiveTwoControllers(Controllers.ControllerLSnap, Controllers.ControllerRMove);
            return;
        }            

        // CarEventState
        if (_carEventState == CarEventState.Playing)
        {
            ActiveTwoControllers(Controllers.ControllerLNothing, Controllers.ControllerRSelect);
            return;
        }            

        // 그 외 : dialog -> end
        ActiveTwoControllers(Controllers.ControllerLNothing, Controllers.ControllerRMove);
    }

    private void Update()
    {
        ChangeGuide();
    }

    private void Awake()
    {
        _friendController = GameObject.Find("Frined").GetComponent<FriendController>();
        _crossState = GameObject.Find("@CrossController").GetComponent<CrossController>()._crossState;
        _carEventState = GameObject.Find("TargetCar").GetComponent<SelectCar>()._state;
    }

    private void ActiveTwoControllers(Controllers _left, Controllers _right)
    {
        foreach (Controllers enumItem in Enum.GetValues(typeof(Controllers)))
        {
            if ((int)enumItem == (int)_left || (int)enumItem == (int)_right)
                this.gameObject.transform.GetChild((int)enumItem).gameObject.SetActive(true);
            else
                this.gameObject.transform.GetChild((int)enumItem).gameObject.SetActive(false);
        }
    }
}
