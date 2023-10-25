using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YJ_GuideController : MonoBehaviour
{
    [SerializeField]
    GameObject _selectGuide;

    [SerializeField]
    GameObject _moveGuide;

    [SerializeField]
    GameObject _player;

    private void Update()
    {
        PlayerState playerState = _player.GetComponent<YJ_PlayerController>()._state;
        switch (playerState)
        {
            case PlayerState.Move:
                _moveGuide.SetActive(true);
                _selectGuide.SetActive(false);
                break;
            case PlayerState.Select:
                _moveGuide.SetActive(false);
                _selectGuide.SetActive(true);
                break;
        }
    }

}
