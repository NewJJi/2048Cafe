using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager
{
	//private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

 //   public void InitPool(GameObject prefab)
 //   {
 //       CreatePool(prefab);
 //   }
 //   public GameObject Pop(string name)
 //   {
 //       return pools[name].Pop();
 //   }
 //   public GameObject Pop(GameObject prefab)
	//{
	//	if (pools.ContainsKey(prefab.name) == false)
	//		CreatePool(prefab);

	//	return pools[prefab.name].Pop();
	//}

	//public bool Push(GameObject go)
	//{
	//	if (pools.ContainsKey(go.name) == false)
	//		return false;

	//	pools[go.name].Push(go);
	//	return true;
	//}

	//public void Clear()
	//{
	//	pools.Clear();
	//}

	//private void CreatePool(GameObject original)
	//{
	//	Pool pool = new Pool(original);
	//	pools.Add(original.name, pool);
	//}
}
//internal class Pool
//{
//    private GameObject prefab;
//    private IObjectPool<GameObject> pool;

//    private Transform root;
//    private Transform Root
//    {
//        get
//        {
//            if (root == null)
//            {
//                GameObject go = new GameObject() { name = $"@{prefab.name}Pool" };
//                root = go.transform;
//            }

//            return root;
//        }
//    }

//    public Pool(GameObject prefab)
//    {
//        this.prefab = prefab;
//        pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
//    }

//    public void Push(GameObject go)
//    {
//        if (go.activeSelf)
//            pool.Release(go);
//    }

//    public GameObject Pop()
//    {
//        return pool.Get();
//    }

//    #region Funcs
//    private GameObject OnCreate()
//    {
//        GameObject go = GameObject.Instantiate(prefab);
//        go.transform.SetParent(Root);
//        go.name = prefab.name;
//        return go;
//    }

//    private void OnGet(GameObject go)
//    {
//        go.SetActive(true);
//    }

//    private void OnRelease(GameObject go)
//    {
//        go.SetActive(false);
//    }

//    private void OnDestroy(GameObject go)
//    {
//        GameObject.Destroy(go);
//    }
//    #endregion
//}
