using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsultantsCRUD : MonoBehaviour
{
    #region ��� ���������� ���� ��������� � ���������� Unity, ��� ��� ��� ������ �� �������
    [SerializeField]
    private GameObject[] Tabs;
    [SerializeField]
    private Button[] Buttons;
    [SerializeField]
    private TMP_InputField[] InputSS;
    [SerializeField]
    private TMP_InputField[] UpdateInputSSTMP;
    [SerializeField]
    private TextMeshProUGUI IdTextUpdate;
    [SerializeField]
    private TMP_InputField InputSSSearch;
    [SerializeField]
    private TMP_Dropdown DropDownSearch;
    [SerializeField] private TMP_Dropdown[] dropdownsID;
    [SerializeField] private GameObject[] spawnObject;
    [SerializeField] private TextMeshProUGUI TextNumberOfAllLines;// ��������� ���� ��� ����������� ���������� ������� � �������
    [SerializeField] private TextMeshProUGUI TextNameTable;       // ��������� ���� ��� ����������� �������� �������

    [SerializeField] private Texture SpriteUp;
    [SerializeField] private Texture SpriteDawn;
    [SerializeField] private RawImage ButtonImage;
    private bool Reverse = false;
    #endregion

    #region ��� ���������� ���� ��������� �����

    static List<string> columnName = new List<string>() { "IDConsultant", "Surname", "Name", "Patronymic", "PhoneNumber", "Mail" , "StudyGroup" };   //������ �������� ��� ���������� dropdown c ���������� ��� � ���� ������
    static List<string> columnNameRus = new List<string>() { "ID", "�������", "���", "��������", "�������", "�����" , "������"};                     //������ �������� ��� ���������� dropdown
    static string TableName = "consultants";
    string TableNameRus = "������������";

    #endregion

    #region ��� ���������� ���� �� ���� �������
    string ob = "ContentYYYYY";                                                            //�������� scroll � ������� ��������� ��� �������
    string obMax = "PanelS�";
    public static List<string> IDExpertsList = new List<string> { };
    public static List<string> UpdateList = new List<string> { };
    public static List<string> textList = new List<string> { };                            // ������ ������� � �������
    public static List<GameObject> createdPrefabs = new List<GameObject> { };              // ������ ������ �� ��������� �������
    static int numberSt = columnNameRus.Count() - 1;                                       // ���������� ���������� � ������� ��� ����� id
    string[] SQLPar = new string[2] { TableName, columnName[0] };
    #endregion

    private void Awake()
    {
        TextNameTable.text = TableNameRus;
        CRUD.FillDropDawn(DropDownSearch, columnNameRus);
        StartCoroutine(WaitForCoroutineShowAll());
        StartCoroutine(WaitForCoroutineFillDropdown());
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

    public void DownloadOneToExcel()
    {
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FDownloadOneToExcel(TableName, columnName,createdPrefabs);
    }

    public void CreateNew()
    {
        string[] strings = new string[numberSt+1];
        for (int i = 0; i < numberSt; i++)
        {
            strings[i] = CRUD.RemoveSpaces(InputSS[i].text);
            InputSS[i].text = "";
        }
        strings[numberSt] = CRUD.HashPassword(InputSS[numberSt].text);
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FCreateNew(strings, SQLPar, IDExpertsList, dropdownsID,textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
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
        for (int i = 0; i < UpdateList.Count-1; i++)
        {
            if (UpdateInputSSTMP.Count() == UpdateList.Count)
            {
                UpdateInputSSTMP[i].text = UpdateList[i];
            }
        }
    }

    public void RealUpdate()
    {
        int CountUpdateInput= UpdateInputSSTMP.Count();
        string[] strings = new string[CountUpdateInput+1];
        strings[0] = IdTextUpdate.text;
        for (int i = 0; i < CountUpdateInput - 1; i++)
        {
            strings[i+1] = CRUD.RemoveSpaces(UpdateInputSSTMP[i].text);
            UpdateInputSSTMP[i].text = "";
        }
        if (UpdateInputSSTMP[CountUpdateInput -1].text != "")
        {
            strings[CountUpdateInput] = CRUD.HashPassword(UpdateInputSSTMP[CountUpdateInput-1].text);
        }
        else
        {
            strings[CountUpdateInput] = UpdateList[UpdateList.Count()-1];
        }
        UpdateInputSSTMP[CountUpdateInput - 1].text = "";


        foreach (var i in strings)
        {
            Debug.Log(i);
        }
        

        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FNewRealUpdate(strings,SQLPar, IDExpertsList, dropdownsID,textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
        InputSSSearch.text = "";
    }

    public void Delete()
    {
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FDelete( InputSSSearch,
            SQLPar, IDExpertsList, dropdownsID,
            textList, createdPrefabs, TextNumberOfAllLines, spawnObject);
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
        myScriptComponent.FShowAll(InputSSSearch,
            SQLPar, IDExpertsList, dropdownsID,
            textList, createdPrefabs, TextNumberOfAllLines, spawnObject);

    }

    public void ReverseAll()
    {
        if (Reverse == false)
        {
            Reverse = true;
            ButtonImage.texture = SpriteUp;
        }
        else
        {
            Reverse = false;
            ButtonImage.texture = SpriteDawn;
        }
        GameObject myObject = GameObject.Find(ob);
        CRUD myScriptComponent = myObject.GetComponent<CRUD>();
        myScriptComponent.FReverse(IDExpertsList, dropdownsID,textList, createdPrefabs, TextNumberOfAllLines, spawnObject, Reverse);
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
            spawnObject[4 ].SetActive(true);
            StartCoroutine(WaitForCoroutineShowAll());
            StartCoroutine(WaitForCoroutineFillDropdown());
        }      
    }
}
