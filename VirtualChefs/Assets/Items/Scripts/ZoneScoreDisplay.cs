using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ZoneScoreDisplay : MonoBehaviour
{
    public TMP_Text text;
    public int tableID;
    public GameObject ZoneParent;
    public GameObject lookAt = null;
    private void Start()
    {
        //text = GetComponent<TextMeshProUGUI>();
        text.text = "";
        if (tableID == null || tableID == 0)
        {
            tableID = ZoneParent.GetComponent<ReadFood>().tableID;
        }
        //IsTaken = false;
    }

    private void Update()
    {
        if (lookAt != null && text.text != "")
        {
            transform.LookAt(lookAt.transform.position);
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    private void OnEnable()
    {
        ReadFood.orderGiven += SetScore;
    }

    private void OnDisable()
    {
        ReadFood.orderGiven -= SetScore;
    }

    private void SetScore(int n, double s)
    {
        text.text = "Score: " + (((int)(s * 100)) / 100);
        StartCoroutine(WaitRoutine(5));
        //text.text = "";
    }

    IEnumerator WaitRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        text.text = "";
    }
}
