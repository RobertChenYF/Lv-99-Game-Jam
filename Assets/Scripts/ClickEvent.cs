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
    [SerializeField]private GameObject player;
    [SerializeField]private GameObject boat;

    private PlayerController playerController;

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
        else{
            text.text = "This is "+this.name + ". This costs " + pearlCost + " pearls.";
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
            if(this.name.Equals("breathing tube")){
                playerController.pipeLevel += 1;
                boat.transform.GetChild(playerController.pipeLevel-1).gameObject.SetActive(true);
            }
            else{
                playerController.maxOxygen += oxygenAdd;
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