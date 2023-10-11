using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendController : MonoBehaviour
{
    [SerializeField]
    GameObject _player;

    [SerializeField]
    Vector3 _delta;     // _player와 유지할 거리

    [SerializeField]
    float _speed = 12.0f;

    [SerializeField]
    GameObject _myCanvas;

    [SerializeField]
    GameObject _goalPanel;

    Queue<GameObject> _targetObj = new Queue<GameObject>();   // 목표 위치에 있는 GameObject들    
    Vector3 _currentDestPos;

    bool _isFirstUI = true;
    bool _isLastUI = false;

    FriendState _state = FriendState.Idle;

    public enum FriendState
    {
        End,
        Idle,
        Moving,
        Max
    }

    void UpdateEnd()
    {
        // 아무것도 안함
    }

    void UpdateMoving()
    {
        // Animation
        Animator anim = GetComponent<Animator>();

        // friend and target dir
        Vector3 targetDir = _currentDestPos - transform.position;

        // friend and player dir
        Vector3 playerDir = transform.position - _player.transform.position;

        // 목표 위치 도달 flow
        // 목표한 사정거리 내 진입시,
        if (targetDir.magnitude < 3.0f)
        {
            // 친구는 플레이어가 아니라 목표 위치를 기준으로 이동
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, targetDir.magnitude);
            transform.position += targetDir.normalized * moveDist;

            // 목표 위치 도달시, 움직임을 멈추고 플레이어를 바라봄
            if (targetDir.magnitude < 0.001f && !_isLastUI)
                OnTargetArrived();
        }
        else
        {
            // target을 향해 이동
            // player와 거리가 멀면 속도 감소
            if (playerDir.magnitude >= 12.0f)
                _speed -= 1.0f;
            // player와 거리가 너무 가까우면 속도 원상 복구
            if (playerDir.magnitude <= 6.0f)
                _speed = 12.0f;

            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, targetDir.magnitude);
            transform.position += targetDir.normalized * moveDist;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDir), 20 * Time.deltaTime);
        }

        anim.SetFloat("speed", _speed);
    }

    void UpdateIdle()
    {
        // Animation
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);

        if (_myCanvas.GetComponent<UI_Dialog>()._moveTrigger == true)
        {
            ChangeTargetPos();

            anim.SetFloat("speed", _speed);
            _state = FriendState.Moving;            
        }
    }

    void OnTargetArrived() {    // 목표 위치 도달시 처리
        // 애니메이션 멈춤
        _state = FriendState.Idle;

        // Player를 향해 회전
        transform.LookAt(_player.transform);

        // 퀘스트 표시 전 이전 목표 없애기
        _goalPanel.SetActive(false);

        // 퀘스트 표시를 위해 UI도 띄워줌
        _myCanvas.gameObject.SetActive(true);

        // UI 표시시 움직임 멈춤
        _myCanvas.GetComponent<UI_Dialog>()._moveTrigger = false;
    }

    public void ChangeTargetPos()    
    {
        // 다음 타겟이 없을 때
        if (_targetObj.Count == 1)
            return;

        // 처음 UI 끝날때는 target 안바꿈
        if (_isFirstUI)
        {
            _isFirstUI = false;
            return;
        }

        // target 바꾸기
        _targetObj.Dequeue();
        _currentDestPos = _targetObj.Peek().transform.position;

        // 마지막 위치에 도달 시
        if (_targetObj.Count == 1)
            _isLastUI = true;               // UI를 표시하지 않음
    }

    private void Awake()
    {
        _player = GameObject.Find("Player");

        _goalPanel = GameObject.Find("AlwaysGoal");

        GameObject _targetParent = GameObject.Find("TargetBucket");
        for (int i = 0; i < _targetParent.transform.childCount; i++)
        {
            _targetObj.Enqueue(_targetParent.transform.GetChild(i).gameObject);
        }

        _currentDestPos = _targetObj.Peek().transform.position;

        _state = FriendState.Idle;
    }

    // 1. target 위치로 계속 이동
    // 2. player와 너무 멀어지면 Idle 상태로 진입

    void LateUpdate()
    {
        switch (_state)
        {
            case FriendState.End:
                UpdateEnd();
                break;
            case FriendState.Idle:
                UpdateIdle();
                break;
            case FriendState.Moving:
                UpdateMoving();
                break;
        }
    }
}
