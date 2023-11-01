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

    // 외부에서 CarSelectController를 활성화 상태로 변경
    // 활성화되면 이벤트 시작

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
        // 이벤트 종료
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