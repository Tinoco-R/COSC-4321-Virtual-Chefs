using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ReadFood : MonoBehaviour
{

    public int tableID;
    public string currentOrder;
    public double score;
    public GameObject ZoneVisual;
    public List<GameObject> foodInZone = new List<GameObject>();
    public double timer = 3.0;

    public delegate void OrderGivenEvent(int n, double s);
    public static event OrderGivenEvent orderGiven;

    private void OnEnable()
    {
        RandomRecipe.recipeMade += OrderReceived;
    }

    private void OnDisable()
    {
        RandomRecipe.recipeMade -= OrderReceived;
    }



    // Start is called before the first frame update
    void Start()
    {
        ZoneVisual.GetComponent<MeshRenderer>().enabled = false;
        foodInZone.Clear();
        currentOrder = "";
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Plate")
        {
            timer = 3.0;
            foodInZone.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Plate")
        {
            timer = 3.0;
            foodInZone.Remove(other.gameObject);
        }
    }


    public void OrderReceived(int n, string c, string t)
    {
        ZoneVisual.GetComponent<MeshRenderer>().enabled = true;
        if (n == tableID)
        {
            score = 0;
            currentOrder = c;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (foodInZone.Count > 0 && timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (foodInZone.Count > 0 && timer <= 0 && currentOrder != "")
        {
            
            turnInPlate();
        }
    }

    void turnInPlate ()
    {
        foreach (GameObject obj in foodInZone)
        {
            //GAMEOBJECT.GetComponent<ClassName>().VariableName;
            double correctCount = 0;
            double totalCount = 0;
            //duplicate checkers
            bool Plate = false;
            bool Meat = false;
            bool Cheese = false;
            bool Tomato = false;
            bool TopBun = false;
            bool BotBun = false;
            bool Lettuce = false;
            //List<GameObject> foodCode = obj.GetComponent<Combine>().plate;
            foreach (Food items in obj.GetComponent<Combine>().plate)
            {
                if (items.tag == "Plate")
                {
                    //ignore
                    //print("plate");
                    correctCount++;
                    totalCount++;
                    Plate = true;
                }
                if (items.tag == "CookedMeat")
                {
                    if (currentOrder[2] != '9' && Meat != true)
                    {
                        //print("meat");
                        correctCount++;
                        totalCount++;
                        Meat = true;
                    } 
                    else
                    {
                        totalCount++;
                    }
                }
                if (items.tag == "TopBun")
                {
                    if (TopBun != true && currentOrder[0] != 3) {
                        //print("top");
                        correctCount++;
                        totalCount++;
                        TopBun = true;
                    } else
                    {
                        totalCount++;
                    }
                }
                if (items.tag == "BottomBun")
                {
                    if (BotBun != true && currentOrder[0] != 3)
                    {
                        //print("bot");
                        correctCount++;
                        totalCount++;
                        BotBun = true;
                    }
                    else
                    {
                        totalCount++;
                    }
                }
                if (items.tag == "TomatoSlice")
                {
                    if (currentOrder[4] != '9' && currentOrder[4] != '0' && Tomato != true)
                    {
                        //print("tomato");
                        correctCount++;
                        totalCount++;
                        Tomato = true;
                    } 
                    else
                    {
                        totalCount++;
                    }
                }
                if (items.tag == "CheeseSlice")
                {
                    if (currentOrder[3] != '9' && currentOrder[3] != '0' && Cheese != true)
                    {
                        //print("cheese");
                        correctCount++;
                        totalCount++;
                        Cheese = true;
                    }
                    else
                    {
                        totalCount++;
                    }
                }
                if (items.tag == "LettuceSlice")
                {
                    if (currentOrder[5] != '9' && currentOrder[5] != '0' && Lettuce != true)
                    {
                        //print("lettuce");
                        correctCount++;
                        totalCount++;
                        Lettuce = true;
                    }
                    else
                    {
                        totalCount++;
                    }
                }
            }
            if (Plate == false) 
            {
                totalCount++;
                //print("noe");
            }
            if (currentOrder[1] != '9' && TopBun == false)
            {
                totalCount++;
                //print("n");
            }
            if (currentOrder[1] != '9' && BotBun == false)
            {
                totalCount++;
                //print("no");
            }
            if (currentOrder[4] != '9' && currentOrder[4] != '0' && Tomato == false)
            {
                totalCount++;
                //print("nomasd");
            }
            if (currentOrder[3] != '9' && currentOrder[3] != '0' && Cheese == false)
            {
                totalCount++;
                //print("nom");
            }
            if (currentOrder[2] != '9' && Meat == false)
            {
                totalCount++;
                //print("noa");
            }
            if (currentOrder[5] != '9' && currentOrder[5] != '0' && Lettuce == false)
            {
                totalCount++;
                //print("nob");
            }
            score = correctCount / totalCount * 100;
            //print(correctCount);
            //print(totalCount);
            //print(score);
            ZoneVisual.GetComponent<MeshRenderer>().enabled = false;
            orderGiven(tableID, score);
            currentOrder = "";
            timer = 3;
            
            foreach (Food items in obj.GetComponent<Combine>().plate)
            {
                if (items.tag != "Plate")
                {
                    Destroy(items.item);
                }
            }
            foodInZone.Remove(obj);
            Destroy(obj);
            break;
        }
    }
}