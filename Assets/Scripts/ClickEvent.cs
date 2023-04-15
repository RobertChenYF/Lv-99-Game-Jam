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

    public TMP_Text text;
    void Start()
    {
        text = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        LevelUp();
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
        if(GlobalData.Instance.pearls >= pearlCost)
        {
            GlobalData.Instance.pearls -= pearlCost;
            if(this.name.Equals("breathing tube")){
                GlobalData.Instance.pipeLevel += 1;
            }
            else{
                GlobalData.Instance.maxOxygen += oxygenAdd;
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