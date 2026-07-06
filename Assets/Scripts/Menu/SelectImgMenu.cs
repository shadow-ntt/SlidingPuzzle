using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectImgMenu : MonoBehaviour
{
    public RectTransform viewPortTransform;
    public RectTransform contentPanelTransform;
    public VerticalLayoutGroup VLG;
    public Button BtnSelectImgPrefab;
    public List<Button> ItemList;
    public Sprite[] SlideImgSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SlideImgSprite = Resources.LoadAll<Sprite>("SlideImages");
        ItemList = new List<Button>();
        InitItems();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //
    void InitItems()
    {
        foreach (var item in SlideImgSprite)
        {
            ItemList.Add(createButton(item));
        }
        //
        int lengthItem = ItemList.Count;
        float itemW = ItemList[0].GetComponent<RectTransform>().rect.width;
        int ItemsToAdd = Mathf.CeilToInt(viewPortTransform.rect.width / (itemW + VLG.spacing));
        for (int i = 0; i < ItemsToAdd; i++)
        {
            Button Btn = createButton(SlideImgSprite[i % lengthItem]);
            RectTransform RT = Btn.GetComponent<RectTransform>();
            RT.SetAsLastSibling();

        }
        for (int i = 0; i < ItemsToAdd; i++)
        {
            int num = ((lengthItem - i - 1) % lengthItem + lengthItem) % lengthItem;

            Button Btn = createButton(SlideImgSprite[num]);
            RectTransform RT = Btn.GetComponent<RectTransform>();
            RT.SetAsFirstSibling();
        }
    }
    Button createButton(Sprite sprt)
    {
        Button Btn = Instantiate(BtnSelectImgPrefab, contentPanelTransform);
        Image img = Btn.GetComponent<Image>();
        img.sprite = sprt;

        Btn.onClick.AddListener(() => DialogYesNo.Instance.Show("Bạn chắc chắn muốn đổi ảnh?", () => handleBtnClick(sprt)));
        return Btn;
    }
    void handleBtnClick(Sprite sprt)
    {
        GameManager gm = FindAnyObjectByType<GameManager>();
        MenuPoup mp = FindAnyObjectByType<MenuPoup>();
        mp.closeAll();
        gm.SetPiece(sprt);
    }
}
