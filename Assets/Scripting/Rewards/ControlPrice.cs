using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ControlPrice : MonoBehaviour
{
    private DatabaseReference dbReference;
    public Text Price; 

    // Start is called before the first frame update
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(UpdatePrices());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator UpdatePrices()
    {
        dbReference.Child("RewardsConTrol").Child(this.gameObject.name).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                object value = task.Result.Value;
                Price.text = value != null ? value.ToString() : "N/A";
            }
            else
            {
                Debug.LogWarning("Không tìm thấy dữ liệu hoặc lỗi kết nối.");
                Price.text = "N/A";
            }
        });
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UpdatePrices());
    }
}
