using NUnit.Framework;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

public class CRUD : MonoBehaviour
{
    #region Ссылки

    public const string urlGetAllIdList = "http://89.148.237.22/legalclinic/F/GetIdList.php";

    const string urlDel = "http://89.148.237.22/legalclinic/F/Delete.php";

    const string urlSearch = "http://89.148.237.22/legalclinic/F/SearchId.php";

    const string urlSearchAll = "http://89.148.237.22/legalclinic/F/SearchAll.php";

    const string urlToExcel = "http://89.148.237.22/legalclinic/F/ToExcel.php";

    public const string urlSelectAll = "http://89.148.237.22/legalclinic/Best/ListAllRowsByTableName.php";//вывод всех строк таблицы по названию таблицы

    const string urlGoToUpdate = "http://89.148.237.22/legalclinic/F/GoToUpdate.php";

    const string urlRealUpdateForAll = "http://89.148.237.22/legalclinic/F/NewRealUpdateForAll.php";

    const string urlCreateNewStr = "http://89.148.237.22/legalclinic/F/CreateNew.php";

    const string urlCreateNewLI = "http://89.148.237.22/legalclinic/Legal/CreateNewLI.php";


    public const string fileURLRegulationLegalClinic = "https://drive.google.com/u/0/uc?id=1z86npAKF5U0LBl2BFjzVqihrshHZX0ze&export=download";  // URL файла для загрузки положения о деятельности юридической клиники НИУ ВШЭ
    public const string fileURLProcedureForCarryingOutActivities = "https://drive.google.com/u/0/uc?id=1iG7qB0IbQowMQCPM5ehyLaSaaW_x_jRi&export=download";  // URL файла для загрузки порядка осуществления деятельности юридической клиники НИУ ВШЭ
    public const string fileURLRegulationPersonalData = "https://drive.google.com/u/0/uc?id=1XpYyBLyYvS35wMtJfQj6bfhv01mryli9&export=download";  // URL файла для загрузки положения об обработке персональных данных

    //public const string fileURLRegulationLegalClinic = "http://89.148.237.22/legalclinic/Doc/6.18.1-01_230621-52.docx";  // URL файла для загрузки положения о деятельности юридической клиники НИУ ВШЭ
    //public const string fileURLProcedureForCarryingOutActivities = "http://89.148.237.22/legalclinic/Doc/6.18.1-01_230621-53.docx";  // URL файла для загрузки порядка осуществления деятельности юридической клиники НИУ ВШЭ
    //public const string fileURLRegulationPersonalData = "http://89.148.237.22/legalclinic/Doc/RegulationOnTheProcessingOfPersonalDataHSE.docx";  // URL файла для загрузки положения об обработке персональных данных

    public const string urlReport1 = "http://89.148.237.22/legalclinic/F/Report1.php";//создание первого отчёта

    public const string urlAuthorization = "http://89.148.237.22/legalclinic/Authorization/Login.php";//ссылка на скрипт для авторизации

    public static string urllAddingaNewTrusteeandStatement = "http://89.148.237.22/legalclinic/Legal/AddingaNewTrusteeandStatement.php";
    
    public static string urllFindIdCustomer = "http://89.148.237.22/legalclinic/Legal/FindIdCustomer.php";

