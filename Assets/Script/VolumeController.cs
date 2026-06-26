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
            // 【修改部分】2. 初始化：优先从本地读取保存的音量，如果没有保存过，则默认使用当前音乐的音量
            float defaultVolume = audioSource.volume;
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume", defaultVolume);

            // 让滑动条位置和实际音量都等于读取到的音量数值
            musicSlider.value = savedVolume;
            audioSource.volume = savedVolume;

            // 3. 核心：用代码监听滑动条的实时数值变化
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

            // 【添加部分】将当前音量实时保存在玩家的本地电脑/手机上
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save(); // 确认保存
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