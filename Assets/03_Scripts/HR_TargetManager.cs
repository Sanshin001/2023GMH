using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_TargetManager : MonoBehaviour
{
    Queue<GameObject> _targetNPC = new Queue<GameObject>();   // 목표 위치에 있는 GameObject들    
    Vector3 _currentTgtNPCPos;
    private float targetNPCSpeed = 2.0f;

    public void InitTargetObj(GameObject target){

        StartCoroutine(StartTargetNPCs(target));

        
        //_targetNPC.Clear();
        // for (int i = 0; i < target.transform.childCount; i++)
        // {
        //     _targetNPC.Enqueue(target.transform.GetChild(i).gameObject);
        // }

        // while(_targetNPC.Count > 0){

        //     _currentTgtNPCPos = _targetNPC.Peek().transform.position;

        //     Debug.Log(_targetNPC.Peek().name + " : " + _currentTgtNPCPos);
        //     _targetNPC.Peek().GetComponent<HR_FollowPath>().moveTarget();
        //     _targetNPC.Dequeue();

        //     //yield return new WaitForSeconds(targetNPCSpeed);
        // }


    }

    private IEnumerator StartTargetNPCs(GameObject target)
    {
        _targetNPC.Clear();
        for (int i = 0; i < target.transform.childCount; i++)
        {
            _targetNPC.Enqueue(target.transform.GetChild(i).gameObject);
        }

        while(_targetNPC.Count > 0){

            _currentTgtNPCPos = _targetNPC.Peek().transform.position;

            Debug.Log(_targetNPC.Peek().name + " : " + _currentTgtNPCPos);
            
            _targetNPC.Peek().GetComponent<HR_FollowPath>().moveTarget();
            _targetNPC.Dequeue();

            yield return new WaitForSeconds(targetNPCSpeed);
        }
        
        // End Coroutine
        yield break;

        //UpdateMoving();
    }
}
