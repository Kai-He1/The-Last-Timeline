using UnityEngine;
using UnityEngine.UI;

public class SpriteToggleSwitch : MonoBehaviour
{
    [Header("图片资源")]
    public Sprite onSprite;   // 开启状态的图片 (Switch_On)
    public Sprite offSprite;  // 关闭状态的图片 (Switch_Off)

    private Image buttonImage; // 按钮自身的 Image 组件
    private bool isOn = false; // 默认是关闭状态

    void Awake()
    {
        // 获取当前按钮上的 Image 组件
        buttonImage = GetComponent<Image>();
        
        // 初始化显示关闭状态的图片
        if (buttonImage != null && offSprite != null)
        {
            buttonImage.sprite = offSprite;
        }
    }

    // 当按钮被点击时调用
    public void ToggleSwitch()
    {
        // 状态取反
        isOn = !isOn;

        // 根据状态更换对应的图片
        if (isOn)
        {
            buttonImage.sprite = onSprite;
        }
        else
        {
            buttonImage.sprite = offSprite;
        }
    }

    // 提供给其他游戏逻辑获取状态（开/关）
    public bool IsSwitchOn()
    {
        return isOn;
    }
}