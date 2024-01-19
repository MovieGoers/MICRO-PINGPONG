using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    GameObject m_mainMenu;
    GameObject m_GameOver;
    GameObject m_GamePanel;

    public GameObject scoreText;
    public GameObject finalScoreText;
    public GameObject highScoreText;

    public enum Panels
    {
        MainMenu,
        GameOver,
        GamePanel,
        SetAllPanelsFalse
    }

    public static UIManager Instance
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


        scoreText = GameObject.Find("Canvas/Game Panel/Score");
        finalScoreText = GameObject.Find("Canvas/Game Over/Final Score");
        highScoreText = GameObject.Find("Canvas/Game Over/High Score");

        m_mainMenu = GameObject.Find("Canvas/Main Menu");
        m_GameOver = GameObject.Find("Canvas/Game Over");
        m_GamePanel = GameObject.Find("Canvas/Game Panel");
    }

    public void SetPanels(Panels panel)
    {
        switch (panel)
        {
            case Panels.MainMenu:
                m_mainMenu.SetActive(true);
                m_GameOver.SetActive(false);
                m_GamePanel.SetActive(false);
                break;
            case Panels.GameOver:
                m_mainMenu.SetActive(false);
                m_GameOver.SetActive(true);
                m_GamePanel.SetActive(false);
                break;
            case Panels.GamePanel:
                m_mainMenu.SetActive(false);
                m_GameOver.SetActive(false);
                m_GamePanel.SetActive(true);
                break;
            case Panels.SetAllPanelsFalse:
                m_mainMenu.SetActive(false);
                m_GameOver.SetActive(false);
                m_GamePanel.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void SetScoreTextAlpha(float alpha)
    {
        Color color_alphaZero = scoreText.GetComponent<Text>().color;
        color_alphaZero.a = alpha;
        scoreText.GetComponent<Text>().color = color_alphaZero;
    }

    public void SetScoreText(int score)
    {
        scoreText.GetComponent<Text>().text = "" + score;

    }

    public void SetHighScoreText(int highScore)
    {
        highScoreText.GetComponent<Text>().text = "HIGH SCORE\n" + highScore;
    }

    public void SetFinalScoreText(int finalScore)
    {
        finalScoreText.GetComponent<Text>().text = "" + finalScore;
    }

    public IEnumerator ShowScore()
    {
        yield return null;
        float text_alpha = 1.0f;

        UIManager.Instance.SetScoreText(GameManager.Instance.score);

        while (text_alpha > 0.0f)
        {
            SetScoreTextAlpha(text_alpha);
            text_alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
