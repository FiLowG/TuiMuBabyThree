using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class NhanThemTui : MonoBehaviour
{
    public GameObject SelectWish;
    public GameObject NhanThemObject;
    private DatabaseReference dbReference;
    private AuthenManager authManager;
    private string userID;
    private string path;
    public Text All_Bags;

    // Start is called before the first frame update
     void Awake()
    {
        
    }
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        if (authManager == null)
            authManager = FindObjectOfType<AuthenManager>();

        userID = authManager.UID.text;
        path = "users/" + userID + "/" + "BagsNormal";

     
}

   

    // Update is called once per frame
    void Update()
    {
     
    }
  


    public void NhanThemDone()
    {
        SelectWish.SetActive(true);
        NhanThemObject.SetActive(false);
    }
}
