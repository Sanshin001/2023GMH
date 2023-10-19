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
    bool _isAnimStart = true;       // _isAnimStart == false�� �����, �ٽ� �ִϸ��̼� ����
    Vector3 _size;

    string[] _goalMessages = {
        "��ǥ : ģ���� ���� �б��� �̵��ϱ�",
        "���� �������� ���Ǹ� �� ��ư�� ������",
        "���ٴϴ� ���� ��� Ⱦ�ܺ����� �ǳ���\n���� ������ �ٽ� ����",
        "ģ���� ���� �б��� �̵�����",
        "�ҹ� �������� ���� �Ű�����",
        "���� �������� ���Ǹ� �� ��ư�� ������",
        "���� ��� Ⱦ�ܺ����� �����ϰ� �ǳ���",
        "ģ���� ���� ������ �������� �̵�����",
    };
    Queue<string> _goalBox = new Queue<string>();

    private void Start()
    {
        Bind<CanvasGroup>(typeof(CanvasGroups));
        Bind<RectTransform>(typeof(RectTransforms));
        Bind<TMP_Text>(typeof(Texts));

        localPos = Get<RectTransform>((int)RectTransforms.GoalPanel);
        _size = localPos.localScale;

        // goalmessages �ʱ�ȭ
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

    public void Init(string title)
    {
        // current message
        string _title = title;

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

        animTime = 1.0f;

        // Fade In
        IEnumerator _fadeIn = UI_Anim.Fade(1, 0, Get<CanvasGroup>((int)CanvasGroups.AlwaysGoal), animTime);
        yield return StartCoroutine(_fadeIn);

        // Go Right
        Vector2 willbePos = Vector2.left * 0.2f - Vector2.up * 0.05f;
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