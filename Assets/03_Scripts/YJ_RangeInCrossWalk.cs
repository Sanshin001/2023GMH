using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_RangeInCrossWalk : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject go = GameObject.Find("@CrossController");
            go.GetComponent<CrossController>()._isInCrosswalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject go = GameObject.Find("@CrossController");
            go.GetComponent<CrossController>()._isInCrosswalk = false;
        }
    }
}
