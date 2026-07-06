using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class CompletedBoard : MonoBehaviour
{
    public static CompletedBoard Instance;
    [SerializeField] private TextMeshProUGUI durationText;
    [SerializeField] private TextMeshProUGUI movedText;
    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Show(string duration, string moved)
    {
        durationText.text = $"time: {duration}";
        movedText.text = $"moved: {moved}";
        gameObject.SetActive(true);

    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
