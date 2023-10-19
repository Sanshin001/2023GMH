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

// 횡단보도가 여러 개여서 건널 때 번호가 바뀜.
// 이때 변하는 오브젝트 & 번호도 같이 변경해주기 위해서 사용
// 자료를 같이 안묶어주면 헷갈릴것같음
enum CrossOption        // 위치에 따라 변하는 것들만 정의함
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

    // CrossOption을 갈아끼우기 위해 부모 오브젝트를 가져옴
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
        // 1. target 부모 받아오기
        GameObject _parent = parent;

        // 2. 이름 순서대로 넣기
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

    // 해당 함수는 Trigger가 되는 FriendController에서 사용
    // num만 업데이트하면 관련 오브젝트는 가져다가 씀
    public void Init()
    {
        // Cross 관련 가장 상위 Canvas 활성화
        _crossCanvasParent.SetActive(true);

        // Ball, BallTarget 활성화
        FindCrossObj((int)CrossOption.BallBucket).SetActive(true);
        FindCrossObj((int)CrossOption.BallTargetBucket).SetActive(true);

        // CrossState에 따라 ball 위치를 변화시킬 것이므로 ball 초기 위치도 저장
        _ballPos = FindCrossObj((int)CrossOption.BallBucket).transform.position;

        // Player 초기 위치 저장
        _playerPos = _player.transform.position;

        // barrier 위치를 player 뒤로 조정
        GameObject ball = FindCrossObj((int)CrossOption.BallBucket);
        FindCrossObj((int)CrossOption.CrossBarriers).transform.position = _player.transform.position + Vector3.forward * 5;


        // 초기화 완료시 state 변경
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
        // Clear 조건 : Player가 Friend 근처에 도달
        if (Vector3.Distance(_friend.transform.position, _player.transform.position) <= 3.0f)
            _crossState = CrossState.Clear;

        // GameOver 조건 : 일단 횡단보도 위에 있어야 함
        if (_isInCrosswalk == false)
            return;

        // 빨간불에 횡단보도에 있으면 GameOver
        if (_isRedLight && _isInCrosswalk)
            _crossState = CrossState.GameOver;

        // 횡단보도를 건널 때, 농구공을 잡지 않으면 GameOver
        GameObject ball = FindCrossObj((int)CrossOption.BallBucket);
        bool isGrabbed = ball.GetComponent<BallController>()._isGrabbed;
        if (!isGrabbed && _isInCrosswalk)
            _crossState = CrossState.GameOver;
    }


    public bool _isRedLight = false;
    IEnumerator _playingAnim;
    IEnumerator UpdateTrafficLight()
    {
        GameObject green = FindCrossObj((int)CrossOption.TrafficLightBucket).transform.GetChild(0).gameObject; // 0번 = GreenLight
        GameObject red = FindCrossObj((int)CrossOption.TrafficLightBucket).transform.GetChild(1).gameObject; // 1번 = RedLight

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
            _isRedLight = false;      // 빨간불 끝나면 isSecEnd = false로 만들어줘서 초록불일때는 게임오버조건을 붙이지 않도록 함

        yield break;
    }

    private void UpdateGameOver()
    {
        // GoalPanel 띄우기
        string reason = "Error";
        GameObject ball = FindCrossObj((int)CrossOption.BallBucket);
        bool isGrabbed = ball.GetComponent<BallController>()._isGrabbed;

        if (isGrabbed == false)   // 공을 잡지 않았을 때
            reason = "손을 잡아야 횡단보도를 건널 수 있어!";

        // 위치가 초기화 되기 전에, _isInCrosswalk로 들어왔을 경우를 대비해서, 
        // 조건을 _isInCrosswalk가 아니라 _isRedLight로 변경
        if (_isRedLight)     // 빨간불에 횡단보도에 있었을 때
            reason = "빨간불일때는 횡단보도를 건널 수 없어!";

        string title = $"{reason}";
        Util.FindChild<UI_Goal>(_playerCanvas, null, true).Init(title);

        // 플레이어 위치 초기화
        _player.transform.position = _playerPos;

        // 농구공 상태 → 초기 위치로 복귀
        // GameOver인데 뭔가 모를 버그로 비활성화 되어있다면, 다시 활성화해줌
        if (ball.activeSelf == false)
            ball.SetActive(true);

        ball.transform.position = _ballPos;
        ball.GetComponent<BallController>().time = 10.0f;

        StopCoroutine(UpdateTrafficLight());

        _isInCrosswalk = false;

        // Playing 상태로 전환
        _crossState = CrossState.Playing;
    }

    private void UpdateClear()
    {
        // 신호등 코루틴 종료
        StopCoroutine(UpdateTrafficLight());
        StopCoroutine(_playingAnim);

        // 공 / 공타겟 / 벽 비활성화
        FindCrossObj((int)CrossOption.BallBucket).SetActive(false);
        FindCrossObj((int)CrossOption.BallTargetBucket).SetActive(false);
        FindCrossObj((int)CrossOption.CrossBarriers).SetActive(false);

        // 신호등 UI 비활성화
        GameObject go = GameObject.Find("PlayerCanvas/TrafficLightCanvas/TrafficLightInUI");
        go.SetActive(false);

        // Panel로 표시
        string title = "Clear!\n횡단보도를 무사히 건넜다!";
        Util.FindChild<UI_Goal>(_playerCanvas, null, true).Init(title);

        // cross num 증가
        _crossNum++;
        Debug.Log(_crossNum);

        // 뭔가 효과가 전부 끝나면 Rest 상태로 전환
        _crossState = CrossState.Rest;
    }

    private void UpdateRest()
    {
        // doing nothing in this time
    }
}

