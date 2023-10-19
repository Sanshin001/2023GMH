using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    GameObject _bucket;

    [SerializeField]
    GameObject _goal;

    private void OnTriggerEnter(Collider other)
    {
        // particle in bucket all on
        _goal.SetActive(false);
        _bucket.SetActive(true);        

        // 3초 뒤 게임 선택 화면으로 이동
        // Invoke(Function, 3f);
    }
}
