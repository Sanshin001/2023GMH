using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;  // 씬 전환을 위해 추가
using UnityEngine.XR.Interaction.Toolkit;

public class MenuManager : MonoBehaviour
{
    public void onClickStageOne()
    {
        SceneManager.LoadScene("StageOneScene");
    }

    public void onClickStageTwo()
    {
        SceneManager.LoadScene("EscapeScene");
    }

    public void onClickStageThree()
    {
        SceneManager.LoadScene("StageThreeScene");
    }

    public void onClickQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
