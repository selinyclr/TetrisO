using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBlockController4 : MonoBehaviour
{
    public List<Transform> ListPiece => listPiece;
    [SerializeField] private List<Transform> listPiece = new List<Transform>();

    private void Start()
    {
        StartCoroutine(MoveDown());
    }

    IEnumerator MoveDown()
    {
        while (true)
        {
            var delay = GameManager4.Instance.GameSpeed;
            yield return new WaitForSeconds(delay);

            var isMovable = GameManager4.Instance.IsInside(GetPreviewPosition());
            if (isMovable)
                Move();
            else
            {
                foreach (var piece in listPiece)
                {
                    int x = Mathf.RoundToInt(piece.position.x);
                    int y = Mathf.RoundToInt(piece.position.y);

                    if (y >= GameManager4.Instance.Grid.GetLength(1))
                    {
                        //GameOver
                        GameManager4.Instance.OnGameOver();
                        yield break;
                    }

                    GameManager4.Instance.Grid[x, y] = true;
                }

                GameManager4.Instance.UpdateRemoveObjectController();

                GameManager4.Instance.Spawn();
                break;
            }
        }
    }

    public List<Vector2> GetPreviewPosition()
    {
        var result = new List<Vector2>();
        foreach (var piece in listPiece)
        {
            var position = piece.position;
            position.y--;
            result.Add(position);
        }

        return result;
    }

    public void Move()
    {
        var position = transform.position;
        position.y--;
        transform.position = position;
    }
}