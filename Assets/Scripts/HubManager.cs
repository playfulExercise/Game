using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    private string dataFilePath = "Assets/data.json";
    private JSONData data;

    public Animator fadeSystem;

    public void Awake()
    {
        fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Return)){
            LoadCorrectHub();
        }
    }

    private int GetNbDungeons(){
        string jsonString = File.ReadAllText(dataFilePath);
        data = JsonUtility.FromJson<JSONData>(jsonString);
        int nbDungeons = 0;
        foreach(Matiere subject in data.infos.matieres){
            nbDungeons += subject.nb_donjons;
        }
        return nbDungeons;
    }

    public void LoadCorrectHub(){
        int nbDungeons = GetNbDungeons();
        if (nbDungeons==3 || nbDungeons==5 || nbDungeons==10){
            StartCoroutine(loadNextScene("Hub_"+nbDungeons));
        } else {
            Debug.Log("Le nombre de donjons dans le fichier data.json est erroné et le monde ne peut être chargé.");
        }
    }

    private IEnumerator loadNextScene(string sceneName)
    {
        fadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}
