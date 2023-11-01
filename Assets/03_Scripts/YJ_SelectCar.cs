using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum CarEventState
{
    Playing,
    End,
    Rest,
}

public class SelectCar : MonoBehaviour
{
    [SerializeField]
    GameObject _effect;

    [SerializeField]
    GameObject _button;

    public CarEventState _state;

    public bool _onReportClicked = false;

    private void Start()
    {
        _state = CarEventState.Rest;
    }

    // �ܺο��� CarSelectController�� Ȱ��ȭ ���·� ����
    // Ȱ��ȭ�Ǹ� �̺�Ʈ ����

    private void Update()
    {
        switch (_state)
        {
            case CarEventState.Playing:
                UpdatePlaying();
                break;
            case CarEventState.End:
                UpdateEnd();
                break;
            case CarEventState.Rest:
                UpdateRest();
                break;
        }
    }
    private void UpdatePlaying()
    {
        // �̺�Ʈ ����
        if (_onReportClicked)
            _state = CarEventState.End;
    }

    private void UpdateEnd()
    {
        _effect.GetComponent<ParticleSystem>().Play();
        this.gameObject.SetActive(false);
        _state = CarEventState.Rest;
        return;
    }

    private void UpdateRest()
    {
        return;
    }

    public void ClickCar()
    {
        Destroy(this.GetComponent<XRSimpleInteractable>());
        _button.SetActive(true);
    }
}