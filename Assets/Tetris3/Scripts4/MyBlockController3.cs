using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBlockController3: MonoBehaviour
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
            var delay = GameManager3.Instance.GameSpeed;
            yield return new WaitForSeconds(delay);

            var isMovable = GameManager3.Instance.IsInside(GetPreviewPosition());
            if (isMovable)
                Move();
            else
            {
                foreach (var piece in listPiece)
                {
                    int x = Mathf.RoundToInt(piece.position.x);
                    int y = Mathf.RoundToInt(piece.position.y);

                    if (y >= GameManager3.Instance.Grid.GetLength(1))
                    {
                        //GameOver
                        GameManager3.Instance.OnGameOver();
                        yield break;
                    }

                    GameManager3.Instance.Grid[x, y] = true;
                }

                GameManager3.Instance.UpdateRemoveObjectController();

                GameManager3.Instance.Spawn();
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