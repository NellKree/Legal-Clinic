using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatementsCRUD : MonoBehaviour
{
    #region Эти переменные надо настроить в инспекторе Unity, так что тут ничего не трогать
    [SerializeField]
    private GameObject[] Tabs;
    [SerializeField]
    private Button[] Buttons;
    [SerializeField]
    private TMP_InputField[] InputSS;
    [SerializeField]
    private TMP_InputField[] UpdateInputSSTMP;
    [SerializeField]
    private TMP_Dropdown[] InputDropdowns;
    [SerializeField]
    private TMP_Dropdown[] UpdateDropdowns;
    [SerializeField]
    private TextMeshProUGUI IdTextUpdate;
    [SerializeField]
    private TMP_InputField InputSSSearch;
    [SerializeField]
    private TMP_Dropdown DropDownSearch;
    [SerializeField] private TMP_Dropdown[] dropdownsID;
    [SerializeField] private GameObject[] spawnObject;
    [SerializeField] private TextMeshProUGUI TextNumberOfAllLines; // текстовое поле для отображения количества позиций в таблице
    [SerializeField] private TextMeshProUGUI TextNameTable;       // текстовое поле для отображения названия таблицы
    #endregion

    #region Эти переменные надо настроить здесь

    static List<string> columnName = new List<string>() { "IDStatements", "IDCustomer", "ApplicationDate", "DescriptionOfTheProblem" };   //список значений для поискового dropdown c названиями как в базе данных
    static List<string> columnNameRus = new List<string>() { "ID", "ID доверителя", "Дата обращения" };                     //список значений для поискового dropdown
    static string TableName = "statements";
    string TableNameRus = "Заявления";

    #endregion

    #region Эти переменные тоже не надо трогать
    string ob = "ContentYYYYY";                                                 //название scroll в котором выводятся все позиции
    string obMax = "PanelSС";

    private List<string>[] ListsDropdown = new List<string>[5];

    public static List<string> IDExpertsList = new List<string> { };
    public static List<string> UpdateList = new List<string> { };
    public static List<string> textList = new List<string> { };                 // список позиций в таблице
    public static List<GameObject> createdPrefabs = new List<GameObject> { };   // список ссылок на созданные префабы
    static int numberSt = columnNameRus.Count() - 1;                            // количество параметров у позиции без учёта id
    string[] SQLPar = new string[2] { TableName, columnName[0] };
    #endregion

    private void Awake()
    {
        TextNameTable.text = TableNameRus;
        CRUD.FillDropDawn(DropDownSearch, columnNameRus);
        StartCoroutine(WaitForCoroutineShowAll());
        StartCoroutine(WaitForCoroutineFillDropdown());
        StartCoroutine(WaitForCoroutine());
    }
    IEnumerator WaitForCoroutine()
    {
        yield return StartCoroutine(CRUD.GetList(CustomerCRUD.urlGetID, FillDroPDown.IDList));

        for (int i = 0; i < InputDropdowns.Count(); i++)
        {
            ListsDropdown[i] = new List<string>();
        }
        ListsDropdown[3] = FillDroPDown.IDList;
        CRUD.FillDay(ListsDropdown[0], 1, 32);
        CRUD.FillDay(ListsDropdown[1], 1, 13);
        CRUD.FillDay(ListsDropdown[2], 2020, 2024);
        ListsDropdown[2].Reverse();
        for (int i = 0; i < InputDropdowns.Count(); i++)
        {
            CRUD.FillDropDawn(InputDropdowns[i], ListsDropdown[i]);
            CRUD.FillDropDawn(UpdateDropdowns[i], ListsDropdown[i]);
        }

    }

    IEnumerator WaitForCoroutineShowAll()
    {
        yield return StartCoroutine(CRUD.IUpdateStrToTable(CRUD.urlSelectAll, SQLPar, textList)); // Ожидание завершения первой корутины      
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.ShowAll(createdPrefabs, textList, TextNumberOfAllLines, spawnObject);
    }
    IEnumerator WaitForCoroutineFillDropdown()
    {
        yield return StartCoroutine(CRUD.IUpdateStrToTable(CRUD.urlGetAllIdList, SQLPar, IDExpertsList)); // Ожидание завершения первой корутины
        for (int i = 0; i < dropdownsID.Count(); i++)
        {
            if (dropdownsID[i] != null & IDExpertsList != null)
            {
                CRUD.FillDropDawn(dropdownsID[i], IDExpertsList);
            }
        }
    }
    void Update()
    {
        if (Tabs != null)
        {
            bool isActive = Tabs[0].activeSelf;
            if (isActive == true)
            {
                Buttons[0].interactable = false;
                bool y = true;
                for (int i = 0; i < InputSS.Length; i++)
                {
                    if (InputSS[i].text == "")
                    {
                        y = false;
                    }
                }
                if (y == true)
                {
                    Buttons[0].interactable = true;
                }
            }
            bool isActive2 = Tabs[1].activeSelf;
            if (isActive2 == true)
            {
                if (InputSSSearch.text == "")
                {
                    Buttons[1].interactable = false;
                    Buttons[2].interactable = false;
                }
                else
                {
                    Buttons[1].interactable = true;
                    Buttons[2].interactable = true;
                }
            }
        }
    }

    public void DownloadToExcel()
    {
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FDownloadToExcel(TableName, columnName);
    }

    public void CreateNew()
    {
        DateTime now = DateTime.Now;
        string inputDate = InputDropdowns[0].options[InputDropdowns[0].value].text + "." + InputDropdowns[1].options[InputDropdowns[1].value].text + "." + InputDropdowns[2].options[InputDropdowns[2].value].text;
        DateTime date;
        bool isDateValid = DateTime.TryParseExact(inputDate, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out date);

        if (!isDateValid)
        {
            Debug.Log("Ошибка: введена некорректная дата!");
        }
        if (date > now)
        {
            Debug.Log("Внимание! Введенная дата больше текущей даты.");
        }
        else
        {
            Debug.Log("Введенная дата меньше или равна текущей дате.");

            if (CRUD.IsValidDate(inputDate))
            {
                Debug.Log("Дата является реальной.");
                string[] strings = new string[3];
                strings[2] = InputSS[0].text;
                strings[1] = InputDropdowns[2].options[InputDropdowns[2].value].text + "-" + InputDropdowns[1].options[InputDropdowns[1].value].text + "-" + InputDropdowns[0].options[InputDropdowns[0].value].text;
                strings[0] = InputDropdowns[3].options[InputDropdowns[3].value].text;
                GameObject myObject = GameObject.Find(ob);
                CRUD myScriptComponent = myObject.GetComponent<CRUD>();
                myScriptComponent.FCreateNew(strings, SQLPar, IDExpertsList, dropdownsID, textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
            }
            else
            {
                Debug.Log("Дата не является реальной.");
            }     
        }
        for (int i = 0; i < InputDropdowns.Count(); i++)
        {
            List<TMP_Dropdown.OptionData> dropdownOptions = InputDropdowns[i].options;
            TMP_Dropdown.OptionData selectedOption = dropdownOptions.Find(option => option.text == ListsDropdown[i][0]);
            InputDropdowns[i].value = dropdownOptions.IndexOf(selectedOption);
        }
        InputSSSearch.text = "";
        InputSS[0].text = "";
    }

    public void GoUpdate()
    {
        GameObject myObject = GameObject.Find(obMax);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FGoToUpdate(UpdateList, dropdownsID, TableName, IdTextUpdate, OnUpdateCompleted);
    }

    public void OnUpdateCompleted()
    {
        UpdateInputSSTMP[0].text = UpdateList[2];
        string[] ss = UpdateList[1].Split("-");
        int j = 2;
        for (int i = 0; i < UpdateDropdowns.Count() - 1; i++)
        {
            List<TMP_Dropdown.OptionData> dropdownOptions0 = UpdateDropdowns[i].options;
            TMP_Dropdown.OptionData selectedOption0 = dropdownOptions0.Find(option => option.text == ss[j]);
            UpdateDropdowns[i].value = dropdownOptions0.IndexOf(selectedOption0);
            j--;
        }
        List<TMP_Dropdown.OptionData> dropdownOptions = UpdateDropdowns[3].options;
        TMP_Dropdown.OptionData selectedOption = dropdownOptions.Find(option => option.text == UpdateList[0]);
        UpdateDropdowns[3].value = dropdownOptions.IndexOf(selectedOption);
    }
    

    public void RealUpdate()
    {
        DateTime now = DateTime.Now;
        string inputDate = UpdateDropdowns[0].options[UpdateDropdowns[0].value].text + "." + UpdateDropdowns[1].options[UpdateDropdowns[1].value].text + "." + UpdateDropdowns[2].options[UpdateDropdowns[2].value].text;
        DateTime date;
        bool isDateValid = DateTime.TryParseExact(inputDate, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out date);
        if (!isDateValid)
        {
            Debug.Log("Ошибка: введена некорректная дата!");
        }
        if (date > now)
        {
            Debug.Log("Внимание! Введенная дата больше текущей даты.");
        }
        else
        {
            Debug.Log("Введенная дата меньше или равна текущей дате.");
            if (CRUD.IsValidDate(inputDate))
            {
                Debug.Log("Дата является реальной.");
                string[] strings = new string[4];
                strings[0] = IdTextUpdate.text;
                strings[3] = UpdateInputSSTMP[0].text;
                strings[2] = UpdateDropdowns[2].options[UpdateDropdowns[2].value].text + "-" + UpdateDropdowns[1].options[UpdateDropdowns[1].value].text + "-" + UpdateDropdowns[0].options[UpdateDropdowns[0].value].text;
                strings[1] = UpdateDropdowns[3].options[UpdateDropdowns[3].value].text;
                GameObject myObject = GameObject.Find(ob);
                CRUD myScriptComponent = myObject.GetComponent<CRUD>();
                myScriptComponent.FNewRealUpdate(strings, SQLPar, IDExpertsList, dropdownsID, textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
            }
            else
            {
                Debug.Log("Дата не является реальной.");
            }
        }

        UpdateInputSSTMP[0].text = "";
        InputSSSearch.text = "";
    }


    public void Delete()
    {
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FDelete(InputSSSearch, SQLPar, IDExpertsList, dropdownsID, textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
    }

    public void Copy()
    {
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FCopy(UpdateList, dropdownsID[2], TableName);
    }

    public void ShowAll()
    {
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FShowAll(InputSSSearch, SQLPar, IDExpertsList, dropdownsID, textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
    }

    public void BackReverseAll()
    {
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FReverse(IDExpertsList, dropdownsID, textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
    }

    public void ReverseAll()
    {
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FReverse(IDExpertsList, dropdownsID, textList, createdPrefabs, TextNumberOfAllLines, spawnObject, true);
    }

    public void Search()
    {
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        if (InputSSSearch.text != "")
        {
            myScriptComponent.FSearch(columnName, DropDownSearch, InputSSSearch, dropdownsID, IDExpertsList, SQLPar, textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
        }
        else
        {
            myScriptComponent.DestroyAllPrefabs(createdPrefabs);
            spawnObject[2].SetActive(true);
            spawnObject[4].SetActive(true);
            StartCoroutine(WaitForCoroutineShowAll());
            StartCoroutine(WaitForCoroutineFillDropdown());
        }
    }
}