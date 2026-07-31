using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private Button playButton;

    private bool isLoading;

    private void Awake()
    {
        if (playButton == null)
        {
            playButton = GetComponentInChildren<Button>();
        }

        if (playButton != null)
        {
            playButton.onClick.AddListener(LoadGameScene);
        }
        else
        {
            Debug.LogWarning("SceneLoader could not find a Play button.", this);
        }
    }

    public void LoadGameScene()
    {
        if (!isLoading)
        {
            StartCoroutine(LoadSceneAsync());
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        isLoading = true;

        if (playButton != null)
        {
            playButton.interactable = false;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(gameSceneName);

        if (operation == null)
        {
            Debug.LogError($"Unable to load scene '{gameSceneName}'. Add it to Build Settings.", this);
            isLoading = false;

            if (playButton != null)
            {
                playButton.interactable = true;
            }

            yield break;
        }

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
