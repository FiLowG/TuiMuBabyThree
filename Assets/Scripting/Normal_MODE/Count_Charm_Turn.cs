using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static AuthenManager;

public class Count_Charm_Turn : MonoBehaviour
{
    public Text count1, count2, count3, count4, count5, count6, count7, count8, count9, count10, count11;

    private int forecurrent1, forecurrent2, forecurrent3, forecurrent4, forecurrent5, forecurrent6, forecurrent7, forecurrent8, forecurrent9, forecurrent10, forecurrent11;
    private int aftercurrent1, aftercurrent2, aftercurrent3, aftercurrent4, aftercurrent5, aftercurrent6, aftercurrent7, aftercurrent8, aftercurrent9, aftercurrent10, aftercurrent11;

    public Text more1, more2, more3, more4, more5, more6, more7, more8, more9, more10, more11;

    private int display1, display2, display3, display4, display5, display6, display7, display8, display9, display10, display11;
    public Text added1, added2, added3, added4, added5, added6, added7, added8, added9, added10, added11;
    private int redun1, redun2, redun3, redun4, redun5, redun6, redun7, redun8, redun9, redun10, redun11;

    private int[] previousForecurrent = new int[11];
    public Text TotalMore;
    public GameObject panelResult;
    private int selectedButtonNumber;
    public Text All_Bags;
    private AntiEditData antiData;
    private SpawnBags spawnBags;
    private EndGame endGame;
    public GameObject StopTouch;
    private UpdateToDB updateDB;
    private DatabaseReference dbReference;
    private AuthenManager authManager;
    private string userID;
    private string path;
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();
        }

        userID = authManager.UID.text;

        path = "users/" + userID + "/" + "BagsNormal";
        updateDB = FindObjectOfType<UpdateToDB>();
        endGame = FindObjectOfType<EndGame>();

        spawnBags = FindObjectOfType<SpawnBags>();
        ResetCounts();
        SubmitToForeCurrent();
        antiData = FindObjectOfType<AntiEditData>();
    }
    void Update()
    {

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
                    All_Bags.text = storedBagValue.ToString();
                    Debug.Log("Retrieved bag value: " + storedBagValue);
                }
            }
        });
    }
    public void UpdateCoinRef(int SLCanThem)
    {
        dbReference.Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Lỗi khi lấy dữ liệu từ Firebase: " + task.Exception);
                return;
            }

            if (task.Result != null && task.Result.Value != null)
            {
                int SLHientai = int.Parse(task.Result.Value.ToString());
                int newValue = SLHientai + SLCanThem;

                dbReference.Child(path).SetValueAsync(newValue).ContinueWithOnMainThread(setTask =>
                {
                    if (setTask.IsCompleted)
                    {
                        All_Bags.text = newValue.ToString(); // cập nhật UI sau khi ghi xong
                        Debug.Log("Cập nhật thành công: " + newValue);
                    }
                    else
                    {
                        Debug.LogError("Ghi dữ liệu thất bại: " + setTask.Exception);
                    }
                });
            }
        });
        
    }

    public void CallEndGame()
    {
        Time.timeScale = 1;
        StartCoroutine(EndAllGame());

    }
    public void CheckEndCounts()
    {
        panelResult.SetActive(true);
        Debug.Log("settime từ CountCharmTurn");
        Time.timeScale = 0;
        SubmitToAfterCurrent();
        UpdateAddedValues();
        CompareToMore();
        SubmitToForeCurrent();
    }
    public void NextPlay()
    {
        panelResult.SetActive(false);
        Time.timeScale = 1;
        if (!isSpawning)
        {
            StartCoroutine(NewTurnSpawn());
        }
        
    }
    public IEnumerator EndAllGame()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        CheckBagsAndEndGame(); // hàm riêng để xử lý firebase async
    }

    private void CheckBagsAndEndGame()
    {
        DatabaseReference userRef = dbReference.Child(path);

        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    int Bags = int.Parse(snapshot.Value.ToString());
                    if (Bags < 1 && spawnBags.bagSpawned.Length == 0)
                    {

                        StopTouch.SetActive(false);
                        endGame.ProcessEndGame(null);
                    }
                }
            }
        });
    }

    public void SetSelectedButtonNumber(int number)
    {
        selectedButtonNumber = number;
        spawnBags.StartCoroutine(spawnBags.SpawnBagsAndLog());
    }
   
    private void ResetDisplayValues()
    {
        display1 = display2 = display3 = display4 = display5 = 0;
        display6 = display7 = display8 = display9 = display10 = display11 = 0;
    }
    private void CompareToMore()
    {
        ResetDisplayValues();
        CompareValues(ref aftercurrent1, ref forecurrent1, ref display1, ref previousForecurrent[0], ref redun1, 1, added1);
        CompareValues(ref aftercurrent2, ref forecurrent2, ref display2, ref previousForecurrent[1], ref redun2, 2, added2);
        CompareValues(ref aftercurrent3, ref forecurrent3, ref display3, ref previousForecurrent[2], ref redun3, 3, added3);
        CompareValues(ref aftercurrent4, ref forecurrent4, ref display4, ref previousForecurrent[3], ref redun4, 4, added4);
        CompareValues(ref aftercurrent5, ref forecurrent5, ref display5, ref previousForecurrent[4], ref redun5, 5, added5);
        CompareValues(ref aftercurrent6, ref forecurrent6, ref display6, ref previousForecurrent[5], ref redun6, 6, added6);
        CompareValues(ref aftercurrent7, ref forecurrent7, ref display7, ref previousForecurrent[6], ref redun7, 7, added7);
        CompareValues(ref aftercurrent8, ref forecurrent8, ref display8, ref previousForecurrent[7], ref redun8, 8, added8);
        CompareValues(ref aftercurrent9, ref forecurrent9, ref display9, ref previousForecurrent[8], ref redun9, 9, added9);
        CompareValues(ref aftercurrent10, ref forecurrent10, ref display10, ref previousForecurrent[9], ref redun10, 10, added10);
        CompareValues(ref aftercurrent11, ref forecurrent11, ref display11, ref previousForecurrent[10], ref redun11, 11, added11);
        DisplayToMore();
    }



    private void CompareValues(ref int after, ref int fore, ref int display, ref int previous, ref int redun, int index, Text addedText)
    {
        // Nếu chỉ mục hiện tại trùng với button được chọn, cộng trực tiếp vào more
        if (selectedButtonNumber == index)
        {
            display += (after - previous);
            previous = after;
            Debug.Log($"Index: {index}, After: {after}, Fore: {fore}, Display (More): {display}, Redun: {redun}, Added: {addedText.text}");
            return;
        }

        if (index == 11 && after >= 1 && after != 0 && int.Parse(addedText.text) != 0)
        {
            int allSPC = after - previous;
            display += allSPC * 3;  // mỗi charm được cộng gấp đôi (1 + 1 bonus)
            previous = after;

            Debug.Log($"Index: {index}, After: {after}, Fore: {fore}, Display (More): {display}, Redun: {redun}, Added: {addedText.text}");
            return;
        }

        // Xét giá trị aftercurrent để cập nhật more và redun
        int newValue = after - previous;
        previous = after;

        if (newValue >= 2)
        {
            int pairs = newValue / 2;  // Số cặp ghép đôi
            display += pairs;
            newValue -= pairs * 2;
        }

        // Xử lý giá trị lẻ còn lại
        redun += newValue;

        // Nếu redun có thể ghép cặp, thì chuyển đổi sang more
        if (redun >= 2)
        {
            int pairFromRedun = redun / 2;
            display += pairFromRedun;
            redun -= pairFromRedun * 2;
        }

        Debug.Log($"Index: {index}, After: {after}, Fore: {fore}, Display (More): {display}, Redun: {redun}, Added: {addedText.text}");
    }
    public bool CheckFirst()
    {
       if (int.Parse(count1.text) + int.Parse(count2.text) + int.Parse(count3.text) + 
           int.Parse(count4.text) + int.Parse(count5.text) + int.Parse(count6.text) +
           int.Parse(count7.text) + int.Parse(count8.text) + int.Parse(count9.text) +
           int.Parse(count10.text) + int.Parse(count11.text) == 0)
        {
            return false;
        }
        return true;
    }
    private void DisplayToMore()
    {
        more1.text = display1.ToString();
        more2.text = display2.ToString();
        more3.text = display3.ToString();
        more4.text = display4.ToString();
        more5.text = display5.ToString();
        more6.text = display6.ToString();
        more7.text = display7.ToString();
        more8.text = display8.ToString();
        more9.text = display9.ToString();
        more10.text = display10.ToString();
        more11.text = display11.ToString();
        int total = display1 + display2 + display3 + display4 + display5 + display6 + display7 + display8 + display9 + display10 + display11;
        TotalMore.text = total.ToString();
        UpdateCoinRef(total);
    }

    private void SubmitToForeCurrent()
    {
        forecurrent1 = int.Parse(count1.text);
        forecurrent2 = int.Parse(count2.text);
        forecurrent3 = int.Parse(count3.text);
        forecurrent4 = int.Parse(count4.text);
        forecurrent5 = int.Parse(count5.text);
        forecurrent6 = int.Parse(count6.text);
        forecurrent7 = int.Parse(count7.text);
        forecurrent8 = int.Parse(count8.text);
        forecurrent9 = int.Parse(count9.text);
        forecurrent10 = int.Parse(count10.text);
        forecurrent11 = int.Parse(count11.text);
    }

    private void SubmitToAfterCurrent()
    {
        aftercurrent1 = int.Parse(count1.text);
        aftercurrent2 = int.Parse(count2.text);
        aftercurrent3 = int.Parse(count3.text);
        aftercurrent4 = int.Parse(count4.text);
        aftercurrent5 = int.Parse(count5.text);
        aftercurrent6 = int.Parse(count6.text);
        aftercurrent7 = int.Parse(count7.text);
        aftercurrent8 = int.Parse(count8.text);
        aftercurrent9 = int.Parse(count9.text);
        aftercurrent10 = int.Parse(count10.text);
        aftercurrent11 = int.Parse(count11.text);
    }
    private void UpdateAddedValues()
    {
        added1.text = "+" + (aftercurrent1 - forecurrent1).ToString();
        added2.text = "+" + (aftercurrent2 - forecurrent2).ToString();
        added3.text = "+" + (aftercurrent3 - forecurrent3).ToString();
        added4.text = "+" + (aftercurrent4 - forecurrent4).ToString();
        added5.text = "+" + (aftercurrent5 - forecurrent5).ToString();
        added6.text = "+" + (aftercurrent6 - forecurrent6).ToString();
        added7.text = "+" + (aftercurrent7 - forecurrent7).ToString();
        added8.text = "+" + (aftercurrent8 - forecurrent8).ToString();
        added9.text = "+" + (aftercurrent9 - forecurrent9).ToString();
        added10.text = "+" + (aftercurrent10 - forecurrent10).ToString();
        added11.text = "+" + (aftercurrent11 - forecurrent11).ToString();
    }

    public int GetForecurrent(int index)
    {
        switch (index)
        {
            case 1: return forecurrent1;
            case 2: return forecurrent2;
            case 3: return forecurrent3;
            case 4: return forecurrent4;
            case 5: return forecurrent5;
            case 6: return forecurrent6;
            case 7: return forecurrent7;
            case 8: return forecurrent8;
            case 9: return forecurrent9;
            case 10: return forecurrent10;
            case 11: return forecurrent11;
            default: return 0;
        }
    }

    public int GetSelectNumber()
    {
        return selectedButtonNumber;
    }
    public void ResetCounts()
    {
        count1.text = "0"; count2.text = "0"; count3.text = "0"; count4.text = "0";
        count5.text = "0"; count6.text = "0"; count7.text = "0"; count8.text = "0";
        count9.text = "0"; count10.text = "0"; count11.text = "0";

        forecurrent1 = forecurrent2 = forecurrent3 = forecurrent4 = forecurrent5 = 0;
        forecurrent6 = forecurrent7 = forecurrent8 = forecurrent9 = forecurrent10 = forecurrent11 = 0;

        aftercurrent2 = aftercurrent3 = aftercurrent4 = aftercurrent5 = 0;
        aftercurrent6 = aftercurrent7 = aftercurrent8 = aftercurrent9 = aftercurrent10 = aftercurrent11 = 0;

        more1.text = "0"; more2.text = "0"; more3.text = "0"; more4.text = "0";
        more5.text = "0"; more6.text = "0"; more7.text = "0"; more8.text = "0";
        more9.text = "0"; more10.text = "0"; more11.text = "0";

        redun1 = redun2 = redun3 = redun4 = redun5 = 0;
        redun6 = redun7 = redun8 = redun9 = redun10 = redun11 = 0;

        display1 = display2 = display3 = display4 = display5 = 0;
        display6 = display7 = display8 = display9 = display10 = display11 = 0;
    }

    private static bool isSpawning = false; 

    IEnumerator NewTurnSpawn()
    {
        isSpawning = true; // Đánh dấu Coroutine đã được chạy
        yield return new WaitForSeconds(1);

        spawnBags.StartCoroutine(spawnBags.SpawnBagsAndLog());

        isSpawning = false; // Reset lại biến sau khi hoàn thành
    }
}
