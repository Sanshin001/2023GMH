using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class HR_PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10.0f;

    void Start()
    {
        // ??? ?????? ???? KeyAction?? OnKeyboard?? ?????? OnKeyboard?? 2?? ?????? ??? ?????? ???????? ?????
        // Managers.Input.KeyAction -= OnKeyboard;
        // Managers.Input.KeyAction += OnKeyboard;

        // Managers.Input.MouseAction -= OnMouseClicked;
        // Managers.Input.MouseAction -= OnMouseClicked;
    }

    void OnKeyboard()
    {
        // if (Input.GetKey(KeyCode.W))
        // {
        //     transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.5f);
        //     transform.position += Vector3.forward * Time.deltaTime * _speed;
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.5f);
        //     transform.position += Vector3.back * Time.deltaTime * _speed;
        // }
        // if (Input.GetKey(KeyCode.A))
        // {
        //     transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.5f);
        //     transform.position += Vector3.left * Time.deltaTime * _speed;
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.5f);
        //     transform.position += Vector3.right * Time.deltaTime * _speed;
        // }
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
    }

    // void OnCollisionEnter(Collision collision){
    //     print(collision.gameObject.name);

    //     if(collision.gameObject.GetComponent<HR_FollowPath>() != null){
    //         print(collision.gameObject.name);
    //     }
    // }

}