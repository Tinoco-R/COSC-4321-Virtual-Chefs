using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHide : MonoBehaviour
{
    public int tableID = 0;
    public bool grow = false;
    public bool parent = false;
    private Vector3 startSize;

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
        startSize = this.transform.localScale;
        if (parent)
        {
            this.transform.localScale = new Vector3(0, 0, 0);
        }
        if (this.GetComponent<MeshRenderer>() != null)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }
        if (this.GetComponent<CustomerHead>() != null)
        {
            this.GetComponent<CustomerHead>().makeNewColor();
        }
    }

    private void RespawnCustomer(int n, string c, string t)
    {
        if (n == tableID)
        {
            grow = true;
            if (parent) {
                this.transform.localScale = new Vector3(0, 0, 0);
                StartCoroutine(Grow((float)0.5));
            }
            if (this.GetComponent<CustomerHead>() != null) {
                this.GetComponent<CustomerHead>().makeNewColor();
            }
            if (this.GetComponent<MeshRenderer>() != null)
            {
                this.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    private void DespawnCustomer(int n, double s)
    {
        if (n == tableID)
        {
            grow = false;
            if (parent)
            {
                this.transform.localScale = startSize;
            }
            StartCoroutine(Shrink((float)0.5));
        }
    }

    IEnumerator Shrink(float duration)
    {
        if (parent)
        {
            float scale = startSize.y;
            while (grow == false && this.transform.localScale.y > 0.0)
            {
                yield return new WaitForSeconds((float)0.01);
                this.transform.localScale -= (new Vector3((float)scale * Time.deltaTime, (float)scale * Time.deltaTime, (float)scale * Time.deltaTime));
            }
        } 
        else
        {
            yield return new WaitForSeconds(duration + (float)4);
            if (grow == false)
            {
                this.GetComponent<MeshRenderer>().enabled = false;
            }
        }

    }

    IEnumerator Grow(float duration)
    {
        float scale = startSize.y;
        while (grow == true && this.transform.localScale.y < scale) {
            yield return new WaitForSeconds((float)0.01);
            this.transform.localScale += (new Vector3((float)scale * Time.deltaTime, (float)scale * Time.deltaTime, (float)scale * Time.deltaTime));
        }
    }
}
