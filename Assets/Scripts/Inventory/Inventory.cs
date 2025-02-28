using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
	private List<InventorySlot> _slots;
	public IReadOnlyList<InventorySlot> Slots => _slots;

	public int SlotCount => _slots.Count;

	public Inventory(List<InventorySlot> data)
	{
		_slots = new List<InventorySlot>(data.Count);
		for (int i = 0; i < data.Count; i++)
		{
			var slot = new InventorySlot
			{
				itemData = data[i].itemData,
				quantity = data[i].quantity,
				isLocked = data[i].isLocked,
				price = data[i].price,
			};
			_slots.Add(slot);
		}
	}

	public void UnlockSlot(int index)
	{
		if (index < 0 || index >= _slots.Count) return;
		if(!_slots[index].isLocked) return;
		_slots[index].isLocked = false; 
		SaveInventary();
	}

	public bool AddItem(ItemData item, int quantity = 1)
	{
		if (item == null || quantity <= 0) return false;

		foreach (var slot in _slots)
		{
			if (!slot.IsEmpty && !slot.isLocked && slot.itemData == item && slot.quantity < item.maxStack)
			{
				int space = item.maxStack - slot.quantity;
				int toAdd = Math.Min(space, quantity);
				slot.quantity += toAdd;
				quantity -= toAdd;
				if (quantity <= 0) 
				{
					SaveInventary();
					return true;
				}
			}
		}

		while (quantity > 0)
		{

			int slotIndex = _slots.FindIndex(s => s.IsEmpty && !s.isLocked);
			if (slotIndex == -1)
			{
				Debug.LogWarning("No empty field for: " + item.itemName);
				return false;
			}
			var targetSlot = _slots[slotIndex];
			targetSlot.itemData = item;
			int toAdd = Math.Min(quantity, item.maxStack);
			targetSlot.quantity = toAdd;
			quantity -= toAdd;
		}
		SaveInventary();
		return true;
	}

	public bool RemoveItem(int slotIndex, int removeQuantity = 1)
	{
		if (slotIndex < 0 || slotIndex >= _slots.Count) return false;
		var slot = _slots[slotIndex];
		if (slot.IsEmpty || slot.isLocked) return false;
		slot.quantity -= removeQuantity;
		if (slot.quantity <= 0)
		{
			slot.itemData = null;
			slot.quantity = 0;
		}
		SaveInventary();
		return true;
	}

	public bool SwapItems(int indexA, int indexB)
	{
		if (indexA < 0 || indexA >= _slots.Count || indexB < 0 || indexB >= _slots.Count)
			return false;
		if (indexA == indexB) return false;
		var slotA = _slots[indexA];
		var slotB = _slots[indexB];
		if (slotA.isLocked || slotB.isLocked) return false;

		if (slotA.IsEmpty && slotB.IsEmpty) return true;
		if (slotA.IsEmpty && !slotB.IsEmpty)
		{
			slotA.itemData = slotB.itemData;
			slotA.quantity = slotB.quantity;
			slotB.itemData = null;
			slotB.quantity = 0;
			return true;
		}
		if (slotB.IsEmpty && !slotA.IsEmpty)
		{
			slotB.itemData = slotA.itemData;
			slotB.quantity = slotA.quantity;
			slotA.itemData = null;
			slotA.quantity = 0;
			return true;
		}

		if (!slotA.IsEmpty && !slotB.IsEmpty)
		{
			if (slotA.itemData == slotB.itemData)
			{
				int total = slotA.quantity + slotB.quantity;
				int maxStack = slotA.itemData.maxStack;
				if (total <= maxStack)
				{
					slotA.quantity = 0;
					slotA.itemData = null;
					slotB.quantity = total;
				}
				else
				{
					slotB.quantity = maxStack;
					slotA.quantity = total - maxStack;
				}
				return true;
			}
			else
			{
				var tempItem = slotA.itemData;
				var tempQty = slotA.quantity;
				slotA.itemData = slotB.itemData;
				slotA.quantity = slotB.quantity;
				slotB.itemData = tempItem;
				slotB.quantity = tempQty;
				return true;
			}
		}
		return false;
	}
	
	public void SaveInventary()
	{
		Debug.Log("Saving process");
		SaveArray saveArray = new();
		saveArray._slotDatas = _slots;
        string stringList = JsonUtility.ToJson(saveArray);
		PlayerSave.SetString("data",stringList);
	}
}