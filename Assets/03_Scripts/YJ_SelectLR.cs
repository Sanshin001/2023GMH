using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LREventState
{
    Playing,
    End,
    Rest,
}


public class SelectLR : MonoBehaviour
{
    enum LROption        // 위치에 따라 변하는 것들만 정의함
    {
        FriendTargetBucket,
        LRBucket,
    }

    public int _LRNum = 0;
    public LREventState _state;
    GameObject _LROptions;
    GameObject _left;
    GameObject _right;

    private void Awake()
    {
        _LROptions = GameObject.Find("LROptions");
        _state = LREventState.Rest;
    }

    public void Init()
    {
        FindLRObj((int)LROption.LRBucket).SetActive(true);
        _left = FindLRObj((int)LROption.LRBucket).transform.GetChild(0).gameObject;
        _right = FindLRObj((int)LROption.LRBucket).transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        switch (_state)
        {
            case LREventState.Playing:
                UpdatePlaying();
                break;
            case LREventState.End:
                UpdateEnd();
                break;
            case LREventState.Rest:
                UpdateRest();
                break;
        }
    }

    private void UpdatePlaying()
    {
        // 이벤트 종료
        // ForNextEvent
        bool _leftHand = _left.GetComponent<HandClick>()._handToggle;
        bool _rightHand = _right.GetComponent<HandClick>()._handToggle;

        if (_leftHand && _rightHand)
            _state = LREventState.End;
    }

    private GameObject FindLRObj(int _num)
    {
        GameObject go = _LROptions.transform.GetChild(_num).transform.GetChild(_LRNum).gameObject;
        if (go == null) return null;
        return go;
    }

    private void UpdateEnd()
    {
        _state = LREventState.Rest;
        _LRNum++;
    }

    private void UpdateRest()
    {
        // doing nothing ..
    }

}