using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_CheckPoint : MonoBehaviour
{
    [SerializeField] private m_CheckPointManager checkPointManager;
    [SerializeField] private string carTag;
    private int pointIndex;
    private m_CheckPoint checkPointBefore;
    private m_CheckPoint checkPointAfter;
    public List<Collider> ranking = new();
    public Transform thisTransform;

    void Start()
    {
        thisTransform = gameObject.transform;
        
        pointIndex = Array.IndexOf(checkPointManager.checkPoints, this);

        if (pointIndex >= 1)
        {
            checkPointBefore = checkPointManager.checkPoints[pointIndex - 1];
        }
        else
        {
            checkPointBefore = checkPointManager.checkPoints[^1];
        }

        if (pointIndex <= checkPointManager.checkPoints.Length - 2)
        {
            checkPointAfter = checkPointManager.checkPoints[pointIndex + 1];
        }
        else
        {
            checkPointAfter = checkPointManager.checkPoints[0];
        }
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(carTag))
        {
            if (checkPointBefore.ranking.IndexOf(collision) >= 0)
            {
                checkPointBefore.ranking.Remove(collision);
                ranking.Add(collision);
                if (pointIndex == 0)
                {
                    if (!checkPointManager.carLapCount.ContainsKey(collision)) checkPointManager.carLapCount.Add(collision, 0);
                    checkPointManager.carLapCount[collision] += 1;
                    if (checkPointManager.carLapCount[collision] == checkPointManager.lastLap + 1)
                    {
                        checkPointManager.lastRanking.Add(collision);
                    }
                }
            }
            else if (checkPointAfter.ranking.IndexOf(collision) >= 0)
            {
                checkPointAfter.ranking.Remove(collision);
                checkPointBefore.ranking.Add(collision);
            }
            else if (ranking.IndexOf(collision) >= 0)
            {
                ranking.Remove(collision);
                checkPointBefore.ranking.Add(collision);
            }
            else
            {
                ranking.Add(collision);
            }

            // carに checkPointAfter.thisTransform を渡す
        }
    }

    public Dictionary<int, List<Collider>> GetCurrentRankingFromPoint()
    {
        ranking.Sort((a, b) => Mathf.RoundToInt(Vector3.SqrMagnitude(b.transform.position - thisTransform.position) - Vector3.SqrMagnitude(a.transform.position - thisTransform.position)));
        Dictionary<int, List<Collider>> result = new();
        for (int i = 0; i < ranking.Count; i++)
        {
            int lapCount = checkPointManager.carLapCount[ranking[i]];
            if (!result.ContainsKey(lapCount))
            {
                result.Add(lapCount, new List<Collider>());
            }
            result[lapCount].Add(ranking[i]);
        }
        return result;
    }
}