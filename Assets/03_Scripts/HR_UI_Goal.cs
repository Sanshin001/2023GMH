using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HR_UI_Goal : UI_Base
{
    [SerializeField]
    GameObject _goalText;

    enum CanvasGroups
    {
        AlwaysGoal,
    }

    enum RectTransforms
    {
        GoalPanel,
    }

    enum Texts
    {
        GoalText,
    }

    RectTransform localPos;
    bool _isAnimStart = true;       // _isAnimStart == false로 만들면, 다시 애니메이션 실행
    Vector3 _size;
    GameObject _friend;

    string[] _goalMessages = {
        "목표 : 주차장으로 이동하기",
        "오토바이를 피하자",
        "갑자기 나타나는 어린이를 조심하자",
        "주차위치에 주차해보자",
        "충돌 횟수",
    };
    
    Queue<string> _goalBox = new Queue<string>();

    private void Start()
    {
        Bind<CanvasGroup>(typeof(CanvasGroups));
        Bind<RectTransform>(typeof(RectTransforms));
        Bind<TMP_Text>(typeof(Texts));

        localPos = Get<RectTransform>((int)RectTransforms.GoalPanel);
        _size = localPos.localScale;

        // goalmessages 초기화
        foreach (string message in _goalMessages)
            _goalBox.Enqueue(message);

        Get<CanvasGroup>((int)CanvasGroups.AlwaysGoal).gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isAnimStart == false)
        {
            _isAnimStart = true;
            StartCoroutine(FadeInOutAndMove());
        }
    }

    void onClickHome()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Init()
    {
        // current message
        string _title = _goalBox.Peek();
        _goalBox.Dequeue();

        if(_goalBox.Count == 0){
            
            _friend = GameObject.Find("Friend");
            _title += " : " + _friend.GetComponent<HR_FriendController>().getHitCntNpc();


            _goalText.SetActive(true);
            Invoke("onClickHome", 3f);
        }

        // change title
        Get<TMP_Text>((int)Texts.GoalText).text = _title;

        // back scale to origin scale
        localPos.localScale = _size;

        // back position to center
        localPos.anchoredPosition = Vector3.zero;

        // animation start
        _isAnimStart = false;
    }

    private IEnumerator FadeInOutAndMove()
    {
        float animTime;

        animTime = 1.5f;

        // Fade In
        IEnumerator _fadeIn = UI_Anim.Fade(1, 0, Get<CanvasGroup>((int)CanvasGroups.AlwaysGoal), animTime);
        yield return StartCoroutine(_fadeIn);

        // Go Right
        Vector2 willbePos = Vector2.left * 0.9f + Vector2.up * 0.6f;
        localPos.anchoredPosition = willbePos;

        // Size Down
        localPos.localScale = _size * 0.5f;

        animTime = 1.0f;

        // Fade Out
        IEnumerator _fadeOut = UI_Anim.Fade(0, 1, Get<CanvasGroup>((int)CanvasGroups.AlwaysGoal), animTime);
        yield return StartCoroutine(_fadeOut);

        // End Coroutine
        yield break;
    }
}