using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileBoard tileBoard;

    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        tileBoard.ClearBoard();
        tileBoard.CreateTile();
        tileBoard.CreateTile();
        tileBoard.enabled = true;
    }

    public void GameOver()
    {
        tileBoard.enabled = false;
    }
}
