using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StoryBoard
{
    public Queue<string[]> _allMessageQueue = new Queue<string[]>();

    public void Init()
   {
        _allMessageQueue.Enqueue(new string[] {
            "�ȳ�! ���� ���� ���� ��� ģ������",
            "9�ñ��� ��ؾ��ϴµ�, ���� 8�� 45���̾�",
            "�츮 ��� �������� �ʱ� ���� ������ ����������!",
            "���� ���� �̵�����",
        });

        _allMessageQueue.Enqueue(new string[] {
            "�����ؼ� ���� ������ ������!",
            "Ⱦ�ܺ��� �տ����� �����ϰ� ���� ��� ����",
        });

        _allMessageQueue.Enqueue(new string[] {
            "�����ڰ� �츮 ���� ���̴� �ҹ� ������ ������ �츮�� ���� ���߾�",
            "�ҹ� �������� ���ּ� �츮�� ���̰� ���������!",
        });

        _allMessageQueue.Enqueue(new string[] {
            "���߾�!",
            "���� �����ڵ� �츮�� �����״� �ʷϺ��϶� �����ؼ� �ǳʰ�����",
        });

        _allMessageQueue.Enqueue(new string[] {
            "���� ���п� ���� �ʰ� �����߾�",
            "���� �б��� ����",
        });
    }
}
