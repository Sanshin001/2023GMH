using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���������� ����� �ִϸ��̼��� �����մϴ�
public class UI_Anim : MonoBehaviour
{
    // Fade In : start=1, end=0
    // Fade Out : start=0, end=1
    // Fade : CanvasGroup�� alpha���� �����ؼ� ������ ����
    public static IEnumerator Fade(float start, float end, CanvasGroup cg, float fadeTime = 0.5f)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            cg.alpha = Mathf.Lerp(start, end, percent);

            yield return null;
        }

        yield break;
    }
}
