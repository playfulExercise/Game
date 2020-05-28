using UnityEngine;

public class ValidateButton : MonoBehaviour
{
    private QuestionManager questionManager;

    private void Awake(){
        questionManager = GameObject.FindGameObjectWithTag("QuestionManager").GetComponent<QuestionManager>();
    }

    // public void OnValidate(){
    //     questionManager.AnswerValidated();
    // }
}