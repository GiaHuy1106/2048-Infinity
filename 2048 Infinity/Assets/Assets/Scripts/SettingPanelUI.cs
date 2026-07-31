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

    [Header("Sprite")]
    [SerializeField] private Image musicIcon;
    [SerializeField] private Sprite[] MusicIcon;
    [SerializeField] private Image sfxIcon;
    [SerializeField] private Sprite[] SfxIcon;

    private bool musicOn = true;
    private bool sfxOn = true;

    private void Awake()
    {
        settingPanel.SetActive(false);
    }

    private void Start()
    {
        musicButton.onClick.AddListener(ToggleMusic);
        UpdateMusicIcon();

        sfxButton.onClick.AddListener(ToggleSFX);
        UpdateSfxIcon();

        settingBTN.onClick.AddListener(OpenSetting);
        closeSetting.onClick.AddListener(CloseSetting);
        
    }

    private void LateUpdate() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSetting();
        }
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
        UpdateMusicIcon();

        AudioManager.Instance.SetMusicVolume(musicOn ? 1f : 0f);
        PlayerPrefs.SetInt("MusicEnabled", musicOn ? 1 : 0);

    }

    private void ToggleSFX()
    {
        sfxOn = !sfxOn;
        UpdateSfxIcon();

        AudioManager.Instance.SetSFXVolume(sfxOn ? 1f : 0f);
        PlayerPrefs.SetInt("SFXEnabled", sfxOn ? 1 : 0);
    }

    private void UpdateMusicIcon()
    {
        if (musicIcon == null || MusicIcon == null || MusicIcon.Length < 2) return;
        musicIcon.sprite = musicOn ? MusicIcon[0] : MusicIcon[1];
    }

    private void UpdateSfxIcon()
    {
        if (sfxIcon == null || SfxIcon == null || SfxIcon.Length < 2) return;
        sfxIcon.sprite = sfxOn ? SfxIcon[0] : SfxIcon[1];
    }
}
