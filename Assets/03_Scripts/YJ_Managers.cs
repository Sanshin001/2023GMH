using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Managers : MonoBehaviour
{
    static Managers s_instance;   // 유일성이 보장된다
    static Managers Instance { get { Init(); return s_instance; } }  // 유일한 매니저를 갖고온다

    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();

    public static InputManager Input { get { return Instance._input; } }

    public static ResourceManager Resource { get { return Instance._resource; } }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // Managers 객체에서 input action 체크
        _input.OnUpdate();
    }

    static void Init()
    {
        GameObject go = GameObject.Find("@Managers");
        if (go == null)
        {
            go = new GameObject { name = "@Managers" };
            go.AddComponent<Managers>();
        }
        // DontDestroyOnLoad(go);
        s_instance = go.GetComponent<Managers>();
    }
}
