using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class JH_NPCManager : MonoBehaviour
{

    public Transform startPoint; // ���� ������ Transform ������Ʈ�� ������ ����
    public Transform endPoint; // �� ������ Transform ������Ʈ�� ������ ����
    public float speed = 5f; // �̵� �ӵ��� ������ ����
    public Animator ami; // �ִϸ��̼��� ������ ����

    private bool isMoving = false; // �̵� ������ ���θ� ��Ÿ���� ����


    void Start()
    {
        ami.SetBool("IsRun", true);
    }

    // Update is called once per frame
    void Update()
    {
        MoveToDestination();
    }

    void MoveToDestination()
    {
        isMoving = true;
        //�޸��� ������� ����
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        float distance = Vector3.Distance(startPoint.position, endPoint.position);
        float startTime = Time.time;
        float journeyLength = distance / speed;

        while (Time.time - startTime < journeyLength)
        {
            float fraction = (Time.time - startTime) / journeyLength;
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, fraction);
            yield return null;
        }

        // �̵��� �Ϸ�Ǹ� �̵� ���� ���¸� ����
        isMoving = false;
    }

}
