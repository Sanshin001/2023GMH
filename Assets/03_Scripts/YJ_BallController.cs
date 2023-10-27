using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallController : UI_Base
{
    [SerializeField]
    GameObject _targetObj;

    public bool _isGrabbed = false;

    // 횡단보도가 z축으로 되어있는지, 아닌지
    [SerializeField]
    public bool _isVertical;

    float totalTime = 10.0f;
    public float time = 10.0f;
    private void Update()
    {
        GameObject go = GameObject.Find("@CrossController");
        bool isRedLight = go.GetComponent<CrossController>()._isRedLight;
        if (isRedLight == true)
            return;

        // Ball Target 위치로 이동
        time -= Time.deltaTime;
        if (time <= 0)
            return;
        float allDistance = (go.GetComponent<CrossController>()._ballPos - _targetObj.transform.position).magnitude;
        float maxDistance = allDistance * (Time.deltaTime / totalTime);
        transform.position = Vector3.MoveTowards(transform.position, _targetObj.transform.position, maxDistance);
    }


    public void KeepGrabbed()
    {
        _isGrabbed = true;
    }

    public void OnNotGrabbed()
    {
        _isGrabbed = false;
    }
}
