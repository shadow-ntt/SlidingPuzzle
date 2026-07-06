using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameBoard GameBoard;
    public bool shuffling;
    public bool isPause;
    //giây
    public float durationPlaying;
    public int moved;
    [SerializeField] public float aspect = 16 / 9;
    [SerializeField] public RectTransform ShufflingUI;
    [SerializeField] public int DurationShufflingUI = 1;
    [SerializeField] private MenuPoup menuPoup;
    [SerializeField] private TextMeshProUGUI durationText;
    [SerializeField] private TextMeshProUGUI movedText;

    void Awake()
    {
        GameBoard = FindAnyObjectByType<GameBoard>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shuffling = false;
        durationPlaying = 0;
        moved = 0;
        isPause = false;
        StartShuffle();
    }
    void Update()
    {
        //completed
        if (GameBoard.isComplete() && !shuffling)
        {
            CompletedBoard.Instance.Show(durationPlaying.ToString("0.00"), moved.ToString("0.00"));
        }
        pauseManager();
        //
        movedText.text = moved.ToString("0.00");
        durationText.text = durationPlaying.ToString("0.00");
        //game complete

    }
    void pauseManager()
    {
        if (!isPause)
        {
            Time.timeScale = 1;
            durationPlaying += Time.deltaTime;
        }
        if (menuPoup.gameObject.activeSelf || (GameBoard.isComplete() && !shuffling))
        {
            isPause = true;
            Time.timeScale = 0;
        }
        else isPause = false;
    }
    public IEnumerator AwaitShuffle(int second)
    {
        shuffling = true;
        isPause = true;
        ShufflingUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(second);
        GameBoard.GeneratePieces();
        GameBoard.Shuffle(200);
        ShufflingUI.gameObject.SetActive(false);
        shuffling = false;
        isPause = false;
        durationPlaying = 0;
        moved = 0;
    }
    public void StartShuffle()
    {
        StartCoroutine(AwaitShuffle(DurationShufflingUI));
    }
    public void SetPiece(Sprite sprt)
    {

        GameBoard.Piece.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = sprt.texture;

        StartShuffle();
    }
}

