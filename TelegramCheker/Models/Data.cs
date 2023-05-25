﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TelegramCheker.Models;
    internal class Data
    {
    private string _paramsFileName = @"Data/Config.json";
    private string _indicatorsFileName = @"Data/Indicators.json";
    public Config ProgramConfig { get; set; }
    public SpamBotIndicators Indicators { get; set; }

    public Data()
    {
        DeserializeData(_paramsFileName);
        if (ProgramConfig == null)
        {
            ProgramConfig = new Config();
            SerializeData(_paramsFileName);
        }

        DeserializeIndicatorsData(_indicatorsFileName);
        if (Indicators == null)
        {
            Indicators = new SpamBotIndicators();
            SerializeData(_indicatorsFileName);
        }
    }


    // сериализация данных в формате JSON - реализация NewtonSoft
    private void SerializeData(string fileName)
    {
        // Формирование строки JSON
        string jsonData = JsonConvert.SerializeObject(Indicators, Newtonsoft.Json.Formatting.Indented);
        // Запись объекта в JSON-файл
        if (!File.Exists(fileName)) { using (File.Create(fileName)) ; Thread.Sleep(1000); }

        File.WriteAllText(fileName, jsonData, Encoding.UTF8);
    } // SerializeDataNs

    // десериализация данных из формата JSON - реализация NewtinSoft
    private void DeserializeData(string fileName)
    {
        // прочитать в строку из текстового файла
        if (!File.Exists(fileName)) { using (File.Create(fileName)) ; Thread.Sleep(1000); }
        string jsonData = File.ReadAllText(fileName, Encoding.UTF8);

        // парсинг в коллекцию из JSON-строки
        // ! в конце строки означает подавление предупреждения компилятора 
        // о возможном NullReference
        ProgramConfig = JsonConvert.DeserializeObject<Config>(jsonData)!;
    } // DeserializeDataNs


    //временно костыльно :)

    // сериализация данных в формате JSON - реализация NewtonSoft
    private void SerializeIndicatorsData(string fileName)
    {
        // Формирование строки JSON
        string jsonData = JsonConvert.SerializeObject(Indicators, Newtonsoft.Json.Formatting.Indented);
        // Запись объекта в JSON-файл
        if (!File.Exists(fileName)) { using (File.Create(fileName)) ; Thread.Sleep(1000); }

        File.WriteAllText(fileName, jsonData, Encoding.UTF8);
    } // SerializeDataNs

    // десериализация данных из формата JSON - реализация NewtinSoft
    private void DeserializeIndicatorsData(string fileName)
    {
        // прочитать в строку из текстового файла
        if (!File.Exists(fileName)) { using (File.Create(fileName)) ; Thread.Sleep(1000); }
        string jsonData = File.ReadAllText(fileName, Encoding.UTF8);

        // парсинг в коллекцию из JSON-строки
        // ! в конце строки означает подавление предупреждения компилятора 
        // о возможном NullReference
        Indicators = JsonConvert.DeserializeObject<SpamBotIndicators>(jsonData)!;
    } // DeserializeDataNs
}

