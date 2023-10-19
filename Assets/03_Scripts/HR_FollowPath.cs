using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_FollowPath : MonoBehaviour
{
    public Transform path; // 선 오브젝트를 나타내는 트랜스폼
    public float speed = 5.0f; // NPC의 이동 속도
    private Transform[] waypoints; // 선 오브젝트의 웨이포인트들
    private int currentWaypointIndex = 0; // 현재 웨이포인트의 인덱스
    private bool _isAnimStart = false;

    public bool isDie = false;
    public float dieForce = 5.0f;

    public int waitBefStart = 0;

    GameObject _friend;

    void Start()
    {
        Debug.Log("path : "+ path);

        if (path == null)
        {
            return;
        }

        // 선 오브젝트의 웨이포인트들을 배열로 저장
        waypoints = new Transform[path.childCount];
        for (int i = 0; i < path.childCount; i++)
        {
            waypoints[i] = path.GetChild(i);
            Debug.Log("waypoints : "+ waypoints[i]);
        }
    }

    void Update()
    {

        if( isDie == false && _isAnimStart == true ){
            if (waypoints.Length == 0)
                return;

            // 현재 웨이포인트
            Transform currentWaypoint = waypoints[currentWaypointIndex];

            // 현재 웨이포인트 방향으로 회전
            Vector3 direction = currentWaypoint.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5.0f);

            // 웨이포인트 방향으로 이동
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);

            // 현재 웨이포인트에 도달했을 때 다음 웨이포인트로 이동
            if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    // 모든 웨이포인트를 돌았을 경우, 처음으로 돌아감
                    //currentWaypointIndex = 0;
                    _isAnimStart = false;
                }
            }
        }
    }

    public void moveTarget(){
        _isAnimStart = true;
        StartCoroutine(WaitBefStart());
    }

    void OnCollisionEnter(Collision collision) {
        print(collision.gameObject.name+"::::");

        if(isDie == false && collision.gameObject.name == "Player"){
            isDie = true;
            
            _friend = GameObject.Find("Friend");
            _friend.GetComponent<HR_FriendController>().HitCntNpc();
            StartCoroutine(DieNpc(collision));
        }
    }

    IEnumerator DieNpc(Collision collision){
            print(gameObject.name+":충돌");
            
            Rigidbody rbody = gameObject.GetComponent<Rigidbody>();
            Vector3 velo = rbody.velocity;
            velo.y = -1.0f;
            velo.z = -1.0f;
            velo.x = -1.0f;
            //rbody.isKinematic = false;
            rbody.AddForce(velo * dieForce);
            rbody.constraints = RigidbodyConstraints.None;

            yield return new WaitForSeconds(2);

            Destroy(gameObject);

    }
    IEnumerator WaitBefStart(){
            yield return new WaitForSeconds(waitBefStart);
    }
}