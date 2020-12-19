using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    [SerializeField] float botSpawnTimer = 15.0f;
    [SerializeField] int enemyCount = 5;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject botPrefab;

    public interface NotifySink
    { void OnEvent(); }

    LinkedList<NotifySink> alertList = new LinkedList<NotifySink>();
    LinkedList<GameObject> bots = new LinkedList<GameObject>();
    List<Checkpoint> cp_list = new List<Checkpoint>();


    void Start()
    {
        //Initiate list of checkpoints and randomize their timers by a small amount
        cp_list.AddRange(FindObjectsOfType<Checkpoint>());
        foreach (Checkpoint cp in cp_list)
        {
            cp.timeSinceCheckIn = Random.Range(0, 5);
        }

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject newBot = Instantiate(botPrefab);
            newBot.name = "PatrolBot: " + i;
            bots.AddFirst(newBot);
            newBot.SetActive(false);
        }

        StartCoroutine(ReleaseBot());
    }

    private void Update()
    {
        //cp_list[3].printPosition();
    }
    
    //Notification list
    public void Register(NotifySink notification)
    { alertList.AddLast(notification); }

    public void Notify(Vector3 notifierPos)
    {
        //foreach (NotifySink note in alertList)
        //{
        //    //if (Vector3.Distance(transform.position, notifierPos) < 100.0f)
        //    //{
        //    note.OnEvent();
        //    Debug.Log(name + " responding to alert flag");
        //    //}
        //}
    }

    public void AlertBots(Vector3 callPos)
    {
        foreach(GameObject bot in bots)
        {
            if (Vector3.Distance(callPos, bot.transform.position) < 40.0f)
            {
                bot.GetComponent<Animator>().SetBool("Alert", true);
                Debug.Log(bot.name + " responding to the call");
            }
        }
    }

    public void NotifyDead(GameObject deadBot)
    { deadBot.SetActive(false); }


    private GameObject NextPriorityCP()
    {
        float tempTime = 0f;
        Checkpoint bestCP = cp_list[1];
        foreach(Checkpoint cp in cp_list)
        {
            if(cp.timeSinceCheckIn > tempTime)
            {
                bestCP = cp;
                tempTime = cp.timeSinceCheckIn;
            }         
        }
        bestCP.timeSinceCheckIn -= 10.0f;
        return bestCP.gameObject;
    }
    
    //Patrol Bot Calls
    IEnumerator ReleaseBot()
    {
        foreach (GameObject bot in bots)
        {

            if (bot.activeSelf == false)
            {
                bot.GetComponent<PatrolBot>().Revive(spawnPoint);
                bot.GetComponent<PatrolBot>().targetObject = AssignCheckpoint();
                bot.GetComponent<PatrolBot>().ActivateSeek();
                break;
            }
        }

        yield return new WaitForSeconds(botSpawnTimer);
        StartCoroutine(ReleaseBot());
    }

    public GameObject AssignCheckpoint()
    {
        return NextPriorityCP();
    }


}
