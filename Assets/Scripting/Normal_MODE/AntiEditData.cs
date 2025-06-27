using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using static AuthenManager;
using System.Threading.Tasks;
using System.Collections;

public class AntiEditData : MonoBehaviour
{
    public Text addSL;
    public Text takeSL;
    public Text AllBags;
    private DatabaseReference dbReference;
    private AuthenManager authManager;
    private string userID;
    private string path;

    

    void Start()
    {
        Debug.Log(this.gameObject.name + "antiEditData");
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();
        }
        userID = authManager.UID.text;
        path = "users/" + userID;
    }

    public void AddValueToDB()
    {
        int inputValue = int.Parse(addSL.text);
        DatabaseReference userRef = dbReference.Child(path).Child("BagsNormal");

        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int currentValue = snapshot.Exists ? int.Parse(snapshot.Value.ToString()) : 0;
                currentValue += inputValue;
                userRef.SetValueAsync(currentValue);
                Debug.Log("Updated value in Firebase: " + currentValue);
            }
        });
    }
     public void testDebug() 
    {
        Debug.LogWarning("Da goi test Debug");
    }
    public void TakeValueFromDB()
    {
        DatabaseReference userRef = dbReference.Child(path).Child("BagsNormal");

        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    int storedValue = int.Parse(snapshot.Value.ToString());
                    takeSL.text = storedValue.ToString();
                    Debug.Log("Retrieved value: " + storedValue);
                }
            }
        });
    }

    public void TakeBags()
    {
        DatabaseReference userRef = dbReference.Child(path).Child("BagsNormal");

        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    int storedBagValue = int.Parse(snapshot.Value.ToString());
                    AllBags.text = storedBagValue.ToString();
                    Debug.Log("Retrieved bag value: " + storedBagValue);
                }
            }
        });
    }



    public void MinusBags()
    {
        DatabaseReference userRef = dbReference.Child(path).Child("BagsNormal");

        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int currentBagsValue = snapshot.Exists ? int.Parse(snapshot.Value.ToString()) : 0;
                if (currentBagsValue > 0)
                {
                    currentBagsValue--;
                    userRef.SetValueAsync(currentBagsValue);

                    Debug.Log("Updated bag value: " + currentBagsValue);
                }
            }
        });
    }
}
