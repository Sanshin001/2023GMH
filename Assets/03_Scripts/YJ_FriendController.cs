using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FriendController : MonoBehaviour
{
    [SerializeField]
    GameObject _player;

    [SerializeField]
    float _speed = 12.0f;

    [SerializeField]
    GameObject _myCanvas;

    [SerializeField]
    GameObject _goalPanel;

    public Queue<GameObject> _targetObj = new Queue<GameObject>();   // 목표 위치에 있는 GameObject들    
    public Vector3 _currentDestPos;

    [SerializeField]
    ArrayList _crossTargetObj;

    [SerializeField]
    GameObject _carTargetObj;

    bool _isFirstUI = true;
    bool _isLastUI = false;

    public FriendState _state = FriendState.Idle;

    GameObject _carEventObj;

    // including cross
    int _targetCount = 0;

    public bool _isDialoging = false;

    public enum FriendState
    {
        End,
        Idle,
        Moving,
        Cross,
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

    bool isNextTargetCrossWalk()
    {
        if (_targetCount == 4 || _targetCount == 11)
            return true;
        return false;
    }

    bool isNextTargetCarEvent()
    {
        if (_targetCount == 7)
            return true;
        return false;
    }

    bool isNextTargetLR()
    {
        if (_targetCount == 2 || _targetCount == 9)
            return true;
        return false;
    }

    void UpdateIdle()
    {
        // Animation
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);

        GameObject crossController = GameObject.Find("@CrossController");

        // game playing or gameover => player cross ing...
        CrossState crossState = crossController.GetComponent<CrossController>()._crossState;
        if (crossState == CrossState.Playing || crossState == CrossState.GameOver)
            return;

        // car click event ing...
        if (_carEventObj.GetComponent<SelectCar>()._state == CarEventState.Playing)
            return;

        GameObject LRController = GameObject.Find("@LRController");
        LREventState lrEventState = LRController.GetComponent<SelectLR>()._state;
        if (lrEventState == LREventState.Playing)
            return;

        // dialog ing..
        _isDialoging = true;

        // dialog end
        if (_myCanvas.GetComponent<UI_Dialog>()._moveTrigger == true)
        {
            ChangeTargetPos();

            // Change State
            // dialog end but next Target is CrossWalk
            if (isNextTargetCrossWalk())
            {
                _state = FriendState.Cross;
                return;
            }

            // dialog end but next Target is Car
            if (isNextTargetCarEvent())
            {
                _state = FriendState.Idle;
                _carEventObj.GetComponent<SelectCar>()._state = CarEventState.Playing;
                return;
            }

            // dialog end but next Target is LR
            if (isNextTargetLR())
            {
                _state = FriendState.Idle;

                LRController.GetComponent<SelectLR>().Init();
                LRController.GetComponent<SelectLR>()._state = LREventState.Playing;
                return;
            }

            anim.SetFloat("speed", _speed);
            _state = FriendState.Moving;
            _isDialoging = false;
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
        // target 수 확인을 위해 증가
        _targetCount++;
        Debug.Log(_targetCount);

        // 다음 타겟이 없을 때
        if (_targetObj.Count == 1)
            return;

        // 처음 UI 끝날때는 target 안바꿈
        if (_isFirstUI)
        {
            _isFirstUI = false;
            return;
        }

        // 다음 타겟이 CrossWalk일 때, target을 crossbucket 내부에 있는 걸로 바꾸기
        if (isNextTargetCrossWalk())
        {
            GameObject go = GameObject.Find("@CrossController");
            int crossNum = go.GetComponent<CrossController>()._crossNum;
            GameObject temp = GameObject.Find("CrossOptions/CrossTargetBucket").transform.GetChild(crossNum).gameObject;
            _currentDestPos = temp.transform.position;
            return;
        }

        // 다음 타겟이 CarEvent일 때, carevent로 바꾸기
        if (isNextTargetCarEvent())
        {
            GameObject go = GameObject.Find("CarEventTarget");
            _currentDestPos = go.transform.position;
            return;
        }

        if (isNextTargetLR())
        {
            GameObject go = GameObject.Find("@LRController");
            int LRNum = go.GetComponent<SelectLR>()._LRNum;
            GameObject temp = GameObject.Find("LROptions/FriendTargetBucket").transform.GetChild(LRNum).gameObject;
            _currentDestPos = temp.transform.position;
            return;
        }

        // target 바꾸기
        _targetObj.Dequeue();
        _currentDestPos = _targetObj.Peek().transform.position;

        // 마지막 위치에 도달 시
        if (_targetObj.Count == 1)
            _isLastUI = true;               // UI를 표시하지 않음
    }

    private void UpdateCross()
    {
        // 이벤트 위치 도달시, Cross 상태로 변경됨
        Vector3 eventDir = _currentDestPos - transform.position;

        // 목표 도달시,
        if (eventDir.magnitude < 0.01f)
        {
            // GameStart
            GameObject go = GameObject.Find("@CrossController");
            go.GetComponent<CrossController>().Init();
            go.GetComponent<CrossController>()._crossState = CrossState.Playing;            

            // Player를 향해 회전
            transform.LookAt(_player.transform);

            // state 변경
            _state = FriendState.Idle;

            return;
        }

        float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, eventDir.magnitude);
        transform.position += eventDir.normalized * moveDist;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(eventDir), 20 * Time.deltaTime);
 
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", _speed);
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

        // current pos
        _currentDestPos = _targetObj.Peek().transform.position;

        _state = FriendState.Idle;

        _crossTargetObj = new ArrayList();
        GameObject _eventTargetParent = GameObject.Find("CrossTargetBucket");
        for (int i = 0; i < _eventTargetParent.transform.childCount; i++)
        {
            _crossTargetObj.Add(_eventTargetParent.transform.GetChild(i).gameObject);
        }

        _carEventObj = GameObject.FindGameObjectWithTag("TargetCar");
    }

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
            case FriendState.Cross:
                UpdateCross();
                break;
        }
    }
}
