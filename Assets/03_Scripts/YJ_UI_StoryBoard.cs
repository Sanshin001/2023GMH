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

        _allMessageQueue.Enqueue(new string[]
        {
            "Ⱦ�ܺ��������� �����ؼ� ���� ������ ������!",
            "�¿츦 ���ǰ� ���ž�",
            "���� �����ʿ� �ִ� �� ��ư�� ������",
        });

        _allMessageQueue.Enqueue(new string[] {
            "Ⱦ�ܺ��� �տ����� �����ϰ� ���� ��� ����",
            "������ ���� ���� ��� ���, ȭ�鿡 ���ִ� ���� ��� ���ž�",
            "���� ���� ��������",
        });

        _allMessageQueue.Enqueue(new string[] {
            "���߾�!",
            "���� �ٽ� ������",
        });

        _allMessageQueue.Enqueue(new string[] {
            "�����ڰ� �츮 ���� ���̴� �ҹ� ������ ������ �츮�� ���� ���Ұž�",
            "�ҹ� �������� �Ű��ؼ� �츮�� ���̰� ���������!",
        });

        _allMessageQueue.Enqueue(new string[] {
            "���߾�!",
            "���� �����ڵ� �츮�� ���ϰž�.",
            "�ǳʱ� ���� �¿츦 ���� ������",
        });

        _allMessageQueue.Enqueue(new string[]
        {
            "�����Ѱ� Ȯ�������� �ʷϺ��϶� �����ؼ� �ǳʰ�����",
        });

        _allMessageQueue.Enqueue(new string[] {
            "���� ���п� ���� �ʰ� �����߾�",
            "���� �б��� ����",
        });
    }
}
