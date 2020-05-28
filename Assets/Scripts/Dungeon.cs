using UnityEngine;

public class Dungeon : MonoBehaviour
{
    private DungeonManager dungeonManager;

    private int id_dungeon;
    private string subject;
    private bool dungeonDone;

    public void SetDone(bool _done){
        dungeonDone = _done;
    }

    public bool IsDone(){
        return dungeonDone;
    }

    public void SetDungeonID(int _id){
        id_dungeon = _id;
    }

    public void SetSubject(string _subject){
        subject = _subject;
    }

    public string GetSubject(){
        return subject;
    }


    private void Awake(){
        dungeonManager = GameObject.FindGameObjectWithTag("DungeonManager").GetComponent<DungeonManager>();
        id_dungeon = -1;
        subject = "";
        dungeonDone = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dungeonManager.SetCurrentSubject(subject);
            dungeonManager.SetCurrentDungeonDone(dungeonDone);
            dungeonManager.SetInRangeDungeon(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player"))
        {
            dungeonManager.SetCurrentSubject("");
            dungeonManager.SetCurrentDungeonDone(false);
            dungeonManager.SetInRangeDungeon(false);
        }
    }
}
