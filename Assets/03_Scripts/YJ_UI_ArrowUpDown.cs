using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_UI_ArrowUpDown : MonoBehaviour
{
    RectTransform _rect;
    IEnumerator _playingAnim;

    private void OnEnable()
    {
        _rect = GetComponent<RectTransform>();
        StartCoroutine(UpAndDown());
    }

    private void OnDisable()
    {
        StopCoroutine(UpAndDown());
        StopCoroutine(_playingAnim);
    }

    IEnumerator UpAndDown()
    {
        while (true)
        {
            _playingAnim = GoDown();
            yield return StartCoroutine(_playingAnim);

            _playingAnim = GoUp();
            yield return StartCoroutine(_playingAnim);
        }
        yield break;
    }

    IEnumerator GoUp()
    {
        float time = 0;
        float aniTime = 0.5f;
        while (time <= aniTime)
        {
            time += Time.deltaTime;
            Vector2 willbePos = Vector2.up * 0.05f;
            _rect.anchoredPosition = willbePos;
        }
        yield break;
    }

    IEnumerator GoDown()
    {
        float time = 0;
        float aniTime = 0.5f;
        while (time <= aniTime)
        {
            time += Time.deltaTime;
            Vector2 willbePos = Vector2.down * 0.05f;
            _rect.anchoredPosition = willbePos;
        }

        yield break;
    }
}
