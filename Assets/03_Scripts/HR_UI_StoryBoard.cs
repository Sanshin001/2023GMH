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
            "오늘은 운전자의 역할을 체험할거야",
            "우리는 주차장으로 가야해",
            "운전할 준비가 되었어?",
        });

        _allMessageQueue.Enqueue(new string[] {
            "주의해야 해! 갑자기 오토바이가 나타났어",
            "안전한 거리를 유지하며 오토바이를 피해봐",
        });

        _allMessageQueue.Enqueue(new string[] {
            "어린이가 보행 중이야. 서행하면서 어린이를 지나가봐",
        });

        _allMessageQueue.Enqueue(new string[] {
            "주차장에 도착했어! 운전자가 주차할 때는 항상 주의해야 해.",
            "주변을 살피면서 주차위치에 주차해봐.",
        });

        _allMessageQueue.Enqueue(new string[] {
            "운전 경험은 어땠어? 운전할때 갑자기 나타나면 피하기 너무 힘들어!",
            "다음에 걸어갈때는 주변을 살피면서 주의 해서 걸어가자!",
        });
    }
}
