using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using System;
using Firebase.Database;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider extensionProvider;

    [Header("IAP Settings")]
    [Tooltip("Chọn index sản phẩm trong danh sách")]
    public int selectedProductIndex = 0;

    [Tooltip("Danh sách các Product ID")]
    public string[] availableProductIDs = new string[] {
        "100tuidacbiet",
        "199tuithuong",
        "25tuidacbiet",
        "50tuidacbiet"
    };

    private string selectedProductID; // ID sản phẩm được chọn

    [Header("Firebase")]
    private DatabaseReference dbReference;
    private AuthenManager authManager;
    private string userID;

    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        authManager = FindObjectOfType<AuthenManager>();
        userID = authManager.UID.text;

        if (selectedProductIndex < availableProductIDs.Length)
        {
            selectedProductID = availableProductIDs[selectedProductIndex];
        }
        else
        {
            Debug.LogError("Invalid product index selected!");
            return;
        }

        if (storeController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (var id in availableProductIDs)
        {
            builder.AddProduct(id, ProductType.Consumable);
        }

        UnityPurchasing.Initialize(this as IDetailedStoreListener, builder);
    }

    public void BuyProduct()
    {
        if (storeController != null)
        {
            Product product = storeController.products.WithID(selectedProductID);
            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("Product not available for purchase");
            }
        }
        else
        {
            Debug.Log("Store not initialized.");
        }
    }

    public void UpdateBags(int addMore, string priority1, string priority2, int addMore2)
    {
        string path = "users/" + userID + "/" + priority1;

        dbReference.Child(path).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int currentValue = snapshot.Exists ? Convert.ToInt32(snapshot.Value) : 0;
                int newValue = currentValue + addMore;

                dbReference.Child(path).SetValueAsync(newValue);
                Debug.Log($"Updated {priority1} to {newValue}");
            }
        });

        if (!string.IsNullOrEmpty(priority2))
        {
            string path2 = "users/" + userID + "/" + priority2;

            dbReference.Child(path2).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    int currentValue2 = snapshot.Exists ? Convert.ToInt32(snapshot.Value) : 0;
                    int newValue2 = currentValue2 + addMore2;

                    dbReference.Child(path2).SetValueAsync(newValue2);
                    Debug.Log($"Updated {priority2} to {newValue2}");
                }
            });
        }
    }


    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        extensionProvider = extensions;
        Debug.Log("IAP Initialized successfully.");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError("IAP Init Failed: " + error + " - " + message);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string purchasedId = args.purchasedProduct.definition.id;

        switch (purchasedId)
        {
            case "100tuidacbiet":
                UpdateBags(100, "BagsBounty", "BagsNormal", 100);
                Debug.Log("Purchase Successful: 100 Túi Đặc Biệt");
                break;
            case "199tuithuong":
                UpdateBags(199, "BagsNormal", "BagsBounty", 50);
                Debug.Log("Purchase Successful: 199 Túi Thường");
                break;
            case "25tuidacbiet":
                UpdateBags(25, "BagsBounty", "BagsNormal", 25);
                Debug.Log("Purchase Successful: 25 Túi Đặc Biệt");
                break;
            case "50tuidacbiet":
                UpdateBags(50, "BagsBounty", "BagsNormal", 50);
                Debug.Log("Purchase Successful: 50 Túi Đặc Biệt");
                break;
            default:
                Debug.LogWarning("Purchase unrecognized: " + purchasedId);
                break;
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.LogWarning($"Purchase failed: {product.definition.id}, Reason: {failureDescription.reason}, Message: {failureDescription.message}");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("IAP Init failed (deprecated method). Error: " + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError("Purchase failed (deprecated method). Reason: " + failureReason);
    }
}
