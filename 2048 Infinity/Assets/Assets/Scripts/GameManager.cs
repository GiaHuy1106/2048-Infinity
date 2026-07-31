using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject StartPanel;
    [SerializeField] private float curtainPullDistance = 35f;
    [SerializeField] private float curtainPullDuration = 0.12f;
    [SerializeField] private float curtainFlyUpDistance = 900f;
    [SerializeField] private float curtainFlyUpDuration = 0.45f;

    [Header("Number Icon Animation")]
    [SerializeField] private RectTransform[] numberIcons;
    [SerializeField] private float numberIconPopDistance = 260f;
    [SerializeField] private float numberIconHorizontalSpread = 55f;
    [SerializeField] private float numberIconPopDuration = 0.42f;
    [SerializeField] private float numberIconStagger = 0.05f;

    [Header("Game UI")]
    [SerializeField] private TileBoard tileBoard;
    [SerializeField] private CanvasGroup gameOver;
    [SerializeField] private GameObject restartBTN;
    [SerializeField] private Button PlayGameBTN;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private int score;
    private RectTransform startPanelRect;
    private Vector2 startPanelStartPosition;
    private Vector2[] numberIconStartPositions;
    private Vector3[] numberIconStartScales;

    private void Start()
    {
        startPanelRect = StartPanel.GetComponent<RectTransform>();
        startPanelStartPosition = startPanelRect.anchoredPosition;
        CacheNumberIconTransforms();

        NewGame();
        PlayGameBTN.onClick.AddListener(StartGame);
        StartPanel.SetActive(true);
        AudioManager.Instance.PlayMusic();
    }

    #region Board build
    public void NewGame()
    {
        SetScore(0);
        highScoreText.text = LoadHighScore().ToString();

        AudioManager.Instance.PlayButtonClick();

        gameOver.alpha = 0f;
        gameOver.interactable = false;

        restartBTN.SetActive(true);

        tileBoard.ClearBoard();
        tileBoard.CreateTile();
        tileBoard.CreateTile();
        tileBoard.enabled = true;
    }
    #endregion

    #region Game Over Panel
    public void GameOver()
    {
        AudioManager.Instance.PlayGameOver();

        tileBoard.enabled = false;
        gameOver.interactable = true;

        StartCoroutine(Fade(gameOver, 1f, 1f));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from/*0f*/, to/*1f*/, elapsed/duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
        restartBTN.SetActive(false);
    }
    #endregion

    #region Score
    public void IncreaseScore (int points)
    {
        SetScore(score + points);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();

        SaveHighScore();
    }

    private void SaveHighScore()
    {
        int highScore = LoadHighScore();

        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }
    #endregion

    #region Start Game Panel
    private void StartGame()
    {
        restartBTN.SetActive(true);
        StartPanel.SetActive(true);
        ResetNumberIcons();
        StartCoroutine(FlyUpStartPanel());

        for (int i = 0; i < numberIcons.Length; i++)
        {
            StartCoroutine(PopNumberIcon(i));
        }
    }
    #region Panel Animation
    private IEnumerator FlyUpStartPanel()
    {
        PlayGameBTN.interactable = false;
        startPanelRect.anchoredPosition = startPanelStartPosition;

        Vector2 pulledDownPosition = startPanelStartPosition + Vector2.down * curtainPullDistance;
        yield return MoveStartPanel(startPanelStartPosition, pulledDownPosition, curtainPullDuration);

        Vector2 flyUpPosition = startPanelStartPosition + Vector2.up * curtainFlyUpDistance;
        yield return MoveStartPanel(pulledDownPosition, flyUpPosition, curtainFlyUpDuration);

        StartPanel.SetActive(false);
        startPanelRect.anchoredPosition = startPanelStartPosition;
        ResetNumberIcons();
        PlayGameBTN.interactable = true;
    }

    private IEnumerator MoveStartPanel(Vector2 from, Vector2 to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float progress = elapsed / duration;
            startPanelRect.anchoredPosition = Vector2.Lerp(from, to, progress * progress * (3f - 2f * progress));
            elapsed += Time.deltaTime;
            yield return null;
        }

        startPanelRect.anchoredPosition = to;
    }
    #endregion

    #region Numbers Animation
    private void CacheNumberIconTransforms()
    {
        numberIconStartPositions = new Vector2[numberIcons.Length];
        numberIconStartScales = new Vector3[numberIcons.Length];

        for (int i = 0; i < numberIcons.Length; i++)
        {
            numberIconStartPositions[i] = numberIcons[i].anchoredPosition;
            numberIconStartScales[i] = numberIcons[i].localScale;
        }
    }

    private IEnumerator PopNumberIcon(int index)
    {
        yield return new WaitForSeconds(index * numberIconStagger);

        RectTransform icon = numberIcons[index];
        Vector2 from = numberIconStartPositions[index];
        float horizontalOffset = (index - (numberIcons.Length - 1) * 0.5f) * numberIconHorizontalSpread;
        Vector2 to = from + new Vector2(horizontalOffset, numberIconPopDistance);

        float elapsed = 0f;
        while (elapsed < numberIconPopDuration)
        {
            float progress = elapsed / numberIconPopDuration;
            float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);

            icon.anchoredPosition = Vector2.Lerp(from, to, easedProgress);
            icon.localScale = Vector3.Lerp(numberIconStartScales[index], numberIconStartScales[index] * 1.2f, Mathf.Sin(progress * Mathf.PI));

            elapsed += Time.deltaTime;
            yield return null;
        }

        icon.anchoredPosition = to;
        icon.localScale = numberIconStartScales[index];
    }

    private void ResetNumberIcons()
    {
        for (int i = 0; i < numberIcons.Length; i++)
        {
            numberIcons[i].anchoredPosition = numberIconStartPositions[i];
            numberIcons[i].localScale = numberIconStartScales[i];
        }
    }
    #endregion

    #endregion
}
