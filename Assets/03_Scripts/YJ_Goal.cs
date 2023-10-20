using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField]
    GameObject _friend;
    
    [SerializeField]
    GameObject _bucket;

    [SerializeField]
    GameObject _goal;

    private void OnTriggerEnter(Collider other)
    {
        // particle in bucket all on
        _goal.SetActive(false);
        _friend.SetActive(false);
        _bucket.SetActive(true);        

        // 3�� �� ���� ���� ȭ������ �̵�
        Invoke("onClickHome", 3f);
    }

    private void onClickHome()
    {
        SceneManager.LoadScene("MainScene");
    }
}
