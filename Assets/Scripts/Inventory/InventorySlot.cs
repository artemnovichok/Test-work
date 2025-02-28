[System.Serializable]
public class InventorySlot
{
	public ItemData itemData =null;

	public int quantity = 0;
	public bool isLocked = false;
	
	public int price = 0;

	public bool IsEmpty => itemData == null;
	public bool IsFull => !IsEmpty && quantity >= (itemData?.maxStack ?? 0);
}