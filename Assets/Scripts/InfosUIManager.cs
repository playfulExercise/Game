using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfosUIManager : MonoBehaviour
{
    private string jsonDataPath = "Assets/data.json";

    public Text progressionText;
    public Text coinsText; 

    private void Awake(){
        string jsonString = File.ReadAllText(jsonDataPath);
        JSONData data = JsonUtility.FromJson<JSONData>(jsonString);

        progressionText.GetComponent<Text>().text = "Progression : " + data.infos.progression_monde + " %";
        coinsText.GetComponent<Text>().text = "Pièces : " + data.infos.nb_pieces.ToString();
    }

    public void LoadShopScene(){
        SceneManager.LoadScene("Shop");
    }
}
