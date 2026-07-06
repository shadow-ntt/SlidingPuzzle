using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectImgMenu : MonoBehaviour
{
    public RectTransform viewPortTransform;
    public RectTransform contentPanelTransform;
    public VerticalLayoutGroup VLG;
    public Button BtnSelectImgPrefab;
    public ScrollRect scrollRect;
    public List<Button> ItemList;
    public Sprite[] SlideImgSprite;
    private Vector2 OldVelocity;
    private bool isUpdated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OldVelocity = Vector2.zero;
        isUpdated = false;
        //load image
        SlideImgSprite = Resources.LoadAll<Sprite>("SlideImages");
        ItemList = new List<Button>();
        InitItems();
    }
    // Update is called once per frame
    void Update()
    {
        //tránh bị gián đoạn khi update lại vị trí
        if (isUpdated)
        {
            scrollRect.velocity = OldVelocity;
            isUpdated = false;
        }
        //lấy giá trị chiều cao của các item, không bao gồm các item clone(khởi tạo thêm)
        //có thể bỏ qua 1 giá trị spacing vì nó cũng không đáng kể
        float itemH = ItemList[0].GetComponent<RectTransform>().rect.width;
        float contentH = ItemList.Count * (itemH + VLG.spacing);
        //lấy vị trí hiện tại trừ hoặc cộng thì tương tự quay lại 1 vòng
        //ví dụ nếu đang ở 5 h=3 thì 5-3=2, sẽ quay lại đúng phần tử đang đứng
        //lớn hơn 0 thì set lại vị trí nhỏ hơn
        if (contentPanelTransform.localPosition.y > 0)
        {
            Canvas.ForceUpdateCanvases();
            contentPanelTransform.localPosition -= new Vector3(0, contentH, 0);
            OldVelocity = scrollRect.velocity;
            isUpdated = true;
        }
        // nhỏ hơn contentH / 2 thì set vị trí lớn hơn
        if (contentPanelTransform.localPosition.y < 0 - contentH / 2)
        {
            Canvas.ForceUpdateCanvases();
            contentPanelTransform.localPosition += new Vector3(0, contentH, 0);
            OldVelocity = scrollRect.velocity;
            isUpdated = true;
        }
    }
    //
    void InitItems()
    {
        //tạo một danh sách button
        foreach (var item in SlideImgSprite)
        {
            ItemList.Add(createButton(item));
        }
        //tạo scroll vô hạn(đang hoàn thiện)
        int lengthItem = ItemList.Count;
        float itemH = ItemList[0].GetComponent<RectTransform>().rect.width;
        int ItemsToAdd = Mathf.CeilToInt(viewPortTransform.rect.width / (itemH + VLG.spacing));
        //thêm item vào đằng sau
        for (int i = 0; i < ItemsToAdd; i++)
        {
            Button Btn = createButton(SlideImgSprite[i % lengthItem]);
            RectTransform RT = Btn.GetComponent<RectTransform>();
            RT.SetAsLastSibling();

        }
        //thêm item vào đằng trước
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
        //gọi dialog để xác nhận
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
