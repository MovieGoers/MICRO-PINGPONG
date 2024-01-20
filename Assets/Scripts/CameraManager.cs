using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;

    public GameObject CameraHolder;
    public GameObject MainCamera;

    public static CameraManager Instance
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

    public IEnumerator ShakeCamera(float duration, float magnitude)
    {
        yield return null;
        Vector3 originalPosition = MainCamera.transform.localPosition;
        // shake camera
        while (duration > 0)
        {
            Debug.Log(MainCamera.transform.localPosition);

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            MainCamera.transform.localPosition = new Vector3(x, y, z);

            duration -= 0.015f;
            yield return new WaitForSeconds(0.015f);
        }


        MainCamera.transform.localPosition = originalPosition;
    }
}


