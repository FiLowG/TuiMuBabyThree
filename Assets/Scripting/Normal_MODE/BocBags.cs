using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BocBags : MonoBehaviour
{
    private SpawnBags spawnBags; // Reference to SpawnBags
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
    private Count_Charm_Turn countTurn;
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

    private AntiEditData antiData;

    // Variables to control movement speed
    public float charmMoveSpeed = 5f;
    public Button BocAll;
    private ControlSPC controlSPC;
    void Start()
    {
        antiData = FindObjectOfType<AntiEditData>();
        controlSPC = FindObjectOfType<ControlSPC>();
        if (spawnBags == null)
        {
            spawnBags = FindObjectOfType<SpawnBags>(); // Find the SpawnBags object in the scene
        }
        if (countTurn == null)
        {
            countTurn = FindObjectOfType<Count_Charm_Turn>(); // Find the SpawnBags object in the scene
        }


        // Scan child objects and add to pool
        ScanChildren(Labubu);
        ScanChildren(Turtle);
        ScanChildren(Lububu);

        

    }

    void Update()
    {
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

    public void OpenBags()
    {
        bool AllBoced = true;

        if (animator != null && !string.IsNullOrEmpty(animationName))
        {
            animator.Play(animationName); // Play animation
            this.gameObject.tag = "Boced"; // Change tag for GameObject
            antiData.MinusBags();
            foreach (GameObject bags in spawnBags.bagSpawned)
            {
                if (bags != null)
                {
                    if (bags.tag != "Boced")
                    {
                        AllBoced = false;
                    }
                }
            }
        }

        Transform buttonBocBags = transform.Find("Button_BocBags");
        if (buttonBocBags != null)
        {
            Destroy(buttonBocBags.gameObject);
        }

        if (AllBoced)
        {
        }

        StartCoroutine(WaitCharmAndDestroy());
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
        }

    }
    
    private void SpawnCharm()
    {
        if (Type_Charm == null)
        {
            return;
        }

        foreach (var pair in charmPool)
        {
            
            if (Type_Charm.tag == pair.Key.tag)
            {
                GameObject[] charms = pair.Value;
                if (charms.Length == 0)
                {
                    continue;
                }
             
                    
                    GameObject selectedCharm = controlSPC.RandomControl(charms);

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
    public void OpenAllBags()
    {
        BocAll.gameObject.SetActive(false);
        StartCoroutine(OpenAllBagsSP());
    }
    IEnumerator OpenAllBagsSP()
    {
        BocAll.interactable = false;

        if (spawnBags.bagSpawned != null)
        {
            foreach (var bag in spawnBags.bagSpawned)
            {
                Debug.Log("Da goi OpenAllBags");
                antiData.MinusBags();
                if (bag != null)
                {
                    BocBags bagScript = bag.GetComponent<BocBags>();
                    if (bagScript != null)
                    {
                        bagScript.OpenBags();
                        yield return new WaitForSeconds(0.08f);
                    }
                }
            }

        }
        yield return new WaitForSeconds(0.5f);

        AddAndRemoveCharm();
        yield return new WaitForSeconds(0.5f);
        BocAll.interactable = true;

    }
   
}
