using UnityEngine;

public class MCQButtonManager : MonoBehaviour
{
    private string buttonText;
    private GameObject questionManager;

    private void Awake(){
        questionManager = GameObject.FindGameObjectWithTag("QuestionManager");
    }

    public void setButtonText(string _text){
        buttonText = _text;
    }

    public void OnChooseAnswser(){
        questionManager.GetComponent<QuestionManager>().MCQChangeAnswser(buttonText);
    }
}
