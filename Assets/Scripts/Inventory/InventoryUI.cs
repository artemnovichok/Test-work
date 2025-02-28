using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] private Transform _slotsParent;
    private GameObject _slotPrefab;
    

    private List<SlotUI> _slotUIList = new();

    void Start()
    {
        _slotPrefab = Resources.Load<GameObject>("Slot");
        for (int i = 0; i < inventory.SlotCount; i++)
        {
            GameObject slotObj = Instantiate(_slotPrefab, _slotsParent);
            SlotUI slotUI = slotObj.GetComponent<SlotUI>();
            slotUI.Init(this, i, (inventory.Slots[i].price == 0 ? null : inventory.Slots[i].price));
            _slotUIList.Add(slotUI);
        }
        RefreshAll();
    }

    public void RefreshSlot(int index)
    {
        if (index < 0 || index >= _slotUIList.Count) return;
        var slotData = inventory.Slots[index];
        var slotUI = _slotUIList[index];
        if (slotData.isLocked)
        {
            slotUI.SetLocked(true);
        }
        else if (slotData.IsEmpty)
        {
            slotUI.SetEmpty();
        }
        else
        {
            Sprite icon = slotData.itemData.icon;
            int qty = slotData.quantity;
            slotUI.SetItem(icon, qty);
        }
        inventory.SaveInventary();
    }

    public void RefreshAll()
    {
        for (int i = 0; i < _slotUIList.Count; i++)
        {
            RefreshSlot(i);
        }
        inventory.SaveInventary();
    }
}