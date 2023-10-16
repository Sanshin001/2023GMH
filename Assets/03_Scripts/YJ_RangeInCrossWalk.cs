using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_RangeInCrossWalk : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = GameObject.Find("@CrossController");
        if (other.gameObject.CompareTag("Player"))
            go.GetComponent<CrossController>()._isInCrosswalk = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject go = GameObject.Find("@CrossController");
        go.GetComponent<CrossController>()._isInCrosswalk = false;
    }
}
