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

        string title = "���߾�!\n���� ģ������ ������";
        Util.FindChild<UI_Goal>(_playerCanvas, null, true).Init(title);

        _barrier.transform.position = _playerCanvas.transform.position + Vector3.forward * 7f;
    }

    //
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _effect.SetActive(true);

            string title = "������ ��Ʈ�ѷ��� ���δ� �տ� ��ư�� �����ž�\n�� ��ư�� ���� ��ġ�� '����'�غ���";
            Util.FindChild<UI_Goal>(_playerCanvas, null, true).Init(title);

            _barrier.transform.position = _playerCanvas.transform.position + Vector3.forward * 7f;
        }
    }
}
