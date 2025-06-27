using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBags_Online : MonoBehaviour
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
    public GameObject bagPrefab_Special;

    // Số lượng túi
    public Text bagCountText;

    // Tốc độ di chuyển
    public float speed = 5f;

    // Private variables
    private GameObject[] slots;

    // Mảng lưu trữ túi đã spawn
    internal GameObject[] bagSpawned;

    public GameObject AutoNote;

    internal BocBags_Online BocBags;
    static Count_Charm_Turn countTurn;
    public GameObject CheckFirst;
    private BagsFromFirebase TakeBagFB;
    public GameObject StopOpen;
    public AudioSource PutBagSFX;

    void Start()
    {
        if (TakeBagFB == null)
        {
            TakeBagFB = FindObjectOfType<BagsFromFirebase>(); // Find the SpawnBags object in the scene
        }

        if (BocBags == null)
        {
            BocBags = FindObjectOfType<BocBags_Online>();
        }

        // Danh sách các slot
        slots = new GameObject[] { slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9, slot10, slot11, slot12, slot13, slot14, slot15, slot16 };

        // Khởi tạo mảng `bagSpawned` với kích thước 0
        bagSpawned = new GameObject[0];

        // Bắt đầu tạo túi và gọi Debug.Log sau khi coroutine kết thúc
        StartCoroutine(SpawnBagsAndLog());
    }
    void Update()
    {

    }
    public void PhatAmThanh()
    {
        PutBagSFX.Play();
    }
    public IEnumerator SpawnBagsAndLog()
    {
        // Gọi coroutine SpawnAndMoveBags
        yield return StartCoroutine(SpawnAndMoveBags());
        if (AutoNote.activeSelf)
        {
            BocBags.OpenAllBags();
        }

    }

    int SpawnPerOne = 0;

    public IEnumerator SpawnAndMoveBags()
    {
        StopOpen.SetActive(true);

        if (CheckFirst.tag == "BocYet")
        {
            yield return new WaitForSecondsRealtime(0.5f);
        }


        while (int.Parse(bagCountText.text) > 0)
        {

            // Xác định số lượng slot cần spawn dựa trên số túi còn lại
            GameObject[] activeSlots = GetActiveSlotsForBagCount(int.Parse(bagCountText.text));

            // Tạo và di chuyển túi
            foreach (var slot in activeSlots)
            {

                if (SpawnPerOne != 16)
                {
                    SpawnPerOne += 1;
                    if (int.Parse(bagCountText.text) <= 0) yield break;

                    // Kiểm tra tag của Type_Bags và chọn prefab tương ứng
                    GameObject bagPrefab = null;

                    if (Type_Bags.CompareTag("TuiMu_Special"))
                    {
                        bagPrefab = bagPrefab_Special;
                    }
                    // Nếu không có prefab phù hợp, tiếp tục với vòng lặp
                    if (bagPrefab == null) continue;

                    // Tạo túi tại vị trí spawn
                    GameObject bag = Instantiate(bagPrefab, spawnPoint.transform.position, Quaternion.identity);

                    // Thêm túi vào mảng `bagSpawned`
                    System.Array.Resize(ref bagSpawned, bagSpawned.Length + 1);
                    bagSpawned[bagSpawned.Length - 1] = bag;
                    int bagcount = int.Parse(bagCountText.text);
                    bagCountText.text = (--bagcount).ToString();
                    Vector3 targetPosition = slot.transform.position;
                    while (Vector3.Distance(bag.transform.position, targetPosition) > 0.1f)
                    {
                        bag.transform.position = Vector3.MoveTowards(bag.transform.position, targetPosition, speed * Time.deltaTime);
                        yield return null; // Đợi frame tiếp theo
                    }
                    PhatAmThanh();
                    if (SpawnPerOne == 16)
                    {
                        SpawnPerOne = 0;
                        yield break;
                    }

                    // Chờ một chút trước khi tạo túi tiếp theo
                    yield return new WaitForSeconds(0.01f);

                    if (bagSpawned.Length == activeSlots.Length - 1)
                    {
                        StopOpen.SetActive(false);
                    }
                }
            }

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
