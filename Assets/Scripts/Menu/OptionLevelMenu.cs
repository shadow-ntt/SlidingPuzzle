using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionLevelMenu : Menu
{
    [SerializeField] public Button ButtonPrefab;
    [SerializeField] public float gap = 3;
    [SerializeField] public int sizeMin = 3;
    [SerializeField] public int sizeMax = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void GenerateMenu()
    {
        RectTransform rectTransform = ButtonPrefab.GetComponent<RectTransform>();
        float hBtn = rectTransform.rect.size.y;
        int lengthBtn = sizeMax - sizeMin + 1;
        //vị trí bắt đầu render: 
        float offsetYStart = (hBtn * lengthBtn + gap * (lengthBtn - 1)) / 2f;
        Debug.Log("offsetYStart " + offsetYStart);
        Debug.Log("hBtn " + hBtn);
        for (int i = 0; i < lengthBtn; i++)
        {
            Button btn = Instantiate(ButtonPrefab, gameObject.transform);
            btn.transform.localPosition = new Vector3(0, offsetYStart - (gap + hBtn) * i, 0);
            btn.GetComponentInChildren<TMP_Text>().text = $"{i + sizeMin}x{i + sizeMin}";
            int row = i + sizeMin;
            int col = i + sizeMin;

            btn.onClick.AddListener(() => changeLevel(col, row));
        }
    }
    void changeLevel(int col, int row)
    {
        GameBoard gb = FindAnyObjectByType<GameBoard>();
        MenuPoup mn = FindAnyObjectByType<MenuPoup>();
        GameManager gm = FindAnyObjectByType<GameManager>();
        gb.row = row;
        gb.col = col;
        mn.closeAll();
        gm.StartShuffle();
    }

}
