using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class JH_NPCManager : MonoBehaviour
{
    public Transform Player;
    public Transform startPoint;
    public Transform endPoint;
    public float movementSpeed = 5f;
    public float delayTime = 2f;
    public Animator animator;

    public AudioSource AudioManager;

    public AudioClip one;
    private bool one_audio_check = false;
    public AudioClip two;
    private bool two_audio_check = true;
    public AudioClip three;
    private bool three_audio_check = true;
    public AudioClip four;
    private bool four_audio_check = true;
    public AudioClip five;
    private bool five_audio_check = true;

    private bool isMoving = false;
    private float startTime;

    private bool isMoving_check_time = true;

    void CheckAudio()
    {
        Debug.Log("매 3초마다 실행됩니다.");
        if (!one_audio_check && !AudioManager.isPlaying)
        {
            transform.LookAt(Player);
            AudioManager.clip = one;
            AudioManager.Play();
            one_audio_check = true;
            two_audio_check = false;
        }
        else if (!two_audio_check && !AudioManager.isPlaying)
        {
            transform.LookAt(Player);
            AudioManager.clip = two;
            AudioManager.Play();
            three_audio_check = false;
            two_audio_check = true;
        }
        else if (!three_audio_check && !AudioManager.isPlaying )
        {
            transform.LookAt(Player);
            AudioManager.clip = three;
            AudioManager.Play();
            four_audio_check = false;
            three_audio_check = true;
        }
        else if (!four_audio_check && !AudioManager.isPlaying)
        {
            transform.LookAt(Player);
            AudioManager.clip = four;
            AudioManager.Play();
            four_audio_check = true;
            isMoving = true; // 4번까지 오디오가 끝나면 NPC 이동
        }
        else if (!five_audio_check && !AudioManager.isPlaying)
        {
            transform.LookAt(Player);
            AudioManager.clip = five;
            AudioManager.Play();
            five_audio_check = true;
        }
    }

    void Start()
    {
        startTime = Time.time;
        InvokeRepeating("CheckAudio", 1f, 5f);
        transform.LookAt(Player);
    }

    void Update()
    {

        if(isMoving_check_time)
        {
            startTime = Time.time;
        }

        // 이동 중일 때만 Update 함수를 실행합니다.
        if (isMoving && !AudioManager.isPlaying)
        {
            isMoving_check_time = false;

            transform.LookAt(endPoint);


            animator.SetBool("IsRun", true);
            float distCovered = (Time.time - startTime) * movementSpeed;
            float journeyLength = Vector3.Distance(startPoint.position, endPoint.position);
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, fracJourney);

            // 이동이 완료되면 이동을 멈춥니다.
            if (fracJourney >= 1f)
            {
                isMoving = false;
                animator.SetBool("IsRun", false);
                transform.LookAt(Player);
            }
        }
    }
}