    #endregion

    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            string base64String = Convert.ToBase64String(hashBytes);
            base64String = base64String.Replace('/', '+').Replace('!', '$');
            return base64String;
        }
    }

    public static bool IsNumber(string input)
    {
        int number;
        return int.TryParse(input, out number);
    }

    public static string RemoveSpaces(string input)
    {
        return new string(input.Where(c => !Char.IsWhiteSpace(c)).ToArray());
    }

    public static bool IsValidDate(string inputDate)
    {
        bool isDateValid = DateTime.TryParseExact(inputDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
        if (isDateValid)
        {
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            isDateValid = date.Day >= 1 && date.Day <= daysInMonth;
        }
        return isDateValid;
    }

    public static bool CompareTimes(string time1, string time2, string time3, string time4)
    {
        int hours1, minutes1, hours2, minutes2, totalMinutes1, totalMinutes2;

        if (!int.TryParse(time1, out hours1) || !int.TryParse(time2, out minutes1) || !int.TryParse(time3, out hours2) || !int.TryParse(time4, out minutes2))
        {
            return false;
        }

        totalMinutes1 = hours1 * 60 + minutes1;

        totalMinutes2 = hours2 * 60 + minutes2;

        if (totalMinutes2 <= totalMinutes1)
        {
            return false;
        }
        return true;
    }

    public static void FillDay(List<string> List, int y, int u)
    {
        for (int i = y; i < u; i++)
        {
            if (i < 10)
            {
                List.Add("0" + i);
            }
            else
            {
                List.Add("" + i);
            }
        }
    }

    #region NewCreate
    public void FCreateNew(string[] strings, string[] sqlParams , List<string> listFillAllDropdown, TMP_Dropdown[] dropdowns, List<string> listCreateNewTable, List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObject)
    {
        StartCoroutine(WaitForCoroutineCreateNew(strings, sqlParams , listFillAllDropdown, dropdowns, listCreateNewTable, createdPrefabs, textAllcount, spawnObject));
    }
    IEnumerator WaitForCoroutineCreateNew(string[] strings,  string[] sqlParams , List<string> listFillAllDropdown, TMP_Dropdown[] dropdowns, List<string> listCreateNewTable, List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObject)
    {

        if (sqlParams [0] == "legalissues")
        {

            yield return StartCoroutine(IAddNewValuesToTable(urlCreateNewLI, sqlParams [0], strings)); // Ожидание завершения первой корутины
            MyFunctionNewDropdownAndNewTable(sqlParams , listFillAllDropdown, dropdowns, listCreateNewTable, createdPrefabs, textAllcount, spawnObject);
        }
        else
        {
            yield return StartCoroutine(IAddNewValuesToTable(urlCreateNewStr, sqlParams [0], strings)); // Ожидание завершения первой корутины
            MyFunctionNewDropdownAndNewTable(sqlParams , listFillAllDropdown, dropdowns, listCreateNewTable, createdPrefabs, textAllcount, spawnObject);
        }
        
    }
    #endregion

    #region NewUpdate

    public void FGoToUpdate(List<string> listUpdate, TMP_Dropdown[] dropdowns, string TableName, TextMeshProUGUI IdTextUpdate, Action onComplete)
    {
        string[] y = new string[2];
        y[0] = dropdowns[0].options[dropdowns[0].value].text;
        y[1] = TableName;
        StartCoroutine(WaitForCoroutineGoToUpdate(y, listUpdate, IdTextUpdate, onComplete));
    }

    IEnumerator WaitForCoroutineGoToUpdate(string[] y, List<string> listUpdate, TextMeshProUGUI IdTextUpdate, Action onComplete)
    {
        yield return StartCoroutine(IUpdateStrToTable(urlGoToUpdate, y, listUpdate));
        IdTextUpdate.text = y[0];
        onComplete?.Invoke();
    }

    public void FNewRealUpdate(string[] strings,string[] sqlParams , List<string> listFillAllDropdown, TMP_Dropdown[] dropdowns, List<string> listCreateNewTable, List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObjects)
    {
        StartCoroutine(WaitForCoroutineNewRealUpdate(strings,sqlParams , listFillAllDropdown, dropdowns, listCreateNewTable, createdPrefabs, textAllcount, spawnObjects));
    }
    IEnumerator WaitForCoroutineNewRealUpdate(string[] strings,  string[] sqlParams , List<string> listFillAllDropdown, TMP_Dropdown[] dropdowns, List<string> listCreateNewTable, List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObjects)
    {
        yield return StartCoroutine(IAddNewValuesToTable(urlRealUpdateForAll, sqlParams [0], strings));
        MyFunctionNewDropdownAndNewTable(sqlParams , listFillAllDropdown, dropdowns, listCreateNewTable, createdPrefabs, textAllcount, spawnObjects);
    }

    #endregion

    #region DownloadAllToExcel
    public void FDownloadToExcel(string TableName, List<string> columnName)
    {

        string[] tableName = new string[1];
        tableName[0] = TableName;
        List<string> list = new List<string>();
        StartCoroutine(WaitForCoroutineDownload(tableName, list, columnName));
    }
    IEnumerator WaitForCoroutineDownload(string[] tableName, List<string> list, List<string> columnName)
    {
        
        yield return StartCoroutine(IUpdateStrToTable(urlToExcel, tableName, list));
        CreateExcelFile(tableName[0], list, columnName);

    }
    private void CreateExcelFile(string tableName, List<string> rowsData, List<string> columnName)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var fileName = tableName + Guid.NewGuid().ToString() + ".xlsx";
        var folderName = "Reports";

        string savePath = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            savePath = FileDownloader.GetDownloadFolderPath();
        }
        else
        {
            savePath = Application.persistentDataPath;
        }
        

        var folderPath = Path.Combine(savePath, folderName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var newFile = new FileInfo(Path.Combine(folderPath, fileName));

        using (var package = new ExcelPackage(newFile))
        {
            var worksheet = package.Workbook.Worksheets.Add(tableName);
            for (int i = 0; i < columnName.Count; i++)
            {
                worksheet.Cells[1, i + 1].Value = columnName[i];
            }
            int numberstr = 2;
            int j = 1;
            for (int i = 0; i < rowsData.Count; i++)
            {
                if (rowsData[i].Contains("<br>"))
                {
                    numberstr++;
                    rowsData[i] = rowsData[i].Replace("<br>", "");
                    j = 1;
                }
                if (Int64.TryParse(rowsData[i], out Int64 result))
                {
                    worksheet.Cells[numberstr, j].Value = result;
                    j++;
                }
                else
                {
                    worksheet.Cells[numberstr, j].Value = rowsData[i];
                    j++;
                }
            }
            int columnCount = worksheet.Dimension.End.Column;
            for (int i = 1; i <= columnCount; i++)
            {
                worksheet.Column(i).AutoFit();
            }
            package.Save();
        }
    }
    #region DownloadOneToExcel
    public void FDownloadOneToExcel(string TableName, List<string> columnName, List<GameObject> createdPrefabs)
    {
        List<string> list = new List<string>();
        string u = "";
        foreach (GameObject OneTableRow in createdPrefabs)
        {
            TextMeshProUGUI[] textMeshProArray = OneTableRow.GetComponentsInChildren<TextMeshProUGUI>();
            if (textMeshProArray != null)
            {
                for (int j = 0; j < textMeshProArray.Length; j++)
                {
                    u += textMeshProArray[j].text + "/";
                }
                u += "<br>";
            }
        }
        string[] ss = u.Split("/");
        list.AddRange(ss);
        list.RemoveAt(list.Count - 1);
        CreateExcelFile(TableName, list, columnName);
    }
    #endregion
    #endregion

    #region Copy
    public void FCopy(List<string> list, TMP_Dropdown dropdown, string TableName)
    {
        list.Clear();
        string[] y = new string[2];
        y[0] = dropdown.options[dropdown.value].text;
        y[1] = TableName;
        StartCoroutine(WaitForCoroutineCopy(y,  list));
    }
    IEnumerator WaitForCoroutineCopy(string[] y,  List<string> list)
    {
        yield return StartCoroutine(IUpdateStrToTable(urlGoToUpdate, y, list));

        string StringBuilder = "";

        for (int i = 0; i < list.Count; i++)
        {
            StringBuilder += list[i] + " ";
        }
        UniClipboard.SetText(StringBuilder);
    }
    #endregion

    #region Delete
    public void FDelete( TMP_InputField InputSSSearch,string[] sqlParams , List<string> listFillAllDropdown, TMP_Dropdown[] dropdowns, List<string> listCreateNewTable, List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObjects)
    {
        string[] strings = new string[3];
        strings[0] = dropdowns[1].options[dropdowns[1].value].text;
        strings[1] = sqlParams [0];
        strings[2] = sqlParams [1];
        InputSSSearch.text = "";
        StartCoroutine(WaitForCoroutineDelete( strings, sqlParams , listFillAllDropdown, dropdowns, listCreateNewTable, createdPrefabs, textAllcount, spawnObjects));
    }
    IEnumerator WaitForCoroutineDelete(string[] strings, string[] sqlParams , List<string> listFillAllDropdown, TMP_Dropdown[] dropdowns,List<string> listCreateNewTable, List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObjects)
    {
        yield return StartCoroutine(IAddNewStrToTable(urlDel, strings)); // Ожидание завершения первой корутины
        MyFunctionNewDropdownAndNewTable(sqlParams , listFillAllDropdown, dropdowns, listCreateNewTable, createdPrefabs, textAllcount, spawnObjects);     
    }
    #endregion

    #region Сбросить и показать всё
    public void FShowAll(TMP_InputField InputSSSearch,string[] sqlParams , List<string> listFillAllDropdown, TMP_Dropdown[] dropdowns,List<string> listCreateNewTable, List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObjects)
    {       
        InputSSSearch.text = "";
        MyFunctionNewDropdownAndNewTable(sqlParams , listFillAllDropdown, dropdowns, listCreateNewTable, createdPrefabs, textAllcount, spawnObjects);
    }

    #endregion

    #region Reverse
    /// <summary>
    /// Изменяет порядок элементов в списках, заполняет выпадающие списки, удаляет существующие игровые объекты и создает новые в соответствии со списками,
    /// а также возвращает исходный порядок элементов, если <paramref name="isReverseMode"/> равен true.
    /// </summary>
    /// <param name="listFillAllDropdown">Список строк для заполнения выпадающих списков.</param>
    /// <param name="dropdowns">Массив выпадающих списков, которые нужно заполнить.</param>
    /// <param name="listCreateNewTable">Список строк для создания  объектов.</param>
    /// <param name="createdPrefabs">Список созданных  объектов, которые нужно удалить.</param>
    /// <param name="textAllcount">Компонент TextMeshProUGUI, отображающий количество созданных  объектов.</param>
    /// <param name="spawnObjects">Массив объектов.</param>
    /// <param name="isReverseMode">Если true, меняет порядок элементов списков на обратный.</param>
    public void FReverse(List<string> listFillAllDropdown, TMP_Dropdown[] dropdowns, List<string> listCreateNewTable,List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObjects, bool isReverseMode = false)
    {
        if (isReverseMode)
        {
            listFillAllDropdown.Reverse();
            listCreateNewTable.Reverse();
        }

        int dropdownsLength = dropdowns.Length;
        for (int i = 0; i < dropdownsLength; i++)
        {
            FillDropDawn(dropdowns[i], listFillAllDropdown);
        }

        DestroyAllPrefabs(createdPrefabs);
        ShowAll(createdPrefabs, listCreateNewTable, textAllcount, spawnObjects);

        if (isReverseMode)
        {
            listFillAllDropdown.Reverse();
            listCreateNewTable.Reverse();
        }
    }
    #endregion

    #region Поиск

    public void FSearch(List<string> columnName, TMP_Dropdown DropDownSearch, TMP_InputField InputSSSearch,TMP_Dropdown[] dropdowns, List<string> IDList, string[] sqlParams ,List<string> listCreateNewTable, List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObjects)
    {
            string[] searchParams = new string[4];
            searchParams[0] = columnName[DropDownSearch.value];
            searchParams[1] = RemoveSpaces(InputSSSearch.text);
            searchParams[2] = sqlParams[0];
            searchParams[3] = sqlParams[1];
            HandleSearchResults(searchParams, dropdowns, IDList, sqlParams, InputSSSearch, listCreateNewTable, createdPrefabs, textAllcount, spawnObjects);
    }
    void HandleSearchResults(string[] searchParams, TMP_Dropdown[] dropdowns, List<string> IDList,  string[] sqlParams ,TMP_InputField InputSSSearch, List<string> listCreateNewTable, List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObjects)
    {
        StartCoroutine(WaitForCoroutineSearch(searchParams, dropdowns, IDList, sqlParams )); // Запуск второй корутины
        StartCoroutine(WaitForCoroutineSearch2(searchParams, InputSSSearch, sqlParams , listCreateNewTable, createdPrefabs, textAllcount, spawnObjects)); // Запуск второй корутины
    }
    IEnumerator WaitForCoroutineSearch(string[] searchParams, TMP_Dropdown[] u, List<string> list,  string[] sqlParams )
    {
        yield return StartCoroutine(IUpdateStrToTable(urlSearch, searchParams, list));
        if (list.Count > 0)
        {
            for (int i = 0; i < u.Count(); i++)
            {
                FillDropDawn(u[i], list);
            }
        }
        else
        {
            StartCoroutine(WaitForCoroutineFillAllDropdown(sqlParams , list, u)); // Запуск второй корутины
        }
    }
    IEnumerator WaitForCoroutineSearch2(string[] searchParams, TMP_InputField InputSSSearch, string[] sqlParams , List<string> listCreateNewTable, List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObjects)
    {
        yield return StartCoroutine(IUpdateStrToTable(urlSearchAll, searchParams, listCreateNewTable));

        if (listCreateNewTable.Count > 0)
        {
            spawnObjects[2].SetActive(true);
            spawnObjects[4].SetActive(true);
            DestroyAllPrefabs(createdPrefabs);
            ShowAll(createdPrefabs, listCreateNewTable, textAllcount, spawnObjects);
        }
        else
        {
            if (spawnObjects.Count() > 3)
            {
                HandleNullResults(createdPrefabs, spawnObjects);
            }
            else
            {
                InputSSSearch.text = "";
                StartCoroutine(WaitForCoroutineCreateNewTable(sqlParams , listCreateNewTable, createdPrefabs, textAllcount, spawnObjects)); // Запуск второй корутины
            }           
        }
    }
    /// <summary>
    /// Обрабатывает пустые результаты поиска, создавая новый префаб.
    /// </summary>
    /// <param name="createdPrefabsList">Список созданных префабов для уничтожения.</param>
    /// <param name="spawnObjectsArray">Array of spawn objects.</param>
    private void HandleNullResults(List<GameObject> createdPrefabsList, GameObject[] spawnObjectsArray)
    {
        DestroyAllPrefabs(createdPrefabsList);

        Vector3 spawnPosition = spawnObjectsArray[1].transform.position;
        GameObject prefabInstance = Instantiate(spawnObjectsArray[3], spawnPosition, Quaternion.identity, spawnObjectsArray[1].transform);
        createdPrefabsList.Add(prefabInstance);
        spawnObjectsArray[2].SetActive(false);
        spawnObjectsArray[4].SetActive(false);
    }

    #endregion

    #region Обновить таблицу после изменений


    void MyFunctionNewDropdownAndNewTable(string[] sqlParams , List<string> listFillAllDropdown, TMP_Dropdown[] dropdowns, List<string> listCreateNewTable, List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObjects)
    {
        StartCoroutine(WaitForCoroutineNew(sqlParams , listFillAllDropdown, dropdowns, listCreateNewTable, createdPrefabs, textAllcount, spawnObjects));
    }

    IEnumerator WaitForCoroutineNew(string[] sqlParams , List<string> listFillAllDropdown, TMP_Dropdown[] dropdowns, List<string> listCreateNewTable, List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObjects)
    {
        yield return StartCoroutine(WaitForCoroutineFillAllDropdown(sqlParams , listFillAllDropdown, dropdowns)); // Ожидание завершения первой корутины   
        yield return StartCoroutine(WaitForCoroutineCreateNewTable(sqlParams , listCreateNewTable, createdPrefabs, textAllcount, spawnObjects));
        if (spawnObjects.Count() > 5)
        {
            if (spawnObjects[5].activeSelf == true)
            {
                FReverse(listFillAllDropdown, dropdowns, listCreateNewTable, createdPrefabs, textAllcount, spawnObjects, true);
            }
        }
    }

    IEnumerator WaitForCoroutineFillAllDropdown(string[] sqlParams , List<string> list, TMP_Dropdown[] dropdowns, int cs = 0)
    {
        yield return StartCoroutine(IUpdateStrToTable(urlGetAllIdList,sqlParams , list)); // Ожидание завершения первой корутины   
        if (cs == 1) list.Reverse();
        for (int i = 0; i < dropdowns.Count(); i++)
        {
            FillDropDawn(dropdowns[i], list);
        }
    }
    IEnumerator WaitForCoroutineCreateNewTable(string[] sqlParams , List<string> list, List<GameObject> createdPrefabs, TextMeshProUGUI textAllcount, GameObject[] spawnObjects, int cs = 0)
    {
        yield return StartCoroutine(IUpdateStrToTable(urlSelectAll, sqlParams , list)); // Ожидание завершения первой корутины
        DestroyAllPrefabs(createdPrefabs);
        
        if (cs == 1) list.Reverse();
        
        ShowAll(createdPrefabs, list, textAllcount, spawnObjects);
    }
    #endregion

    #region Удалить все строки таблицы
    public void DestroyAllPrefabs(List<GameObject> createdPrefabs)
    {
        foreach (GameObject prefab in createdPrefabs)
        {
            Destroy(prefab);
        }
        createdPrefabs.Clear();

    }
    #endregion

    #region Вывести все строки таблицы

    public void ShowAll(List<GameObject> createdPrefabs, List<string> list, TextMeshProUGUI textAllcount, GameObject[] spawnObjects)
    {
        textAllcount.text = list.Count.ToString();
        float otherWidth = 0;
        if (spawnObjects.Length > 2)
        {
            RectTransform otherRectTransform = spawnObjects[2].GetComponent<RectTransform>();
            otherWidth = Mathf.FloorToInt(otherRectTransform.rect.width);
            if (otherWidth == 0) otherWidth = 2200;
        }
        for (int i = 0; i < list.Count; i++)
        {
            Vector3 spawnPosition = spawnObjects[1].transform.position + i * new Vector3(0, -270, 0);
            GameObject prefabInstance = Instantiate(spawnObjects[0], spawnPosition, Quaternion.identity, spawnObjects[1].transform);
            createdPrefabs.Add(prefabInstance);
            prefabInstance.transform.SetParent(transform, false);

            RectTransform prefabRectTransform = prefabInstance.GetComponent<RectTransform>();
            prefabRectTransform.sizeDelta = new Vector2(otherWidth, prefabRectTransform.sizeDelta.y);

            TextMeshProUGUI[] textMeshProArray = prefabInstance.GetComponentsInChildren<TextMeshProUGUI>();

            string[] pages = list[i].Split('!');

            if (textMeshProArray != null)
            {
                for (int j = 0; j < textMeshProArray.Length; j++)
                {
                    textMeshProArray[j].text = pages[j];
                }

            }
        }
    }


    #endregion

    #region Заполнить dropdown
    /// <summary>
    /// Заполняет выпадающий список данными из переданного списка.
    /// </summary>
    /// <param name="dropdown">Выпадающий список.</param>
    /// <param name="options">Список опций для выпадающего списка.</param>
    public static void FillDropDawn(TMP_Dropdown dropdown, List<string> options)
    {
        // Проверяем, что dropdown существует
        if (dropdown == null)
        {
            Debug.LogError("Dropdown object is null!");
            return;
        }

        // Очищаем старые опции выпадающего списка
        dropdown.ClearOptions();

        // Заполняем список опциями из переданного списка
        foreach (string option in options)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(option));
        }

        // Устанавливаем выбранную опцию на первую
        dropdown.value = 0;
        dropdown.RefreshShownValue();
    }

    #endregion

    #region Получить из сервера

    public static IEnumerator GetList(string uri, List<string> List)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                default:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string[] strings = webRequest.downloadHandler.text.Split("/");
                    List.Clear();
                    List.AddRange(strings);
                    List.RemoveAt(List.Count - 1);
                    break;
            }
        }
    }
    #endregion

    #region Отправить на сервер
    public static IEnumerator IAddNewStrToTable(string uri, string[] strings)
    {
        WWWForm form = new WWWForm();

        for (int i = 0; i < strings.Length; i++)
        {
            form.AddField("NewForm" + i, strings[i]);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public static IEnumerator IAddNewValuesToTable(string url, string TableName, string[] strings)
    {
        WWWForm form = new WWWForm();

        for (int i = 0; i < strings.Length; i++)
        {
            form.AddField("values[]", strings[i]);

        }
        form.AddField("TableName", TableName);


        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    #endregion

    #region Отправить и получить
    public static IEnumerator IUpdateStrToTable(string uri, string[] strings, List<string> List)
    {
        WWWForm form = new WWWForm();

        for (int i = 0; i < strings.Length; i++)
        {
            form.AddField("NewForm" + i, strings[i]);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(pages[page] + ": Error: " + www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                string[] ss = www.downloadHandler.text.Split("/");
                List.Clear();
                List.AddRange(ss);
                List.RemoveAt(List.Count - 1);
            }
        }
    }
    public static IEnumerator IUpdateStrToTable(string uri, string TableName, string[] strings, List<string> List)
    {
        WWWForm form = new WWWForm();

        for (int i = 0; i < strings.Length; i++)
        {
            form.AddField("values[]", strings[i]);

        }
        form.AddField("TableName", TableName);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(pages[page] + ": Error: " + www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string[] ss = www.downloadHandler.text.Split("/");
                List.Clear();
                List.AddRange(ss);
                List.RemoveAt(List.Count - 1);
                //Debug.Log("До выполнения: " + List[0]);
            }
        }
    }
    #endregion

}
