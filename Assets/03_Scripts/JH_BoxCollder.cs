using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class JH_BoxCollder : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform startPos;
    Transform mytransPos;
    public AudioClip clip;
    public AudioClip endclip;
    public AudioSource AudioSorce;
    public GameObject g;


    void Start()
    {
        mytransPos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            Debug.Log(other.tag);
            mytransPos.position = startPos.position;
            if (!AudioSorce.isPlaying)
            {
                AudioSorce.clip = clip;
                AudioSorce.Play();
            }
        }

        if(other.tag == "goal")
        {
            Debug.Log(other.tag);
            g.SetActive(true);

            AudioSorce.clip = endclip;
            AudioSorce.Play();

            Invoke("SceneMove", 3f);
        }

        
    }


    void SceneMove()
    {
        Debug.Log("씬 이동 구문 넣어야함");
    }

}
