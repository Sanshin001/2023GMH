using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Goal : UI_Base
{
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

    string[] _goalMessages = {
        "목표 : 학교로 이동하기",
        "버튼을 눌러 손을 들고 이동하자",
        "불법 주정차를 계속 눌러 없애자",
        "목표 : 횡단보도 안전하게 건너기",
        "계속 학교로 이동",
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

    public void Init()
    {
        // current message
        string _title = _goalBox.Peek();
        _goalBox.Dequeue();

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