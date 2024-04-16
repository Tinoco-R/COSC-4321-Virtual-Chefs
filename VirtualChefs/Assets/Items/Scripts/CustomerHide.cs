using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHide : MonoBehaviour
{
    public int tableID = 0;
    private void OnEnable()
    {
        RandomRecipe.recipeMade += RespawnCustomer;
        ReadFood.orderGiven += DespawnCustomer;
    }

    private void OnDisable()
    {
        RandomRecipe.recipeMade -= RespawnCustomer;
        ReadFood.orderGiven -= DespawnCustomer;
    }

    // Start is called before the first frame update
    void Start()
    {
        DespawnCustomer(tableID, 0);
    }

    private void RespawnCustomer(int n, string c, string t)
    {
        if (n == tableID)
        {
            this.GetComponent<MeshRenderer>().enabled = true;
            if (this.GetComponent<CustomerHead>() != null) {
                this.GetComponent<CustomerHead>().makeNewColor();
            }
        }
    }

    private void DespawnCustomer(int n, double s)
    {
        if (n == tableID)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
