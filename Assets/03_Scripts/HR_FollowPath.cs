using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_FollowPath : MonoBehaviour
{
    public Transform path; // �� ������Ʈ�� ��Ÿ���� Ʈ������
    public float speed = 5.0f; // NPC�� �̵� �ӵ�
    private Transform[] waypoints; // �� ������Ʈ�� ��������Ʈ��
    private int currentWaypointIndex = 0; // ���� ��������Ʈ�� �ε���
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

        // �� ������Ʈ�� ��������Ʈ���� �迭�� ����
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

            // ���� ��������Ʈ
            Transform currentWaypoint = waypoints[currentWaypointIndex];

            // ���� ��������Ʈ �������� ȸ��
            Vector3 direction = currentWaypoint.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5.0f);

            // ��������Ʈ �������� �̵�
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);

            // ���� ��������Ʈ�� �������� �� ���� ��������Ʈ�� �̵�
            if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    // ��� ��������Ʈ�� ������ ���, ó������ ���ư�
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
            print(gameObject.name+":�浹");
            
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