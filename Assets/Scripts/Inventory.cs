using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private string jsonDataPath = "Assets/data.json";
    private int coinsCount;
    public Text coinsCountText;

    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Inventory dans la scène");
            return;
        }

        instance = this;

        string jsonString = File.ReadAllText(jsonDataPath);
        JSONData data = JsonUtility.FromJson<JSONData>(jsonString);
        coinsCount = data.infos.nb_pieces;
        coinsCountText.text = data.infos.nb_pieces.ToString();
    }

    public void AddCoins(int count)
    {
        coinsCount += count;
        coinsCountText.text = coinsCount.ToString();

        string jsonString = File.ReadAllText(jsonDataPath);
        JSONData data = JsonUtility.FromJson<JSONData>(jsonString);
        data.infos.nb_pieces += count;
        string jsonUpdated = JsonUtility.ToJson(data, true);
        File.WriteAllText(jsonDataPath, jsonUpdated);

    }
}
