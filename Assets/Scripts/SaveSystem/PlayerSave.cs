using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
	private static Dictionary<string, int> intData = new Dictionary<string, int>();
	private static Dictionary<string, string> stringData = new Dictionary<string, string>();
	private static Dictionary<string, float> floatData = new Dictionary<string, float>();

	private static Dictionary<string, List<int>> listIntData = new Dictionary<string, List<int>>();
	private static Dictionary<string, List<string>> listStringData = new Dictionary<string, List<string>>();
	private static Dictionary<string, List<float>> listFloatData = new Dictionary<string, List<float>>();

	private static Dictionary<string, Dictionary<string, int>> dictIntData = new Dictionary<string, Dictionary<string, int>>();
	private static Dictionary<string, Dictionary<string, string>> dictStringData = new Dictionary<string, Dictionary<string, string>>();
	private static Dictionary<string, Dictionary<string, float>> dictFloatData = new Dictionary<string, Dictionary<string, float>>();

	private static string dataFilePath = string.Empty;

	private void Awake() 
	{
		DontDestroyOnLoad(gameObject);
		dataFilePath = Path.Combine(Application.persistentDataPath, "data.json");
	}
	public static void StartLoad()
	{
		if (string.IsNullOrEmpty(dataFilePath))
		{
			dataFilePath = Path.Combine(Application.persistentDataPath, "data.json");
		}
		LoadData();
	}


	#region SaveAndLoad

	private static void SaveData()
	{
		SaveDataStructure data = new()
		{
			IntData = ConvertDictionaryToList(intData),
			StringData = ConvertDictionaryToList(stringData),
			FloatData = ConvertDictionaryToList(floatData),

			ListIntData = ConvertDictionaryToList(listIntData),
			ListStringData = ConvertDictionaryToList(listStringData),
			ListFloatData = ConvertDictionaryToList(listFloatData),

			DictIntData = ConvertDictionaryToList(dictIntData),
			DictStringData = ConvertDictionaryToList(dictStringData),
			DictFloatData = ConvertDictionaryToList(dictFloatData)
		};

		string jsonData = JsonUtility.ToJson(data, true);
		//Debug.Log($"Saved Data: {jsonData}");
		try
		{
			File.WriteAllText(dataFilePath, jsonData);
			Debug.Log($"Data saved to {dataFilePath}");
		}
		catch (Exception ex)
		{
			Debug.LogError($"Error saving data to file: {ex.Message}");
		}
	}

	private static void LoadData()
{
	if (File.Exists(dataFilePath))
	{
		try
		{
			string jsonData = File.ReadAllText(dataFilePath);
			Debug.Log($"Data loaded from {dataFilePath}: {jsonData}");

			SaveDataStructure data = JsonUtility.FromJson<SaveDataStructure>(jsonData);
			if (data != null)
			{
				intData = ConvertListToDictionary(data.IntData);
				stringData = ConvertListToDictionary(data.StringData);
				floatData = ConvertListToDictionary(data.FloatData);

				listIntData = ConvertListToDictionaryListInt(data.ListIntData);
				listStringData = ConvertListToDictionaryListString(data.ListStringData);
				listFloatData = ConvertListToDictionaryListFloat(data.ListFloatData);

				dictIntData = ConvertListToDictionaryDictInt(data.DictIntData);
				dictStringData = ConvertListToDictionaryDictString(data.DictStringData);
				dictFloatData = ConvertListToDictionaryDictFloat(data.DictFloatData);
			}
		}
		catch (Exception e)
		{
			Debug.LogError($"Error loading data: {e.Message}");
			DeleteAll();
		}
	}
	else
	{
		Debug.LogWarning("No data file found. Initializing default data.");
		DeleteAll();
	}
}


	#endregion

	public static void SetInt(string key, int value)
	{
		intData[key] = value;
		SaveData();
	}

	public static int GetInt(string key, int defaultValue = 0)
	{
		if (intData.ContainsKey(key))
		{
			return intData[key];
		}
		return defaultValue;
	}

	public static void SetString(string key, string value)
	{
		stringData[key] = value;
		SaveData();
	}

	public static string GetString(string key, string defaultValue = "")
	{
		if (stringData.ContainsKey(key))
		{
			return stringData[key];
		}
		return defaultValue;
	}

	public static void SetFloat(string key, float value)
	{
		floatData[key] = value;
		SaveData();
	}

	public static float GetFloat(string key, float defaultValue = 0.0f)
	{
		if (floatData.ContainsKey(key))
		{
			return floatData[key];
		}
		return defaultValue;
	}

	public static bool HasKey(string key)
	{
		return intData.ContainsKey(key) || stringData.ContainsKey(key) || floatData.ContainsKey(key)
			|| listIntData.ContainsKey(key) || listStringData.ContainsKey(key) || listFloatData.ContainsKey(key)
			|| dictIntData.ContainsKey(key) || dictStringData.ContainsKey(key) || dictFloatData.ContainsKey(key);
	}

	public static List<string> GetAllKeys()
	{
		List<string> keys = new();
		keys.AddRange(intData.Keys);
		keys.AddRange(stringData.Keys);
		keys.AddRange(floatData.Keys);
		keys.AddRange(listIntData.Keys);
		keys.AddRange(listStringData.Keys);
		keys.AddRange(listFloatData.Keys);
		keys.AddRange(dictIntData.Keys);
		keys.AddRange(dictStringData.Keys);
		keys.AddRange(dictFloatData.Keys);
		return keys;
	}

	public static void DeleteKey(string key)
	{
		intData.Remove(key);
		stringData.Remove(key);
		floatData.Remove(key);
		listIntData.Remove(key);
		listStringData.Remove(key);
		listFloatData.Remove(key);
		dictIntData.Remove(key);
		dictStringData.Remove(key);
		dictFloatData.Remove(key);
		SaveData();
	}

	public static void DeleteAll()
	{
		intData.Clear();
		stringData.Clear();
		floatData.Clear();
		listIntData.Clear();
		listStringData.Clear();
		listFloatData.Clear();
		dictIntData.Clear();
		dictStringData.Clear();
		dictFloatData.Clear();
		SaveData();
	}

	#region List and Dictionary

	private static List<KeyValuePair<K, V>> ConvertDictionaryToList<K, V>(Dictionary<K, V> dict)
	{
		if (dict == null) return null;
		var list = new List<KeyValuePair<K, V>>();
		foreach (var pair in dict)
		{
			list.Add(new KeyValuePair<K, V>(pair.Key, pair.Value));
		}
		return list;
	}

	private static Dictionary<K, V> ConvertListToDictionary<K, V>(List<KeyValuePair<K, V>> list)
	{
		if (list == null) return null;
		var dict = new Dictionary<K, V>();
		foreach (var pair in list)
		{
			dict[pair.Key] = pair.Value;
		}
		return dict;
	}

	public static void SetListInt(string key, List<int> value)
	{
		listIntData[key] = value;
		SaveData();
	}

	public static List<int> GetListInt(string key, List<int> defaultValue = null)
	{
		if (listIntData.ContainsKey(key))
		{
			return listIntData[key];
		}
		return defaultValue;
	}

	private static Dictionary<string, List<int>> ConvertListToDictionaryListInt(List<KeyValuePair<string, List<int>>> list)
	{
		if (list == null) return null;
		var dict = new Dictionary<string, List<int>>();
		foreach (var pair in list)
		{
			dict[pair.Key] = pair.Value;
		}
		return dict;
	}

	// List<string>
	public static void SetListString(string key, List<string> value)
	{
		listStringData[key] = value;
		SaveData();
	}

	public static List<string> GetListString(string key, List<string> defaultValue = null)
	{
		if (listStringData.ContainsKey(key))
		{
			return listStringData[key];
		}
		return defaultValue;
	}


	private static Dictionary<string, List<string>> ConvertListToDictionaryListString(List<KeyValuePair<string, List<string>>> list)
	{
		if (list == null) return null;
		var dict = new Dictionary<string, List<string>>();
		foreach (var pair in list)
		{
			dict[pair.Key] = pair.Value;
		}
		return dict;
	}

	// List<float>
	public static void SetListFloat(string key, List<float> value)
	{
		listFloatData[key] = value;
		SaveData();
	}

	public static List<float> GetListFloat(string key, List<float> defaultValue = null)
	{
		if (listFloatData.ContainsKey(key))
		{
			return listFloatData[key];
		}
		return defaultValue;
	}

	private static Dictionary<string, List<float>> ConvertListToDictionaryListFloat(List<KeyValuePair<string, List<float>>> list)
	{
		if (list == null) return null;
		var dict = new Dictionary<string, List<float>>();
		foreach (var pair in list)
		{
			dict[pair.Key] = pair.Value;
		}
		return dict;
	}

	public static void SetDictInt(string key, Dictionary<string, int> value)
	{
		dictIntData[key] = value;
		SaveData();
	}

	public static Dictionary<string, int> GetDictInt(string key, Dictionary<string, int> defaultValue = null)
	{
		if (dictIntData.ContainsKey(key))
		{
			return dictIntData[key];
		}
		return defaultValue;
	}

	private static Dictionary<string, Dictionary<string, int>> ConvertListToDictionaryDictInt(List<KeyValuePair<string, Dictionary<string, int>>> list)
	{
		if (list == null) return null;
		var dict = new Dictionary<string, Dictionary<string, int>>();
		foreach (var pair in list)
		{
			dict[pair.Key] = pair.Value;
		}
		return dict;
	}

	public static void SetDictString(string key, Dictionary<string, string> value)
	{
		dictStringData[key] = value;
		SaveData();
	}

	public static Dictionary<string, string> GetDictString(string key, Dictionary<string, string> defaultValue = null)
	{
		if (dictStringData.ContainsKey(key))
		{
			return dictStringData[key];
		}
		return defaultValue;
	}

	private static Dictionary<string, Dictionary<string, string>> ConvertListToDictionaryDictString(List<KeyValuePair<string, Dictionary<string, string>>> list)
	{
		if (list == null) return null;
		var dict = new Dictionary<string, Dictionary<string, string>>();
		foreach (var pair in list)
		{
			dict[pair.Key] = pair.Value;
		}
		return dict;
	}

	public static void SetDictFloat(string key, Dictionary<string, float> value)
	{
		dictFloatData[key] = value;
		SaveData();
	}

	public static Dictionary<string, float> GetDictFloat(string key, Dictionary<string, float> defaultValue = null)
	{
		if (dictFloatData.ContainsKey(key))
		{
			return dictFloatData[key];
		}
		return defaultValue;
	}

	private static Dictionary<string, Dictionary<string, float>> ConvertListToDictionaryDictFloat(List<KeyValuePair<string, Dictionary<string, float>>> list)
	{
		if (list == null) return null;
		var dict = new Dictionary<string, Dictionary<string, float>>();
		foreach (var pair in list)
		{
			dict[pair.Key] = pair.Value;
		}
		return dict;
	}
	#endregion

	#region saveStruct

	[Serializable]
	private class SaveDataStructure
	{
		public List<KeyValuePair<string, int>> IntData;
		public List<KeyValuePair<string, string>> StringData;
		public List<KeyValuePair<string, float>> FloatData;

		public List<KeyValuePair<string, List<int>>> ListIntData;
		public List<KeyValuePair<string, List<string>>> ListStringData;
		public List<KeyValuePair<string, List<float>>> ListFloatData;

		public List<KeyValuePair<string, Dictionary<string, int>>> DictIntData;
		public List<KeyValuePair<string, Dictionary<string, string>>> DictStringData;
		public List<KeyValuePair<string, Dictionary<string, float>>> DictFloatData;
	}

	[Serializable]
	public class KeyValuePair<K, V>
	{
		public K Key;
		public V Value;

		public KeyValuePair(K key, V value)
		{
			Key = key;
			Value = value;
		}
	}
	#endregion

}