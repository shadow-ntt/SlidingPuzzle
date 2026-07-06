using System.Collections.Generic;
using UnityEngine;
using Game;
public class GameBoard : MonoBehaviour
{
    [SerializeField] public Transform Piece;
    [SerializeField] public float gap;
    public List<Transform> Pieces = new List<Transform>();
    private GameManager gameManager;
    private int posEmpty;
    private float widthPiece;
    private float heightPiece;
    [SerializeField] public float aspect = 16 / 9;
    [SerializeField] public int row = 4;
    [SerializeField] public int col = 4;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    void Start()
    {

        widthPiece = (float)1 / row;
        heightPiece = (float)1 / col;
        float hBoard = widthPiece * row * transform.localScale.x * 2 + gap * (row - 1);
        float wBoard = heightPiece * col * transform.localScale.y * 2 + gap * (col - 1);
        var offset = new Vector2(0.25f + 0.5f + wBoard, 0.25f + hBoard);
        var gameCamera = Camera.main.GetComponent<GameCamera>();
        //vùng camera nhìn thấy từ đáy của Block tới đỉnh bảng chơi, margin=0.75
        gameCamera.View(
            new Rect(
                -offset.x / 2,
                -offset.y / 2,
                offset.x,
                offset.y
            ),
            hBoard
        );
    }

    // Update is called once per frame
    void Update()
    {
        //bắt sự kiện click chuột
        if (Input.GetMouseButtonDown(0) && !gameManager.isPause)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                for (int i = 0; i < this.Pieces.Count; i++)
                {
                    if (this.Pieces[i].position == hit.transform.position)
                    {
                        Debug.Log(gameManager.durationPlaying);
                        //trên, dưới không cần check, nên để col để luôn đúng
                        //dưới
                        if (SwapIfValid(i, col, col))
                        {
                            gameManager.moved += 1;
                            break;
                        }
                        // trên
                        if (SwapIfValid(i, -col, col))
                        {
                            gameManager.moved += 1;
                            break;
                        }
                        //phải
                        if (SwapIfValid(i, 1, col - 1))
                        {
                            gameManager.moved += 1;
                            break;
                        }
                        //trái
                        if (SwapIfValid(i, -1, 0))
                        {
                            gameManager.moved += 1;
                            break;
                        }
                    }
                }
            }

        }
    }

    public void GeneratePieces()
    {
        clearAllPiece();
        widthPiece = (float)1 / row;
        heightPiece = (float)1 / col;
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                Transform piece = Instantiate(Piece, transform);
                this.Pieces.Add(piece);
                //render từ dưới lên trên
                piece.transform.localPosition = new Vector3(-1 + 2 * (c + 0.5f) * heightPiece * aspect, -1 + 2 * (r + 0.5f) * widthPiece);
                piece.transform.localScale = new Vector3(heightPiece * 2 * aspect - gap, widthPiece * 2 - gap, 1);
                piece.name = $"{col * r + c}";
                if (r == row - 1 && c == col - 1)
                {
                    piece.gameObject.SetActive(false);
                    this.posEmpty = row * col - 1;
                    break;
                }
                //cắt ảnh
                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                Vector2[] uvs = new Vector2[4];
                uvs[0] = new Vector2(c * heightPiece, r * widthPiece);//(0,0)
                uvs[1] = new Vector2((c + 1) * heightPiece, r * widthPiece);//(1,0)
                uvs[2] = new Vector2(c * heightPiece, (r + 1) * widthPiece);//(0,1)
                uvs[3] = new Vector2((c + 1) * heightPiece, (r + 1) * widthPiece);//(1,1)
                mesh.uv = uvs;
            }
        }
    }
    bool SwapIfValid(int index, int offset, int colCheck)
    {
        int posSwap = index + offset;
        if (this.posEmpty == posSwap && index % col != colCheck && index < row * col && index > -1)
        {
            //đổi cả index và vị trí
            (this.Pieces[index], this.Pieces[posEmpty]) = (this.Pieces[posEmpty], this.Pieces[index]);
            (this.Pieces[index].localPosition, this.Pieces[posEmpty].localPosition) = (this.Pieces[posEmpty].localPosition, this.Pieces[index].localPosition);
            posEmpty = index;
            return true;
        }
        return false;
    }

    public bool isComplete()
    {
        for (int i = 0; i < this.Pieces.Count; i++)
        {
            if (!this.Pieces[i].transform.name.Equals($"{i}"))
            {
                return false;
            }
        }
        return true;
    }
    public void Shuffle(int countShuffle)
    {
        int count = 0;
        while (count != countShuffle)
        {
            int[] offset = { posEmpty - 1, posEmpty + 1, posEmpty - col, posEmpty + col };
            int rdOffset = offset[UnityEngine.Random.Range(0, offset.Length)];
            if (SwapIfValid(rdOffset, col, col)) continue;//dưới
            if (SwapIfValid(rdOffset, -col, col)) continue;//trên
            if (SwapIfValid(rdOffset, 1, col - 1)) continue;//phải
            if (SwapIfValid(rdOffset, -1, 0)) continue;//trái
            count++;
        }
    }
    public void clearAllPiece()
    {
        foreach (var piece in this.Pieces)
        {
            Destroy(piece.gameObject);
            this.Pieces = new List<Transform>();
        }
    }
    // public IEnumerator Shuffle(int countShuffle, int waitSecond)
    // {
    //     yield return new WaitForSeconds(waitSecond);
    //     int count = 0;
    //     while (count != countShuffle)
    //     {
    //         int[] offset = { posEmpty - 1, posEmpty + 1, posEmpty - col, posEmpty + col };
    //         int rdOffset = offset[UnityEngine.Random.Range(0, offset.Length)];
    //         if (SwapIfValid(rdOffset, col, col)) continue;//dưới
    //         if (SwapIfValid(rdOffset, -col, col)) continue;//trên
    //         if (SwapIfValid(rdOffset, 1, col - 1)) continue;//phải
    //         if (SwapIfValid(rdOffset, -1, 0)) continue;//trái
    //         count++;
    //     }
    //     shuffling = false;
    // }
}
