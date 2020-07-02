using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public List<GameObject> requiredItems;
    private List<Item> itemScriptList;
    public List<int> requiredAmounts;
    public bool built;

    private SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        itemScriptList = new List<Item>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        foreach(GameObject g in requiredItems)
        {
            if(g.TryGetComponent(out Item item)){
                itemScriptList.Add(item);
            }
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private bool CanBeBuilt()
    {
        for(int i = 0; i < requiredAmounts.Count; i++)
        {
            if(requiredAmounts[i] > 0)
            {
                return false;
            }
        }
        return true;
    }
    public void Interact(Dictionary<string, int> playerInventory)
    {
        CheckMaterials(playerInventory);
        built = CanBeBuilt();
        if (built)
        {
            Debug.Log("Building has been built!");
            renderer.enabled = true;
        }
        else
        {
            Debug.Log("Not enough materials");
        }
        
    }
    private void CheckMaterials(Dictionary<string, int> playerInventory)
    {
        for(int i = 0; i < itemScriptList.Count; i++)
        {
            if (playerInventory.ContainsKey(itemScriptList[i].itemName))
            {
                if(requiredAmounts[i] >= 0)
                {
                    requiredAmounts[i] -= playerInventory[itemScriptList[i].itemName];
                }
                
            }
        }
    }
    public List<string> GetRequiredItemKeys()
    {
        List<string> ret = new List<string>();
        for(int i =0; i < itemScriptList.Count; i++)
        { 
            ret.Add(itemScriptList[i].itemName);
        }
        return ret;
    }
    
}
