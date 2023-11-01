using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEditor.Experimental;

public enum PlayerState
{
    Move,
    Select,
}

public class YJ_PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject _playerCanvas;

    [SerializeField]
    GameObject _friend;

    [SerializeField]
    GameObject _friendCanvas;

    [SerializeField]
    GameObject _lrController;

    [SerializeField]
    GameObject _carController;

    float _speed = 10.0f;
    public PlayerState _state;
    Vector3 _currentPos;

    private void Init()
    {
        string title = "오른쪽 조이스틱을 이용하여 앞으로 이동하고\n왼쪽 조이스틱을 이용하여 회전해보자";
        Util.FindChild<UI_Goal>(_playerCanvas, null, true).Init(title);
        _playerCanvas.SetActive(true);
        _state = PlayerState.Move;
    }

    void Start()
    {
        transform.position = new Vector3(1.7f, -0.41f, -9.43f);
        transform.rotation = Quaternion.Euler(0, 180f, 0);

        Invoke("Init", 3f);
    }

    private void LateUpdate()
    {
        switch (_state)
        {
            case PlayerState.Move:
                UpdateMove();
                break;
            case PlayerState.Select:
                OnSelect();
                break;
        }
    }

    private void UpdateMove()
    {
        if (_friend.GetComponent<FriendController>()._state == FriendController.FriendState.Cross)
        {
            transform.position = _currentPos;
            return;
        }

        OnNearNPC();
    }

    private void OnNearNPC()
    {
        Vector3 dir = this.transform.position - _friend.transform.position;
        Vector3 friendDir = _friend.transform.forward.normalized;
        Vector3 playerDir = transform.forward.normalized;

        if (Vector3.Dot(friendDir, playerDir) <= 0 &&  dir.magnitude < 5.0f)
        {
            // this.GetComponent<ActionBasedSnapTurnProvider>().enabled = false;

            // if near NPC, go to NPC
            // player and friend dir
            Vector3 targetDir = _friend.transform.position - transform.position;
            // 목표 위치 도달시, 친구를 바라봄
            if (targetDir.magnitude < 3.0f)
            {
                if (_playerCanvas.activeSelf == true)
                    _playerCanvas.SetActive(false);

                _currentPos = transform.position;
                transform.LookAt(_friend.transform);
                _state = PlayerState.Select;
                return;
            }

            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, targetDir.magnitude);
            transform.position += targetDir.normalized * moveDist;
        }
    }
    

    private void OnSelect()
    {
        transform.position = _currentPos;

        // related select event, even if dialog end, can't move
        if (_lrController.GetComponent<SelectLR>()._state == LREventState.Playing)
            return;
        if (_carController.GetComponent<SelectCar>()._state == CarEventState.Playing)
            return;

        // dialog end -> move!
        if (_friendCanvas.GetComponent<UI_Dialog>()._moveTrigger == true)
        {
            // this.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
            _state = PlayerState.Move;
        }
    }
}
