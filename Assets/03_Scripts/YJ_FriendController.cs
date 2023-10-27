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

    public Queue<GameObject> _targetObj = new Queue<GameObject>();   // ��ǥ ��ġ�� �ִ� GameObject��    
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
        // �ƹ��͵� ����
    }

    void UpdateMoving()
    {
        // Animation
        Animator anim = GetComponent<Animator>();

        // friend and target dir
        Vector3 targetDir = _currentDestPos - transform.position;

        // friend and player dir
        Vector3 playerDir = transform.position - _player.transform.position;

        // ��ǥ ��ġ ���� flow
        // ��ǥ�� �����Ÿ� �� ���Խ�,
        if (targetDir.magnitude < 3.0f)
        {
            // ģ���� �÷��̾ �ƴ϶� ��ǥ ��ġ�� �������� �̵�
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, targetDir.magnitude);
            transform.position += targetDir.normalized * moveDist;

            // ��ǥ ��ġ ���޽�, �������� ���߰� �÷��̾ �ٶ�
            if (targetDir.magnitude < 0.001f && !_isLastUI)
                OnTargetArrived();
        }
        else
        {
            // target�� ���� �̵�
            // player�� �Ÿ��� �ָ� �ӵ� ����
            if (playerDir.magnitude >= 12.0f)
                _speed -= 1.0f;
            // player�� �Ÿ��� �ʹ� ������ �ӵ� ���� ����
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

    void OnTargetArrived() {    // ��ǥ ��ġ ���޽� ó��
        // �ִϸ��̼� ����
        _state = FriendState.Idle;

        // Player�� ���� ȸ��
        transform.LookAt(_player.transform);

        // ����Ʈ ǥ�� �� ���� ��ǥ ���ֱ�
        _goalPanel.SetActive(false);

        // ����Ʈ ǥ�ø� ���� UI�� �����
        _myCanvas.gameObject.SetActive(true);

        // UI ǥ�ý� ������ ����
        _myCanvas.GetComponent<UI_Dialog>()._moveTrigger = false;
    }

    public void ChangeTargetPos()    
    {
        // target �� Ȯ���� ���� ����
        _targetCount++;
        Debug.Log(_targetCount);

        // ���� Ÿ���� ���� ��
        if (_targetObj.Count == 1)
            return;

        // ó�� UI �������� target �ȹٲ�
        if (_isFirstUI)
        {
            _isFirstUI = false;
            return;
        }

        // ���� Ÿ���� CrossWalk�� ��, target�� crossbucket ���ο� �ִ� �ɷ� �ٲٱ�
        if (isNextTargetCrossWalk())
        {
            GameObject go = GameObject.Find("@CrossController");
            int crossNum = go.GetComponent<CrossController>()._crossNum;
            GameObject temp = GameObject.Find("CrossOptions/CrossTargetBucket").transform.GetChild(crossNum).gameObject;
            _currentDestPos = temp.transform.position;
            return;
        }

        // ���� Ÿ���� CarEvent�� ��, carevent�� �ٲٱ�
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

        // target �ٲٱ�
        _targetObj.Dequeue();
        _currentDestPos = _targetObj.Peek().transform.position;

        // ������ ��ġ�� ���� ��
        if (_targetObj.Count == 1)
            _isLastUI = true;               // UI�� ǥ������ ����
    }

    private void UpdateCross()
    {
        // �̺�Ʈ ��ġ ���޽�, Cross ���·� �����
        Vector3 eventDir = _currentDestPos - transform.position;

        // ��ǥ ���޽�,
        if (eventDir.magnitude < 0.01f)
        {
            // GameStart
            GameObject go = GameObject.Find("@CrossController");
            go.GetComponent<CrossController>().Init();
            go.GetComponent<CrossController>()._crossState = CrossState.Playing;            

            // Player�� ���� ȸ��
            transform.LookAt(_player.transform);

            // state ����
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
