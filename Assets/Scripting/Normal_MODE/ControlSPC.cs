using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSPC : MonoBehaviour
{
    private int spawnCount = 0;
    private int untilSPC = 0;
    private BocBags bocbags;
    private DatabaseReference dbReference;
    private AuthenManager authManager;
    private string userID;
    private string path;
    private int From;
    private int To;
    // Start is called before the first frame update
    async void Start()
    {
        bocbags = FindObjectOfType<BocBags>();
        
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();
        }

        userID = authManager.UID.text;
        path = "DataControl/NormalSPC/";

        await LoadFromAndToAsync();
        untilSPC = UnityEngine.Random.Range(From, To);
        Debug.LogWarning("untilSPC: " + untilSPC);
    }

    public async Task LoadFromAndToAsync()
    {
        var fromSnapshot = await dbReference.Child(path).Child("From").GetValueAsync();
        if (fromSnapshot.Exists && int.TryParse(fromSnapshot.Value.ToString(), out int fromValue))
        {
            From = fromValue;
        }

        var toSnapshot = await dbReference.Child(path).Child("To").GetValueAsync();
        if (toSnapshot.Exists && int.TryParse(toSnapshot.Value.ToString(), out int toValue))
        {
            To = toValue;
        }

        Debug.Log($"From: {From}, To: {To}");
    }

    void Update()
    {

    }

    public GameObject RandomControl(GameObject[] charms)
    {

        spawnCount++;
        Debug.LogWarning("SpawnCount: " +spawnCount + "/ UntilSPC: " + untilSPC);
        if (untilSPC == spawnCount)
        {
            GameObject selectedCharm = charms[10];

            spawnCount = 0;
            untilSPC = UnityEngine.Random.Range(From, To);
            return selectedCharm;

        }
        else
        {
            int randomIndex = UnityEngine.Random.Range(0, charms.Length - 1);
            GameObject selectedCharm = charms[randomIndex];
            return selectedCharm;
        }
    }
}