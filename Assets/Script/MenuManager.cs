using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("UI 面板物体")]
    public GameObject mainMenuPanel; // 拖入你的主菜单面板
    public GameObject settingsPanel; // 拖入你的设置面板

    void Update()
    {
        // 实时检测玩家是否按下了键盘上的 Escape (Esc) 键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 只有当设置面板正处于打开状态时，按下 Esc 才会触发返回
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                GoBackToMainMenu();
            }
        }
    }

    // 执行返回主菜单的逻辑
    public void GoBackToMainMenu()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // 隐藏设置面板
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);  // 重新显示主菜单面板
        }
        
        Debug.Log("已通过 Esc 或 Back 按钮返回主菜单");
    }

    // 如果你在主菜单点击了 "Setting" 按钮，可以用这个方法打开设置
    public void OpenSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }
}