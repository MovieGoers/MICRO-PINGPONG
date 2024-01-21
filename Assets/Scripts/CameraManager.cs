using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;

    public GameObject CameraHolder;
    public GameObject MainCamera;
    public GameObject Player;

    public float CameraMovementRatio;

    bool isCameraShakeOn;

    Vector3 originalPlayerPosition;
    Vector3 originalCameraHolderPosition;

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

    private void Start()
    {
        isCameraShakeOn = true;
        originalPlayerPosition = Player.transform.position;
        originalCameraHolderPosition = CameraHolder.transform.position;
    }

    private void Update()
    {
        ChangeCameraHolderPosition((Player.transform.position - originalPlayerPosition) * CameraMovementRatio);
    }

    public IEnumerator ShakeCamera(float duration, float magnitude)
    {
        yield return null;
        if (isCameraShakeOn)
        {
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

    public void SetCameraShake(bool isOn)
    {
        isCameraShakeOn = isOn;
    }

    public void ChangeCameraHolderPosition(Vector3 localPosition)
    {
        CameraHolder.transform.position = originalCameraHolderPosition + localPosition;
    }
}


