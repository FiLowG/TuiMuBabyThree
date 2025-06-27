using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using Firebase.Database;
using Firebase.Extensions;
using Unity.VisualScripting;

public class SubmitFormNhanQua : MonoBehaviour
{
    // Tham chiếu đến các trường nhập liệu và nút submit trong Unity
    public InputField HoVaTen;
    public InputField PhoneNumber;
    public InputField DiaChi;
    public InputField Email;
    public Text LoaiQua;
    public Text GiaDoi;
    public GameObject FormNhanQua;
    private DatabaseReference dbReference;
    private AuthenManager authManager;
    private string userID;
    private OnBuyFromDB OnBuy;
    // Bot Telegram
    
    public GameObject PopUp_Notice;
    public Text NoticeText;
    public Text NoticeText2;

    void Start()
    {
        OnBuy = FindObjectOfType<OnBuyFromDB>();
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        if (authManager == null)
        {
            authManager = FindObjectOfType<AuthenManager>();
        }

        userID = authManager.UID.text;
    }

    public void OnSubmitClicked()
    {
        string name = string.IsNullOrEmpty(HoVaTen.text) ? "none" : HoVaTen.text;
        string sdt = string.IsNullOrEmpty(PhoneNumber.text) ? "none" : PhoneNumber.text;
        string diaChi = string.IsNullOrEmpty(DiaChi.text) ? "none" : DiaChi.text;
        string email = Email != null && !string.IsNullOrEmpty(Email.text) ? Email.text : "none";
        string loaiQua = LoaiQua != null && !string.IsNullOrEmpty(LoaiQua.text) ? LoaiQua.text : "none";
        string howmuch = GiaDoi != null && !string.IsNullOrEmpty(GiaDoi.text) ? GiaDoi.text : "none";
        string fullMessage =
            $"Nhận được yêu cầu gửi quà mới!\n" +
            $"Người nhận: {name}\n" +
            $"SĐT: {sdt}\n" +
            $"Địa Chỉ: {diaChi}\n" +
            $"Email: {email}\n" +
            $"Loại Quà: {loaiQua}\n" +
            $"Giá Đổi: {howmuch}\n" +
            $"UserID: {userID}";

        StartCoroutine(SendMessageToTelegram(fullMessage));
    }

    IEnumerator SendMessageToTelegram(string message)
    {
        string url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={UnityWebRequest.EscapeURL(message)}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Message sent successfully!");
            OnBuy.MinusCoin();
            PopUp_Notice.SetActive(true);
            NoticeText.text = "Quy đổi thành công!";
            NoticeText2.text = "Hãy chú ý điện thoại của bạn để xác nhận nhé!";

        }
        else
        {
            Debug.Log("Error sending message: " + request.error);
        }
    }
}
