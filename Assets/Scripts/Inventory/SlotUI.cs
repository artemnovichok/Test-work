using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
	[SerializeField] private Image _iconImage;
	[SerializeField] private TMP_Text _qtyText;
	[SerializeField] private Image lockedOverlay;
	private InventoryUI _inventoryUI;
	private int _slotIndex;

	private int? _slotPrice = null;


	private GameObject _dragIcon;
	private int _originalSlotIndex;

	public void Init(InventoryUI InventoryUI, int SlotIndex, int? SlotPrice = null)
	{
		_inventoryUI = InventoryUI;
		_slotIndex = SlotIndex;
		_slotPrice = SlotPrice;
	}

	public void SetEmpty()
	{
		_iconImage.enabled = false;
		_qtyText.text = "";
		lockedOverlay.enabled = false;
	}

	public void SetLocked(bool locked)
	{
		lockedOverlay.enabled = locked;
		if (locked)
		{
			_iconImage.enabled = false;
			_qtyText.text = _slotPrice == null ? "" : _slotPrice.Value.ToString() ;
		}
	}

	public void SetItem(Sprite icon, int quantity)
	{
		lockedOverlay.enabled = false;
		_iconImage.enabled = true;
		_iconImage.sprite = icon;
		if (quantity > 1) _qtyText.text = quantity.ToString();
		else _qtyText.text = "";
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (_inventoryUI.inventory.Slots[_slotIndex].IsEmpty) return;
		_originalSlotIndex = _slotIndex;
		_dragIcon = new GameObject("DraggingIcon");
		_dragIcon.transform.SetParent(_inventoryUI.transform, worldPositionStays: false);
		var image = _dragIcon.AddComponent<Image>();
		image.sprite = _iconImage.sprite;
		image.rectTransform.sizeDelta = _iconImage.rectTransform.sizeDelta;
		_dragIcon.transform.SetAsLastSibling();
		_iconImage.color = new Color(1, 1, 1, 0.5f);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (_dragIcon != null)
		{
			_dragIcon.transform.position = eventData.position;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (_dragIcon != null)
		{
			Destroy(_dragIcon);
		}
		_iconImage.color = Color.white;
	}

	public void OnDrop(PointerEventData eventData)
	{
		SlotUI sourceSlotUI = eventData.pointerDrag?.GetComponent<SlotUI>();
		if (sourceSlotUI == null) return;
		int fromIndex = sourceSlotUI._originalSlotIndex;
		int toIndex = this._slotIndex;
		if (_inventoryUI.inventory.SwapItems(fromIndex, toIndex))
		{
			_inventoryUI.RefreshSlot(fromIndex);
			_inventoryUI.RefreshSlot(toIndex);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (_slotPrice == null)
		{
			return;
		}
		Debug.Log("Process purchasing");
		_inventoryUI.inventory.UnlockSlot(_slotIndex);
		_inventoryUI.RefreshAll();
	}
}
