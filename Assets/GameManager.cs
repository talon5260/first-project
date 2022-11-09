using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject talkPanel;
    public TextMeshProUGUI talkText;
    public GameObject scanObject;
    public PlayerAction player;
    public TalkManager talkManager;

    public bool isTalkPanelActive;
    public int Health_Point;
    public int talkIndex;
    public Image[] Life;
    public GameObject Restart_Button;

    public int TotalCheescCount;
    public int CheeseCount;
    public TextMeshProUGUI CheeseCountText;
    public TextMeshProUGUI TotalCheeseText;
    public GameObject ClearText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Awake()
    {
        TotalCheeseText.text = "  /  " + TotalCheescCount.ToString();
    }

    public void SearchAction(GameObject scan_Object)
    {
        scanObject = scan_Object;
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNpc);
        talkPanel.SetActive(isTalkPanelActive);
    }
    public void HealthDown()
    {
        if (Health_Point > 1)
        {
            Health_Point--;
            Life[Health_Point].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            Health_Point--;
            Life[Health_Point].color = new Color(1, 0, 0, 0.4f);
            player.OnDie();
            Text buttonText = Restart_Button.GetComponentInChildren<Text>();
            Restart_Button.SetActive(true);
        }
    }
    public void GetItem(int count)
    {
        CheeseCountText.text = count.ToString();
        CheeseCount++;
        if (CheeseCount == 3)
        {
            ClearText.SetActive(true);
            player.PlaySound("FINISH");
            Invoke("GoScene1",3);
        }
    }

    void GoScene1()
    {
        SceneManager.LoadScene("Scene1");
        ClearText.SetActive(false);        
    }

    void Talk(int id, bool isNpc) 
    {
        string talkData = talkManager.GetTalk(id, talkIndex);
        if (talkData == null) 
        {
            isTalkPanelActive = false;
            talkIndex = 0;
            return;
        }
        if (isNpc) 
        {
            talkText.text = talkData;
        }
        else
        {
            talkText.text = talkData;
        }
        isTalkPanelActive = true;
        talkIndex++;
    }
}
