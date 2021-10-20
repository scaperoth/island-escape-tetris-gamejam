using UnityEngine;
using UnityEngine.Events;

public class PlayField : MonoBehaviour
{
    [SerializeField]
    private GameData _gameData;

    public Transform[,] grid { get; private set; }
    public int width { get; private set; }
    public int height { get; private set; }

    [SerializeField]
    private Color _gizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

    public UnityEvent<int> OnLineCleared;

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

    public void DeleteRow(int x)
    {
        for (int z = 0; z < height; ++z)
        {
            grid[x, z].gameObject.SetActive(false);
            grid[x, z] = null;
        }
    }

    public void DecreaseRow(int x)
    {
        for (int z = 0; z < height; ++z)
        {
            if (grid[x, z] != null)
            {
                // Move one towards bottom
                grid[x + 1, z] = grid[x, z];
                grid[x, z] = null;

                // Update Block position
                grid[x+1, z].position += Vector3.right;
            }
        }
    }

    public void DecreaseRowsAbove(int x)
    {
        for (int i = x; i >= 0; --i)
            DecreaseRow(i);
    }

    public bool IsRowFull(int x)
    {
        for (int z = 0; z < height; ++z)
            if (grid[x, z] == null)
                return false;
        return true;
    }

    public void DeleteFullRows()
    {
        int numRowsDeleted = 0;
        for (int x = 0; x < width; ++x)
        {
            if (IsRowFull(x))
            {
                DeleteRow(x);
                DecreaseRowsAbove(x);
                numRowsDeleted++;
            }
        }

        if(numRowsDeleted > 0)
        {
            OnLineCleared.Invoke(numRowsDeleted);
        }
    }
}
