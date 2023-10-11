using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    // resource 관리 함수들을 래핑해주는 작업.
    // 만약 resource 관리를 이상하게 하고 있으면 여기에서 일괄 변경

    public T Load<T>(string path) where T : Object { // where T : Object
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        return Object.Instantiate(prefab, parent);  // 본인의 Instantiate가 아니라 Object.Instantiate를 사용
    }

    public void Destroy(GameObject go) 
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}
