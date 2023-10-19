using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CrossState
{
    Playing,
    GameOver,
    Clear,
    Rest,
}

// Ⱦ�ܺ����� ���� ������ �ǳ� �� ��ȣ�� �ٲ�.
// �̶� ���ϴ� ������Ʈ & ��ȣ�� ���� �������ֱ� ���ؼ� ���
// �ڷḦ ���� �ȹ����ָ� �򰥸��Ͱ���
enum CrossOption        // ��ġ�� ���� ���ϴ� �͵鸸 ������
{
    CrossTargetBucket,
    CrossBarriers,
    BallBucket,
    BallTargetBucket,
    TrafficLightBucket,
}

public class CrossController : MonoBehaviour
{
    public CrossState _crossState;

    GameObject _player;
    GameObject _friend;
    GameObject _playerCanvas;
    GameObject[] _greenLightArrows;
    GameObject[] _redLightArrows;

    int _crossNum;

    // CrossOption�� ���Ƴ���� ���� �θ� ������Ʈ�� ������
    GameObject _crossOptions;
    public Vector3 _ballPos;
    Vector3 _playerPos;

    private void Awake()
    {
        _crossState = CrossState.Rest;
        _friend = GameObject.Find("Friend");
        _player = GameObject.Find("Player");
        _playerCanvas = GameObject.Find("PlayerCanvas");
        _crossOptions = GameObject.Find("CrossOptions");

        _greenLightArrows = GameObject.FindGameObjectsWithTag("GreenLightArrow");
        _redLightArrows = GameObject.FindGameObjectsWithTag("RedLightArrow");
    }

    private void Update()
    {
        switch (_crossState)
        {
            case CrossState.Playing:
                UpdatePlaying();
                break;
            case CrossState.GameOver:
                UpdateGameOver();
                break;
            case CrossState.Clear:
                UpdateClear();
                break;
            case CrossState.Rest:
                UpdateRest();
                break;
        }
    }

    // �ش� �Լ��� Trigger�� �Ǵ� FriendController���� ���
    // num�� ������Ʈ�ϸ� ���� ������Ʈ�� �����ٰ� ��
    public void Init()
    {
        // GameObject�� ��Ȱ��ȭ�Ǿ��ִ»����̸�, ���� �����ü�����
        // ���� ���� Ȱ��ȭ
        var valList = Enum.GetValues(typeof(CrossOption));
        foreach (var val in valList)
        {
            FindCrossObj((int)val).SetActive(true);
        }

        // Player �ʱ� ��ġ ����
        _playerPos = _player.transform.position;

        // CrossState�� ���� ball ��ġ�� ��ȭ��ų ���̹Ƿ� ball �ʱ� ��ġ�� ����
        _ballPos = FindCrossObj((int)CrossOption.BallBucket).transform.position;

        // �ʱ�ȭ �Ϸ�� state ����
        _crossState = CrossState.Playing;

        // barrier ��ġ�� player �ڷ� ����
        GameObject ball = FindCrossObj((int)CrossOption.BallBucket);
        if (ball.GetComponent<BallController>()._isVertical == false)
            FindCrossObj((int)CrossOption.CrossBarriers).transform.position = _player.transform.position + Vector3.forward * 5;

        StartCoroutine(UpdateTrafficLight());
    }

    private GameObject FindCrossObj(int _num)
    {
        return _crossOptions.transform.GetChild(_num).transform.GetChild(_crossNum).gameObject;
    }

    public bool _isInCrosswalk = false;
    private void UpdatePlaying()
    {
        // Clear ���� : Player�� Friend ��ó�� ����
        Debug.Log(Vector3.Distance(_friend.transform.position, _player.transform.position));
        if (Vector3.Distance(_friend.transform.position, _player.transform.position) <= 3.0f)
            _crossState = CrossState.Clear;

        // GameOver ���� : �ϴ� Ⱦ�ܺ��� ���� �־�� ��
        if (_isInCrosswalk == false)
            return;

        // �����ҿ� Ⱦ�ܺ����� ������ GameOver
        if (_isRedLight)
            _crossState = CrossState.GameOver;

        // Ⱦ�ܺ����� �ǳ� ��, �󱸰��� ���� ������ GameOver
        GameObject ball = FindCrossObj((int)CrossOption.BallBucket);
        bool isGrabbed = ball.GetComponent<BallController>()._isGrabbed;
        if (!isGrabbed)
            _crossState = CrossState.GameOver;
    }


