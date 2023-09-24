using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private int slotAmount;
    [SerializeField] private int goldAmount;
    private string fileName = "/options1.ghdv";
    private Dictionary<string, int> _gameData = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            FillDictionaryAndSaveToFolder();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadDictionaryFromFolder(_gameData);
        }
    }
    
    private void FillDictionaryAndSaveToFolder()
    {
        _gameData.Add(nameof(health), health);
        _gameData.Add(nameof(damage), damage);
        _gameData.Add(nameof(slotAmount), slotAmount);
        _gameData.Add(nameof(goldAmount), goldAmount);
        
        SaveDictionaryToFile(_gameData, Application.persistentDataPath + fileName);
    }
    
    private void LoadDictionaryFromFolder(Dictionary<string, int> dict)
    {
        dict = LoadDictionaryFromFile(fileName);
        print(" ");
        print("Load files...");
        foreach (var VARIABLE in _gameData)
        {
            print(VARIABLE);
        }
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
}
