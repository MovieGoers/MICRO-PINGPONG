using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;

    public GameObject item_BallSizeGrow; // �� ũ�� ���� ������
    public GameObject item_PlayerSizeGrow; // �÷��̾� ũ�� ���� ������
    public GameObject item_ScoreDouble; // ���� ȹ�� �� �� ���� ������

    public GameObject ItemSpawnStartPoint; // ������ ���� ���� ��ġ ǥ�ø� ���� ���� ������Ʈ.
    public GameObject ItemSpawnEndPoint; // ������ ���� �� ��ġ ǥ�ø� ���� ���� ������Ʈ.

    public float itemDuration; // ������ ȿ�� ���� �ð�
    public float itemSpawnTime; // ������ ȿ�� ���� �� �ٽ� �����ϱ������ �ð�.
    public float ItemMaxScale; // ������ �ִ� ũ��

    public float ballMaxScale; // �� �ִ� ���� ũ��

    public float playerMaxScale; // �÷��̾� �ִ� ���� ũ��
    public float itemRotationSpeed; // ������ ȸ�� �ӵ�

    public Coroutine[] coroutineEffect; // ������ ȿ�� ���� ���� �ڷ�ƾ array.

    int itemNum; // ������ ��ü ����.

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

        // ������ �ִϸ��̼� ����.
        StartCoroutine("PlayBallSizeGrowAnimation");
        StartCoroutine("PlayPlayerItemAnimation");
        StartCoroutine("PlayScoreItemAnimation");
    }

    public void SpawnItem(GameObject item)
    {
        Vector3 spawnPosition;

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
                UIManager.Instance.SetItemEffectText("BIGGER BALL");
                coroutineEffect[0] = StartCoroutine(IncreaseBallSize());
                break;
            case "Item-PlayerSizeGrow":
                UIManager.Instance.SetItemEffectText("BIGGER PLAYER");
                coroutineEffect[1] = StartCoroutine(IncreasePlayerSize());
                break;
            case "Item-ScoreDouble":
                UIManager.Instance.SetItemEffectText("DOUBLE POINTS");
                coroutineEffect[2] = StartCoroutine(DoubleScore());
                break;
            default:
                break;
        }
        StartCoroutine("PlayItemEffectTextAnimation");
    }

    public void DeactivateItemEffect()
    {
        // ��� ������ ȿ�� �ڷ�ƾ ����.
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

        while (ball.transform.localScale.x < ballMaxScale)
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

    public IEnumerator PlayItemEffectTextAnimation()
    {
        yield return 0;

        UIManager.Instance.itemEffectText.SetActive(true);
        float text_alpha = 0.0f;

        for(int i=0;i<4;i++)
        {
            while (text_alpha < 1.0f)
            {
                UIManager.Instance.SetItemEffectTextAlpha(text_alpha);
                text_alpha += 0.03f;
                yield return new WaitForSeconds(0.01f);
            }
            while (text_alpha > 0.0f)
            {
                UIManager.Instance.SetItemEffectTextAlpha(text_alpha);
                text_alpha -= 0.03f;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
