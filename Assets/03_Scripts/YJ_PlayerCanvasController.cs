using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerCanvasController : ContinuousMoveProviderBase
{
    [SerializeField]
    GameObject _player;

    [SerializeField]
    [Tooltip("The Input System Action that will be used to read Move data from the right hand controller. Must be a Value Vector2 Control.")]
    InputActionProperty m_RightHandMoveAction = new InputActionProperty(new InputAction("Right Hand Move", expectedControlType: "Vector2"));
    /// <summary>
    /// The Input System Action that Unity uses to read Move data from the right hand controller. Must be a <see cref="InputActionType.Value"/> <see cref="Vector2Control"/> Control.
    /// </summary>
    public InputActionProperty rightHandMoveAction
    {
        get => m_RightHandMoveAction;
    }

    protected override Vector2 ReadInput()
    {
        var rightHandValue = m_RightHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
        return rightHandValue;
    }

    void LateUpdate()
    {
        if (_player.GetComponent<YJ_PlayerController>()._state == PlayerState.Select)
            return;

        Vector3 playerPos = _player.transform.localPosition + _player.transform.forward * 3f;
        transform.position = new Vector3(playerPos.x, transform.position.y, playerPos.z);

        if (ReadInput() != Vector2.zero)
            transform.rotation = Quaternion.LookRotation(-_player.transform.forward);
    }
}
