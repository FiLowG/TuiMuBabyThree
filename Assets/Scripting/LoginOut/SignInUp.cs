using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;


public class AuthenManager : MonoBehaviour
{
    [Header("Input Fields")]
    public InputField Username;
    public InputField MatKhau;
    public InputField NhapLaiMatKhau;
    public InputField NameLogin;
    public InputField MatKhauLogin;

    [Header("UI Elements")]
    public GameObject NoticeObject;
    public Text NoticeText;
    public GameObject ContainUID;
    public Text UID;
    private FirebaseAuth auth;
    private DatabaseReference databaseRef;
    DateTime utcNow;
    public GameObject KeepScript;
    private string filePath;

    void Start()
    {
        if (this.gameObject.name != "LogOut")
        {
        SavePlayerData SavePlayerDat = FindObjectOfType<SavePlayerData>();

        filePath = Path.Combine(Application.persistentDataPath, "htc.json");

        utcNow = DateTime.UtcNow;

        DontDestroyOnLoad(ContainUID);
        DontDestroyOnLoad(KeepScript);


        auth = FirebaseAuth.DefaultInstance;
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;

        if (NoticeObject != null)
        {
            NoticeObject.SetActive(false);
        }

        SavePlayerDat.InLog();
        }
    }

    private void ShowNotice(string message)
    {
        NoticeText.text = message;
        NoticeObject.SetActive(true);
    }

    public void DangKy()
    {
        string username = Username.text.Trim();
        string matkhau = MatKhau.text.Trim();
        string nhapLaiMatKhau = NhapLaiMatKhau.text.Trim();

        // Kiểm tra điều kiện nhập liệu
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(matkhau) || string.IsNullOrEmpty(nhapLaiMatKhau))
        {
            ShowNotice("Không được bỏ trống thông tin!");
            return;
        }

        // Kiểm tra tên đăng nhập chỉ chứa ký tự chữ không dấu hoặc số
        if (!Regex.IsMatch(username, "^[a-zA-Z0-9]+$"))
        {
            ShowNotice("Tên đăng nhập chỉ được chứa chữ cái và số!");
            return;
        }

        // Kiểm tra độ dài mật khẩu
        if (matkhau.Length < 6)
        {
            ShowNotice("Mật khẩu phải có ít nhất 6 ký tự!");
            return;
        }

        // Kiểm tra mật khẩu trùng khớp
        if (matkhau != nhapLaiMatKhau)
        {
            ShowNotice("Mật khẩu không trùng nhau!");
            return;
        }

        string email = $"{username}@blindbags.com";

        // Kiểm tra trên Firebase Authentication
        auth.CreateUserWithEmailAndPasswordAsync(email, matkhau).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                ShowNotice("Tài khoản đã tồn tại!");
                return;
            }

            FirebaseUser user = auth.CurrentUser;
            if (user != null)
            {
                string uid = user.UserId;

                // Lưu thông tin người dùng vào Firebase Realtime Database
                User newUser = new User
                {
                    UID = uid,
                    LoginName = username,
                    Password = matkhau,
                    Name = "guest",
                    RareRate = utcNow.Minute,
                    BagsNormal = 0,
                    BagsBounty = 0,
                    DiamondQuantity = 0,
                    CoinQuantity = 0,
                    Bought = "0,0,0,0"
                };

                databaseRef.Child("users").Child(uid).SetRawJsonValueAsync(JsonUtility.ToJson(newUser)).ContinueWithOnMainThread(dbTask =>
                {
                    if (dbTask.IsCompleted)
                    {
                        ShowNotice("Đăng ký tài khoản thành công!");
                    }
                    else
                    {
                        ShowNotice("Lỗi khi lưu dữ liệu người dùng!");
                    }
                });
            }
        });
    }
    public void DangNhap()
    {
        string username = NameLogin.text.Trim();
        string matkhau = MatKhauLogin.text.Trim();

        // Kiểm tra điều kiện nhập liệu
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(matkhau))
        {
            ShowNotice("Không được bỏ trống thông tin!");
            return;
        }

        // Kiểm tra tên đăng nhập chỉ chứa ký tự chữ không dấu hoặc số
        if (!Regex.IsMatch(username, "^[a-zA-Z0-9]+$"))
        {
            ShowNotice("Tên đăng nhập chỉ được chứa chữ cái và số!");
            return;
        }

        string email = $"{username}@blindbags.com";

        auth.SignInWithEmailAndPasswordAsync(email, matkhau).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                ShowNotice("Tài khoản hoặc mật khẩu không chính xác!");
                return;
            }

            FirebaseUser user = auth.CurrentUser;
            if (user != null)
            {
                string uid = user.UserId; // Lấy UID của người dùng
                UID.text = uid; // Gán UID vào biến Text UID
                ShowNotice("Đăng nhập thành công!");
                UpdateLoginData();
                // Chuyển sang Scene MENU
                SceneManager.LoadScene("MENU");
            }
        });

    }
    public void LogOut()
    {
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length >= 2)
        {
            lines[0] = "";  // Xóa nội dung dòng đầu tiên
            lines[1] = "";  // Xóa nội dung dòng thứ hai
        }
        File.WriteAllLines(filePath, lines);
        Debug.Log("Cleared login data.");
        SceneManager.LoadScene("LoginOut");
    }

    public void UpdateLoginData()
    {
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length >= 2)
        {
            lines[0] = NameLogin.text;   // Cập nhật dòng đầu tiên
            lines[1] = MatKhauLogin.text; // Cập nhật dòng thứ hai
        }
        else
        {
            // Nếu file không đủ 2 dòng, đảm bảo ghi lại đúng số lượng dòng
            lines = new string[] { NameLogin.text, MatKhauLogin.text };
        }

        File.WriteAllLines(filePath, lines);
        Debug.Log("File updated with new login data.");
    }


    [System.Serializable]
    public class User
    {
        public string UID;
        public string LoginName;
        public string Password;
        public string Name;
        public int RareRate;
        public int BagsNormal;
        public int BagsBounty;
        public int DiamondQuantity;
        public int CoinQuantity;
        public string Bought;
    }
}
