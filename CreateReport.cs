using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class CreateReport : MonoBehaviour
{
    
    private static readonly List<string> rList = new List<string>();

    [SerializeField] private TMP_Dropdown[] dropdowns1;

    public void CreateReport1(List<string> rList)
    {
        // Создание нового Excel-файла
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        var fileName = $"Student{dropdowns1[0].options[dropdowns1[0].value].text}activityreport{Guid.NewGuid()}.xlsx";

        string savePath = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            savePath = FileDownloader.GetDownloadFolderPath();
        }
        else
        {
            savePath = Application.persistentDataPath;
        }

        var newFile = new FileInfo(Path.Combine(savePath, fileName));
        using var package = new ExcelPackage(newFile);
        var worksheet = package.Workbook.Worksheets.Add("Отчёт");
        string[] data = { "Фамилия", "Имя", "Отчество", "Телефон", "Почта", "Группа", "Количество дел", "Количество активных дел", "Количество закрытых дел", "Количество консультаций" };
        for (int i = 0; i < data.Length; i++)
        {
            worksheet.Cells[1, i + 1].Value = data[i];
        }
        for (int i = 0; i < rList.Count; i++)
        {
            if (Int64.TryParse(rList[i], out Int64 result))
            {
                worksheet.Cells[2, i + 1].Value = result;
            }
            else
            {
                worksheet.Cells[2, i + 1].Value = rList[i];
            }
        }
        int columnCount = worksheet.Dimension.End.Column;
        for (int i = 1; i <= columnCount; i++)
        {
            worksheet.Column(i).AutoFit();
        }
        try
        {
            package.Save();
        }
        catch (IOException ex)
        {
            Debug.LogError($"Error saving Excel file: {ex}");
        }
    }
    public void r1()
    {
        var selectedOption = dropdowns1[0].options[dropdowns1[0].value];
        string[] strings = { selectedOption.text };
        StartCoroutine(WaitForCoroutine(strings));
    }
    private IEnumerator WaitForCoroutine(string[] strings)
    {
        yield return StartCoroutine(CRUD.IUpdateStrToTable(CRUD.urlReport1, strings, rList)); // Ожидание завершения первой корутины
        if (rList == null)
        {
            Debug.LogError("Error getting data from server");
            yield break;
        }
        CreateReport1(rList);
    }
}

