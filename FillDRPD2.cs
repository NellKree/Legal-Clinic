using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class FillDRPD2 : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown[] dropdowns;

    public static List<string> IDCaseList = new List<string> { };

    private List<string>[] Lists = new List<string>[9];

    //string urlGetIDCase = "http://localhost/legalclinic/GetIDCase.php";
    string urlGetIDCase = "http://89.148.237.22/legalclinic/GetIDCase.php";

    //string urlAddNewConsultations = "http://localhost/legalclinic/AddNewConsultations.php";
    string urlAddNewConsultations = "http://89.148.237.22/legalclinic/AddNewConsultations.php";

    private void Awake()
    {

        StartCoroutine(CRUD.GetList(urlGetIDCase, IDCaseList));
    }

    void Start()
    {
        for (int i = 0; i < dropdowns.Count()-1; i++)
        {
            Lists[i] = new List<string>();
        }
        Lists[0] = IDCaseList;
        Lists[8] = new List<string>() { "Онлайн", "Очный" };
        //лист даты
        CRUD.FillDay(Lists[1], 1, 32);
        CRUD.FillDay(Lists[2], 1, 13);
        CRUD.FillDay(Lists[3], 2021, 2024);
        //листы времени
        CRUD.FillDay(Lists[4], 0, 24);
        CRUD.FillDay(Lists[5], 0, 60);
        //листы времени
        CRUD.FillDay(Lists[6], 0, 24);
        CRUD.FillDay(Lists[7], 0, 60);
        for (int i = 0; i < dropdowns.Count(); i++)
        {
            if (dropdowns[i] != null & Lists[i] != null)
            {
                CRUD.FillDropDawn(dropdowns[i], Lists[i]);
            }
        }
    }
    public void AddNewConsultations()
    {
        string[] strings = new string[5];
        strings[0] = dropdowns[0].options[dropdowns[0].value].text;
        strings[1] = dropdowns[8].options[dropdowns[8].value].text;
        strings[2] = dropdowns[3].options[dropdowns[3].value].text +
                "-" + dropdowns[2].options[dropdowns[2].value].text +
                "-" + dropdowns[1].options[dropdowns[1].value].text;
        strings[3] = dropdowns[4].options[dropdowns[4].value].text +
                ":" + dropdowns[5].options[dropdowns[5].value].text;
        strings[4] = dropdowns[6].options[dropdowns[6].value].text +
                ":" + dropdowns[7].options[dropdowns[7].value].text;
        StartCoroutine(AddStartStates.IAddNewStrToTable(urlAddNewConsultations, strings));
    }
   
}
