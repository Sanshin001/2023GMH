using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ArrowBlink : UI_Base
{
    enum Images
    {
        ImageArrow,
    }

    enum CanvasGroups
    {
        ImageArrow,
    }

    IEnumerator _fadeInOut;

    private void Awake()
    {
        Bind<Image>(typeof(Images));
        Bind<CanvasGroup>(typeof(CanvasGroups));

        // animation init
        _fadeInOut = FadeInOut();
    }

    private void OnEnable()
    {
        // Fade ȿ���� In -> Out ���� �ݺ��Ѵ�.
        StartCoroutine(_fadeInOut);
    }

    private void OnDisable()
    {
        StopCoroutine(_fadeInOut);
    }

    private IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(UI_Anim.Fade(1, 0, Get<CanvasGroup>((int)CanvasGroups.ImageArrow)));    // Fade In

            yield return StartCoroutine(UI_Anim.Fade(0, 1, Get<CanvasGroup>((int)CanvasGroups.ImageArrow)));    // Fade Out
        }
    }
}