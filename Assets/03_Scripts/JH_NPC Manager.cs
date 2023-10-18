using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class JH_NPCManager : MonoBehaviour
{

    public Transform startPoint; // 시작 지점의 Transform 컴포넌트를 연결할 변수
    public Transform endPoint; // 끝 지점의 Transform 컴포넌트를 연결할 변수
    public float speed = 5f; // 이동 속도를 조절할 변수
    public Animator ami; // 애니메이션을 조절할 변수

    private bool isMoving = false; // 이동 중인지 여부를 나타내는 변수


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
        //달리는 모션으로 변경
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

        // 이동이 완료되면 이동 중인 상태를 해제
        isMoving = false;
    }

}
