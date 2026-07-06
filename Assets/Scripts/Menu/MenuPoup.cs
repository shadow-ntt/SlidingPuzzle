using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuPoup : MonoBehaviour
{
    private Stack Menus;
    void Awake()
    {
        this.Menus = new Stack();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PushMenu(Menu menu)
    {
        //thêm vào thì ẩn cái hiện tại đi, cái mới vào thì bật lên
        if (Menus.Count >= 1)
        {
            Menu menuCurrent = (Menu)this.Menus.Peek();
            menuCurrent.gameObject.SetActive(false);
        }
        this.Menus.Push(menu);
        menu.gameObject.SetActive(true);
    }
    public void PopMenu()
    {
        //ẩn cái hiện tại đi, bật cái trước đó lên
        Menu menuPop = (Menu)this.Menus.Pop();
        menuPop.gameObject.SetActive(false);
        //nếu không còn cái nào thì tắt cái popup này đi
        if (Menus.Count <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Menu menuCurrent = (Menu)this.Menus.Peek();
            menuCurrent.gameObject.SetActive(true);
        }

    }
    public void closeAll()
    {
        foreach (Menu item in Menus)
        {
            item.gameObject.SetActive(false);
        }
        this.Menus = new Stack();
        gameObject.SetActive(false);
    }
    //

}
