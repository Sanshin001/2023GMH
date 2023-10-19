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
        // goalPanel Ȱ��ȭ
        _goalPanel.GetComponent<UI_Goal>().Init();
        _goalPanel.SetActive(true);
    }

    public void Init()
    {
        Bind<TMP_Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        
        this.gameObject.AddUIEvent(OnDialogClicked);

        // ���丮���� �ʱ�ȭ
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

    // clicked -> �ش� �޼��� ����
    // 1. �޼��� ť���� �ϳ� ������ �⺻���� ���� (0 -> default)
    // 2. Ŭ���� �޼��� ť���� �ϳ� ���� -> null�̸� ����
    // 3. �޼��� ť�� ������� end
    public void OnDialogClicked(PointerEventData data)
    {
        if (isTyping)
        {
            isTyping = false;

            // typing ȿ�� �ߴ�
            StopCoroutine("OnTypingText");

            // ��� ��ȭ�� ���
            GetText((int)Texts.TextDialog).text = _messageBox.Peek();

            // �������� �Ѿ�޶�� ȿ���� �ֱ� ���� ȭ��ǥ�� �����Ÿ�
            GetImage((int)Images.ImageArrow).gameObject.SetActive(true);
        }
        else
        {
            if (_messageBox.Count == 1)     // ��� ��ȭ ����
            {
                // storyManager���� ���� UI�� �����Ƿ� ���� ó���� ����
                if (_storyManager._allMessageQueue.Count == 1)
                {
                    _messageBox.Clear();

                    // Friend, Player �̵� ����
                    _moveTrigger = true;

                    // ���� Canvas �����
                    this.gameObject.SetActive(false);

                    return;
                }

                // ���� �޼��� ǥ��
                _storyManager._allMessageQueue.Dequeue();
                _currentMessages = _storyManager._allMessageQueue.Peek();

                // _messageBox�� _currentMessages�� �ʱ�ȭ
                InitMessageBox();

                // Friend, Player �̵� ����
                _moveTrigger = true;

                // ���� Canvas �����
                this.gameObject.SetActive(false);

                return;
            }

            // ���� �ؽ�Ʈ ��ġ
            GetText((int)Texts.TextDialog).text = _messageBox.Dequeue();

            // typing animation ����
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
