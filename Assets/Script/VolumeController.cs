using UnityEngine;
using UnityEngine.UI; // 必须引入 UI 命名空间

public class VolumeController : MonoBehaviour
{
    [Header("音频与UI组件")]
    public AudioSource audioSource; // 拖入你的 BackgroundMusic 物体
    public Slider musicSlider;     // 拖入你的 Music Slider 物体

    void Start()
    {
        // 1. 确保读取到组件
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        if (musicSlider != null && audioSource != null)
        {
            // 2. 初始化：让滑动条的默认位置等于当前音乐的音量大小
            musicSlider.value = audioSource.volume;

            // 3. 核心：用代码监听滑动条的实时数值变化
            // 当滑动条被拖动时，会自动触发下面的 OnSliderValueChanged 方法
            musicSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    // 当滑动条数值改变时执行的方法
    private void OnSliderValueChanged(float value)
    {
        if (audioSource != null)
        {
            // 将滑动条的数值（0.0 ~ 1.0）直接赋值给音频的音量
            audioSource.volume = value;
        }
    }

    void OnDestroy()
    {
        // 良好的编程习惯：销毁时移除监听，防止内存泄漏
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}