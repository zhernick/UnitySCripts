using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Newtonsoft.Json;
using UnityEngine;
public class SaveSystemEncrypt : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private int slotAmount;
    [SerializeField] private int goldAmount;
    private string fileName = "/options.ghdv";
    private Dictionary<string, int> _gameData = new();

    
    private readonly byte[] Key = new byte[32] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0xFE, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0xFE, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10 };
    private readonly byte[] IV = new byte[16] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0xFE, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10 };

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) FillDictionaryAndSaveToFolder();
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadDictionary(_gameData);
        }
    }

    private void LoadDictionary(Dictionary<string, int> dict)
    {
        dict = LoadDictionary(fileName);
        print(" ");
        print("Load encrypted files...");
        foreach (var VARIABLE in _gameData)
        {
            print(VARIABLE);
        }
    }

    private void FillDictionaryAndSaveToFolder()
    {
        _gameData.Add(nameof(health), health);
        _gameData.Add(nameof(damage), damage);
        _gameData.Add(nameof(slotAmount), slotAmount);
        _gameData.Add(nameof(goldAmount), goldAmount);
        
        SaveDictionaryWithCrypt(_gameData, Application.persistentDataPath + fileName);
    }
    
    private void SaveDictionaryToFile(Dictionary<string, int> dictionary, string filePath)
    {
        // Преобразуем Dictionary в JSON-строку
        string json = JsonConvert.SerializeObject(dictionary);

        // Записываем JSON-строку в файл
        File.WriteAllText(filePath, json);
    }
    
    private Dictionary<string, int> LoadDictionaryFromFile(string fileName)
    {
        // Считываю файл
        string json = File.ReadAllText(Application.persistentDataPath + fileName);
        // Добавляю в Словарь полученные данные из файла
        var dictionary = JsonUtility.FromJson<Dictionary<string, int>>(json);
        return dictionary;
    }
    private void SaveDictionaryWithCrypt(Dictionary<string, int> dictionary, string fileName)
    {
        // Convert the dictionary to a byte array
        byte[] data = ObjectToByteArray(dictionary);

        // Encrypt the data
        byte[] encryptedData = Encrypt(data);

        // Write the encrypted data to a file
        File.WriteAllBytes(fileName, encryptedData);
    }
    
    private byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    private byte[] Encrypt(byte[] data)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }
    }
    private byte[] Decrypt(byte[] data)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }
    }
    
    private Dictionary<string, int> LoadDictionary(string fileName)
    {
        // Read the encrypted data from the file
        byte[] encryptedData = File.ReadAllBytes(Application.persistentDataPath + fileName);

        // Decrypt the data
        byte[] data = Decrypt(encryptedData);

        // Convert the byte array back to a dictionary
        return (Dictionary<string, int>)ByteArrayToObject(data);
    }

    private object ByteArrayToObject(byte[] arrBytes)
    {
        using (MemoryStream memStream = new MemoryStream())
        {
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = (object)binForm.Deserialize(memStream);
            return obj;
        }
    }
}