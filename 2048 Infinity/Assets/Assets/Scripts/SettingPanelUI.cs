using UnityEngine;
using UnityEngine.UI;

public class SettingPanelUI : MonoBehaviour
{
    [Header("Setting Panel")]
    [SerializeField] private GameObject settingPanel;

    [Header("Button")]
    [SerializeField] private Button musicButton;
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button settingBTN;
    [SerializeField] private Button closeSetting;

    private bool musicOn = true;
    private bool sfxOn = true;

    private void Awake()
    {
        settingPanel.SetActive(false);
    }

    private void Start()
    {
        musicButton.onClick.AddListener(ToggleMusic);
        sfxButton.onClick.AddListener(ToggleSFX);

        settingBTN.onClick.AddListener(OpenSetting);
        closeSetting.onClick.AddListener(CloseSetting);
    }

    public void OpenSetting()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        settingPanel.SetActive(false);
    }

    private void ToggleMusic()
    {
        musicOn = !musicOn;

        AudioManager.Instance.SetMusicVolume(musicOn ? 1f : 0f);
        PlayerPrefs.SetInt("MusicEnabled", musicOn ? 1 : 0);
    }

    private void ToggleSFX()
    {
        sfxOn = !sfxOn;

        AudioManager.Instance.SetSFXVolume(sfxOn ? 1f : 0f);
        PlayerPrefs.SetInt("SFXEnabled", sfxOn ? 1 : 0);
    }
}