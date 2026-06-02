using System.Collections; // ⚠️ 新增：使用协程必须引入这个命名空间
using UnityEngine;
using UnityEngine.UI;    // ⚠️ 新增：控制 Slider 组件必须引入
using UnityEngine.SceneManagement; 

public class MenuManager : MonoBehaviour
{
    [Header("UI 面板物体")]
    public GameObject mainMenuPanel; // 拖入你的主菜单面板
    public GameObject settingsPanel; // 拖入你的设置面板

    [Header("加载页面组件 (新加)")]
    public GameObject loadingPanel;   // 拖入你的 Loading 整个黑底面板
    public Slider loadingSlider;       // 拖入 Loading 面板里的小圆点进度条 Slider

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

    // 修改后：点击 New Game 按钮时调用这个方法
    public void StartNewGame()
    {
        // 开启后台异步加载，去加载你的 "Level_1" 场景
        StartCoroutine(LoadSceneAsyncCoroutine("Level_1")); 
    }

    // 新增：后台异步加载的核心逻辑
    IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        // 1. 显示黑色加载页面，关闭主菜单
        if (loadingPanel != null) loadingPanel.SetActive(true);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // 2. 让 Unity 在后台偷偷加载 "Level_1"
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        // 先不允许加载完了自动跳转，为了给进度条走满的时间
        operation.allowSceneActivation = false;

        // 3. 当关卡还没完全加载好时，每一帧刷新进度条
        while (!operation.isDone)
        {
            // operation.progress 范围是 0.0 到 0.9。除以 0.9 把它规范化到 0.0 ~ 1.0
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            if (loadingSlider != null)
            {
                // 让小圆点随着加载进度往右走
                loadingSlider.value = progress;
            }

            // 当后台已经 100% 加载完毕 (即 progress 达到 0.9)
            if (operation.progress >= 0.9f)
            {
                // 故意等 0.5 秒，让玩家能看一眼加载满的丝滑效果
                yield return new WaitForSeconds(0.5f);
                
                // 允许跳转，正式进入游戏
                operation.allowSceneActivation = true;
            }

            yield return null; // 每一帧等待，防止游戏画面卡死
        }
    }

    // 执行返回主菜单的逻辑（保持不变）
    public void GoBackToMainMenu()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); 
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);  
        }
        
        Debug.Log("已通过 Esc 或 Back 按钮返回主菜单");
    }

    // 打开设置菜单（保持不变）
    public void OpenSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }
}