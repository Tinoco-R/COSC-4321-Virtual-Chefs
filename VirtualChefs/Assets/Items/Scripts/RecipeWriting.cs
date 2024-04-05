using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RecipeWriting : MonoBehaviour
{

    public TMP_Text text;
    public int tableID;
    string curOrder;
    public GameObject TicketBody;
    public GameObject Housing;
    //private GameObject TicketText = this;
    //public RandomRecipe RecipeManager;
    private void Start()
    {
        //hide
        TicketBody.GetComponent<MeshRenderer>().enabled = false;
        TicketBody.GetComponent<BoxCollider>().enabled = false;
        //this.GetComponent<Renderer>().enabled = false;
        text = GetComponent<TextMeshProUGUI>();
        text.text = "";
        if (tableID == null)
        {
            tableID = 0;
        }
        //IsTaken = false;
    }
    private void OnEnable()
    {
        //RecipeManager = GetComponent<RandomRecipe>();
        /*
        RandomRecipe.makeBurger += WriteBurger;
        RandomRecipe.makeSalad += WriteSalad;
        RandomRecipe.makeTaco += WriteTaco;
        */
        //RandomRecipe.m_MyEvent.AddListener(WriteRecipe);
        RandomRecipe.recipeMade += WriteRecipe;
        ReadFood.orderGiven += ClearWriting;
    }

    private void OnDisable()
    {
        /*
        RandomRecipe.makeBurger -= WriteBurger;
        RandomRecipe.makeSalad -= WriteSalad;
        RandomRecipe.makeTaco -= WriteTaco;
        */
        //RecipeManager.m_MyEvent.RemoveListener(WriteRecipe);
        RandomRecipe.recipeMade -= WriteRecipe;
        ReadFood.orderGiven -= ClearWriting;
    }

    public void WriteRecipe(int n, string c, string t)
    {
        //IsTaken = true;
        //ShowModel

        //this.GetComponent<Renderer>().enabled = true;
        if (n == tableID)
        {
            TicketBody.GetComponent<MeshRenderer>().enabled = true;
            TicketBody.GetComponent<BoxCollider>().enabled = true;
            text.text = "<size=12%><b>Table " + n + "</b>\n" + t;
            curOrder = c;
        }
    }

    public void ClearWriting(int n, double s)
    {
        //hide model

        //this.GetComponent<Renderer>().enabled = false;
        //move to waypoint only move parent

        if (n == tableID)
        {
            TicketBody.GetComponent<MeshRenderer>().enabled = false;
            TicketBody.GetComponent<BoxCollider>().enabled = false;
            TicketBody.transform.position = Housing.transform.position;
            TicketBody.transform.rotation = Housing.transform.rotation;
            text.text = "";
            curOrder = "";
        }
    }
}