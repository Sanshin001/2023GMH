using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_UI_StoryBoard
{
    public Queue<string[]> _allMessageQueue = new Queue<string[]>();
    public Queue<string[]> _allfinalMessageQueue = new Queue<string[]>();

    public void Init()
   {
        _allMessageQueue.Enqueue(new string[] {
            "������ �������� ������ ü���Ұž�",
            "�츮�� ���������� ������",
            "������ �غ� �Ǿ���?",
        });

        _allMessageQueue.Enqueue(new string[] {
            "�����ؾ� ��! ���ڱ� ������̰� ��Ÿ����",
            "������ �Ÿ��� �����ϸ� ������̸� ���غ�",
        });

        _allMessageQueue.Enqueue(new string[] {
            "��̰� ���� ���̾�. �����ϸ鼭 ��̸� ��������",
        });

        _allMessageQueue.Enqueue(new string[] {
            "�����忡 �����߾�! �����ڰ� ������ ���� �׻� �����ؾ� ��.",
            "�ֺ��� ���Ǹ鼭 ������ġ�� �����غ�.",
        });

        _allMessageQueue.Enqueue(new string[] {
            "���� ������ ���? �����Ҷ� ���ڱ� ��Ÿ���� ���ϱ� �ʹ� �����!",
            "������ �ɾ���� �ֺ��� ���Ǹ鼭 ���� �ؼ� �ɾ��!",
        });
    }
}
