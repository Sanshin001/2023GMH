using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class UI_Dialog : UI_Base
{
    public bool _moveTrigger = false;

    enum Texts          // TMP_Text type
    {
        TextDialog,
    }

    enum Images
    {
        ImageDialog,
        ImageArrow,
    }

    GameObject _goalPanel;

    private float typingSpeed = 0.1f;
    private bool isTyping = false;

    UI_StoryBoard _storyManager = new UI_StoryBoard();
    string[] _currentMessages;
    Queue<string> _messageBox = new Queue<string>();

    private void Awake()
    {
        Init();
    }

    private void OnDisable()
    {
        // goalPanel 활성화
        _goalPanel.GetComponent<UI_Goal>().Init();
        _goalPanel.SetActive(true);
    }

    public void Init()
    {
        Bind<TMP_Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        
        this.gameObject.AddUIEvent(OnDialogClicked);

        // 스토리보드 초기화
        _storyManager.Init();
        _currentMessages = _storyManager._allMessageQueue.Peek();
        InitMessageBox();

        _goalPanel = GameObject.Find("AlwaysGoal");
    }

    private void InitMessageBox()
    {
        _messageBox.Clear();

        foreach (string message in _currentMessages)
        {
            _messageBox.Enqueue(message);
        }

        GetText((int)Texts.TextDialog).text = _messageBox.Peek();
        GetImage((int)Images.ImageArrow).gameObject.SetActive(true);
    }

    // clicked -> 해당 메서드 실행
    // 1. 메세지 큐에서 하나 꺼내서 기본으로 설정 (0 -> default)
    // 2. 클릭시 메세지 큐에서 하나 꺼냄 -> null이면 종료
    // 3. 메세지 큐가 비었으면 end
    public void OnDialogClicked(PointerEventData data)
    {
        if (isTyping)
        {
            isTyping = false;

            // typing 효과 중단
            StopCoroutine("OnTypingText");

            // 모든 대화문 출력
            GetText((int)Texts.TextDialog).text = _messageBox.Peek();

            // 다음으로 넘어가달라는 효과를 넣기 위해 화살표를 깜빡거림
            GetImage((int)Images.ImageArrow).gameObject.SetActive(true);
        }
        else
        {
            if (_messageBox.Count == 1)     // 모든 대화 종료
            {
                // storyManager에도 남은 UI가 없으므로 종료 처리를 해줌
                if (_storyManager._allMessageQueue.Count == 1)
                {
                    _messageBox.Clear();

                    // Friend, Player 이동 시작
                    _moveTrigger = true;

                    // 현재 Canvas 사라짐
                    this.gameObject.SetActive(false);

                    return;
                }

                // 다음 메세지 표시
                _storyManager._allMessageQueue.Dequeue();
                _currentMessages = _storyManager._allMessageQueue.Peek();

                // _messageBox를 _currentMessages로 초기화
                InitMessageBox();

                // Friend, Player 이동 시작
                _moveTrigger = true;

                // 현재 Canvas 사라짐
                this.gameObject.SetActive(false);

                return;
            }

            // 다음 텍스트 배치
            GetText((int)Texts.TextDialog).text = _messageBox.Dequeue();

            // typing animation 실행
            StartCoroutine("OnTypingText");
        }
    }

    private IEnumerator OnTypingText ()
    {
        isTyping = true;
        string dialog = _messageBox.Peek();

        int index = 0;
        while (index < dialog.Length)
        {
            GetText((int)Texts.TextDialog).text = dialog.Substring(0, index);
            index++;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        
        GetImage((int)Images.ImageArrow).gameObject.SetActive(true);
    }
}
