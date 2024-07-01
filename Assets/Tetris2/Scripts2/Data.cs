using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static readonly Dictionary<Tetromino, Vector2Int[]> Cells = new Dictionary<Tetromino, Vector2Int[]>
    {
        { Tetromino.I, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0) } },
        { Tetromino.J, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1) } },
        { Tetromino.L, new Vector2Int[] { new Vector2Int(2, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1) } },
        { Tetromino.O, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) } },
        { Tetromino.S, new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) } },
        { Tetromino.T, new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1) } },
        { Tetromino.Z, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(2, 1) } }
    };

    public static readonly Dictionary<Tetromino, Vector2Int[,]> WallKicks = new Dictionary<Tetromino, Vector2Int[,]>
    {
        { Tetromino.I, new Vector2Int[,] { { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(2, 0), new Vector2Int(-1, 2), new Vector2Int(2, -1) } } },
        { Tetromino.J, new Vector2Int[,] { { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 0) } } },
        { Tetromino.L, new Vector2Int[,] { { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 0) } } },
        { Tetromino.O, new Vector2Int[,] { { new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) } } },
        { Tetromino.S, new Vector2Int[,] { { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 0) } } },
        { Tetromino.T, new Vector2Int[,] { { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 0) } } },
        { Tetromino.Z, new Vector2Int[,] { { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 0) } } }
    };

    public static readonly Dictionary<Tetromino, float[]> RotationMatrices = new Dictionary<Tetromino, float[]>
    {
        { Tetromino.I, new float[] { 0, 1, -1, 0 } },
        { Tetromino.J, new float[] { 0, 1, -1, 0 } },
        { Tetromino.L, new float[] { 0, 1, -1, 0 } },
        { Tetromino.O, new float[] { 0, 1, -1, 0 } },
        { Tetromino.S, new float[] { 0, 1, -1, 0 } },
        { Tetromino.T, new float[] { 0, 1, -1, 0 } },
        { Tetromino.Z, new float[] { 0, 1, -1, 0 } }
    };
}
