using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    [SerializeField]
    private GameData _gameData;

    public Transform[,] grid { get; private set; }
    public int width { get; private set; }
    public int height { get; private set; }

    [SerializeField]
    private Color _gizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

    void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawCube(transform.position, new Vector3(_gameData.width, 0.1f, _gameData.height));
    }

    private void Awake()
    {
        width = _gameData.width;
        height = _gameData.height;
        grid = new Transform[_gameData.width, _gameData.height];
    }

    public Vector3 RoundVec3(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x),
                           Mathf.Round(v.y),
                           Mathf.Round(v.z));
    }

    public bool InsideBorder(Vector3 pos)
    {
        return ((int)pos.z >= 0 &&
                (int)pos.z < height &&
                (int)pos.x < width);
    }

    public void DeleteRow(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void DecreaseRow(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] != null)
            {
                // Move one towards bottom
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                // Update Block position
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < height; ++i)
            DecreaseRow(i);
    }

    public bool IsRowFull(int y)
    {
        for (int x = 0; x < width; ++x)
            if (grid[x, y] == null)
                return false;
        return true;
    }

    public void DeleteFullRows()
    {
        for (int y = 0; y < height; ++y)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                --y;
            }
        }
    }
}
