using System.Collections.Generic;
using UnityEngine;

public class MyPlayerInput3 : MonoBehaviour
{
    public bool IsPressLeft => Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
    public bool IsPressRight => Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
    public bool IsPressUp => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
    public bool IsPressDown => Input.GetKeyDown(KeyCode.Space);

    enum Direction { none, sol, sag, asagi, yukari }

    private Direction suruklemeYonu = Direction.none;
    private Direction suruklemeBittiYonu = Direction.none;

    private float sonrakiDokunmaZamani;
    private float sonrakiSuruklemeZamani;

    [Range(0.05f, 1f)] public float minDokunmaZamani = 0.15f;
    [Range(0.05f, 1f)] public float minSuruklemeZamani = 0.3f;

    private bool dokundumu = false;

    private float moveCooldown = 0.2f; 
    private float lastMoveTime = 0f;

    private bool isBlockPlaced = false; 

    private Vector2 lastDragPosition;

    private void OnEnable()
    {
        TouchManager.DragEvent += SurukleFNC;
        TouchManager.SwipeEvent += SurukleBittiFNC;
        TouchManager.TapEvent += TapFNC;
    }

    private void OnDisable()
    {
        TouchManager.DragEvent -= SurukleFNC;
        TouchManager.SwipeEvent -= SurukleBittiFNC;
        TouchManager.TapEvent -= TapFNC;
    }

    private void Update()
    {
        if (Time.time - lastMoveTime >= moveCooldown)
        {
            if (IsPressLeft || IsPressRight)
            {
                SagaSolaHareketFNC();
                lastMoveTime = Time.time;
            }
            else if (IsPressUp)
            {
                YukariHareketFNC();
                lastMoveTime = Time.time;
            }
            else if (IsPressDown)
            {
                if (!isBlockPlaced)
                {
                    AsagiHareketFNC();
                    lastMoveTime = Time.time;
                }
            }
            else if (suruklemeYonu == Direction.sag && Time.time > sonrakiDokunmaZamani)
            {
                SagaHareketFNC();
                sonrakiDokunmaZamani = Time.time + minDokunmaZamani;
                suruklemeYonu = Direction.none;
                lastMoveTime = Time.time;
            }
            else if (suruklemeYonu == Direction.sol && Time.time > sonrakiDokunmaZamani)
            {
                SolaHareketFNC();
                sonrakiDokunmaZamani = Time.time + minDokunmaZamani;
                suruklemeYonu = Direction.none;
                lastMoveTime = Time.time;
            }
            else if (suruklemeBittiYonu == Direction.yukari && Time.time > sonrakiSuruklemeZamani)
            {
                YukariHareketFNC();
                sonrakiSuruklemeZamani = Time.time + minSuruklemeZamani;
                suruklemeBittiYonu = Direction.none;
                lastMoveTime = Time.time;
            }
            else if (suruklemeYonu == Direction.asagi && Time.time > sonrakiDokunmaZamani)
            {
                if (!isBlockPlaced)
                {
                    AsagiHareketFNC();
                    suruklemeYonu = Direction.none;
                    lastMoveTime = Time.time;
                }
            }
            else if (dokundumu)
            {
                YukariHareketFNC();
                dokundumu = false;
                lastMoveTime = Time.time;
            }
        }

        dokundumu = false;
    }

    private void YukariHareketFNC()
    {
        Debug.Log("Yukarı tuşuna basıldı veya tıklama algılandı");
        Rotate();
    }

    private void AsagiHareketFNC()
    {
        Debug.Log("Boşluk tuşuna basıldı");
        MoveDownInstant();
    }

    private void SagaHareketFNC()
    {
        Debug.Log("Sağa kaydırıldı");
        var value = 1;
        var isMovable = GameManager3.Instance.IsInside(GetPreviewHorizontalPosition(value));
        if (isMovable)
            MoveHorizontal(value);
    }

    private void SolaHareketFNC()
    {
        Debug.Log("Sola kaydırıldı");
        var value = -1;
        var isMovable = GameManager3.Instance.IsInside(GetPreviewHorizontalPosition(value));
        if (isMovable)
            MoveHorizontal(value);
    }

    private void SagaSolaHareketFNC()
    {
        Debug.Log("Sağ veya sol tuşuna basıldı");
        var value = IsPressLeft ? -1 : 1;
        var isMovable = GameManager3.Instance.IsInside(GetPreviewHorizontalPosition(value));
        if (isMovable)
            MoveHorizontal(value);
    }

    private List<Vector2> GetPreviewPosition()
    {
        var result = new List<Vector2>();
        var listPiece = GameManager3.Instance.Current.ListPiece;
        var pivot = GameManager3.Instance.Current.transform.position;
        foreach (var piece in listPiece)
        {
            var position = piece.position;

            position -= pivot;
            position = new Vector3(position.y, -position.x, 0);
            position += pivot;

            result.Add(position);
        }
        return result;
    }

    Direction YonuBelirleFNC(Vector2 suruklemeHareket)
    {
        Direction suruklemeYonu = Direction.none;

        if (Mathf.Abs(suruklemeHareket.x) > Mathf.Abs(suruklemeHareket.y))
        {
            suruklemeYonu = (suruklemeHareket.x >= 0) ? Direction.sag : Direction.sol;
        }
        else
        {
            suruklemeYonu = (suruklemeHareket.y >= 0) ? Direction.yukari : Direction.asagi;
        }

        return suruklemeYonu;
    }

    void SurukleFNC(Vector2 suruklemeHareket)
    {
        if (Vector2.Distance(suruklemeHareket, lastDragPosition) > 1.0f) // Minimum mesafe kontrolü
        {
            suruklemeYonu = YonuBelirleFNC(suruklemeHareket);
            lastDragPosition = suruklemeHareket;
        }
    }

    void SurukleBittiFNC(Vector2 suruklemeHareket)
    {
        if (Vector2.Distance(suruklemeHareket, lastDragPosition) > 1.0f) // Minimum mesafe kontrolü
        {
            suruklemeBittiYonu = YonuBelirleFNC(suruklemeHareket);
            lastDragPosition = suruklemeHareket;
        }
    }

    void TapFNC(Vector2 suruklemeHareket)
    {
        dokundumu = true;
    }

    private List<Vector2> GetPreviewHorizontalPosition(int value)
    {
        var result = new List<Vector2>();
        var listPiece = GameManager3.Instance.Current.ListPiece;
        foreach (var piece in listPiece)
        {
            var position = piece.position;
            position.x += value;
            result.Add(position);
        }

        return result;
    }

    private void MoveHorizontal(int value)
    {
        var current = GameManager3.Instance.Current.transform;
        var position = current.position;
        position.x += value;
        current.position = position;
    }

    private void Rotate()
    {
        var current = GameManager3.Instance.Current.transform;
        var angles = current.eulerAngles;
        angles.z += -90;
        current.eulerAngles = angles;
    }

    private void MoveDownInstant()
    {
        var current = GameManager3.Instance.Current;
        while (GameManager3.Instance.IsInside(current.GetPreviewPosition()))
        {
            current.Move();
        }
        isBlockPlaced = true; 
    }

    public void ResetBlockPlaced()
    {
        isBlockPlaced = false; 
    }
}
