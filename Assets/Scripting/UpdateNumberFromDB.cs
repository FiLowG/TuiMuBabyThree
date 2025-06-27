using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateNumberFromDB : MonoBehaviour
{
    private DatabaseReference dbReference;
    private AuthenManager authManager;
    private string userID;
    private string path;
    public string Priority;
    public Text TextCount;
    private void Awake()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();
        }

        userID = authManager.UID.text;

        path = "users/" + userID + "/" + Priority;
        StartCoroutine(WaitUpdate());
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateCoin()
    {
        Debug.Log(TextCount.gameObject.name);

        dbReference.Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                TextCount.text = task.Result.Value.ToString();
            }
        });
        TextCount.gameObject.GetComponent<Text>().enabled = false;
        TextCount.gameObject.GetComponent<Text>().enabled = true;

    }
    IEnumerator WaitUpdate()
    {
        UpdateCoin();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(WaitUpdate());
    }
}
