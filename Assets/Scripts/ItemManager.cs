using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;

    public GameObject item_BallSizeGrow;
    public GameObject item_PlayerSizeGrow;
    public GameObject item_ScoreDouble;

    public GameObject ItemSpawnStartPoint;
    public GameObject ItemSpawnEndPoint;

    public float itemDuration;
    public float itemSpawnTime;
    public float ItemMaxScale;

    public float ballMaxScale;

    public float playerMaxScale;
    public float itemRotationSpeed;

    public Coroutine[] coroutineEffect;

    int itemNum;

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
        itemNum = 3;
        coroutineEffect = new Coroutine[itemNum];
        StartCoroutine("PlayBallSizeGrowAnimation");
        StartCoroutine("PlayPlayerItemAnimation");
        StartCoroutine("PlayScoreItemAnimation");
    }

    public void SpawnItem(GameObject item)
    {
        Vector3 spawnPosition;

/*        int item_randomInt = Random.Range(0, 2);
        switch (item_randomInt) {
            case 0:
                item = item_BallSizeGrow;
                break;
            case 1:
                item = item_PlayerSizeGrow;
                break;
            default:
                break;
        }*/

        item.SetActive(true);

        spawnPosition.x = Random.Range(ItemSpawnStartPoint.transform.position.x, ItemSpawnEndPoint.transform.position.x);
        spawnPosition.y = Random.Range(ItemSpawnStartPoint.transform.position.y, ItemSpawnEndPoint.transform.position.y);
        spawnPosition.z = Random.Range(ItemSpawnStartPoint.transform.position.z, ItemSpawnEndPoint.transform.position.z);

        item.transform.position = spawnPosition;
    }

    public void ActivateItemEffect(string item)
    {
        AudioManager.Instance.Play("ItemGet");
        switch (item)
        {
            case "Item-BallSizeGrow":
                coroutineEffect[0] = StartCoroutine(IncreaseBallSize());
                break;
            case "Item-PlayerSizeGrow":
                coroutineEffect[1] = StartCoroutine(IncreasePlayerSize());
                break;
            case "Item-ScoreDouble":
                coroutineEffect[2] = StartCoroutine(DoubleScore());
                break;
            default:
                break;
        }
    }

    public void DeactivateItemEffect()
    {
        for(int i = 0; i < itemNum; i++)
        {
            if (coroutineEffect[i] != null)
                StopCoroutine(coroutineEffect[i]);
        }

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

        SpawnItem(item_PlayerSizeGrow);
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

        SpawnItem(item_BallSizeGrow);
    }

    IEnumerator DoubleScore()
    {
        yield return null;
        PlayerScript.Instance.addingScore = 2;
        yield return new WaitForSeconds(itemDuration);
        PlayerScript.Instance.addingScore = 1;

        yield return new WaitForSecondsRealtime(itemSpawnTime);

        SpawnItem(item_ScoreDouble);
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
            item_PlayerSizeGrow.transform.Rotate(new Vector3(itemRotationSpeed, itemRotationSpeed, itemRotationSpeed));
        }
    }

    public IEnumerator PlayScoreItemAnimation()
    {
        yield return null;
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            item_ScoreDouble.transform.Rotate(new Vector3(itemRotationSpeed, itemRotationSpeed, itemRotationSpeed));
        }
    }
}
