using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameConfig/GameConfig")]
public class GameConfig : ScriptableObject
{
	private static GameConfig _instance;
	public static GameConfig Instance
	{
		get
		{
			if (_instance == null)
			{
				GameConfig config = Resources.Load<GameConfig>("GameConfig");
				
				_instance = config ?? CreateInstance<GameConfig>();
				Load();
			}
			return _instance;
		}
		set
		{

		}
	}

	private static void Load()
	{
		string data = PlayerSave.GetString("data", string.Empty);
		if (!string.IsNullOrEmpty(data))
		{
			Debug.Log(data);
			_instance._slotDatas = JsonUtility.FromJson<SaveArray>(data)._slotDatas;
		}
	}

	[Header("GameSettings")]
	[Space(5)]


	[SerializeField]
	[Tooltip("Field")]
	private List<InventorySlot> _slotDatas = new();
	public List<InventorySlot> SlotDatas => _slotDatas;

}

public struct SaveArray
{
	public List<InventorySlot> _slotDatas;
}
