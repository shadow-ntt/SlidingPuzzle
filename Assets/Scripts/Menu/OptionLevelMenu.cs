using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionLevelMenu : Menu
{
    [SerializeField] public Button ButtonPrefab;
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
        // ví dụ là 5x5, 3x3, thì leng sẽ là 5-3+1
        int lengthBtn = sizeMax - sizeMin + 1;
        for (int i = 0; i < lengthBtn; i++)
        {
            Button btn = Instantiate(ButtonPrefab, gameObject.transform);
            btn.GetComponentInChildren<TMP_Text>().text = $"{i + sizeMin}x{i + sizeMin}";
            //vấn đề cloresure trong lambda
            int row = i + sizeMin;
            int col = i + sizeMin;
            btn.onClick.AddListener(() => DialogYesNo.Instance.Show(
                $"Bạn có chắc chắn là muốn đổi sang level {col}x{row}",
                () => changeLevel(col, row)));
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
