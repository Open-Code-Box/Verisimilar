using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryData : MonoBehaviour
{

    public GameObject targetDrop;

    public InventoryItemData noneData; // used to set slot to none
    public InventoryItemData[] ItemList; // Items
    public InventoryItemData[] AvaibleItem; // All avaible items sorted by ID

    public RawImage[] SlotRI; // RawImage of Slots
    public Toggle[] SlotT; // Toggle of Slots

    public GameObject Statistic; // Self explanatory
    public TMP_Text[] StatisticT; // Text of Statistic, 0 - ItemName, 1 - Description, 2 - Buffs

    private InventoryItemData selectedItem;
    private int indexItem;

    bool toggleShowItems = true;

    private void Start()
    {
        selectedItem = noneData;
        indexItem = -1;
        setShowdisplayStatistic(false);

        for (int i = 0; i < SlotRI.Length; i++)
        {
            updateSlot(i, ItemList[i].preview);
        }

        // TEST //
        //saveItems(0);
        loadItems(0);

    }

    // Update slot textures
    private void updateSlot(int Index, Texture preview)
    {
        SlotRI[Index].texture = preview;
    }

    // Swap between 2 slots
    private void swapSlot(int Index_1, int Index_2)
    {

        InventoryItemData[] tmpItem = { ItemList[Index_1], ItemList[Index_2] };

        ItemList[Index_1] = tmpItem[1];
        ItemList[Index_2] = tmpItem[0];

        updateSlot(Index_1, ItemList[Index_1].preview);
        updateSlot(Index_2, ItemList[Index_2].preview);

    }

    // Display text to the statistic box
    private void displayStatistic(int Index)
    {
        StatisticT[0].text = ItemList[Index].displayName;
        StatisticT[1].text = ItemList[Index].displayDescription;
        StatisticT[2].text = ItemList[Index].displayBuff0 + "\n" + ItemList[Index].displayBuff1;
    }

    // Toggle visibility
    private void setShowdisplayStatistic(bool show)
    {
        Statistic.SetActive(show);
    }

    // Loads saved items
    public void loadItems(int index)
    {
        if (GameObject.Find("SaveSystem"))
        {
            SaveData data = GameObject.Find("SaveSystem").GetComponent<SaveData>();
            if (data.load(index) != null)
            {
                for (int i = 0; i < ItemList.Length; i++)
                {
                    ItemList[i] = AvaibleItem[int.Parse(data.load(index).inventoryId[i])]; // Extract avaible item using ID from data
                    updateSlot(i, ItemList[i].preview);
                }
            }
        }
        else // If gameobject not found
        {
            Debug.LogError("No 'SaveSystem' found in scene!");
        }
    }

    public void saveItems(int index)
    {
        if (GameObject.Find("SaveSystem"))
        {
            SaveData data = GameObject.Find("SaveSystem").GetComponent<SaveData>();
            if (data.load(index) != null)
            {
                data.save(index);
            }
        }
        else
        {
            Debug.LogError("No 'SaveSystem' found in scene!");
        }
    }

    // Check toggle for items swap
    public void checkToggle()
    {
        int tmpCount = 0;
        int[] tmpArray = { 0, 0 };
        for (int i = 0; i < SlotT.Length; i++)
        {
            if (SlotT[i].isOn)
            {
                if (tmpCount < 3)
                {
                    tmpArray[tmpCount] = i;
                    tmpCount++;
                }
            }
        }

        if (tmpCount == 2) // Display and swap slots
        {
            swapSlot(tmpArray[0], tmpArray[1]);
            SlotT[tmpArray[0]].isOn = false;
            SlotT[tmpArray[1]].isOn = false;

        }
        else if (tmpCount == 1) // Display choosen
        {
            if (ItemList[tmpArray[0]].id != "0")
            {
                selectedItem = ItemList[tmpArray[0]];
                indexItem = tmpArray[0];
                setShowdisplayStatistic(true);
                displayStatistic(tmpArray[0]);
            }
            else
            {
                selectedItem = noneData; // Just to be safe
                indexItem = -1; // Just to be safe
                setShowdisplayStatistic(false);
            }
        }
        else // Hide when not choose or swapped
        {
            selectedItem = noneData;
            indexItem = -1;
            setShowdisplayStatistic(false);
        }

    }

    // Toggle show items
    public void toggleShow()
    {
        toggleShowItems = !toggleShowItems;
        gameObject.SetActive(toggleShowItems);
    }

    // Get item from list
    public InventoryItemData getItem(int Index)
    {
        return ItemList[Index];
    }

    // Drop item from list
    public void dropItem()
    {
        if (selectedItem.id != "0" && indexItem >= 0)
        {
            Instantiate(ItemList[indexItem].prefab, targetDrop.transform.position, Quaternion.identity);

            ItemList[indexItem] = noneData;
            updateSlot(indexItem, ItemList[indexItem].preview);

            SlotT[indexItem].isOn = false;
            setShowdisplayStatistic(false);
        }
    }

    // Save item to the list
    public bool storeItem(InventoryItemData value)
    {
        bool found = false;
        for (int i = 0; i < SlotRI.Length; i++)
        {
            if (ItemList[i].id == "0")
            {
                ItemList[i] = value;
                updateSlot(i, ItemList[i].preview);
                found = true;
                break;
            }
        }

        return found;

    }

}