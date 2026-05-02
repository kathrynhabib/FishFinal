using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour
{

	public GameObject fishPrefab;
	[SerializeField] float tankSizeX = 5, tankSizeY = 5, tankSizeZ = 5;
	public static float tankSizeXS = 5, tankSizeYS = 5, tankSizeZS = 5;
	//public static Vector3 tankSize = new Vector3(tankSizeX, tankSizeY, tankSizeZ);
	[SerializeField] int numFish = 10;
	public static int numFishS;
	public static GameObject[] allFish;// = new GameObject[numFishS];

	public static Vector3 goalPos = Vector3.zero;

	// Start is called before the first frame update

	private void Awake()
	{
		allFish = new GameObject[numFish];
	}
	void Start()
	{
		numFishS = numFish;

		tankSizeXS = tankSizeX;
		tankSizeYS = tankSizeY;
		tankSizeZS = tankSizeZ;


		for (int i = 0; i < numFish; i++)
		{
			Vector3 pos = new Vector3(Random.Range(-tankSizeXS, tankSizeXS),
									  Random.Range(-tankSizeYS, tankSizeYS),
									  Random.Range(-tankSizeZS, tankSizeZS));

			allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
			Debug.Log("Spawned fish at: " + pos);


			//allFish[i].AddComponent<Flock>();
		}
	}

	// Update is called once per frame
	void Update()
	{
		tankSizeXS = tankSizeX;
		tankSizeYS = tankSizeY;
		tankSizeZS = tankSizeZ;


		if (Random.Range(0, 1000) < 50)
		{
			goalPos = new Vector3(Random.Range(-tankSizeXS, tankSizeXS),
								  Random.Range(-tankSizeYS, tankSizeYS),
								  Random.Range(-tankSizeZS, tankSizeZS));
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(tankSizeX * 2, tankSizeY * 2, tankSizeZ * 2));
	}
}

