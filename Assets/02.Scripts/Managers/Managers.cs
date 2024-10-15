using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    private static Managers Instance { get { Init(); return s_instance; } }

    private PoolManager poolManager = new PoolManager();
	//private DataManager dataManager = new DataManager();
	private GameManager inGameSystem = new GameManager();
	private UiManager inGameUiController = new UiManager();

	public static PoolManager Pool { get { return Instance?.poolManager; } }
	//public static DataManager DataManager { get { return Instance?.dataManager; } }
	public static GameManager InGameSystem 
	{ 
		get 
		{ 
			return Instance?.inGameSystem;
        }

        set
        {
			if(Instance.inGameSystem == null)
            {
				Instance.inGameSystem = value;
            }
        }
	}
	public static UiManager InGameUiController
	{
		get
		{
			return Instance?.inGameUiController;
		}

		set
		{
			if (Instance.inGameUiController == null)
			{
				Instance.inGameUiController = value;
			}
		}
	}

	public static void Init()
	{
		if (s_instance == null)
		{
			GameObject go = GameObject.Find("@Managers");
			if (go == null)
			{
				go = new GameObject { name = "@Managers" };
				go.AddComponent<Managers>();
			}

			DontDestroyOnLoad(go);

			// √ ±‚»≠
			s_instance = go.GetComponent<Managers>();
		}
	}
}
