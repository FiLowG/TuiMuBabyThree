using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public GameObject Endgame_Panel;
    private Count_Charm_Turn countCharmTurn;
    public Text Total_Charm;
    public Text Total_Diamond;

    // Khai báo 11 biến text playing
    public Text playing1, playing2, playing3, playing4, playing5, playing6, playing7, playing8, playing9, playing10, playing11;

    // Khai báo 11 biến text endgame
    public Text endgame1, endgame2, endgame3, endgame4, endgame5, endgame6, endgame7, endgame8, endgame9, endgame10, endgame11;

    void Start()
    {
        countCharmTurn = FindObjectOfType<Count_Charm_Turn>();
    }
     void OnApplicationQuit()
    {
        ProcessEndGame(null);
    }
    public Text AllBags;
     void Update()
    {
       
    }
    public void ProcessEndGame(GameObject OtherNeedOFF)
    { 
        Endgame_Panel.SetActive(true);

        int notWish = 0;
        int wish = 0;
        int special = 0;

        int selectedIndex = countCharmTurn.GetSelectNumber();

        for (int i = 1; i <= 11; i++)
        {
            if (i == 11)  // forecurrent11
            {
                special = countCharmTurn.GetForecurrent(i);
            }
            else if (i == selectedIndex)  // forecurrent được chọn
            {
                wish = countCharmTurn.GetForecurrent(i);
            }
            else
            {
                notWish += countCharmTurn.GetForecurrent(i);
            }
            if (OtherNeedOFF != null)
            {
                OtherNeedOFF.gameObject.SetActive(false);
            }
        }

        Total_Charm.text = (notWish + wish + special).ToString();
        Total_Diamond.text = (notWish + (wish * 2) + (special * 5)).ToString();
        TransferPlayingToEndgame();

        Debug.Log("Not Wish: " + notWish + ", Wish: " + wish + ", Special: " + special);
    }

    /// <summary>
    /// Hàm truyền giá trị từ playing vào endgame
    /// </summary>
    public void TransferPlayingToEndgame()
    {
        endgame1.text = playing1.text;
        endgame2.text = playing2.text;
        endgame3.text = playing3.text;
        endgame4.text = playing4.text;
        endgame5.text = playing5.text;
        endgame6.text = playing6.text;
        endgame7.text = playing7.text;
        endgame8.text = playing8.text;
        endgame9.text = playing9.text;
        endgame10.text = playing10.text;
        endgame11.text = playing11.text;

        Debug.Log("Transfer complete: Playing values copied to Endgame.");
    }
    public void settime()
    {
        Time.timeScale = 1;
    }
}
