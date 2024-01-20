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
    GameObject m_Options;

    public GameObject scoreText;
    public GameObject finalScoreText;
    public GameObject highScoreText;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    Rect screenRect;

    public bool isCursorOutOfScreen;

    int currentResolutionIndex;

    public enum Panels
    {
        MainMenu,
        GameOver,
        GamePanel,
        Options,
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
        m_Options = GameObject.Find("Canvas/Options");
    }

    private void Start()
    {
        isCursorOutOfScreen = true;

        screenRect = new Rect(0, 0, Screen.width, Screen.height);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option_str = resolutions[i].width + "X" + resolutions[i].height;
            options.Add(option_str);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        if (!screenRect.Contains(Input.mousePosition))
        {
            isCursorOutOfScreen = true;
            HandleCursorOutOfScreen(); 
        }
        else
        {
            isCursorOutOfScreen = false;
            HandleCursorOnScreen();
        }

    }

    public void SetPanels(Panels panel)
    {
        switch (panel)
        {
            case Panels.MainMenu:
                m_mainMenu.SetActive(true);
                m_GameOver.SetActive(false);
                m_GamePanel.SetActive(false);
                m_Options.SetActive(false);
                break;
            case Panels.GameOver:
                m_mainMenu.SetActive(false);
                m_GameOver.SetActive(true);
                m_GamePanel.SetActive(false);
                m_Options.SetActive(false);
                break;
            case Panels.GamePanel:
                m_mainMenu.SetActive(false);
                m_GameOver.SetActive(false);
                m_GamePanel.SetActive(true);
                m_Options.SetActive(false);
                break;
            case Panels.SetAllPanelsFalse:
                m_mainMenu.SetActive(false);
                m_GameOver.SetActive(false);
                m_GamePanel.SetActive(false);
                m_Options.SetActive(false);
                break;
            case Panels.Options:
                m_mainMenu.SetActive(false);
                m_GameOver.SetActive(false);
                m_GamePanel.SetActive(false);
                m_Options.SetActive(true);
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
            text_alpha -= 0.03f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void HideCursor()
    {
        Cursor.visible = false;
    }

    public void DisplayCursor()
    {
        Cursor.visible = true;
    }

    void HandleCursorOutOfScreen()
    {
        if(GameManager.Instance.gameState == GameManager.GameStates.GameState)
        {
            DisplayCursor();
        }
    }

    void HandleCursorOnScreen()
    {
        if (GameManager.Instance.gameState == GameManager.GameStates.GameState)
        {
            HideCursor();
        }
    }

    public void SetResolutionByValue(int width, int height)
    {
        Screen.SetResolution(width, height, Screen.fullScreen);
        screenRect = new Rect(0, 0, width, height);
    }
    public void SetResolutionByIndex(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
        screenRect = new Rect(0, 0, resolutions[index].width, resolutions[index].height);
    }

    public void ToggleFullScreen(bool isOn)
    {
        Screen.fullScreen = isOn;
    }
}
