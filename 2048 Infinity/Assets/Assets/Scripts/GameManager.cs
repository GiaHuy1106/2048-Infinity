using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game UI")]
    [SerializeField] private TileBoard tileBoard;
    [SerializeField] private CanvasGroup gameOver;
    [SerializeField] private GameObject restartBTN;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private int score;

    private void Start()
    {
        NewGame();
        restartBTN.SetActive(true);
        AudioManager.Instance.PlayMusic();
    }

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
}
