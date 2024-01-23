using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;

    public GameObject item_BallSizeGrow;
    public GameObject Item_PlayerSizeGrow;

    public GameObject ItemSpawnStartPoint;
    public GameObject ItemSpawnEndPoint;

    public float itemDuration;
    public float itemSpawnTime;
    public float ItemMaxScale;

    public float ballMaxScale;

    public float playerMaxScale;
    public float playerItemRotationSpeed;

    public Coroutine coroutineEffect;

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
        StartCoroutine("PlayBallSizeGrowAnimation");
        StartCoroutine("PlayPlayerItemAnimation");
    }

    public void SpawnItem()
    {
        Vector3 spawnPosition;

        // --------------------------
        GameObject item = Item_PlayerSizeGrow; // 나중에 랜덤으로 설정.


        item.SetActive(true);

        spawnPosition.x = Random.Range(ItemSpawnStartPoint.transform.position.x, ItemSpawnEndPoint.transform.position.x);
        spawnPosition.y = Random.Range(ItemSpawnStartPoint.transform.position.y, ItemSpawnEndPoint.transform.position.y);
        spawnPosition.z = Random.Range(ItemSpawnStartPoint.transform.position.z, ItemSpawnEndPoint.transform.position.z);

        item.transform.position = spawnPosition;

        m_itemCount += 1;
    }

    public void ActivateItemEffect(string item)
    {
        switch (item)
        {
            case "Item-BallSizeGrow":
                coroutineEffect = StartCoroutine(IncreaseBallSize());
                break;
            case "Item-PlayerSizeGrow":
                coroutineEffect = StartCoroutine(IncreasePlayerSize());
                break;
            default:
                break;
        }
    }

    public void DeactivateItemEffect()
    {
        if(coroutineEffect != null)
            StopCoroutine(coroutineEffect);
    }

    IEnumerator IncreasePlayerSize()
    {
        yield return null;
        GameObject player = PlayerScript.Instance.gameObject;
        Vector3 originalScale = PlayerScript.Instance.originalPlayerScale;

        while (player.transform.localScale.x < playerMaxScale)
        {
            yield return new WaitForSeconds(0.05f);
            player.transform.localScale += new Vector3(0.05f, 0f, 0.05f);
        }

        player.transform.localScale = new Vector3(playerMaxScale, originalScale.y, playerMaxScale);
        yield return new WaitForSeconds(itemDuration);

        while (player.transform.localScale.x > originalScale.x)
        {
            yield return new WaitForSeconds(0.05f);
            player.transform.localScale -= new Vector3(0.05f, 0f, 0.05f);
        }

        player.transform.localScale = originalScale;

        yield return new WaitForSecondsRealtime(itemSpawnTime);

        SpawnItem();
    }

    IEnumerator IncreaseBallSize()
    {
        yield return null;
        GameObject ball = BallScript.Instance.GetBallObject();
        Vector3 originalBallScale = BallScript.Instance.originalBallScale;

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

        yield return new WaitForSecondsRealtime(itemSpawnTime);

        SpawnItem();
    }
    public IEnumerator PlayBallSizeGrowAnimation()
    {
        yield return null;
        Vector3 originalItemScale = item_BallSizeGrow.transform.localScale;
        while (true)
        {
            while (item_BallSizeGrow.transform.localScale.x < ItemMaxScale)
            {
                yield return new WaitForSeconds(0.05f);
                item_BallSizeGrow.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            }

            item_BallSizeGrow.transform.localScale = new Vector3(ItemMaxScale, ItemMaxScale, ItemMaxScale);

            while (item_BallSizeGrow.transform.localScale.x > originalItemScale.x)
            {
                yield return new WaitForSeconds(0.05f);
                item_BallSizeGrow.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
            }

            item_BallSizeGrow.transform.localScale = originalItemScale;
        }
    }

    public IEnumerator PlayPlayerItemAnimation()
    {
        yield return null;
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            Item_PlayerSizeGrow.transform.Rotate(new Vector3(playerItemRotationSpeed, playerItemRotationSpeed, playerItemRotationSpeed));
        }
    }
}
