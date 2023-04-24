using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TMPro;



public class ClickEvent : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject dialogBox;

    [SerializeField]private int oxygenAdd = 10;
    [SerializeField]private int pearlCost = 20;
    private GameObject player;
    private GameObject boat;
    private PlayerController playerController;


    [TextArea(3,5)]
    public string itemDescription;

    public TMP_Text text;
    void Start()
    {
        text = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        boat = GameObject.Find("boatRoot");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(this.name.Equals("Exit"))
        {
            playerController.Dive();
            dialogBox.SetActive(false);
        }
        else if (this.name.Equals("man"))
        {
            
        }
        else
        {
            LevelUp();
        }
    }
 
    public void OnPointerEnter(PointerEventData eventData)
    {   
        if(this.name.Equals("Exit")){
            text.text = "Do you want to leave?";
        }
        else if(this.name.Equals("man")){
            text.text = "Welcome back! You have " + playerController.pearlNumber + " pearls.\nDo you want to buy something?";
        }
        else{
            text.text = "This is "+this.name + ". This costs " + pearlCost + " pearls. " + itemDescription;
        }
        
        dialogBox.SetActive(true);
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        dialogBox.SetActive(false);
    }
    void LevelUp()
    {   
        //升级
        if(playerController.pearlNumber >= pearlCost)
        {
            playerController.pearlNumber -= pearlCost;
            SoundManager.Instance.PlayLevelUp();
            if(this.name.Equals("breathing tube")){
                playerController.pipeLevel += 1;
                boat.transform.GetChild(playerController.pipeLevel-1).gameObject.SetActive(true);
            }
            else if(this.name.Equals("oxygen tank")){
                playerController.maxOxygen = 140;
                playerController.airTank.SetActive(true);
            }
            else if(this.name.Equals("thruster")){
                playerController.Thruster.SetActive(true);
            }
            else if(this.name.Equals("light stick")){
                playerController.haveLight = true;
            }
            else if(this.name.Equals("headlamp")){
                playerController.headLight.SetActive(true);
            }
            else if(this.name.Equals("heat resistant suit")){
                playerController.haveHeatSuit = true;

                foreach (GameObject suit in playerController.heatSuit)
                {
                    suit.SetActive(true);
                }
            }
            
            dialogBox.SetActive(false);
            this.gameObject.SetActive(false);
        }
        else
        {
            text.text = "Not Enough Pearls!";
            StartCoroutine(ShowText());
        }
    }
    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(1);
        dialogBox.SetActive(false);
    }
}