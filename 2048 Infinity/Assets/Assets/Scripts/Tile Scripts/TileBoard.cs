using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;

public class TileBoard : MonoBehaviour
{
    public GameManager gameManager;
    public Tile tilePrefab;
    public TileColorData[] tileState;

    private TileGrid grid;
    private List<Tile> tiles;
    private bool waiting;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16); //16 is 4x4 (board size)
    }

    public void ClearBoard()
    {
        foreach (var cell in grid.cells)
        {
            cell.tile = null;
        }

        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        tiles.Clear();
    }

    public void CreateTile() //random spawn 
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileState[0], 2); //spawn 2 first tiles
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    private void Update()
    {
        if (!waiting)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveTiles(Vector2Int.up, 0 , 1, 1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);            
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTiles(Vector2Int.left, 1 , 1, 0 , 1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTiles(Vector2Int.right, grid.width - 2, -1 , 0, 1);        
        }
        }
    }

    private void MoveTiles(Vector2Int direction, 
    int startX, int IncrementX,
    int startY, int IncrementY)
    {
        bool changed = false;
        for (int x = startX; x >= 0 && x < grid.width; x += IncrementX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += IncrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile (Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                //Number merged
                if (CanMerge(tile, adjacent.tile))
                {
                    Merged(tile, adjacent.tile);
                    return true;
                }
                break;
            }

            newCell = adjacent; //
            adjacent = grid.GetAdjacentCell(adjacent, direction); //
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }

    private bool CanMerge (Tile a, Tile b)
    {
        return a.number == b.number && !b.locked;
    }

    private void Merged(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merged(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileState.Length -1);
        int number = b.number * 2;

        b.SetState(tileState[index], number);

        gameManager.IncreaseScore(number);
    }

    private int IndexOf(TileColorData state)
    {
        for (int i = 0; i < tileState.Length; i++)
        {
            if (state == tileState[i])
            {
                return i;
            }
        }

        return -1; //return -1 when its not found
    }

    private IEnumerator WaitForChanges()
    {
        waiting = true;

        yield return new WaitForSeconds(0.1f);

        waiting = false;

        //
        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        //
        if (tiles.Count != grid.size)
        {
            CreateTile();
        }     

        if(CheckForgameOver())
        {
            gameManager.GameOver();
        }
    }

    private bool CheckForgameOver()
    {
        if (tiles.Count != grid.size) return false;

        foreach (var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            if (up != null && CanMerge(tile, up.tile)) return false;

            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            if (down != null && CanMerge(tile, down.tile)) return false;

            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            if (left != null && CanMerge(tile, left.tile)) return false;

            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);
            if (right != null && CanMerge(tile, right.tile)) return false;
        }

        return true;
    }
}
