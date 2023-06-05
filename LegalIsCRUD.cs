using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LegalIsCRUD : MonoBehaviour
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
    [SerializeField] private TextMeshProUGUI TextNumberOfAllLines;                          // текстовое поле для отображения количества позиций в таблице
    [SerializeField] private TextMeshProUGUI TextNameTable;       // текстовое поле для отображения названия таблицы
    #endregion

    #region Эти переменные надо настроить здесь

    static List<string> columnName = new List<string>() { "IDLegalIssues", "IDConsultant", "IDTeacher", "AreaOfLaw", "TeacherVerificationStatus", "CaseStatus" };   //список значений для поискового dropdown c названиями как в базе данных
    static List<string> columnNameRus = new List<string>() { "ID", "ID консультанта", "ID эксперта", "Область права", "Статус проверки преподавателем", "Статус дела"};                     //список значений для поискового dropdown

    static string TableName = "legalissues";
    string TableNameRus = "Правовые вопросы";
    #endregion

    #region Эти переменные тоже не надо трогать
    string ob = "ContentYYYYY";                                                                                                                      //название scroll в котором выводятся все позиции
    string obMax = "PanelSС";

    private static List<string> IDListLIS = new List<string> { };
    private static List<string> IDListSt = new List<string> { };
    private static List<string> IDListCons = new List<string> { };
    private static List<string> IDListTea = new List<string> { };
    string[] SQLParSt = new string[2] { "statements", "IDStatements" };
    string[] SQLParCons = new string[2] { "consultants", "IDConsultant" };
    string[] SQLParTea = new string[2] { "teacher", "IDTeacher" };


    private List<string>[] ListsDropdown = new List<string>[5];

    public static List<string> IDExpertsList = new List<string> { };
    public static List<string> UpdateList = new List<string> { };
    public static List<string> textList = new List<string> { };                             // список позиций в таблице
    public static List<GameObject> createdPrefabs = new List<GameObject> { };               // список ссылок на созданные префабы
    static int numberSt = columnName.Count() - 1;                                       // количество параметров у позиции без учёта id
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
        yield return StartCoroutine(CRUD.IUpdateStrToTable(CRUD.urlGetAllIdList, SQLPar, IDListLIS));
        yield return StartCoroutine(CRUD.IUpdateStrToTable(CRUD.urlGetAllIdList, SQLParSt, IDListSt));
        yield return StartCoroutine(CRUD.IUpdateStrToTable(CRUD.urlGetAllIdList, SQLParCons, IDListCons));
        yield return StartCoroutine(CRUD.IUpdateStrToTable(CRUD.urlGetAllIdList, SQLParTea, IDListTea));
        for (int i = 0; i < InputDropdowns.Count(); i++)
        {
            ListsDropdown[i] = new List<string>();
        }
        IDListSt.RemoveAll(x => IDListLIS.Contains(x));

        ListsDropdown[0] = IDListCons;
        ListsDropdown[1] = IDListTea;
        ListsDropdown[2] = new List<string>() { "Назначено", "В ожидании ответа", "Одобрено", "Отклонено", "Перенесено", "Завершено", "Не состоялось" };
        ListsDropdown[3] = new List<string>() { "Новое дело", "Рассмотрение", "Отложено", "Решение вынесено", "Апелляция", "Исполнение решения", "Закрыто" };
        ListsDropdown[4] = IDListSt;

        for (int i = 0; i < InputDropdowns.Length; i++)
        {
            CRUD.FillDropDawn(InputDropdowns[i], ListsDropdown[i]);
        }
        for (int i = 0; i < UpdateDropdowns.Length; i++)
        {
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
        string[] strings = new string[6];
        strings[0] = InputDropdowns[4].options[InputDropdowns[4].value].text;
        strings[1] = InputDropdowns[0].options[InputDropdowns[0].value].text;
        strings[2] = InputDropdowns[1].options[InputDropdowns[1].value].text;
        strings[3] = InputSS[0].text;
        strings[4] = InputDropdowns[2].options[InputDropdowns[2].value].text;
        strings[5] = InputDropdowns[3].options[InputDropdowns[3].value].text;
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FCreateNew(strings, SQLPar, IDExpertsList, dropdownsID, textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
        StartCoroutine(WaitForCoroutine());
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
        string item = UpdateList[2];
        UpdateList.RemoveAt(2);
        UpdateList.Insert(UpdateList.Count, item);
        for (int i = 0; i < UpdateDropdowns.Length; i++)
        {
            List<TMP_Dropdown.OptionData> dropdownOptions = UpdateDropdowns[i].options;
            TMP_Dropdown.OptionData selectedOption = dropdownOptions.Find(option => option.text == UpdateList[i]);
            UpdateDropdowns[i].value = dropdownOptions.IndexOf(selectedOption);
        }
        UpdateInputSSTMP[0].text = UpdateList[4];
    }


    public void RealUpdate()
    {
        string[] strings = new string[6];
        strings[0] = IdTextUpdate.text;
        strings[1] = UpdateDropdowns[0].options[UpdateDropdowns[0].value].text;
        strings[2] = UpdateDropdowns[1].options[UpdateDropdowns[1].value].text;
        strings[3] = UpdateInputSSTMP[0].text;
        strings[4] = UpdateDropdowns[2].options[UpdateDropdowns[2].value].text;
        strings[5] = UpdateDropdowns[3].options[UpdateDropdowns[3].value].text;

        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FNewRealUpdate(strings, SQLPar, IDExpertsList, dropdownsID, textList, createdPrefabs, TextNumberOfAllLines, spawnObject);

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