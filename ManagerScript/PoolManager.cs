using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolableObj
{
    public GameObject objPrefab;
    public int initSpawnNum;
    public bool isExpandable;
}


public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    public List<PoolableObj> poolableObjs;
    private List<GameObject> pooledObjs = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (PoolableObj aObj in poolableObjs)
        {
            for (int i = 0; i < aObj.initSpawnNum; i++)
            {
                GameObject aObjPrefab = Instantiate(aObj.objPrefab);
                aObjPrefab.SetActive(false);
                pooledObjs.Add(aObjPrefab);
            }
        }
    }

    public GameObject GetObjFromPool(string tag)
    {
        foreach(GameObject aObj in pooledObjs)
        {
            if (!aObj.activeInHierarchy && aObj.tag == tag)
            {
                return aObj;
            }
        }

        foreach (PoolableObj aObj in poolableObjs)
        {
            if (aObj.isExpandable && aObj.objPrefab.tag == tag)
            {
                GameObject aObjPrefab = Instantiate(aObj.objPrefab);
                aObjPrefab.SetActive(false);
                pooledObjs.Add(aObjPrefab);
                return aObjPrefab;
            }
        }

        return null;
    }


    private void Update()
    {
        
    }
}
