using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using static AuthenManager;

public class BocBags_Online : MonoBehaviour
{
    private SpawnBags_Online spawnBags; // Reference to SpawnBags
    public Animator animator; // Animator to play the animation
    public string animationName; // The name of the animation to play

    // Declare 3 GameObjects and Type_Charm variable
    public GameObject Labubu;
    public GameObject Turtle;
    public GameObject Lububu;
    public GameObject Type_Charm;

    // Variables to control scale and speed
    public float scaleIncrease = 0.05f; // Scale increase speed
    public float maxScaleIncrease = 1.5f; // Maximum scale increase (total scale)
    public float Speed_Up;
    public GameObject Charm_Spawned;
    private RandomCharm randomCharm;
    private AuthenManager authManager;
    public Text All_Bags;
    private string path;


    private Dictionary<GameObject, GameObject[]> charmPool = new Dictionary<GameObject, GameObject[]>();

    // Points declaration
    public Transform Point_1;
    public Transform Point_2;
    public Transform Point_3;
    public Transform Point_4;
    public Transform Point_5;
    public Transform Point_6;
    public Transform Point_7;
    public Transform Point_8;
    public Transform Point_9;
    public Transform Point_10;
    public Transform Point_Special;
    private DatabaseReference databaseRef;
    private string UID;
    public GameObject CheckFirst;
    // Variables to control movement speed
    public float charmMoveSpeed = 5f;
    public Button BocAll;
    private int valueRare;
    private int bagsBounty;
    private BagsFromFirebase TakeBagFB;
    public GameObject StopTouch;
    public GameObject Panel_NhanThemTui;
    void Awake()
    {
        
        if (TakeBagFB == null)
        {
            TakeBagFB = FindObjectOfType<BagsFromFirebase>(); // Find the SpawnBags object in the scene
        }

        if (spawnBags == null)
        {
            spawnBags = FindObjectOfType<SpawnBags_Online>(); // Find the SpawnBags object in the scene
        }

        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();

        }

