using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerCRUD : MonoBehaviour
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
    [SerializeField] private TextMeshProUGUI TextNumberOfAllLines;// текстовое поле для отображения количества позиций в таблице
    [SerializeField] private TextMeshProUGUI TextNameTable;       // текстовое поле для отображения названия таблицы

    #endregion

    #region Эти переменные надо настроить здесь

    static List<string> columnName = new List<string>() { "IDCustomer", "Surname", "Name", "Patronymic", "PhoneNumber", "Mail", "DateOfBirth", "IDCategory" };   //список значений для поискового dropdown c названиями как в базе данных
    static List<string> columnNameRus = new List<string>() { "ID", "Фамилия", "Имя", "Отчество", "Телефон", "Почта", "Дата рождения", "Категория" };                     //список значений для поискового dropdown
    static string TableName = "customer";
    string TableNameRus = "Доверители";

    public static string urlGetIdCategor = "http://89.148.237.22/legalclinic/Customer/GetIdWhere.php";

    string urlGetNameCategor = "http://89.148.237.22/legalclinic/Customer/GetCatWhere.php";

    public static string urlGetID = "http://89.148.237.22/legalclinic/GetID.php";
    #endregion

    #region Эти переменные тоже не надо трогать
    string ob = "ContentYYYYY";                                                 //название scroll в котором выводятся все позиции
    string obMax = "PanelSС";
    public static TMP_Dropdown[] StaticCustomerInputDropdowns = new TMP_Dropdown[4];
    public static TMP_Dropdown[] StaticCustomerUpdateDropdowns = new TMP_Dropdown[4];
    private List<string>[] ListsDropdown = new List<string>[4];
    public static List<string> IDCategList = new List<string> { };
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
        Array.Copy(InputDropdowns, StaticCustomerInputDropdowns, 4);
        Array.Copy(UpdateDropdowns, StaticCustomerUpdateDropdowns, 4);
    }
    IEnumerator WaitForCoroutine()
    {
        yield return StartCoroutine(CRUD.GetList(CategOfCustomer.urlGetCategory, FillDroPDown.CategList));

        for (int i = 0; i < InputDropdowns.Count(); i++)
        {
            ListsDropdown[i] = new List<string>();
        }
        ListsDropdown[3] = FillDroPDown.CategList;
        CRUD.FillDay(ListsDropdown[0], 1, 32);
        CRUD.FillDay(ListsDropdown[1], 1, 13);
        CRUD.FillDay(ListsDropdown[2], 1946, 2018);
        ListsDropdown[2].Reverse();
        for (int i = 0; i < InputDropdowns.Count(); i++)
        {
            CRUD.FillDropDawn(InputDropdowns[i], ListsDropdown[i]);
            CRUD.FillDropDawn(UpdateDropdowns[i], ListsDropdown[i]);
        }

    }
    IEnumerator WaitForCoroutineFillDD()
    {
        yield return StartCoroutine(CRUD.GetList(urlGetID, FillDroPDown.IDList)); // Ожидание завершения первой корутины      
        CRUD.FillDropDawn(FillDroPDown.Staticdropdowns[1], FillDroPDown.IDList);

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
        string inputDate = InputDropdowns[0].options[InputDropdowns[0].value].text + "." + InputDropdowns[1].options[InputDropdowns[1].value].text + "." + InputDropdowns[2].options[InputDropdowns[2].value].text;
        if (CRUD.IsValidDate(inputDate))
        {
            Debug.Log("Дата является реальной.");
            string[] SQLParGetIdCat = new string[2] { InputDropdowns[3].options[InputDropdowns[3].value].text, "categoriesofcustomers" };
            StartCoroutine(WaitForCoroutineReturnIdCatCreate(SQLParGetIdCat));
        }
        else
        {
            Debug.Log("Дата не является реальной.");
        }
        InputSSSearch.text = "";

    }
    IEnumerator WaitForCoroutineReturnIdCatCreate(string[] SQLParGetIdCat)
    {
        yield return StartCoroutine(CRUD.IUpdateStrToTable(urlGetIdCategor, SQLParGetIdCat, IDCategList)); // Ожидание завершения первой корутины      
        string[] strings = new string[numberSt];
        for (int i = 0; i < InputSS.Count(); i++)
        {
            strings[i] = CRUD.RemoveSpaces(InputSS[i].text);
            InputSS[i].text = "";
        }
        strings[numberSt-2] = InputDropdowns[2].options[InputDropdowns[2].value].text + "-" + InputDropdowns[1].options[InputDropdowns[1].value].text + "-" + InputDropdowns[0].options[InputDropdowns[0].value].text;
        strings[numberSt - 1] = IDCategList[0];
        for (int i = 0; i < InputDropdowns.Count(); i++)
        {
            List<TMP_Dropdown.OptionData> dropdownOptions = InputDropdowns[i].options;
            TMP_Dropdown.OptionData selectedOption = dropdownOptions.Find(option => option.text == ListsDropdown[i][0]);
            InputDropdowns[i].value = dropdownOptions.IndexOf(selectedOption);
        }
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FCreateNew(strings, SQLPar, IDExpertsList, dropdownsID, textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
        StartCoroutine(WaitForCoroutineFillDD());
    }

    public void GoUpdate()
    {
        GameObject myObject = GameObject.Find(obMax);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FGoToUpdate(UpdateList, dropdownsID, TableName, IdTextUpdate, OnUpdateCompleted);
    }

    public void OnUpdateCompleted()
    {
        string[] SQLParGetIdCat = new string[2] { UpdateList[UpdateList.Count-1], "categoriesofcustomers" };
        StartCoroutine(WaitForCoroutineReturnNameCat(SQLParGetIdCat));
    }
    IEnumerator WaitForCoroutineReturnNameCat(string[] SQLParGetIdCat)
    {
        yield return StartCoroutine(CRUD.IUpdateStrToTable(urlGetNameCategor, SQLParGetIdCat, IDCategList)); // Ожидание завершения первой корутины      

        for (int i = 0; i < UpdateInputSSTMP.Count(); i++)
        {
            UpdateInputSSTMP[i].text = UpdateList[i];
        }
        string[] ss = UpdateList[UpdateList.Count - 2].Split("-");
        int j = 2;
        for (int i = 0; i < UpdateDropdowns.Count()-1; i++)
        {
            List<TMP_Dropdown.OptionData> dropdownOptions0 = UpdateDropdowns[i].options;
            TMP_Dropdown.OptionData selectedOption0 = dropdownOptions0.Find(option => option.text == ss[j]);
            UpdateDropdowns[i].value = dropdownOptions0.IndexOf(selectedOption0);
            j--;
        }
        List<TMP_Dropdown.OptionData> dropdownOptions = UpdateDropdowns[3].options;
        TMP_Dropdown.OptionData selectedOption = dropdownOptions.Find(option => option.text == IDCategList[0]);
        UpdateDropdowns[3].value = dropdownOptions.IndexOf(selectedOption);
    }

    public void RealUpdate()
    {
        string inputDate = UpdateDropdowns[0].options[UpdateDropdowns[0].value].text + "." + UpdateDropdowns[1].options[UpdateDropdowns[1].value].text + "." + UpdateDropdowns[2].options[UpdateDropdowns[2].value].text;
        if (CRUD.IsValidDate(inputDate))
        {
            Debug.Log("Дата является реальной.");
            string[] SQLParGetIdCat = new string[2] { UpdateDropdowns[3].options[UpdateDropdowns[3].value].text, "categoriesofcustomers" };
            StartCoroutine(WaitForCoroutineReturnIdCatUpdate(SQLParGetIdCat));
        }
        else
        {
            Debug.Log("Дата не является реальной.");
        }
        InputSSSearch.text = "";
    }
    IEnumerator WaitForCoroutineReturnIdCatUpdate(string[] SQLParGetIdCat)
    {
        yield return StartCoroutine(CRUD.IUpdateStrToTable(urlGetIdCategor, SQLParGetIdCat, IDCategList)); // Ожидание завершения первой корутины      
        string[] strings = new string[numberSt+1];
        strings[0] = IdTextUpdate.text;
        for (int i = 0; i < UpdateInputSSTMP.Count(); i++)
        {
            strings[i+1] = CRUD.RemoveSpaces(UpdateInputSSTMP[i].text);
            UpdateInputSSTMP[i].text = "";
        }
        strings[numberSt - 1] = UpdateDropdowns[2].options[UpdateDropdowns[2].value].text + "-" + UpdateDropdowns[1].options[UpdateDropdowns[1].value].text + "-" + UpdateDropdowns[0].options[UpdateDropdowns[0].value].text;
        strings[numberSt ] = IDCategList[0];
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FNewRealUpdate(strings, SQLPar, IDExpertsList, dropdownsID, textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
        StartCoroutine(WaitForCoroutineFillDD());
    }

    public void Delete()
    {
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FDelete(InputSSSearch,SQLPar, IDExpertsList, dropdownsID,textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
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
        myScriptComponent.FShowAll(InputSSSearch,SQLPar, IDExpertsList, dropdownsID,textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
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
