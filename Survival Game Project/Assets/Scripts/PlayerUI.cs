using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // Start is called before the first frame update
    public float itemCount;
    public GameObject player;
    public Player playerScript;
    #region Inventory
    public float maxInvRows;
    public float maxInvCols;
    public Vector2 invCellSize;
    public GameObject invCell;
    public RectTransform inventoryGrid;
    public GridLayoutGroup inventoryGridSettings;
    public GameObject inventory;
    public List<GameObject> inventoryCells;
    #endregion

    void Start()
    {
        playerScript = player.GetComponent<Player>();
        
        //playerInventory = ConvertToList(playerScript.inventory);
        //itemCount = playerScript.inventory.Count;
        /* foreach(ItemProperties item in playerInventory)
         {
             GameObject cell = Instantiate(invCell);
             if(cell.transform.GetChild(0).TryGetComponent(out Text counter))
             {
                 //int maxAmount = playerInventory[playerInventory.IndexOf(item)].maxAmount;
                // int currentAmount = 
             }
             cell.transform.parent = inventoryGrid;
         }*/
        
        /*maxInvCols = itemCount;
        if(itemCount < maxInvRows)
        {
            maxInvRows = 1;
        }
        else
        {
            maxInvRows = 6;
        }*/
        foreach(GameObject g in playerScript.gameManager.activeItems)
        {
            
            Item gItem = g.GetComponent<Item>();
            GameObject cell = Instantiate(invCell);
            cell.transform.parent = inventoryGrid;
            Image image = cell.transform.GetChild(1).GetComponent<Image>();
            image.sprite = gItem.sprite;
            if (playerScript.inventory.ContainsKey(gItem.itemName))
            {
                cell.transform.GetChild(0).GetComponent<Text>().text = 0 + "/" + gItem.maxAmount.ToString();
            }
            inventoryCells.Add(cell);
        }
        /*
        for(int i = 0; i < maxInvCols; i++)
        {
            
            for (int j = 0; j < maxInvRows; j++)
            {
                GameObject cell = Instantiate(invCell);
                cell.transform.parent = inventoryGrid;
                cell.GetComponentInChildren<Image>().sprite = playerScript.gameManager.activeItems[i].GetComponent<Item>().sprite;
            }
        }*/
        inventoryGridSettings.cellSize = invCellSize;
    }

    // Update is called once per frame
    void Update()
    {
        //playerInventory = ConvertToList(playerScript.inventory);
        itemCount = playerScript.inventory.Count;
        UpdateInventory();
        ToggleInventory();
    }
    void ToggleInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool active = inventory.activeSelf;
            inventory.SetActive(!active);
        }
    }
    void UpdateInventory()
    {
        for(int i = 0; i < inventoryCells.Count; i++)
        {
            if(playerScript.gameManager.activeItems[i].TryGetComponent(out Item item) && playerScript.inventory.ContainsKey(item.itemName))
            {
                Text textToUpdate = inventoryCells[i].transform.GetChild(0).GetComponent<Text>();
                textToUpdate.text = playerScript.inventory[item.itemName].ToString();
            }
        }
    }
}
