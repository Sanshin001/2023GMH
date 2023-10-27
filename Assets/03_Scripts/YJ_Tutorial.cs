using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class YJ_Tutorial : MonoBehaviour
{
    [SerializeField]
    GameObject _flowers;

    [SerializeField]
    GameObject _bench;

    [SerializeField]
    GameObject _effect;

    [SerializeField]
    GameObject _playerCanvas;

    [SerializeField]
    GameObject _barrier;

    // select -> active flower
    public void ActiveFlower()
    {
        _flowers.SetActive(true);
        _bench.SetActive(false);
        _effect.SetActive(false);

        string title = "잘했어!\n이제 친구에게 가보자";
        Util.FindChild<UI_Goal>(_playerCanvas, null, true).Init(title);

        _barrier.transform.position = _playerCanvas.transform.position + Vector3.forward * 7f;
    }

    //
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _effect.SetActive(true);

            string title = "오른쪽 컨트롤러를 감싸는 손에 버튼이 있을거야\n그 버튼을 눌러 벤치를 '선택'해보자";
            Util.FindChild<UI_Goal>(_playerCanvas, null, true).Init(title);

            _barrier.transform.position = _playerCanvas.transform.position + Vector3.forward * 7f;
        }
    }
}
