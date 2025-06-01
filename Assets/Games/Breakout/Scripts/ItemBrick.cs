using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBrick : Brick
{
    public GameObject Item;
    private GameObject item;

    private void Awake()
    {
        item = Instantiate(Item, transform.position, Item.transform.rotation);
        item.SetActive(false);
    }

    public override void SetColors(Color32 mainColor, Color32 subColor)
    {
        base.SetColors(mainColor, subColor);
        item.GetComponent<Item>().SetColor(subColor);
    }

    public override void OnBrickDestroyed() 
    {
        DropItem();
        base.OnBrickDestroyed();
    }

    public GameObject GetItemObject()
    {
        return item;
    }

    private void DropItem()
    {
        item.SetActive(true);
    }
}
