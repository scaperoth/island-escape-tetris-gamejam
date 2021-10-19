using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "GameData")]
public class GameData : ScriptableObject
{
    public int width = 20;
    public int height = 10;
    public float moveSpeed = 1;
}
