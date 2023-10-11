using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StoryBoard
{
    public Queue<string[]> _allMessageQueue = new Queue<string[]>();

    public void Init()
   {
        _allMessageQueue.Enqueue(new string[] {
            "안녕! 오늘 나랑 같이 등교할 친구구나",
            "9시까지 등교해야하는데, 지금 8시 45분이야",
            "우리 모두 지각하지 않기 위해 빠르게 움직여야해!",
            "나를 따라서 이동해줘",
        });

        _allMessageQueue.Enqueue(new string[] {
            "조심해서 가지 않으면 위험해!",
            "횡단보도 앞에서는 안전하게 손을 들고 가자",
        });

        _allMessageQueue.Enqueue(new string[] {
            "운전자가 우리 옆에 보이는 불법 주정차 때문에 우리를 보지 못했어",
            "불법 주정차를 없애서 우리를 보이게 만들어주자!",
        });

        _allMessageQueue.Enqueue(new string[] {
            "잘했어!",
            "이제 운전자도 우리가 보일테니 초록불일때 조심해서 건너가보자",
        });

        _allMessageQueue.Enqueue(new string[] {
            "오늘 덕분에 늦지 않고 도착했어",
            "같이 학교로 가자",
        });
    }
}
