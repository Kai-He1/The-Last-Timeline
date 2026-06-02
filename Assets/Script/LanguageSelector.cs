using UnityEngine;
using TMPro; // 必须引入 TextMeshPro 命名空间

public class LanguageSelector : MonoBehaviour
{
    [Header("UI 文本组件")]
    public TextMeshProUGUI languageText; // 拖入中间显示语言的 Text (TMP)

    [Header("支持的语言列表")]
    public string[] languages = { "English", "中文", "日本語", "Español", "Français" };

    private int currentIndex = 0; // 当前选中的语言索引（0 代表 English）

    void Start()
    {
        // 游戏启动时，初始化显示当前的语言
        UpdateLanguageText();
    }

    // 点击右箭头 `>` 时调用
    public void NextLanguage()
    {
        currentIndex++;
        // 如果超过了列表长度，循环回到第一个 (0)
        if (currentIndex >= languages.Length)
        {
            currentIndex = 0;
        }
        UpdateLanguageText();
    }

    // 点击左箭头 `<` 时调用
    public void PreviousLanguage()
    {
        currentIndex--;
        // 如果小于 0，循环回到最后一个
        if (currentIndex < 0)
        {
            currentIndex = languages.Length - 1;
        }
        UpdateLanguageText();
    }

    // 刷新界面上的文本显示
    private void UpdateLanguageText()
    {
        if (languageText != null)
        {
            languageText.text = languages[currentIndex];
        }
        
        // 打印当前选中的语言，方便你调试
        Debug.Log("当前选择的语言是: " + languages[currentIndex]);
    }

    // 提供给其他脚本获取当前语言名称的接口
    public string GetCurrentLanguage()
    {
        return languages[currentIndex];
    }
}