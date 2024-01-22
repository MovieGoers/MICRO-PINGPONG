using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;

    public GameObject item_BallSizeGrow;

    public GameObject ItemSpawnStartPoint;
    public GameObject ItemSpawnEndPoint;

    public float itemDuration;
    public float ballMaxScale;
    public float ItemMaxScale;

    int m_itemCount;

    public static ItemManager Instance
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
        m_itemCount = 0;
        SpawnItem(item_BallSizeGrow);
    }

    public void SpawnItem(GameObject item)
    {
        Vector3 spawnPosition;

        item.SetActive(true);

        spawnPosition.x = Random.Range(ItemSpawnStartPoint.transform.position.x, ItemSpawnEndPoint.transform.position.x);
        spawnPosition.y = Random.Range(ItemSpawnStartPoint.transform.position.y, ItemSpawnEndPoint.transform.position.y);
        spawnPosition.z = Random.Range(ItemSpawnStartPoint.transform.position.z, ItemSpawnEndPoint.transform.position.z);

        m_itemCount += 1;
        switch (item.name)
        {
            case "Item-BallSizeGrow":
                StartCoroutine(PlayBallSizeGrowAnimation(item));
                item.transform.position = spawnPosition;
                break;
            default:
                break;
        }
    }

    public void ActivateItem(string item)
    {
        switch (item)
        {
            case "Item-BallSizeGrow":
                StartCoroutine(IncreaseBallSize());
                break;
            default:
                break;
        }
    }

    IEnumerator IncreaseBallSize()
    {
        yield return null;
        GameObject ball = BallScript.Instance.GetBallObject();
        Vector3 originalBallScale = ball.transform.localScale;

        while(ball.transform.localScale.x < ballMaxScale)
        {
            yield return new WaitForSeconds(0.05f);
            ball.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
        }

        ball.transform.localScale = new Vector3(ballMaxScale, ballMaxScale, ballMaxScale);
        yield return new WaitForSeconds(itemDuration);

        while (ball.transform.localScale.x > originalBallScale.x)
        {
            yield return new WaitForSeconds(0.05f);
            ball.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
        }

        ball.transform.localScale = originalBallScale;
    }

    IEnumerator PlayBallSizeGrowAnimation(GameObject item)
    {
        yield return null;
        Vector3 originalItemScale = item.transform.localScale;
        while (item != null)
        {
            while (item.transform.localScale.x < ItemMaxScale)
            {
                yield return new WaitForSeconds(0.05f);
                item.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            }

            item.transform.localScale = new Vector3(ItemMaxScale, ItemMaxScale, ItemMaxScale);

            while (item.transform.localScale.x > originalItemScale.x)
            {
                yield return new WaitForSeconds(0.05f);
                item.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
            }

            item.transform.localScale = originalItemScale;
        }
    }
}
