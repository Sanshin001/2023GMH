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
    GameObject _crossCanvasParent;

    ArrayList _greenLightArrows;
    ArrayList _redLightArrows;

    public int _crossNum = 0;

    // CrossOption�� ���Ƴ���� ���� �θ� ������Ʈ�� ������
    GameObject _crossOptions;
    public Vector3 _ballPos;
    Vector3 _playerPos;

    [SerializeField]
    Material _greenOnMt;
    [SerializeField]
    Material _greenOffMt;
    [SerializeField]
    Material _redOnMt;
    [SerializeField]
    Material _redOffMt;

    private void InitArrows(GameObject parent, ArrayList arrowList)
    {
        // 1. target �θ� �޾ƿ���
        GameObject _parent = parent;

        // 2. �̸� ������� �ֱ�
        for (int i = _parent.transform.childCount - 1; i >= 0 ; i--)
        {
            arrowList.Add(_parent.transform.GetChild(i).gameObject);
        }
    }

    private void Awake()
    {
        _crossState = CrossState.Rest;
        _friend = GameObject.Find("Friend");
        _player = GameObject.Find("Player");
        _playerCanvas = GameObject.Find("PlayerCanvas");
        _crossCanvasParent = GameObject.Find("Player/PlayerCanvas/TrafficLightCanvas/TrafficLightInUI");
        _crossOptions = GameObject.Find("CrossOptions");

        _greenLightArrows = new ArrayList();
        _redLightArrows = new ArrayList();

        InitArrows(_crossCanvasParent.transform.GetChild(0).gameObject, _greenLightArrows);
        InitArrows(_crossCanvasParent.transform.GetChild(1).gameObject, _redLightArrows);
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
        // Cross ���� ���� ���� Canvas Ȱ��ȭ
        _crossCanvasParent.SetActive(true);

        // Ball, BallTarget Ȱ��ȭ
        FindCrossObj((int)CrossOption.BallBucket).SetActive(true);
        FindCrossObj((int)CrossOption.BallTargetBucket).SetActive(true);

        // CrossState�� ���� ball ��ġ�� ��ȭ��ų ���̹Ƿ� ball �ʱ� ��ġ�� ����
        _ballPos = FindCrossObj((int)CrossOption.BallBucket).transform.position;

        // Player �ʱ� ��ġ ����
        _playerPos = _player.transform.position;

        // barrier ��ġ�� player �ڷ� ����
        GameObject ball = FindCrossObj((int)CrossOption.BallBucket);
        FindCrossObj((int)CrossOption.CrossBarriers).transform.position = _player.transform.position + Vector3.forward * 5;


        // �ʱ�ȭ �Ϸ�� state ����
        _crossState = CrossState.Playing;

        StartCoroutine(UpdateTrafficLight());
    }

    private GameObject FindCrossObj(int _num)
    {
        GameObject go = _crossOptions.transform.GetChild(_num).transform.GetChild(_crossNum).gameObject;
        if (go == null) return null;
        return go;
    }

    public bool _isInCrosswalk = false;
    private void UpdatePlaying()
    {
        // Clear ���� : Player�� Friend ��ó�� ����
        if (Vector3.Distance(_friend.transform.position, _player.transform.position) <= 3.0f)
            _crossState = CrossState.Clear;

        // GameOver ���� : �ϴ� Ⱦ�ܺ��� ���� �־�� ��
        if (_isInCrosswalk == false)
            return;

        // �����ҿ� Ⱦ�ܺ����� ������ GameOver
        if (_isRedLight && _isInCrosswalk)
            _crossState = CrossState.GameOver;

        // Ⱦ�ܺ����� �ǳ� ��, �󱸰��� ���� ������ GameOver
        GameObject ball = FindCrossObj((int)CrossOption.BallBucket);
        bool isGrabbed = ball.GetComponent<BallController>()._isGrabbed;
        if (!isGrabbed && _isInCrosswalk)
            _crossState = CrossState.GameOver;
    }


    public bool _isRedLight = false;
    IEnumerator _playingAnim;
    IEnumerator UpdateTrafficLight()
    {
        GameObject green = FindCrossObj((int)CrossOption.TrafficLightBucket).transform.GetChild(0).gameObject; // 0�� = GreenLight
        GameObject red = FindCrossObj((int)CrossOption.TrafficLightBucket).transform.GetChild(1).gameObject; // 1�� = RedLight

        while (true)
        {
            // green light init : Active GreenLight, Deactive RedLight
            foreach (GameObject go in _greenLightArrows)
                go.SetActive(true);
            foreach (GameObject go in _redLightArrows)
                go.SetActive(false);

            // red off on
            _crossCanvasParent.transform.GetChild(3).gameObject.SetActive(true);
            // green off off
            _crossCanvasParent.transform.GetChild(5).gameObject.SetActive(false);

            green.GetComponent<MeshRenderer>().material = _greenOnMt;
            red.GetComponent<MeshRenderer>().material = _redOffMt;

            // ball pos
            FindCrossObj((int)CrossOption.BallBucket).SetActive(true);
            FindCrossObj((int)CrossOption.BallBucket).transform.position = _ballPos;
            FindCrossObj((int)CrossOption.BallBucket).GetComponent<BallController>().time = 10.0f;

            // ball target active
            FindCrossObj((int)CrossOption.BallTargetBucket).SetActive(true);

            _playingAnim = ChangeTrafficLight(_greenLightArrows, true);
            yield return StartCoroutine(_playingAnim);

            // red light init : Active RedLight, Deactive GreenLight
            foreach (GameObject go in _greenLightArrows)
                go.SetActive(false);
            foreach (GameObject go in _redLightArrows)
                go.SetActive(true);

            // red off off
            _crossCanvasParent.transform.GetChild(3).gameObject.SetActive(false);
            // green off on
            _crossCanvasParent.transform.GetChild(5).gameObject.SetActive(true);

            green.GetComponent<MeshRenderer>().material = _greenOffMt;
            red.GetComponent<MeshRenderer>().material = _redOnMt;

            // ball pos
            FindCrossObj((int)CrossOption.BallBucket).transform.position = _ballPos;
            FindCrossObj((int)CrossOption.BallBucket).SetActive(false);            
            // ball target deactive
            FindCrossObj((int)CrossOption.BallTargetBucket).SetActive(false);

            _playingAnim = ChangeTrafficLight(_redLightArrows, false);
            yield return StartCoroutine(_playingAnim);
        }
    }

    IEnumerator ChangeTrafficLight(ArrayList trafficLightArrows, bool isGreenLight)
    {
        int time = 0;
        int maxTime = trafficLightArrows.Count - 1;

        while (time <= maxTime)           // 4-> 1 3-> 2 2-> 3 1-> 4 0-> 5
        {
            yield return new WaitForSeconds(1.0f);          // todo: change this seconds
            GameObject go = (GameObject) trafficLightArrows[time];
            go.SetActive(false);
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

        // ��ġ�� �ʱ�ȭ �Ǳ� ����, _isInCrosswalk�� ������ ��츦 ����ؼ�, 
        // ������ _isInCrosswalk�� �ƴ϶� _isRedLight�� ����
        if (_isRedLight)     // �����ҿ� Ⱦ�ܺ����� �־��� ��
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
        StopCoroutine(_playingAnim);

        // �� / ��Ÿ�� / �� ��Ȱ��ȭ
        FindCrossObj((int)CrossOption.BallBucket).SetActive(false);
        FindCrossObj((int)CrossOption.BallTargetBucket).SetActive(false);
        FindCrossObj((int)CrossOption.CrossBarriers).SetActive(false);

        // ��ȣ�� UI ��Ȱ��ȭ
        GameObject go = GameObject.Find("PlayerCanvas/TrafficLightCanvas/TrafficLightInUI");
        go.SetActive(false);

        // Panel�� ǥ��
        string title = "Clear!\nȾ�ܺ����� ������ �ǳԴ�!";
        Util.FindChild<UI_Goal>(_playerCanvas, null, true).Init(title);

        // cross num ����
        _crossNum++;
        Debug.Log(_crossNum);

        // ���� ȿ���� ���� ������ Rest ���·� ��ȯ
        _crossState = CrossState.Rest;
    }

    private void UpdateRest()
    {
        // doing nothing in this time
    }
}

