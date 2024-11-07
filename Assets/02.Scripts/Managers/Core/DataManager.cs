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


public class DataManager
{
    private static KeyData keyData = null;
    private const string keyPassword = "sd5f7b69z2GFD2b1f5B2x3hJ567JDF12";
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
            Debug.Log(filePath);
            Debug.Log("ASDFFASDDFSAFSDASFDAFSDASFDAFSDASFD");
            string readData = await File.ReadAllTextAsync(filePath);

            if (!string.IsNullOrEmpty(readData))
            {
                if (PlayerPrefs.HasKey("SavedInitData"))
                {
                    string decrpytData = Decrypt(readData);
                    T data = JsonConvert.DeserializeObject<T>(decrpytData);
                    return data;
                }
                else
                {
                    PlayerPrefs.SetInt("SavedInitData", 1);
                    File.Delete(filePath);
                    T data = new T();
                    return data;
                }
            }
            else
            {
                T data = new T();
                return data;
            }
        }
        else
        {
            T data = new T();
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
    // AES 암호화 함수
    public static string Encrypt(string plainText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(keyPassword);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.GenerateIV(); // IV 생성
            byte[] iv = aes.IV;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                byte[] encryptedBytes = PerformCryptography(plainBytes, encryptor);

                // IV와 암호문을 결합
                byte[] combined = new byte[iv.Length + encryptedBytes.Length];
                Buffer.BlockCopy(iv, 0, combined, 0, iv.Length);
                Buffer.BlockCopy(encryptedBytes, 0, combined, iv.Length, encryptedBytes.Length);

                return Convert.ToBase64String(combined); // Base64 인코딩
            }
        }
    }

    // AES 복호화 함수
    public static string Decrypt(string encryptedText)
    {
        if (string.IsNullOrEmpty(encryptedText))
        {
            throw new ArgumentException("Encrypted text cannot be null or empty.");
        }

        byte[] fullCipher = Convert.FromBase64String(encryptedText);
        byte[] iv = new byte[16]; // IV는 16바이트
        byte[] cipherText = new byte[fullCipher.Length - iv.Length];

        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipherText, 0, cipherText.Length);

        byte[] keyBytes = Encoding.UTF8.GetBytes(keyPassword);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                byte[] decryptedBytes = PerformCryptography(cipherText, decryptor);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }

    // 암호화/복호화 수행
    private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
    {
        try
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
        catch (Exception ex)
        {
            Debug.LogError($"Cryptography error: {ex.Message}");
            throw; // 예외를 다시 던져서 호출자에게 전달
        }
    }
}
