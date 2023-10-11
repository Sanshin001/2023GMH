using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendController : MonoBehaviour
{
    [SerializeField]
    GameObject _player;

    [SerializeField]
    Vector3 _delta;     // _player�� ������ �Ÿ�

    [SerializeField]
    float _speed = 12.0f;

    [SerializeField]
    GameObject _myCanvas;

    [SerializeField]
    GameObject _goalPanel;

    Queue<GameObject> _targetObj = new Queue<GameObject>();   // ��ǥ ��ġ�� �ִ� GameObject��    
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
        // ���� Ÿ���� ���� ��
        if (_targetObj.Count == 1)
            return;

        // ó�� UI �������� target �ȹٲ�
        if (_isFirstUI)
        {
            _isFirstUI = false;
            return;
        }

        // target �ٲٱ�
        _targetObj.Dequeue();
        _currentDestPos = _targetObj.Peek().transform.position;

        // ������ ��ġ�� ���� ��
        if (_targetObj.Count == 1)
            _isLastUI = true;               // UI�� ǥ������ ����
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

    // 1. target ��ġ�� ��� �̵�
    // 2. player�� �ʹ� �־����� Idle ���·� ����

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
