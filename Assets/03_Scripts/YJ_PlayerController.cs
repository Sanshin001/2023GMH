using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class YJ_PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject _playerCanvas;

    [SerializeField]
    GameObject _friend;

    void Start()
    {
        string title = "오른쪽 조이스틱을 이용하여 앞으로 이동하고\n왼쪽 조이스틱을 이용하여 회전해보자";
        Util.FindChild<UI_Goal>(_playerCanvas, null, true).Init(title);
        _playerCanvas.SetActive(true);
    }

    private void OnNearNPC()
    {
        Vector3 dir = this.transform.position - _friend.transform.position;
        if (dir.magnitude < 3.0f)
        {

        }
    }

    [SerializeField]
    GameObject _player;
}