    public bool _isRedLight = true;
    IEnumerator UpdateTrafficLight()
    {
        int time = 5;       // todo : time�� �ƴ϶� ���� ���������� �־����
        while (time >= 0)
        {
            // green light init : Active GreenLight, Deactive RedLight
            foreach (GameObject go in _greenLightArrows)
                go.SetActive(true);
            foreach (GameObject go in _redLightArrows)
                go.SetActive(false);

            // ball pos
            FindCrossObj((int)CrossOption.BallBucket).transform.position = _ballPos;

            yield return StartCoroutine(ChangeTrafficLight(_greenLightArrows, true));

            // red light init : Active RedLight, Deactive GreenLight
            foreach (GameObject go in _greenLightArrows)
                go.SetActive(false);
            foreach (GameObject go in _redLightArrows)
                go.SetActive(true);

            // ball pos
            FindCrossObj((int)CrossOption.BallBucket).transform.position = _ballPos;

            yield return StartCoroutine(ChangeTrafficLight(_redLightArrows, false));

            time--;
        }
        yield break;
    }

    IEnumerator ChangeTrafficLight(GameObject[] trafficLightArrows, bool isGreenLight)
    {
        int time = 0;
        int maxInt = trafficLightArrows.Length - 1;

        while (time <= maxInt)           // 4-> 1 3-> 2 2-> 3 1-> 4 0-> 5
        {
            yield return new WaitForSeconds(1.0f);          // todo: change this seconds
            trafficLightArrows[time].SetActive(false);
            time++;
        }

        if (isGreenLight)
            _isRedLight = true;
        else
            _isRedLight = false;      // ������ ������ isSecEnd = false�� ������༭ �ʷϺ��϶��� ���ӿ��������� ������ �ʵ��� ��

        yield break;
    }

    private void UpdateGameOver()
    {
        // GoalPanel ����
        string reason = "Error";
        GameObject ball = FindCrossObj((int)CrossOption.BallBucket);
        bool isGrabbed = ball.GetComponent<BallController>()._isGrabbed;
        if (isGrabbed == false)   // ���� ���� �ʾ��� ��
            reason = "���� ��ƾ� Ⱦ�ܺ����� �ǳ� �� �־�!";
        if (_isInCrosswalk)     // �����ҿ� Ⱦ�ܺ����� �־��� ��
            reason = "�������϶��� Ⱦ�ܺ����� �ǳ� �� ����!";

        string title = $"{reason}";
        Util.FindChild<UI_Goal>(_playerCanvas, null, true).Init(title);

        // �÷��̾� ��ġ �ʱ�ȭ
        _player.transform.position = _playerPos;

        // �󱸰� ���� �� �ʱ� ��ġ�� ����
        // GameOver�ε� ���� �� ���׷� ��Ȱ��ȭ �Ǿ��ִٸ�, �ٽ� Ȱ��ȭ����
        if (ball.activeSelf == false)
            ball.SetActive(true);

        ball.transform.position = _ballPos;
        ball.GetComponent<BallController>().time = 10.0f;

        StopCoroutine(UpdateTrafficLight());
        _isInCrosswalk = false;

        // Playing ���·� ��ȯ
        _crossState = CrossState.Playing;
    }

    private void UpdateClear()
    {
        // ��ȣ�� �ڷ�ƾ ����
        StopCoroutine(UpdateTrafficLight());

        // ��ȣ�� UI ��Ȱ��ȭ
        GameObject go = GameObject.Find("PlayerCanvas/TrafficLightInUI");
        go.SetActive(false);

        // Effect �ֱ� & Panel�� ǥ��
        string title = "Clear!\nȾ�ܺ����� ������ �ǳԴ�!";
        Util.FindChild<UI_Goal>(_playerCanvas, null, true).Init(title);

        // ���� ȿ���� ���� ������ Rest ���·� ��ȯ
        _crossState = CrossState.Rest;
    }

    private void UpdateRest()
    {
        // doing Nothing
    }
}

