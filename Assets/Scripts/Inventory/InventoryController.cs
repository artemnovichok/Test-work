using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryController : MonoBehaviour
{
	[SerializeField] private InventoryUI _inventoryUI;

	[Header("Buttons")]
	[SerializeField] private Button _shootBtn;
	[SerializeField] private Button _addCartridgeBtn;
	[SerializeField] private Button _addRandomItemBtn;
	[SerializeField] private Button _deleteRandomItemBtn;

	private ItemData _ammo1Data, _ammo2Data;
	private ItemData[] _weaponItems;
	private ItemData[] _headItems;
	private ItemData[] _torsoItems;

	#region consts
	private const string _pathData = "Data/";
	private const string _pathBullet = "Bullets/";
	private const string _pathHead = "Head/";
	private const string _pathTorso = "Torso/";
	private const string _pathWeapons = "Weapons/";
	#endregion

	void Awake()
	{
		_ammo1Data = LoadData($"{_pathBullet}Bullet1");
		_ammo2Data = LoadData($"{_pathBullet}Bullet2");
		_weaponItems = LoadAllData($"{_pathWeapons}");
		_headItems = LoadAllData($"{_pathHead}");
		_torsoItems = LoadAllData($"{_pathTorso}");
		_inventoryUI.inventory = new Inventory(GameConfig.Instance.SlotDatas);
	}

	private void OnEnable()
	{
		_shootBtn.onClick.AddListener(OnShootButton);
		_addCartridgeBtn.onClick.AddListener(OnAddAmmoButton);
		_addRandomItemBtn.onClick.AddListener(OnAddItemButton);
		_deleteRandomItemBtn.onClick.AddListener(OnRemoveItemButton);
	}

	private void OnDisable()
	{
		_shootBtn.onClick.RemoveAllListeners();
		_addCartridgeBtn.onClick.RemoveAllListeners();
		_addRandomItemBtn.onClick.RemoveAllListeners();
		_deleteRandomItemBtn.onClick.RemoveAllListeners();
	}

	private ItemData LoadData(string path)
	{
		ItemData itemData = null;
		itemData = Resources.Load<ItemData>($"{_pathData}{path}");
		return itemData;
	}

	private ItemData[] LoadAllData(string path)
	{
		ItemData[] itemData;
		itemData = Resources.LoadAll<ItemData>($"{_pathData}{path}");
		return itemData;
	}

	private void OnShootButton()
	{
		var slots = _inventoryUI.inventory.Slots;
		var ammoSlots = new List<int>();
		for (int i = 0; i < slots.Count; i++)
		{
			if (!slots[i].IsEmpty && slots[i].itemData is AmmoData && slots[i].quantity > 0)
			{
				ammoSlots.Add(i);
			}
		}
		if (ammoSlots.Count == 0)
		{
			return;
		}
		int randomIndex = Random.Range(0, ammoSlots.Count);
		int slotToUse = ammoSlots[randomIndex];
		_inventoryUI.inventory.RemoveItem(slotToUse, 1);
		_inventoryUI.RefreshSlot(slotToUse);
	}

	private void OnAddAmmoButton()
	{
		if (_ammo1Data != null)
		{
			_inventoryUI.inventory.AddItem(_ammo1Data, _ammo1Data.maxStack);
		}
		if (_ammo2Data != null)
		{
			_inventoryUI.inventory.AddItem(_ammo2Data, _ammo2Data.maxStack);
		}
		_inventoryUI.RefreshAll();
	}

	private void OnAddItemButton()
	{
		if (_weaponItems != null && _weaponItems.Length > 0)
		{
			ItemData randomWeapon = _weaponItems[Random.Range(0, _weaponItems.Length)];
			_inventoryUI.inventory.AddItem(randomWeapon, 1);
		}
		if (_headItems != null && _headItems.Length > 0)
		{
			ItemData randomHead = _headItems[Random.Range(0, _headItems.Length)];
			_inventoryUI.inventory.AddItem(randomHead, 1);
		}
		if (_torsoItems != null && _torsoItems.Length > 0)
		{
			ItemData randomTorso = _torsoItems[Random.Range(0, _torsoItems.Length)];
			_inventoryUI.inventory.AddItem(randomTorso, 1);
		}
		_inventoryUI.RefreshAll();
	}

	private void OnRemoveItemButton()
	{
		int randomIndex = Random.Range(0, _inventoryUI.inventory.SlotCount);
		var slot = _inventoryUI.inventory.Slots[randomIndex];
		if (slot.IsEmpty || slot.isLocked)
		{

			randomIndex = Array.FindIndex(_inventoryUI.inventory.Slots.ToArray(), x => x != null);
			if (randomIndex == -1)
			{
				Debug.LogError("No any items in inventory.");
				return;
			}
			slot = _inventoryUI.inventory.Slots[randomIndex];
		}
		_inventoryUI.inventory.RemoveItem(randomIndex, slot.quantity);
		_inventoryUI.RefreshSlot(randomIndex);
		Debug.Log($"Delete: {randomIndex}");
	}
}
