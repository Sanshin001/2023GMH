using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandClick : MonoBehaviour
{
    public bool _handToggle = false;

    public void OnClickHand()
    {
        _handToggle = true;
        this.gameObject.SetActive(false);
    }
}
