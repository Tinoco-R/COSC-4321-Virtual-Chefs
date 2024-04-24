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
    public double ticketTimerBase = 60;
    public double ticketTimer = 60;
    private Color baseColor;

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
        ticketTimer = ticketTimerBase;
        ZoneVisual.GetComponent<MeshRenderer>().enabled = false;
        foodInZone.Clear();
        currentOrder = "";
        baseColor = ZoneVisual.GetComponent<Renderer>().material.color;
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
            if(foodInZone.Count == 0)
            {
                ZoneVisual.GetComponent<Renderer>().material.color = baseColor;
            }
        }
    }


    public void OrderReceived(int n, string c, string t)
    {
        ZoneVisual.GetComponent<MeshRenderer>().enabled = true;
        if (n == tableID)
        {
            score = 0;
            ticketTimer = ticketTimerBase;
            currentOrder = c;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (foodInZone.Count > 0 && timer > 0 && currentOrder != "")
        {
            timer -= Time.deltaTime;
            float ratio = (float)timer / 3;
            if (timer < 0)
            {
                ratio = 0;
            }
            Color lerpedColor = Color.white;
            //lerpedColor = Color.Lerp(Color.white, new Color((float)0.0745, (float)0.4902, (float)0.5098, 1), ratio);
            lerpedColor = Color.Lerp(Color.white, baseColor, ratio);
            ZoneVisual.GetComponent<Renderer>().material.color = lerpedColor;
        }
        if (currentOrder != "" && ticketTimer > 0)
        {
            ticketTimer -= Time.deltaTime;
        }
        if (foodInZone.Count > 0 && timer <= 0 && currentOrder != "")
        {
            turnInPlate();
            //ZoneVisual.GetComponent<Renderer>().material.color = new Color((float)0.0745, (float)0.4902, (float)0.5098, 1);
            ZoneVisual.GetComponent<Renderer>().material.color = baseColor;
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
            bool RareMeat = false;
            bool MediumMeat = false;
            bool WellDoneMeat = false;
            bool Cheese = false;
            bool Tomato = false;
            bool TopBun = false;
            bool BotBun = false;
            bool TopBunToasted = false;
            bool BotBunToasted = false;
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
                if (items.tag == "RareMeat")
                {
                    if (currentOrder[2] == '0' && RareMeat != true)
                    {
                        //print("meat");
                        correctCount++;
                        totalCount++;
                        RareMeat = true;
                    } 
                    else
                    {
                        totalCount++;
                    }
                }
                if (items.tag == "MediumMeat")
                {
                    if (currentOrder[2] == '1' && MediumMeat != true)
                    {
                        //print("meat");
                        correctCount++;
                        totalCount++;
                        MediumMeat = true;
                    }
                    else
                    {
                        totalCount++;
                    }
                }
                if (items.tag == "WellDoneMeat")
                {
                    if (currentOrder[2] == '2' && WellDoneMeat != true)
                    {
                        //print("meat");
                        correctCount++;
                        totalCount++;
                        WellDoneMeat = true;
                    }
                    else
                    {
                        totalCount++;
                    }
                }
                if (items.tag == "TopBun")
                {
                    if (TopBun != true && currentOrder[1] == '0') {
                        //print("top");
                        correctCount++;
                        totalCount++;
                        TopBun = true;
                    } else
                    {
                        totalCount++;
                    }
                }
                if (items.tag == "TopBunToasted")
                {
                    if (TopBunToasted != true && currentOrder[1] == '1')
                    {
                        //print("top");
                        correctCount++;
                        totalCount++;
                        TopBunToasted = true;
                    }
                    else
                    {
                        totalCount++;
                    }
                }
                if (items.tag == "BottomBun")
                {
                    if (BotBun != true && currentOrder[1] == '0')
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
                if (items.tag == "BottomBunToasted")
                {
                    if (BotBunToasted != true && currentOrder[1] == '1')
                    {
                        //print("bot");
                        correctCount++;
                        totalCount++;
                        BotBunToasted = true;
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
            if (currentOrder[1] == '0' && TopBun == false)
            {
                totalCount++;
                //print("n");
            }
            if (currentOrder[1] == '0' && BotBun == false)
            {
                totalCount++;
                //print("no");
            }
            if (currentOrder[1] == '1' && TopBunToasted == false)
            {
                totalCount++;
                //print("n");
            }
            if (currentOrder[1] == '1' && BotBunToasted == false)
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
            if (currentOrder[2] == '0' && RareMeat == false)
            {
                totalCount++;
                //print("noa");
            }
            if (currentOrder[2] == '1' && MediumMeat == false)
            {
                totalCount++;
                //print("noa");
            }
            if (currentOrder[2] == '2' && WellDoneMeat == false)
            {
                totalCount++;
                //print("noa");
            }
            if (currentOrder[5] != '9' && currentOrder[5] != '0' && Lettuce == false)
            {
                totalCount++;
                //print("nob");
            }
            double multiplier = (ticketTimer / ticketTimerBase) * 2;
            if (multiplier < 0.25)
            {
                multiplier = 0.25;
            }
            score = correctCount / totalCount * 100 * multiplier;
            GameObject ScoreInstance = GameObject.FindGameObjectWithTag("ScoreBox");
            TotalScoreReader totalScore = ScoreInstance.GetComponent<TotalScoreReader>();
            totalScore.totalScore += score;
            
            
            
            if (correctCount == 1)
            {
                score = 0;
            }
            //print(correctCount);
            //print(totalCount);
            //print(score);
            ZoneVisual.GetComponent<MeshRenderer>().enabled = false;
            orderGiven(tableID, score);
            currentOrder = "";
            timer = 3;
            ticketTimer = ticketTimerBase;
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