using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    private static MaterialManager instance;

    public Material neonWallMaterial;

    public static MaterialManager Instance
    {
        get
        {
            return instance;
        }
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
    }

    private void Start()
    {
    }
}
