using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    private string jsonDataPath = "Assets/data.json";
    private JSONData data;

    private CurrentDungeonData currentDungeonData;

    private List<Dungeon> dungeons;

    private bool inRangeDungeon = false;
    private string currentSubject;
    private bool currentDungeonDone;
    
    private Animator fadeSystem;
    private GameObject dungeonUI;
    private Text dungeonUIText;
    private GameObject dungeonUIButton;
    private GameObject congratsMessage;
    private GameObject infosUI;


    private void Start(){
        fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();

        currentDungeonData = GameObject.FindGameObjectWithTag("CurrentDungeonData").GetComponent<CurrentDungeonData>();

        congratsMessage = GameObject.FindGameObjectWithTag("CongratsMessage");
        infosUI = GameObject.FindGameObjectWithTag("InfosUI");

        dungeonUI = GameObject.FindGameObjectWithTag("DungeonUI");
        dungeonUIText = GameObject.FindGameObjectWithTag("DungeonUIText").GetComponent<Text>();
        dungeonUIButton = GameObject.FindGameObjectWithTag("DungeonUIButton");
        dungeonUI.SetActive(false);

        GameObject[] dungeonsArray = GameObject.FindGameObjectsWithTag("Dungeon");
        dungeons = new List<Dungeon>();
        foreach(GameObject dungeon in dungeonsArray){
            dungeons.Add(dungeon.GetComponent<Dungeon>());
        }

        SetInfosInDungeons();
    }

    private void Update(){
        if(data.infos.progression_monde == 100){
            congratsMessage.SetActive(true);
        } else {
            congratsMessage.SetActive(false);
        }

        if(inRangeDungeon){
            SetDungeonUI(true);
            if (Input.GetKeyDown(KeyCode.Return)){
                LoadDungeon();
            }
        } else {
            SetDungeonUI(false);
        }
    }

    public void SetInRangeDungeon(bool _inRange){
        inRangeDungeon = _inRange;
    }

    public void SetCurrentSubject(string _subject){
        currentSubject = _subject;
    }

    public void SetCurrentDungeonDone(bool _done){
        currentDungeonDone = _done;
    }

    private int GetNbDungeons(string _sceneName){
        string string_tmp = string.Empty;
        int res = -1;
        for (int i=0; i< _sceneName.Length; i++)
        {
            if (System.Char.IsDigit(_sceneName[i]))
                string_tmp += _sceneName[i];
        }

        if (string_tmp.Length>0){
            res = int.Parse(string_tmp);
        }
        
        return res;
    }

    private void SetInfosInDungeons(){
        string jsonString = File.ReadAllText(jsonDataPath);
        data = JsonUtility.FromJson<JSONData>(jsonString);

        for(int i=0; i<dungeons.Count; ++i){
            dungeons[i].SetDungeonID(i);
        }

        string sceneName = SceneManager.GetActiveScene().name;
        int nbDungeons = GetNbDungeons(sceneName);

        foreach(Matiere subject in data.infos.matieres){
            int nbDungeonsBySubject = subject.nb_donjons;
            while(nbDungeonsBySubject > subject.nb_donjons_finis){
                int randint = Random.Range(0, nbDungeons);
                if (dungeons[randint].GetSubject().Equals("")){
                    dungeons[randint].SetSubject(subject.nom_matiere);
                    nbDungeonsBySubject -= 1;
                }
            }
        }
        for(int i=0; i<dungeons.Count; i++){
            if(dungeons[i].GetSubject().Equals("")){
                dungeons[i].SetDone(true);
            }
        }
    }

    private void SetDungeonUI(bool _param){
        if (_param){
            if (currentDungeonDone){
                dungeonUIText.text = "Tu as déjà fini ce donjon.";
                dungeonUIButton.SetActive(false);
            } else {
                dungeonUIText.text = "Tu approches d'un donjon !\nMatière : " + currentSubject;
            }
            dungeonUI.SetActive(true);
        } else {
            dungeonUIText.text = "";
            dungeonUIButton.SetActive(true);
            dungeonUI.SetActive(false);
        }
    }

    private void LoadDungeon(){
        if(!currentSubject.Equals("")){
            inRangeDungeon = false;
            infosUI.SetActive(false);
            currentDungeonData.SetCurrentSubject(currentSubject);
            currentDungeonData.SetHubName(SceneManager.GetActiveScene().name);
            StartCoroutine(loadNextScene());
        }
    }

    private int GetNbOfDungeons(){
        int nbOfDungeons = 0;
        int sceneNumber = SceneManager.sceneCountInBuildSettings;
        string[] allScenes;
        allScenes = new string[sceneNumber];
        for (int i=0; i<sceneNumber; i++)
        {
            allScenes[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }
        List<Scene> scenes = new List<Scene>();
        foreach(string sceneName in allScenes){
            if(sceneName.StartsWith("Dungeon_")){
                nbOfDungeons += 1;
            }
        }
        return nbOfDungeons;
    }

    private IEnumerator loadNextScene()
    {
        fadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Dungeon_"+Random.Range(1, GetNbOfDungeons()+1));
    }
}
