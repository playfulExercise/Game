using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfDungeon : MonoBehaviour
{

    public int coins;

    private string jsonDataPath = "Assets/data.json";
    private JSONData data;
    
    private QuestionManager questionManager;

    private Animator fadeSystem;

    private void Awake(){
        fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();

        questionManager = GameObject.FindGameObjectWithTag("QuestionManager").GetComponent<QuestionManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(questionManager.GetNbEnemies() == questionManager.GetNbEnemiesDefeated()){  
            if (collision.CompareTag("Player"))
            {
                Inventory.instance.AddCoins(coins);
                IncrementCurrentMatiere();
                SetWorldProgression();
                StartCoroutine(loadNextScene());
            }
        }
    }

    private IEnumerator loadNextScene()
    {
        fadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(questionManager.GetHubName());
    }

    private void IncrementCurrentMatiere(){
        string jsonString = File.ReadAllText(jsonDataPath);
        data = JsonUtility.FromJson<JSONData>(jsonString);

        foreach (Matiere matiere in data.infos.matieres){
            if (matiere.nom_matiere.Equals(questionManager.GetCurrentSubject())){
                matiere.nb_donjons_finis += 1;
            }
        }

        string jsonUpdated = JsonUtility.ToJson(data, true);
        File.WriteAllText(jsonDataPath, jsonUpdated);
    }

    private void SetWorldProgression(){
        string jsonString = File.ReadAllText(jsonDataPath);
        data = JsonUtility.FromJson<JSONData>(jsonString);

        int nbDungeons = 0;
        int nbDungeonsDone = 0;
        foreach(Matiere subject in data.infos.matieres){
            nbDungeons += subject.nb_donjons;
            nbDungeonsDone += subject.nb_donjons_finis;
        }
        data.infos.progression_monde = Mathf.RoundToInt((nbDungeonsDone*100)/nbDungeons);

        string jsonUpdated = JsonUtility.ToJson(data, true);
        File.WriteAllText(jsonDataPath, jsonUpdated);
    }
}
