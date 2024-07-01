using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float moveDelay = 0.1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float moveTime;
    private float lockTime;

    private enum Direction { none, left, right, down, up }
    private Direction dragDirection = Direction.none;
    private Direction swipeDirection = Direction.none;

    private float nextTouchTime;
    private float nextSwipeTime;

    [Range(0.05f, 1f)] public float minTouchTime = 0.15f;
    [Range(0.05f, 1f)] public float minSwipeTime = 0.3f;

    private bool tapped = false;

    private Vector2 lastDragPosition;
    private bool blockLocked = false;

    private void OnEnable()
    {
        TouchManager.DragEvent += OnDrag;
        TouchManager.SwipeEvent += OnSwipe;
        TouchManager.TapEvent += OnTap;
    }

    private void OnDisable()
    {
        TouchManager.DragEvent -= OnDrag;
        TouchManager.SwipeEvent -= OnSwipe;
        TouchManager.TapEvent -= OnTap;
    }

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.data = data;
        this.board = board;
        this.position = position;

        rotationIndex = 0;
        stepTime = Time.time + stepDelay;
        moveTime = Time.time + moveDelay;
        lockTime = 0f;
        blockLocked = false;

        if (cells == null)
        {
            cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update()
    {
        if (blockLocked) return; // Eğer blok kilitlendiyse güncelleme yapma

        board.Clear(this);

        lockTime += Time.deltaTime;

        if (tapped)
        {
            Rotate(1);
            tapped = false;
        }

        if (Time.time > moveTime)
        {
            HandleMoveInputs();
        }

        if (Time.time > stepTime)
        {
            Step();
        }

        board.Set(this);
    }

    private void HandleMoveInputs()
    {
        if (dragDirection == Direction.left && Time.time > nextTouchTime)
        {
            Move(Vector2Int.left);
            nextTouchTime = Time.time + minTouchTime;
            dragDirection = Direction.none;
        }
        else if (dragDirection == Direction.right && Time.time > nextTouchTime)
        {
            Move(Vector2Int.right);
            nextTouchTime = Time.time + minTouchTime;
            dragDirection = Direction.none;
        }
        else if (swipeDirection == Direction.up && Time.time > nextSwipeTime)
        {
            Rotate(1);
            nextSwipeTime = Time.time + minSwipeTime;
            swipeDirection = Direction.none;
        }
        // Aşağıdaki blok kaldırıldı
        /*
        else if (dragDirection == Direction.down && Time.time > nextTouchTime)
        {
            HardDrop();
            dragDirection = Direction.none;
        }
        */
    }

    private void Step()
    {
        stepTime = Time.time + stepDelay;

        if (!Move(Vector2Int.down))
        {
            Lock();
        }
    }

    private void HardDrop()
    {
        // Blok kilitlenmesini doğru şekilde ayarla
        blockLocked = true;

        while (Move(Vector2Int.down))
        {
            continue;
        }

        Lock();
    }

    private void Lock()
    {
        board.Set(this);
        board.ClearLines();

        // Gecikmeli yeni blok oluşturma ve blockLocked durumunu false yapma
        Invoke("SpawnNewPiece", 0.1f);
    }

    private void SpawnNewPiece()
    {
        board.SpawnPiece();
        blockLocked = false;
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = position + (Vector3Int)translation;

        if (board.IsValidPosition(this, newPosition))
        {
            position = newPosition;
            lockTime = 0f;
            return true;
        }

        return false;
    }

    private void Rotate(int direction)
    {
        int originalRotation = rotationIndex;
        rotationIndex = Wrap(rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);

        if (!TestWallKicks(rotationIndex, direction))
        {
            rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        float[] matrix = data.RotationMatrix;

        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int cell = cells[i];

            float x = cell.x;
            float y = cell.y;

            // Adjust for I and O tetromino rotations
            switch (data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    x -= 0.5f;
                    y -= 0.5f;
                    break;
            }

            int newX = Mathf.RoundToInt((x * matrix[0] * direction) + (y * matrix[1] * direction));
            int newY = Mathf.RoundToInt((x * matrix[2] * direction) + (y * matrix[3] * direction));

            // Adjust back for I and O tetromino rotations
            switch (data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    newX += 1;
                    newY += 1;
                    break;
            }

            cells[i] = new Vector3Int(newX, newY, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }

    private Direction DetermineDirection(Vector2 dragMovement)
    {
        Direction dragDirection = Direction.none;

        if (Mathf.Abs(dragMovement.x) > Mathf.Abs(dragMovement.y))
        {
            dragDirection = (dragMovement.x >= 0) ? Direction.right : Direction.left;
        }
        else
        {
            dragDirection = (dragMovement.y >= 0) ? Direction.up : Direction.down;
        }

        return dragDirection;
    }

    private void OnDrag(Vector2 dragMovement)
    {
        if (Vector2.Distance(dragMovement, lastDragPosition) > 1.0f)
        {
            dragDirection = DetermineDirection(dragMovement);
            lastDragPosition = dragMovement;
        }
    }

    private void OnSwipe(Vector2 dragMovement)
    {
        if (Vector2.Distance(dragMovement, lastDragPosition) > 1.0f)
        {
            swipeDirection = DetermineDirection(dragMovement);
            lastDragPosition = dragMovement;
        }
    }

    private void OnTap(Vector2 dragMovement)
    {
        tapped = true;
    }
}
