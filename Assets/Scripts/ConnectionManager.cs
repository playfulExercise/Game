using MongoDB.Bson;
using UnityEngine;
using Unity.IO;
using MongoDB.Bson.Serialization;
using System.IO;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviour
{
    private string jsonDataPath = "Assets/data.json";

    private BsonDocument infos;
    public BsonArray questions;
	public string eleve_prenom;

    public void SetInfos(BsonDocument _infos){
        infos = _infos;
    }

    public void SetQuestions(BsonArray _questions){
        questions = _questions;
    }

    public void SetElevePrenom(string _prenom){
        eleve_prenom = _prenom;
    }

    public void Connect(){
        CreateJSONDataFile();
        SceneManager.LoadScene("StartMenu");
    }

    private void CreateJSONDataFile(){
        string json = "";
        json += "{";
        json += "\"prenom\" : \"\",";
        json += "\"infos\" : ";
        json += infos.ToString();
        json += ",";
        json += "\"questions\" : ";
        json += questions.ToString();
        json += "}";
        File.WriteAllText(jsonDataPath, json);
    }
}
