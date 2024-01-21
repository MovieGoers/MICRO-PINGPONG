using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public GameObject player;

    public float mouseZ;
    public static InputManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        player = GameObject.Find("Player");
    }

    void Start()
    {
        
    }
    void Update()
    {
        if(GameManager.Instance.gameState == GameManager.GameStates.GameState && !UIManager.Instance.isCursorOutOfScreen && !GameManager.Instance.DebugMode)
            DragPlayer();
    }

    private void DragPlayer()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mouseZ);
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        player.transform.position = new Vector3(objectPosition.x, objectPosition.y, player.transform.position.z);
    }
}
