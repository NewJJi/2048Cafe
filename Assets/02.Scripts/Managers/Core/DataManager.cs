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

[CreateAssetMenu(fileName = "KeyData", menuName = "ScriptableObjects/KeyData", order = 1)]
public class KeyData : ScriptableObject
{
    public string key;
}

public class DataManager
{
    private static KeyData keyData = null;
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

            //await SaveDataAsync<T>(data, name);
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
            //SaveData<T>(data, name);
            return data;
        }
    }
    // AES ��ȣȭ �Լ�
    public static string Encrypt(string plainText)
    {
        if(keyData == null)
        {
            keyData = Resources.Load<KeyData>("KeyData");
        }

        byte[] keyBytes = Encoding.UTF8.GetBytes(keyData.key);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = new byte[16]; // IV�� �⺻������ 0���� ����
            aes.Mode = CipherMode.CBC;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                byte[] encryptedBytes = PerformCryptography(plainBytes, encryptor);
                return Convert.ToBase64String(encryptedBytes);
            }
        }
    }

    // AES ��ȣȭ �Լ�
    public static string Decrypt(string encryptedText)
    {
        if (keyData == null)
        {
            keyData = Resources.Load<KeyData>("KeyData");
        }

        byte[] keyBytes = Encoding.UTF8.GetBytes(keyData.key);
        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = new byte[16]; // ��ȣȭ�� ���� ���� IV ���
            aes.Mode = CipherMode.CBC;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                byte[] decryptedBytes = PerformCryptography(encryptedBytes, decryptor);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }

    // ��ȣȭ/��ȣȭ ����
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
}
