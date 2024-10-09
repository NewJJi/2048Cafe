using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;
using System;
using Newtonsoft.Json;
using System.IO;
using static Define;
using System.Threading.Tasks;

public interface ILoader
{

}

public class DataManager : MonoBehaviour
{
    #region SingleTon
    private static DataManager instance;
    public static DataManager Instance { get { Init(); return instance; } }
    public static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("DataManager");
            if (go == null)
            {
                go = new GameObject { name = "DataManager" };
                go.AddComponent<DataManager>();
            }

            DontDestroyOnLoad(go);

            // 초기화
            instance = go.GetComponent<DataManager>();
        }
    }
    #endregion

    #region Data
    public SaveData saveData;

    public List<Sprite> beverageSpriteList;
    public List<Sprite> bakerySpriteList;
    public List<Sprite> desertSpriteList;

    public TextAsset beverageInfoData;
    public TextAsset bakeryInfoData;
    public TextAsset desertInfoData;

    public FoodDataInfoBundle beverageInfoBundle;
    public FoodDataInfoBundle bakeryInfoBundle;
    public FoodDataInfoBundle desertInfoBundle;

    public int[] expandUpgradeCost;

    [ContextMenu("Food Data Init")]
    public void InitFoodData()
    {
        beverageInfoBundle = JsonConvert.DeserializeObject<FoodDataInfoBundle>(beverageInfoData.text);
        bakeryInfoBundle = JsonConvert.DeserializeObject<FoodDataInfoBundle>(bakeryInfoData.text);
        desertInfoBundle = JsonConvert.DeserializeObject<FoodDataInfoBundle>(desertInfoData.text);
    }

    public async Task InitSaveData()
    {
        saveData = await LoadDataAsync<SaveData>("SaveData");
    }
    public void InitSaveData2()
    {
        saveData = LoadData<SaveData>("SaveData");
    }


    [ContextMenu("Reset Save Data")]
    public void ResetSaveData()
    {
        saveData = new SaveData();
    }

    public FoodDataInfo GetFoodInfo(ERecipeType eRecipeType, int index)
    {
        FoodDataInfo foodDataInfo = new FoodDataInfo();
        switch (eRecipeType)
        {
            case ERecipeType.Beverage:
                return beverageInfoBundle.beverageDataList[index];
            case ERecipeType.Bakery:
                return bakeryInfoBundle.beverageDataList[index];
            case ERecipeType.Desert:
                return desertInfoBundle.beverageDataList[index];
            default:
                Debug.LogError("Exception");
                return null;
        }
    }

    public Sprite GetFoodSprite(ERecipeType eRecipeType, int index)
    {
        switch (eRecipeType)
        {
            case ERecipeType.Beverage:
                return beverageSpriteList[index];
            case ERecipeType.Bakery:
                return bakerySpriteList[index];
            case ERecipeType.Desert:
                return desertSpriteList[index];
            default:
                Debug.LogError("Exception");
                return null;
        }
    }

    #endregion

    #region Save And Load
    private static string key = "Your32ByteKeyHere123456789012345"; // 16, 24, 32 characters for AES
    string savePath = "";
    public void SaveData<T>(T data,string name) where T : ILoader
    {
        string saveData = JsonConvert.SerializeObject(data);
        string encryptData = Encrypt(saveData);

        if (string.IsNullOrEmpty(savePath))
        {
            savePath = Path.Combine(Application.persistentDataPath,"Data");
        }

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        Debug.Log($"{savePath}/{name}");
        File.WriteAllText($"{savePath}/{name}.json", encryptData);
    }
    public async Task SaveDataAsync<T>(T data, string name) where T : ILoader
    {
        string saveData = JsonConvert.SerializeObject(data);
        string encryptData = Encrypt(saveData);

        if (string.IsNullOrEmpty(savePath))
        {
            savePath = Path.Combine(Application.persistentDataPath, "Data");
        }

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        Debug.Log($"{savePath}/{name}");
        await File.WriteAllTextAsync($"{savePath}/{name}.json", encryptData);
    }
    public async Task<T> LoadDataAsync<T>(string path) where T : ILoader, new()
    {
        if (string.IsNullOrEmpty(savePath))
        {
            savePath = Path.Combine(Application.persistentDataPath, "Data");
        }

        string filePath = $"{savePath}/{path}.json";

        if (File.Exists(filePath))
        {
            string readData = await File.ReadAllTextAsync(filePath);
            string decrpytData = Decrypt(readData);
            T data = JsonConvert.DeserializeObject<T>(decrpytData);
            return data;
        }
        else
        {
            T data = new T();

            await SaveDataAsync<T>(data, name);

            return data;
        }
    }

    public T LoadData<T>(string path) where T : ILoader, new()
    {
        if (string.IsNullOrEmpty(savePath))
        {
            savePath = Path.Combine(Application.persistentDataPath, "Data");
        }

        string filePath = $"{savePath}/{path}.json";

        if (File.Exists(filePath))
        {
            string readData = File.ReadAllText(filePath);
            string decrpytData = Decrypt(readData);
            T data = JsonConvert.DeserializeObject<T>(decrpytData);
            return data;
        }
        else
        {
            T data = new T();
            SaveData<T>(data, name);
            return data;
        }
    }
    // AES 암호화 함수
    public static string Encrypt(string plainText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = new byte[16]; // IV는 기본적으로 0으로 설정
            aes.Mode = CipherMode.CBC;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                byte[] encryptedBytes = PerformCryptography(plainBytes, encryptor);
                return Convert.ToBase64String(encryptedBytes);
            }
        }
    }

    // AES 복호화 함수
    public static string Decrypt(string encryptedText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = new byte[16]; // 복호화할 때도 같은 IV 사용
            aes.Mode = CipherMode.CBC;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                byte[] decryptedBytes = PerformCryptography(encryptedBytes, decryptor);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }

    // 암호화/복호화 수행
    private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
    {
        using (var memoryStream = new System.IO.MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }
    }
    #endregion
}
