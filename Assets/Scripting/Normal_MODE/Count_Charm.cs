using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Count_Charm : MonoBehaviour
{
    public Text count;
    private SpawnBags spawnBags;
    public GameObject SpawnedList;
    private AntiEditData antiData;
    private Count_Charm_Turn countTurn;

    private static bool isSpawning = false; // Biến static để kiểm soát Coroutine

    void Start()
    {
        if (antiData == null)
        {
            antiData = FindObjectOfType<AntiEditData>();
        }

        if (countTurn == null)
        {
            countTurn = FindObjectOfType<Count_Charm_Turn>();
        }

        if (spawnBags == null)
        {
            spawnBags = FindObjectOfType<SpawnBags>();
        }
        count.text = "0"; // Khởi tạo giá trị ban đầu của text là "0"
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(this.gameObject.tag))
        {
            int currentCount = int.Parse(count.text);
            currentCount++;
            count.text = currentCount.ToString();
            Destroy(other.gameObject);

            if (!isSpawning) 
            {
                StartCoroutine(NewTurnSpawn());
            }
        }
    }

    IEnumerator NewTurnSpawn()
    {
        isSpawning = true; // Đánh dấu Coroutine đã được chạy
        yield return new WaitForSeconds(1);

        if (SpawnedList.transform.childCount < 1)
        {
            bool check = countTurn.CheckFirst();

            if (check)
            {
                countTurn.CheckEndCounts();
                antiData.TakeBags();

            }
        }

        isSpawning = false; // Reset lại biến sau khi hoàn thành
    }
}
