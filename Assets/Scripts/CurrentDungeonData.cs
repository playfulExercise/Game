using UnityEngine;

public class CurrentDungeonData : MonoBehaviour
{
    private string currentSubject;
    private string hubName;

    public void SetCurrentSubject(string _subject){
        currentSubject = _subject;
    }

    public string GetCurrentSubject(){
        return currentSubject;
    }

    public void SetHubName(string _hubName){
        hubName = _hubName;
    }

    public string GetHubName(){
        return hubName;
    }  
}
