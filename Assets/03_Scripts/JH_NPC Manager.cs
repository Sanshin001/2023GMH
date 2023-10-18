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
        ami.SetBool("IsRun", true);
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
            ami.SetBool("IsRun", false);
            yield return null;
        }

        // �̵��� �Ϸ�Ǹ� �̵� ���� ���¸� ����
        isMoving = false;
    }

}
