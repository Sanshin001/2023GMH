using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공통적으로 사용할 애니메이션을 정의합니다
public class UI_Anim : MonoBehaviour
{
    // Fade In : start=1, end=0
    // Fade Out : start=0, end=1
    // Fade : CanvasGroup의 alpha값을 변경해서 투명도를 조절
    public static IEnumerator Fade(float start, float end, CanvasGroup cg, float fadeTime = 0.5f)
    {
        float current = 0;
        float percent;

        while (current < fadeTime)
        {
            current += Time.deltaTime;
            percent = (1 - Mathf.Cos((current * Mathf.PI) / 2));

            cg.alpha = Mathf.Lerp(start, end, percent);

            yield return null;
        }

        yield break;
    }
}
