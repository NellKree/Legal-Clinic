using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FillReportDD : MonoBehaviour
{
    string urlGetAllIdList = "http://localhost/legalclinic/F/GetIdList.php";
    [SerializeField] private TMP_Dropdown[] dropdowns1;
    [SerializeField] private TMP_Dropdown[] dropdowns2;

    string[] SQLPar1 = new string[2] { "consultants", "IDConsultant" };
    string[] SQLPar2 = new string[2] { "teacher", "IDTeacher" };
    private void Awake()
    {
        StartCoroutine(WaitForCoroutineFillAllDropdown(SQLPar1, ConsultantsCRUD.IDExpertsList, dropdowns1));
        StartCoroutine(WaitForCoroutineFillAllDropdown(SQLPar2, ExpertsCrud.IDExpertsList, dropdowns2));
    }

    public void Fill()
    {
        StartCoroutine(WaitForCoroutineFillAllDropdown(SQLPar1, ConsultantsCRUD.IDExpertsList, dropdowns1));
        StartCoroutine(WaitForCoroutineFillAllDropdown(SQLPar2, ExpertsCrud.IDExpertsList, dropdowns2));
    }
    IEnumerator WaitForCoroutineFillAllDropdown(string[] SQLPar, List<string> list, TMP_Dropdown[] dropdowns)
    {
        yield return StartCoroutine(CRUD.IUpdateStrToTable(urlGetAllIdList, SQLPar, list)); // ќжидание завершени€ первой корутины   

        for (int i = 0; i < dropdowns.Count(); i++)
        {
            CRUD.FillDropDawn(dropdowns[i], list);
        }
    }
}
