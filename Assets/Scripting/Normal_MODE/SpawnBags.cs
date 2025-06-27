using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class SpawnBags : MonoBehaviour
{
    // Vị trí spawn
    public GameObject spawnPoint;

    // Các slot (không dùng mảng)
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject slot4;
    public GameObject slot5;
    public GameObject slot6;
    public GameObject slot7;
    public GameObject slot8;
    public GameObject slot9;
    public GameObject slot10;
    public GameObject slot11;
    public GameObject slot12;
    public GameObject slot13;
    public GameObject slot14;
    public GameObject slot15;
    public GameObject slot16;

    // Prefab túi
    public GameObject Type_Bags;
    public GameObject bagPrefab_Pink;
    public GameObject bagPrefab_Blue;

    // Số lượng túi
    public Text bagCountText;

    // Tốc độ di chuyển
    public float speed = 5f;

    // Private variables
    private int bagCount;
    private GameObject[] slots;

    // Mảng lưu trữ túi đã spawn
    internal GameObject[] bagSpawned;

    public GameObject AutoNote;

    internal BocBags BocBags;
    private Count_Charm_Turn countTurn;
    public GameObject StopOpen;
    private DatabaseReference dbReference;
    private AuthenManager authManager;
    private string userID;
    private string path;
    public AudioSource PutBagSFX;
    void Start()
    {
       
        if (countTurn == null)
        {
            countTurn = FindObjectOfType<Count_Charm_Turn>();
        }
        // Lấy số lượng túi từ Text
        if (!int.TryParse(bagCountText.text, out bagCount))
        {
            return;
        }
        if (BocBags == null)
        {
            BocBags = FindObjectOfType<BocBags>();
        }

        // Danh sách các slot
        slots = new GameObject[] { slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9, slot10, slot11, slot12, slot13, slot14, slot15, slot16 };

        // Khởi tạo mảng `bagSpawned` với kích thước 0
        bagSpawned = new GameObject[0];

        /*  // Bắt đầu tạo túi và gọi Debug.Log sau khi coroutine kết thúc
          StartCoroutine(SpawnBagsAndLog());*/
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();
        }

        userID = authManager.UID.text;
        path = "users/" + userID + "/" + "BagsNormal";

    }
    public void PhatAmThanh()
    {
        PutBagSFX.Play();
    }
    public IEnumerator SpawnBagsAndLog()
    {
        Debug.Log("Đã gọi SpawnAndLog");
        StopOpen.SetActive(true);

        int bagCount = 0;

        // Gọi Firebase và đợi lấy dữ liệu xong
        var task = dbReference.Child(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("Firebase GetValueAsync bị lỗi: " + task.Exception);
            yield break;
        }

        DataSnapshot snapshot = task.Result;
        if (snapshot.Exists)
        {
            bagCount = int.Parse(snapshot.Value.ToString());
        }
        else
        {
            Debug.LogWarning("Không tìm thấy dữ liệu túi.");
            yield break;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        // Gọi spawn sau khi đã lấy được bagCount
        yield return StartCoroutine(SpawnAndMoveBags(bagCount));

        if (AutoNote.activeSelf)
        {
            BocBags.OpenAllBags(); // Gọi mở tất cả túi nếu AutoNote bật
        }
    }

    private void Update()
    {
        Debug.Log(Time.timeScale);
    }
    int SpawnPerOne = 0;
    public IEnumerator SpawnAndMoveBags(int bagCount)
    {

        while (bagCount > 0)
        {
            GameObject[] activeSlots = GetActiveSlotsForBagCount(bagCount);

            foreach (var slot in activeSlots)
            {
                if (SpawnPerOne != 16)
                {
                    SpawnPerOne++;
                    if (bagCount <= 0) yield break;

                    GameObject bagPrefab = null;
                    if (Type_Bags.CompareTag("TuiMu_Pink"))
                        bagPrefab = bagPrefab_Pink;
                    else if (Type_Bags.CompareTag("TuiMu_Blue"))
                        bagPrefab = bagPrefab_Blue;

                    if (bagPrefab == null) continue;

                    GameObject bag = Instantiate(bagPrefab, spawnPoint.transform.position, Quaternion.identity);
                    System.Array.Resize(ref bagSpawned, bagSpawned.Length + 1);
                    bagSpawned[bagSpawned.Length - 1] = bag;

                    bagCount--;
                    bagCountText.text = bagCount.ToString();
                    Vector3 targetPosition = slot.transform.position;
                    while (Vector3.Distance(bag.transform.position, targetPosition) > 0.1f)
                    {
                        bag.transform.position = Vector3.MoveTowards(bag.transform.position, targetPosition, speed * Time.deltaTime);
                        yield return null;
                    }
                    PhatAmThanh();
                    if (SpawnPerOne == 16)
                    {
                        SpawnPerOne = 0;
                        StopOpen.SetActive(false);
                        yield break;
                    }
                    else if (bagCount == 0) 
                    {
                        SpawnPerOne = 0;
                        StopOpen.SetActive(false);
                        yield break;
                    }

                    yield return new WaitForSeconds(0.01f);
                }
            }
        }
    }


    private IEnumerator GetBagCountFromDB(System.Action<int> onBagCountReady)
    {
        var task = dbReference.Child(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("Firebase get failed: " + task.Exception);
            onBagCountReady(0); // fallback
            yield break;
        }

        DataSnapshot snapshot = task.Result;
        if (snapshot.Exists)
        {
            int count = int.Parse(snapshot.Value.ToString());
            onBagCountReady(count);
        }
        else
        {
            onBagCountReady(0);
        }
    }

    // Phương thức này sẽ trả về danh sách các slot cần dùng tùy theo số lượng túi còn lại
    private GameObject[] GetActiveSlotsForBagCount(int remainingBags)
    {
        if (remainingBags >= 16) return new GameObject[] { slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9, slot10, slot11, slot12, slot13, slot14, slot15, slot16 };
        if (remainingBags == 15) return new GameObject[] { slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9, slot10, slot11, slot12, slot13, slot14, slot15 };
        if (remainingBags == 14) return new GameObject[] { slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9, slot10, slot11, slot12, slot13, slot14 };
        if (remainingBags == 13) return new GameObject[] { slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9, slot10, slot11, slot12, slot13 };
        if (remainingBags == 12) return new GameObject[] { slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9, slot10, slot11, slot12 };
        if (remainingBags == 11) return new GameObject[] { slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9, slot10, slot11 };
        if (remainingBags == 10) return new GameObject[] { slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9, slot10 };
        if (remainingBags == 9) return new GameObject[] { slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9 };
        if (remainingBags == 8) return new GameObject[] { slot5, slot6, slot7, slot8, slot9, slot10, slot11, slot12 };
        if (remainingBags == 7) return new GameObject[] { slot5, slot6, slot7, slot8, slot9, slot10, slot11 };
        if (remainingBags == 6) return new GameObject[] { slot5, slot6, slot7, slot8, slot9, slot10 };
        if (remainingBags == 5) return new GameObject[] { slot5, slot6, slot7, slot8, slot9 };
        if (remainingBags == 4) return new GameObject[] { slot5, slot6, slot7, slot8 };
        if (remainingBags == 3) return new GameObject[] { slot6, slot7, slot11 };
        if (remainingBags == 2) return new GameObject[] { slot6, slot7 };
        if (remainingBags == 1) return new GameObject[] { slot6 };

        return new GameObject[0]; // Không còn túi nào
    }

    public void OnAutoNote()
    {
        if (!AutoNote.activeSelf)
        {
            AutoNote.SetActive(true);
        }
        else if (AutoNote.activeSelf)
        {
            AutoNote.SetActive(false);
        }
    }
}
