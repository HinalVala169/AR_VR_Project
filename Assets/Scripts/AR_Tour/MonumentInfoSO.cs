using UnityEngine;

[CreateAssetMenu(fileName = "MonumentInfo", menuName = "ScriptableObjects/MonumentInfoSO")]
public class MonumentInfoSO : ScriptableObject
{
    public TextAsset jsonTextAsset; // Reference to the TextAsset containing the JSON
    public MonumentInformation monumentInformation;

    private void OnValidate()
    {
        LoadJsonFromTextAsset();
    }

    

    private void LoadJsonFromTextAsset()
    {
        if (jsonTextAsset != null)
        {
            string jsonInput = jsonTextAsset.text;
            monumentInformation = JsonUtility.FromJson<MonumentInformation>(jsonInput);
        }
        else
        {
            Debug.LogError("TextAsset is not assigned!");
        }
    }
}

[System.Serializable]
public class MonumentInformation
{
    public InfoEntry[] monumentInformation; // Array to hold the monument info entries
}

[System.Serializable]
public class InfoEntry
{
    public string title;
    public string[] details;
}
