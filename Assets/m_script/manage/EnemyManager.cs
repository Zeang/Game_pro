using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;
    public int maxEnemyNum = 5;


    private List<GameObject> Enemies = new List<GameObject>();

    void Start()
    {


        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    private void Update()
    {
        foreach(GameObject item in Enemies)
        {
            if (!item)
            {
                Enemies.Remove(item);
            }
        }
    }


    void Spawn()
    {

        {
            Debug.Log(playerHealth.currentHealth);
            if (playerHealth.isDead == true)
            {
                return;
            }

            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            if(Enemies.Count<maxEnemyNum)
            Enemies.Add(Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation));


        }

    }
}
