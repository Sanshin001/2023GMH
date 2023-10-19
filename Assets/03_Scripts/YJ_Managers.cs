using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Managers : MonoBehaviour
{
    static Managers s_instance;   // ���ϼ��� ����ȴ�
    static Managers Instance { get { Init(); return s_instance; } }  // ������ �Ŵ����� �����´�

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
        // Managers ��ü���� input action üũ
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