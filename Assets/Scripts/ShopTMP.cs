using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopTMP : MonoBehaviour
{
    private void Awake(){
        if(Input.GetKeyDown(KeyCode.Return)){
            LoadStartMenu();
        }
    }

    public void LoadStartMenu(){
        SceneManager.LoadScene("StartMenu");
    }
}
