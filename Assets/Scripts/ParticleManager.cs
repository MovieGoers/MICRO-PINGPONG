using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private static ParticleManager instance;

    public ParticleSystem Spark;

    public static ParticleManager Instance
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

    public void SetSparkPosition(Vector3 vec)
    {
        Spark.transform.position = vec;
    }
    public void PlaySpark()
    {
        ParticleSystem newSpark = Instantiate(Spark, Spark.transform.position, Spark.transform.rotation);
        newSpark.Play();
    }
}
