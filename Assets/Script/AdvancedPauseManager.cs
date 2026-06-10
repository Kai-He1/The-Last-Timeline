using UnityEngine;
using UnityEngine.SceneManagement;

public class AdvancedPauseManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuPanel;   // 拖入你的 暂停主菜单 面板
    public GameObject settingsPanel;    // 拖入你的 设置 面板

    private bool isPaused = false;

    void Start()
    {
        // 游戏刚开始时，确保所有面板都是隐藏的，时间正常流动
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        // 监听键盘的 Esc 键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // 【核心逻辑】如果当前设置面板正打开着，按 Esc 应该返回暂停面板
                if (settingsPanel.activeSelf)
                {
                    OpenPauseMenu();
                }
                // 如果设置面板没开（也就是只有暂停面板开着），按 Esc 应该直接恢复游戏
                else if (pauseMenuPanel.activeSelf)
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }
        }
    }

    // 触发暂停
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        OpenPauseMenu(); // 默认打开暂停面板
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 恢复游戏 (对应 Resume 按钮)
    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // --- 面板切换逻辑 ---

    // 1. 显示暂停面板，隐藏设置面板 (对应 Settings 面板里的 Back 按钮，以及 Esc 返回)
    public void OpenPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    // 2. 显示设置面板，隐藏暂停面板 (对应 暂停面板里的 Settings 按钮)
    public void OpenSettings()
    {
        pauseMenuPanel.SetActive(false); // 关闭暂停面板
        settingsPanel.SetActive(true);   // 打开设置面板
    }

    // --- 其他按钮 ---
    public void BackToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}