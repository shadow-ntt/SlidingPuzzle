using System.Collections;
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

    void Awake()
    {
        GameBoard = FindAnyObjectByType<GameBoard>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GameBoard.GeneratePieces();
        shuffling = false;
        durationPlaying = 0;
        moved = 0;
        isPause = false;
    }
    void Update()
    {
        if (GameBoard.isComplete() && !shuffling)
        {
            StartShuffle();
        }
        pauseManager();

    }
    void pauseManager()
    {
        if (!isPause)
        {
            Time.timeScale = 1;
            durationPlaying += Time.deltaTime;
        }
        if (menuPoup.gameObject.activeSelf) isPause = true;
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
        durationPlaying = 0;
        moved = 0;
        shuffling = false;
        isPause = false;
    }
    public void StartShuffle()
    {
        StartCoroutine(AwaitShuffle(DurationShufflingUI));
    }
}

