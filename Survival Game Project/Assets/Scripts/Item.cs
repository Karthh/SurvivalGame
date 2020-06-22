using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item: MonoBehaviour
{
    public string itemName;
    public int maxAmount;
    public Sprite sprite;

    public Item(Item item)
    {
        this.itemName = item.itemName;
        this.maxAmount = item.maxAmount;
        this.sprite = item.sprite;

    }
    private void Start()
    {

    }

}
