using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;

public class ExcelToJsonConverter
{
    [MenuItem("Tools/ParseExcel %#K")]
    public static void ParseExcelDataToJson()
    {
        ParseExcelDataToJson<FoodDataInfoBundle, FoodDataInfo>("BeverageData");
        ParseExcelDataToJson<FoodDataInfoBundle, FoodDataInfo>("BakeryData");
        ParseExcelDataToJson<FoodDataInfoBundle, FoodDataInfo>("DesertData");
        Debug.Log("DataTransformer Completed");
    }

    #region Helpers
    private static void ParseExcelDataToJson<Loader, LoaderData>(string filename) where Loader : new() where LoaderData : new()
    {
        Loader loader = new Loader();
        FieldInfo field = loader.GetType().GetField("foodDataList");
        field.SetValue(loader, ParseExcelDataToList<LoaderData>(filename));

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/03.Resources/Data/JsonData/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    private static List<LoaderData> ParseExcelDataToList<LoaderData>(string filename) where LoaderData : new()
    {
        List<LoaderData> loaderDatas = new List<LoaderData>();

        string[] lines = File.ReadAllText($"{Application.dataPath}/03.Resources/Data/ExcelData/{filename}.csv").Split("\n");

        // 첫 번째 행은 필드명이므로, 두 번째 행부터 데이터 처리
        for (int l = 1; l < lines.Length; l++)
        {
            string[] row = lines[l].Replace("\r", "").Split(',');
            if (row.Length == 0 || string.IsNullOrEmpty(row[0]))
                continue;

            LoaderData loaderData = new LoaderData();

            // BeverageData 필드에 데이터를 매핑
            System.Reflection.FieldInfo[] fields = typeof(LoaderData).GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int f = 0; f < fields.Length; f++)
            {
                FieldInfo field = fields[f];
                Type type = field.FieldType;
                string cellValue = row[f];

                if (string.IsNullOrEmpty(cellValue))
                {
                    Debug.LogWarning($"Empty value at row {l}, column {f}.");
                    continue;
                }

                // 데이터 변환
                if (type.IsGenericType)
                {
                    object value = ConvertList(cellValue, type);
                    field.SetValue(loaderData, value);
                }
                else
                {
                    object value = ConvertValue(cellValue, field.FieldType);
                    if (value != null)
                    {
                        Debug.Log($"Assigning {cellValue} to field {field.Name}");
                        field.SetValue(loaderData, value);
                    }
                }
            }

            loaderDatas.Add(loaderData);
        }

        return loaderDatas;
    }

    private static object ConvertValue(string value, Type type)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        TypeConverter converter = TypeDescriptor.GetConverter(type);
        return converter.ConvertFromString(value);
    }

    private static object ConvertList(string value, Type type)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        Type valueType = type.GetGenericArguments()[0];
        Type genericListType = typeof(List<>).MakeGenericType(valueType);
        var genericList = Activator.CreateInstance(genericListType) as IList;

        var list = value.Split('&').Select(x => ConvertValue(x, valueType)).ToList();

        foreach (var item in list)
            genericList.Add(item);

        return genericList;
    }
    #endregion
}