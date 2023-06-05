using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PriemCRUD : MonoBehaviour
{
    #region ��� ���������� ���� ��������� � ���������� Unity, ��� ��� ��� ������ �� �������
    [SerializeField]
    private GameObject[] Tabs;
    [SerializeField]
    private Button[] Buttons;
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
    [SerializeField] private TextMeshProUGUI TextNumberOfAllLines;                          // ��������� ���� ��� ����������� ���������� ������� � �������
    [SerializeField] private TextMeshProUGUI TextNameTable;       // ��������� ���� ��� ����������� �������� �������
    #endregion

    #region ��� ���������� ���� ��������� �����

    static List<string> columnName = new List<string>() { "IDConsultation", "IDLegalIssues", "MeetingFormat", "DateOfConsultation", "ConsultationStartTime", "EndTimeOfTheConsultation" };   //������ �������� ��� ���������� dropdown c ���������� ��� � ���� ������
    static List<string> columnNameRus = new List<string>() { "ID", "ID ����", "������ �������", "���� ������������", "����� ������", "����� ���������" };                     //������ �������� ��� ���������� dropdown

    static string TableName = "consultations";
    string TableNameRus = "������������";
    #endregion

    #region ��� ���������� ���� �� ���� �������
    string ob = "ContentYYYYY";                                                                                                                      //�������� scroll � ������� ��������� ��� �������
    string obMax = "PanelS�";

    private static List<string> IDListLIS = new List<string> { };

    string[] SQLParLIS = new string[2] { "legalissues", "IDLegalIssues" };



    private List<string>[] ListsDropdown = new List<string>[9];

    public static List<string> IDExpertsList = new List<string> { };
    public static List<string> UpdateList = new List<string> { };
    public static List<string> textList = new List<string> { };                             // ������ ������� � �������
    public static List<GameObject> createdPrefabs = new List<GameObject> { };               // ������ ������ �� ��������� �������
    static int numberSt = columnName.Count() - 1;                                       // ���������� ���������� � ������� ��� ����� id
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
        yield return StartCoroutine(CRUD.IUpdateStrToTable(CRUD.urlGetAllIdList, SQLParLIS, IDListLIS));


        for (int i = 0; i < InputDropdowns.Count(); i++)
        {
            ListsDropdown[i] = new List<string>();
        }
        ListsDropdown[0] = IDListLIS;
        ListsDropdown[1] = new List<string>() { "������", "�����" };
        CRUD.FillDay(ListsDropdown[2], 1, 32);
        CRUD.FillDay(ListsDropdown[3], 1, 13);
        CRUD.FillDay(ListsDropdown[4], 2021, 2024);
        //����� �������
        CRUD.FillDay(ListsDropdown[5], 0, 24);
        CRUD.FillDay(ListsDropdown[6], 0, 60);
        //����� �������
        CRUD.FillDay(ListsDropdown[7], 0, 24);
        CRUD.FillDay(ListsDropdown[8], 0, 60);

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
        yield return StartCoroutine(CRUD.IUpdateStrToTable(CRUD.urlSelectAll, SQLPar, textList)); // �������� ���������� ������ ��������      
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.ShowAll(createdPrefabs, textList, TextNumberOfAllLines, spawnObject);
    }
    IEnumerator WaitForCoroutineFillDropdown()
    {
        yield return StartCoroutine(CRUD.IUpdateStrToTable(CRUD.urlGetAllIdList, SQLPar, IDExpertsList)); // �������� ���������� ������ ��������
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
            bool isActive2 = Tabs[0].activeSelf;
            if (isActive2 == true)
            {
                if (InputSSSearch.text == "")
                {
                    Buttons[0].interactable = false;
                    Buttons[1].interactable = false;
                }
                else
                {
                    Buttons[0].interactable = true;
                    Buttons[1].interactable = true;
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
        string inputDate = InputDropdowns[2].options[InputDropdowns[2].value].text + "." + InputDropdowns[3].options[InputDropdowns[3].value].text + "." + InputDropdowns[4].options[InputDropdowns[4].value].text;
        DateTime date;
        bool isDateValid = DateTime.TryParseExact(inputDate, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out date);

        string hours1, minutes1, hours2, minutes2;
        hours1 = InputDropdowns[5].options[InputDropdowns[5].value].text;
        minutes1 = InputDropdowns[6].options[InputDropdowns[6].value].text;
        hours2 = InputDropdowns[7].options[InputDropdowns[7].value].text;
        minutes2 = InputDropdowns[8].options[InputDropdowns[8].value].text;
        if (!isDateValid)
        {
            Debug.Log("������: ������� ������������ ����!");
        }
        if (date > now)
        {
            Debug.Log("��������! ��������� ���� ������ ������� ����.");
        }
        else
        {
            Debug.Log("��������� ���� ������ ��� ����� ������� ����.");
            if (CRUD.CompareTimes(hours1, minutes1, hours2, minutes2))
            {
                if (CRUD.IsValidDate(inputDate))
                {
                    Debug.Log("���� �������� ��������.");
                    string[] strings = new string[5];
                    strings[0] = InputDropdowns[0].options[InputDropdowns[0].value].text;

                    strings[1] = InputDropdowns[1].options[InputDropdowns[1].value].text;

                    strings[2] = InputDropdowns[4].options[InputDropdowns[4].value].text + "-" + InputDropdowns[3].options[InputDropdowns[3].value].text + "-" + InputDropdowns[2].options[InputDropdowns[2].value].text;
                    strings[3] = hours1 + ":" + minutes1;
                    strings[4] = hours2 + ":" + minutes2;

                    GameObject myObject = GameObject.Find(ob);
                    CRUD myScriptComponent = myObject.GetComponent<CRUD>();
                    myScriptComponent.FCreateNew(strings, SQLPar, IDExpertsList, dropdownsID, textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
                }
                else
                {
                    Debug.Log("���� �� �������� ��������.");
                }
            }
            else
            {
                Debug.Log("������ ����� �������.");
            }            
        }
        StartCoroutine(WaitForCoroutine());
        InputSSSearch.text = "";

    }

    public void GoUpdate()
    {
        GameObject myObject = GameObject.Find(obMax);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FGoToUpdate(UpdateList, dropdownsID, TableName, IdTextUpdate, OnUpdateCompleted);
    }

    public void OnUpdateCompleted()
    {
        for (int i = 0; i < 2; i++)
        {
            List<TMP_Dropdown.OptionData> dropdownOptions11 = UpdateDropdowns[i].options;
            TMP_Dropdown.OptionData selectedOption = dropdownOptions11.Find(option => option.text == UpdateList[i]);
            UpdateDropdowns[i].value = dropdownOptions11.IndexOf(selectedOption);
        }

        string[] ss = UpdateList[2].Split("-");
        int j = 2;
        for (int i = 2; i < 5; i++)
        {
            List<TMP_Dropdown.OptionData> dropdownOptions0 = UpdateDropdowns[i].options;
            TMP_Dropdown.OptionData selectedOption0 = dropdownOptions0.Find(option => option.text == ss[j]);
            UpdateDropdowns[i].value = dropdownOptions0.IndexOf(selectedOption0);
            j--;
        }
        string[] sstime1 = UpdateList[3].Split(":");
        
        List<TMP_Dropdown.OptionData> dropdownOptions = UpdateDropdowns[5].options;
        TMP_Dropdown.OptionData selectedOption1 = dropdownOptions.Find(option => option.text == sstime1[0]);
        UpdateDropdowns[5].value = dropdownOptions.IndexOf(selectedOption1);
         dropdownOptions = UpdateDropdowns[6].options;
        selectedOption1 = dropdownOptions.Find(option => option.text == sstime1[1]);
        UpdateDropdowns[6].value = dropdownOptions.IndexOf(selectedOption1);

        string[] sstime2 = UpdateList[4].Split(":");

        List<TMP_Dropdown.OptionData> dropdownOptions1 = UpdateDropdowns[7].options;
        selectedOption1 = dropdownOptions1.Find(option => option.text == sstime2[0]);
        UpdateDropdowns[7].value = dropdownOptions1.IndexOf(selectedOption1);
        List<TMP_Dropdown.OptionData> dropdownOptions2 = UpdateDropdowns[8].options;
        selectedOption1 = dropdownOptions2.Find(option => option.text == sstime2[1]);
        UpdateDropdowns[8].value = dropdownOptions2.IndexOf(selectedOption1);

    }


    public void RealUpdate()
    {
        DateTime now = DateTime.Now;
        string inputDate = UpdateDropdowns[2].options[UpdateDropdowns[2].value].text + "." + UpdateDropdowns[3].options[UpdateDropdowns[3].value].text + "." + UpdateDropdowns[4].options[UpdateDropdowns[4].value].text;
        DateTime date;
        bool isDateValid = DateTime.TryParseExact(inputDate, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out date);

        string hours1, minutes1, hours2, minutes2;
        hours1 = UpdateDropdowns[5].options[UpdateDropdowns[5].value].text;
        minutes1 = UpdateDropdowns[6].options[UpdateDropdowns[6].value].text;
        hours2 = UpdateDropdowns[7].options[UpdateDropdowns[7].value].text;
        minutes2 = UpdateDropdowns[8].options[UpdateDropdowns[8].value].text;
        if (!isDateValid)
        {
            Debug.Log("������: ������� ������������ ����!");
        }
        if (date > now)
        {
            Debug.Log("��������! ��������� ���� ������ ������� ����.");
        }
        else
        {
            Debug.Log("��������� ���� ������ ��� ����� ������� ����.");
            if (CRUD.CompareTimes(hours1, minutes1, hours2, minutes2))
            {
                if (CRUD.IsValidDate(inputDate))
                {
                    Debug.Log("���� �������� ��������.");
                    string[] strings = new string[6];
                    strings[0] = IdTextUpdate.text;
                    strings[1] = UpdateDropdowns[0].options[UpdateDropdowns[0].value].text;

                    strings[2] = UpdateDropdowns[1].options[UpdateDropdowns[1].value].text;

                    strings[3] = UpdateDropdowns[4].options[UpdateDropdowns[4].value].text + "-" + UpdateDropdowns[3].options[UpdateDropdowns[3].value].text + "-" + UpdateDropdowns[2].options[UpdateDropdowns[2].value].text;
                    strings[4] = hours1 + ":" + minutes1;
                    strings[5] = hours2 + ":" + minutes2;

                    GameObject myObject = GameObject.Find(ob);
                    CRUD myScriptComponent = myObject.GetComponent<CRUD>();
                    myScriptComponent.FNewRealUpdate(strings, SQLPar, IDExpertsList, dropdownsID, textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
                }
                else
                {
                    Debug.Log("���� �� �������� ��������.");
                }
            }
            else
            {
                Debug.Log("������ ����� �������.");
            }
        }
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