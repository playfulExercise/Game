using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskQuestion : MonoBehaviour
{

    public Enemy enemy;

    private QuestionManager questionManager;
    
    public List<string> subjectList = new List<string>();
    public bool freeQuestion = true;
    public bool mcqQuestion = true;
    public bool alreadyAskedQuestionsAccepted = false;


    void Awake()
    {
        questionManager = GameObject.FindGameObjectWithTag("QuestionManager").GetComponent<QuestionManager>();

        if (!questionManager.GetCurrentSubject().Equals("")){
            subjectList.Add(questionManager.GetCurrentSubject());
        } else {
            string[] subjects = {"francais", "anglais", "maths", "histoire", "geographie"};
            subjectList.AddRange(subjects);
        }
    }

    void Update(){
        if (questionManager.GetCurrentEnemyID() == enemy.GetEnemyID() && (questionManager.IsAnswered() || questionManager.GetCurrentQuestionID()==-1)){
            StartCoroutine(DestroySelf());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            questionManager.GetComponent<QuestionManager>().SetCurrentEnemyID(enemy.GetEnemyID());

            questionManager.SetReward(enemy.GetReward());
            questionManager.SetDegats(enemy.GetDamage());
            
            questionManager.GetComponent<QuestionManager>().GetNewQuestion(subjectList, freeQuestion, mcqQuestion, alreadyAskedQuestionsAccepted);
        }
    }

    private IEnumerator DestroySelf(){
        GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
