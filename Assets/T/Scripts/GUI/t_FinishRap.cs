using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class t_FinishRap : MonoBehaviour
{


    public m_CheckPointManager CheckPointManager;
    public TextMeshProUGUI Raptext;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        Raptext.text = (CheckPointManager.lastLap).ToString();
    }
}
