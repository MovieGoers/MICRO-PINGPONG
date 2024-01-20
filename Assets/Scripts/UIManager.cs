using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public Toggle FullscreenToggleUI;

    public Dropdown resolutionDropdown;

    Rect screenRect;

    public bool isCursorOutOfScreen;

    int currentResolutionIndex;

    List<string> options;
    List<Vector2Int> resolutions;

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

        resolutionDropdown.ClearOptions();

        options = new List<string>();
        resolutions = new List<Vector2Int>();

        // 초기 옵션 설정 (추후에 리팩토링 필요.)
        options.Add("1280 X 720");
        resolutions.Add(new Vector2Int(1280, 720));
        options.Add("1600 X 900");
        resolutions.Add(new Vector2Int(1600, 900));
        options.Add("1920 X 1080");
        resolutions.Add(new Vector2Int(1920, 1080));

        // JSON 파일에서 해상도 읽어온 뒤에 설정.

        Settings setting = new Settings();

        if (File.Exists(Application.dataPath + "SettingsFile.json"))
        {
            setting = LoadScreenSettingsToJSON();
        }
        else
        {
            SaveScreenSettingsToJSON(1280, 720);
            setting.resolution_width = 1280;
            setting.resolution_height = 720;
        }

        SetResolutionByValue(setting.resolution_width, setting.resolution_height);
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (resolutions[i].x == Screen.width && resolutions[i].y == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        FullscreenToggleUI.isOn = Screen.fullScreen;

        screenRect = new Rect(0, 0, Screen.width, Screen.height);
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
        Screen.SetResolution(resolutions[index].x, resolutions[index].y, Screen.fullScreen);
        screenRect = new Rect(0, 0, resolutions[index].x, resolutions[index].y);
        SaveScreenSettingsToJSON(resolutions[index].x, resolutions[index].y);
    }

    public void ToggleFullScreen(bool isOn)
    {
        Screen.fullScreen = isOn;
    }

    public void SaveScreenSettingsToJSON(int width, int height)
    {
        Settings setting = new Settings();
        setting.resolution_width = width;
        setting.resolution_height = height;

        string json = JsonUtility.ToJson(setting, true);
        File.WriteAllText(Application.dataPath + "SettingsFile.json", json);
    }

    public Settings LoadScreenSettingsToJSON()
    {
        string json = File.ReadAllText(Application.dataPath + "SettingsFile.json");
        Settings setting = JsonUtility.FromJson<Settings>(json);

        return setting;
    }
}
