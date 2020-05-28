using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    private string jsonDataPath = "Assets/data.json";
    private JSONData data;

    private CurrentDungeonData currentDungeonData;
    private string hubName;
    private string currentSubject; 

    private GameObject player;
    private float moveSpeedSave;
    private float jumpForceSave;

    private Question currentQuestion;
    private int currentQuestionID;
    private bool isAnswered = false;
    private int remainingTrials;
    private string mcqChosenAnswer;
    
    private int currentEnemyID;
    private int reward;
    private int damage;

    private int nbEnemies;
    public int nbEnemiesDefeated;

    private bool questionModeActivated = false;
    private GameObject questionModeUI;
    private GameObject freeUI;
    private GameObject mcqUI;
    private Text questionText;
    private Text freeAnswerText;
    private GameObject[] mcqButtonTextArray;
    private Text mcqAnswerText;
    private Text remainingTrialsText;

    public string GetHubName(){
        return hubName;
    }

    public string GetCurrentSubject(){
        return currentSubject;
    }

    public int GetCurrentQuestionID(){
        return currentQuestionID;
    }

    public bool IsAnswered(){
        return isAnswered;
    }

    public void SetReward(int _reward){
        reward = _reward;
    }

    public void SetDegats(int _damage){
        damage = _damage;
    }

    private void Awake(){
        player = GameObject.FindGameObjectWithTag("Player");

        currentDungeonData = GameObject.FindGameObjectWithTag("CurrentDungeonData").GetComponent<CurrentDungeonData>();
        currentSubject = currentDungeonData.GetCurrentSubject();
        hubName = currentDungeonData.GetHubName();
    
        questionModeUI = GameObject.FindGameObjectWithTag("QuestionModeUI");
        freeUI = GameObject.FindGameObjectWithTag("FreeUI");
        mcqUI = GameObject.FindGameObjectWithTag("McqUI");

        questionText = GameObject.FindGameObjectWithTag("QuestionText").GetComponent<Text>();
        freeAnswerText = GameObject.FindGameObjectWithTag("FreeAnswerText").GetComponent<Text>();
        mcqButtonTextArray = GameObject.FindGameObjectsWithTag("McqButtonText");
        mcqAnswerText = GameObject.FindGameObjectWithTag("McqAnswerText").GetComponent<Text>();
        remainingTrialsText = GameObject.FindGameObjectWithTag("RemainingTrialsText").GetComponent<Text>();

        nbEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        nbEnemiesDefeated = 0;
    
        SetEnemiesID();

        questionModeUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && questionModeActivated)
        {
            ToggleQuestionMode(false);
        }
        if (Input.GetKeyDown(KeyCode.Return) && questionModeActivated)
        {
            AnswerValidated();
        }
    }

    public int GetNbEnemiesDefeated(){
        return nbEnemiesDefeated;
    }

    public int GetNbEnemies(){
        return nbEnemies;
    }

    private void ToggleQuestionMode(bool _isActivated)
    {
        if (_isActivated)
        {
            if (currentQuestionID != -1){
                questionModeActivated = true;
                // Sauvegarde des valeurs de moveSpeed et jumpForce puis 
                moveSpeedSave = player.GetComponent<PlayerMovement>().moveSpeed;
                jumpForceSave = player.GetComponent<PlayerMovement>().jumpForce;
                // moveSpeed et jumpForce à 0 => déplacements impossibles
                player.GetComponent<PlayerMovement>().moveSpeed = 0;
                player.GetComponent<PlayerMovement>().jumpForce = 0;

                if (currentQuestion.type.Equals("libre")){
                    mcqUI.SetActive(false);
                    freeUI.SetActive(true);
                    freeUI.GetComponent<InputField>().Select();
                }
                else if (currentQuestion.type.Equals("qcm")){
                    freeUI.SetActive(false);
                    int i = 0;
                    foreach (GameObject button in mcqButtonTextArray){
                        button.GetComponent<MCQButtonManager>().setButtonText(currentQuestion.propositions_qcm[i]);
                        button.GetComponent<Text>().text = currentQuestion.propositions_qcm[i];
                        i++;
                    }
                }
                questionModeUI.SetActive(true);
            }
        } 
        else
        {
            questionModeActivated = false;
            // moveSpeed et jumpForce récupèrent leur valeur de base => déplacements possibles
            player.GetComponent<PlayerMovement>().moveSpeed = moveSpeedSave;
            player.GetComponent<PlayerMovement>().jumpForce = jumpForceSave;

            questionModeUI.SetActive(false);
            mcqUI.SetActive(true);
            freeUI.SetActive(true);
        }
    }

    private void SetEnemiesID(){
        int cpt = 0;
        GameObject[] enemiesArray;
        enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemiesArray){
            enemy.GetComponent<Enemy>().SetEnemyID(cpt++);
        }
    }

    public void SetCurrentEnemyID(int _id){
        currentEnemyID = _id;
    }

    public int GetCurrentEnemyID(){
        return currentEnemyID;
    }




    /////////////////////
    // PARTIE QUESTION //
    /////////////////////

    public void GetNewQuestion(List<string> _subjectList, bool _free, bool _mcq, bool _alreadyAsked)
    {
        isAnswered = false;
        remainingTrials = 3;
        ChangeTextRemainingTrials();

        currentQuestion = PickQuestion(_subjectList, _free, _mcq, _alreadyAsked);

        if(currentQuestion != null){
            currentQuestionID = currentQuestion.id;
            DisplayCurrentQuestion();
            ToggleQuestionMode(true);
        } else {
            currentQuestionID = -1;
            nbEnemiesDefeated++;
        }
    }

    private void DisplayCurrentQuestion()
    {
        if (currentQuestionID != -1)
        {
            questionText.text = currentQuestion.intitule;
        } else
        {
            questionText.text = "Désolé... Il n'y a pas de questions à poser.";
        }
    }

    // true -> 1, false -> 0
    private int BoolToInt(bool _it)
    {
        return _it ? 1 : 0;
    }

    private Question PickQuestion(List<string> _subjectList, bool _free, bool _mcq, bool _alreadyAsked)
    {
        Question output = new Question();

        // Récupération des données du fichier data.json
        string jsonString = File.ReadAllText(jsonDataPath);
        data = JsonUtility.FromJson<JSONData>(jsonString);

        // Transformation des bools en paramètres en un nombre décimal
        string optStr = BoolToInt(_free).ToString() + BoolToInt(_mcq).ToString() + BoolToInt(_alreadyAsked).ToString();
        int options = System.Convert.ToInt32(optStr, 2);
        if (options > 7)
        {
            Debug.Log("ERROR : trop d'options données pour la recherche aléatoire de question");
        }

        // Récupération de la liste des questions appartenant aux matières choisies.
        Question[] allQuestions = data.questions;
        List<Question> allQuestionsReduced = new List<Question>();
        string subjects = string.Join(", ", _subjectList);
        foreach (Question question in allQuestions) {
            if (subjects.Contains(question.matiere))
            {
                allQuestionsReduced.Add(question);
            }
        }
        
        // Récupération de la liste des questions en suivant les options données en paramètres
        List<Question> questionList = new List<Question>();
        switch (options)
        {
            // Pas encore posée
            case 0:
            case 6:
                foreach (Question question in allQuestionsReduced)
                {
                    if (!question.deja_pose)
                    {
                        questionList.Add(question);
                    }
                }
                break;
            // Déjà posée
            case 1:
            case 7:
                foreach (Question question in allQuestionsReduced)
                {
                    questionList.Add(question);
                }
                break;
            // QCM & Pas encore posée
            case 2:
                foreach (Question question in allQuestionsReduced)
                {
                    if (question.type.Equals("qcm") && !question.deja_pose)
                    {
                        questionList.Add(question);
                    }
                }
                break;
            // QCM & Déjà posée
            case 3:
                foreach (Question question in allQuestionsReduced)
                {
                    if (question.type.Equals("qcm"))
                    {
                        questionList.Add(question);
                    }
                }
                break;
            // libre & Pas encore posée
            case 4:
                foreach (Question question in allQuestionsReduced)
                {
                    if (question.type.Equals("libre") && !question.deja_pose)
                    {
                        questionList.Add(question);
                    }
                }
                break;
            // libre & Déjà posée
            case 5:
                foreach (Question question in allQuestionsReduced)
                {
                    if (question.type.Equals("libre"))
                    {
                        questionList.Add(question);
                    }
                }
                break;
        }

        // On récupère au hasard une question dans la liste obtenue.
        if (questionList.Count != 0)
        {
            output = questionList[Random.Range(0, questionList.Count)];
        } else
        {
            output = null;
            Debug.Log("Il n'y a plus de questions à poser pour la/les matière/s choisies.");
        }
       
        return output;
    }

    



    ////////////////////
    // PARTIE REPONSE //
    ////////////////////

    public void MCQChangeAnswser(string _answer){
        mcqChosenAnswer = _answer;
        mcqAnswerText.text = "Réponse choisie : " + mcqChosenAnswer;
    }

    private string FormatedString(string _str){
        return _str.ToLower().Trim();
    }

    private bool VerifyAnswer()
    {
        bool output = false;

        if (currentQuestion.type.Equals("libre"))
        {
            if (currentQuestion.strict_libre)
            {
                foreach(string possibleAnswer in currentQuestion.reponses_libre)
                {
                    if (FormatedString(freeAnswerText.text).Equals(FormatedString(possibleAnswer)))
                    {
                        output = true;
                    }
                }
            } else {
                foreach(string possibleAnswer in currentQuestion.reponses_libre)
                {
                    if (FormatedString(freeAnswerText.text).Contains(FormatedString(possibleAnswer)))
                    {
                        output = true;
                    }
                }
            }
            
        }
        else if (currentQuestion.type.Equals("qcm")){
            string answer = currentQuestion.propositions_qcm[currentQuestion.reponse_qcm];
            if (mcqChosenAnswer.Equals(answer)){
                output = true;
            }
        }

        return output;
    }

    public void AnswerValidated()
    {
        if (VerifyAnswer())
        {
            string messageToDisplay = "Bravo ! C'est la bonne réponse ! \nBon courage pour la suite de ton aventure !";
            StartCoroutine(DisplayMessageDuring(2f, messageToDisplay, questionText.text, true));
            UpdateDataQuestionDejaPose();
            Inventory.instance.AddCoins(reward);
            nbEnemiesDefeated++;
            isAnswered = true;
        } else
        {
            remainingTrials--;
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            ChangeTextRemainingTrials();
            if(remainingTrials == 0){
                string answer = "";
                if (currentQuestion.type.Equals("libre")){
                    answer = currentQuestion.reponses_libre[0];
                } 
                else if (currentQuestion.type.Equals("qcm")){
                    answer = currentQuestion.propositions_qcm[currentQuestion.reponse_qcm];                        
                }
                string messageToDisplay = "Dommage... \n" +
                                            "Tu n'as plus d'essai disponible. \n" +
                                            "La réponse était : " + answer + ".";
                StartCoroutine(DisplayMessageDuring(5f, messageToDisplay, questionText.text, true));
            } else {
                string messageToDisplay = "Dommage ce n'est pas la bonne réponse... \nEssaie encore !";
                StartCoroutine(DisplayMessageDuring(2f, messageToDisplay, questionText.text, false));
            }
        }
    }

    private IEnumerator DisplayMessageDuring(float _seconds, string _message, string _previousMessage, bool _isLastMessage)
    {
        questionText.text = _message;
        yield return new WaitForSeconds(_seconds);
        if(_isLastMessage){
            ToggleQuestionMode(false);
            freeUI.GetComponent<InputField>().text = "";
            mcqAnswerText.text = "Clique sur un bouton !";
        } else {
            questionText.text = _previousMessage;
            freeUI.GetComponent<InputField>().text = "";
        }
    }

    private void ChangeTextRemainingTrials(){
        switch(remainingTrials){
            case 3:
                    remainingTrialsText.text = remainingTrials + "/3";
                    remainingTrialsText.color = new Vector4(0, 255, 0, 255);
                    break;
            case 2:
                    remainingTrialsText.text = remainingTrials + "/3";
                    remainingTrialsText.color = new Vector4(255, 127, 0, 255);
                    break;
            case 1:
                    remainingTrialsText.text = remainingTrials + "/3";
                    remainingTrialsText.color = new Vector4(255, 0, 0, 255);
                    break;
            default:
                    remainingTrialsText.text = remainingTrials + "/3";
                    break;
        }
    }

    private void UpdateDataQuestionDejaPose(){
        foreach (Question question in data.questions){
            if (question.id == currentQuestionID){
                question.deja_pose = true;
            }
        }

        string jsonUpdated = JsonUtility.ToJson(data, true);
        File.WriteAllText(jsonDataPath, jsonUpdated);
    }
}