        if (randomCharm == null)
        {
            randomCharm = FindObjectOfType<RandomCharm>(); // Find the SpawnBags object in the scene
        }
       
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        UID = authManager.UID.text;
        path = "users/" + UID + "/" + "BagsBounty";
    }
   

    void Start()
    { 
        // Scan child objects and add to pool
        ScanChildren(Labubu);
        ScanChildren(Turtle);
        ScanChildren(Lububu);
    }

    void Update()
    {
        valueRare = randomCharm.valuerate;
        if (!BocAll.gameObject.activeSelf)
        {
            if (spawnBags.bagSpawned.Length == 0)
            {
                BocAll.gameObject.SetActive(true);
            }
        }
    }

    private void ScanChildren(GameObject parent)
    {
        if (parent == null) return;

        GameObject[] childArray = new GameObject[parent.transform.childCount];
        int index = 0;
        foreach (Transform child in parent.transform)
        {
            childArray[index++] = child.gameObject;
        }
        charmPool[parent] = childArray;
    }

    public void AddAndRemoveCharm()
    {
        if (Charm_Spawned == null) return;

        foreach (Transform charm in Charm_Spawned.transform)
        {
            string charmTag = charm.tag;
            Transform targetPoint = GetTargetPoint(charmTag);

            if (targetPoint != null)
            {
                StartCoroutine(MoveToTarget(charm.gameObject, targetPoint.position));
            }
        }
    }

    private Transform GetTargetPoint(string charmTag)
    {
        // Map charm tags to points
        return charmTag switch
        {
            "Charm_1" => Point_1,
            "Charm_2" => Point_2,
            "Charm_3" => Point_3,
            "Charm_4" => Point_4,
            "Charm_5" => Point_5,
            "Charm_6" => Point_6,
            "Charm_7" => Point_7,
            "Charm_8" => Point_8,
            "Charm_9" => Point_9,
            "Charm_10" => Point_10,
            "Charm_Special" => Point_Special,
            _ => null,
        };
    }

    IEnumerator MoveToTarget(GameObject charm, Vector3 targetPosition)
    {
            yield return new WaitForSeconds(1.5f);
            while (charm != null && Vector3.Distance(charm.transform.position, targetPosition) > 0.1f)
            {

                charm.transform.position = Vector3.MoveTowards(charm.transform.position, targetPosition, charmMoveSpeed * Time.deltaTime);
                yield return null;
            }
    }

    IEnumerator WaitCharmAndDestroy()
    {
        yield return new WaitForSeconds(0.2f);
        SpawnCharm();
        yield return new WaitForSeconds(1.5f);

        if (spawnBags.bagSpawned != null)
        {
            spawnBags.bagSpawned = spawnBags.bagSpawned.Where(bag => bag != this.gameObject).ToArray();
        }
        Destroy(this.gameObject);
        if (spawnBags.bagSpawned.Length == 0)
        {
            BocAll.onClick.Invoke();
            StartCoroutine(EndAllGame());

        }

    }
    IEnumerator ScaleAndMoveCharm(GameObject charm)
    {
        if (charm == null) yield break;

        Vector3 targetScale = charm.transform.localScale + Vector3.one * maxScaleIncrease;

        while (charm.transform.localScale.x < targetScale.x)
        {
            charm.transform.localScale += Vector3.one * scaleIncrease * Time.deltaTime;
            charm.transform.position += Vector3.up * Speed_Up * Time.deltaTime;

            yield return null;
        }

    }

    public void OpenBags()
    {
        CheckFirst.tag = "Boced";

        if (animator != null && !string.IsNullOrEmpty(animationName))
        {
            animator.Play(animationName); // Play animation
            this.gameObject.tag = "Boced"; // Change tag for GameObject
        }
        Transform buttonBocBags = transform.Find("Button_BocBags");
        if (buttonBocBags != null)
        {
            Destroy(buttonBocBags.gameObject);
        } 

            StartCoroutine(WaitCharmAndDestroy());
    }

    private async void SpawnCharm()
    {
        if (Type_Charm == null)
        {
            return;
        }
        GameObject charmRandom = await randomCharm.GetRandomCharm();

        if (charmRandom == null)
        {

            foreach (var pair in charmPool)
            {
                if (Type_Charm.tag == pair.Key.tag)
                {
                    GameObject[] charms = pair.Value;
                    if (charms.Length == 0)
                    {
                        continue;
                    }

                    int randomIndex = Random.Range(0, charms.Length);
                    GameObject selectedCharm = charms[randomIndex];

                    // Instantiate and set as child of Charm_Spawned
                    GameObject spawnedCharm = Instantiate(selectedCharm, transform.position, Quaternion.identity);
                    if (Charm_Spawned != null)
                    {
                        spawnedCharm.transform.SetParent(Charm_Spawned.transform); // Set as child of Charm_Spawned
                    }

                    StartCoroutine(ScaleAndMoveCharm(spawnedCharm));
                }
            }
        }
        if (charmRandom != null)
        {
            GameObject spawnedCharm = Instantiate(charmRandom, transform.position, Quaternion.identity);
            if (Charm_Spawned != null)
            {

                spawnedCharm.transform.SetParent(Charm_Spawned.transform); // Set as child of Charm_Spawned
            }

            StartCoroutine(ScaleAndMoveCharm(spawnedCharm));
        }
        if (TakeBagFB.bagsBounty >= 0)
        {
            TakeBagFB.SetBagsBounty(--TakeBagFB.bagsBounty);
        };
        SetRareRate();
    }

    private async void SetRareRate()
    {
        if (valueRare > 0)
        {
            await databaseRef.Child("users").Child(UID).Child("RareRate").SetValueAsync(--valueRare);
            Debug.Log("RareRate đã được cập nhật: " + valueRare);
        }
    }
    public void OpenAllBags()
    {
        BocAll.gameObject.SetActive(false);
        StartCoroutine(OpenAllBagSP());
    }
    public IEnumerator OpenAllBagSP()
    {
        CheckFirst.tag = "Boced";
        BocAll.interactable = false;

        if (spawnBags.bagSpawned != null)
        {
            foreach (var bag in spawnBags.bagSpawned)
            {
                if (bag != null)
                {
                    BocBags_Online bagScript = bag.GetComponent<BocBags_Online>();
                    if (bagScript != null)
                    {
                        bagScript.OpenBagsAll();
                        yield return new WaitForSeconds(0.08f);
                        SetRareRate();
                    }
                }

            }
           
        }
        yield return new WaitForSeconds(0.5f);

        AddAndRemoveCharm();
        yield return new WaitForSeconds(0.5f);
        BocAll.interactable = true;
        StartCoroutine(EndAllGame());

    }
    public void OpenBagsAll()
    {
        CheckFirst.tag = "Boced";

        if (animator != null && !string.IsNullOrEmpty(animationName))
        {
            animator.Play(animationName); // Play animation
            this.gameObject.tag = "Boced"; // Change tag for GameObject
        }

        Transform buttonBocBags = transform.Find("Button_BocBags");
        if (buttonBocBags != null)
        {
            Destroy(buttonBocBags.gameObject);
        }

        StartCoroutine(WaitCharmAndDestroy());
    }
    public IEnumerator EndAllGame()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        CheckBagsAndEndGame(); // hàm riêng để xử lý firebase async
    }

    private void CheckBagsAndEndGame()
    {
        DatabaseReference userRef = databaseRef.Child(path);

        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    int Bags = int.Parse(snapshot.Value.ToString());
                    if (Bags < 1)
                    {

                        StopTouch.SetActive(false);
                        Panel_NhanThemTui.SetActive(true);      
                    }
                }
            }
        });
    }

}