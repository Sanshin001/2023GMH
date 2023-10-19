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

        _allMessageQueue.Enqueue(new string[]
        {
            "횡단보도에서는 조심해서 가지 않으면 위험해!",
            "좌우를 살피고 갈거야",
            "왼쪽 오른쪽에 있는 손 버튼을 눌러줘",
        });

        _allMessageQueue.Enqueue(new string[] {
            "횡단보도 앞에서는 안전하게 손을 들고 가자",
            "오늘은 직접 손을 드는 대신, 화면에 떠있는 손을 잡고 갈거야",
            "나는 먼저 가있을게",
        });

        _allMessageQueue.Enqueue(new string[] {
            "잘했어!",
            "이제 다시 가보자",
        });

        _allMessageQueue.Enqueue(new string[] {
            "운전자가 우리 옆에 보이는 불법 주정차 때문에 우리를 보지 못할거야",
            "불법 주정차를 신고해서 우리를 보이게 만들어주자!",
        });

        _allMessageQueue.Enqueue(new string[] {
            "잘했어!",
            "이제 운전자도 우리가 보일거야.",
            "건너기 전에 좌우를 먼저 살피자",
        });

        _allMessageQueue.Enqueue(new string[]
        {
            "안전한걸 확인했으니 초록불일때 조심해서 건너가보자",
        });

        _allMessageQueue.Enqueue(new string[] {
            "오늘 덕분에 늦지 않고 도착했어",
            "같이 학교로 가자",
        });
    }
}
