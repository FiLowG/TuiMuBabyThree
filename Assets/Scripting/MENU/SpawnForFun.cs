using System.Collections;
using UnityEngine;

public class SpawnForFun : MonoBehaviour
{
    public GameObject Bag; // Prefab túi được gán từ Inspector
    public float spawnRadius = 3f; // Bán kính vòng tròn

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnRandomBag();
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnRandomBag()
    {
        Vector2 randomPos = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPos.x, randomPos.y, 0f) + transform.position;

        GameObject spawnedBag = Instantiate(Bag, spawnPosition, Quaternion.identity);
        Destroy(spawnedBag, 7f);
    }
}
