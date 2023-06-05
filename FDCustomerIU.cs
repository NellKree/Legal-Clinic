using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

using UnityEngine;

public class FDCustomerIU : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown[] dropdownsInput;
    [SerializeField] private TMP_Dropdown[] dropdownsUpdate;
    private List<string>[] Lists = new List<string>[4];
    private List<string>[] ListsStaements = new List<string>[4];
    private List<string>[] ListsLegalIs = new List<string>[3];
    private void Awake()
    {
        StartCoroutine(CRUD.GetList(CategOfCustomer.urlGetCategory, FillDroPDown.CategList));

    }
    public void FillLegalIsDD()
    {
        for (int i = 0; i < dropdownsInput.Count(); i++)
        {
            ListsLegalIs[i] = new List<string>();
        }
        ListsLegalIs[0] = StatementsCRUD.IDExpertsList;
        ListsLegalIs[1] = ConsultantsCRUD.IDExpertsList;
        ListsLegalIs[2] = ExpertsCrud.IDExpertsList;

        for (int i = 0; i < dropdownsInput.Count(); i++)
        {
            if (dropdownsInput[i] != null & ListsLegalIs[i] != null)
            {
                CRUD.FillDropDawn(dropdownsInput[i], ListsLegalIs[i]);
            }
        }
        for (int i = 0; i < dropdownsUpdate.Count(); i++)
        {
            if (dropdownsUpdate[i] != null & ListsLegalIs[i] != null)
            {
                CRUD.FillDropDawn(dropdownsUpdate[i], ListsLegalIs[i]);
            }
        }
    }

    public void Fill()
    {
        StartCoroutine(WaitForCoroutine());
    }
    IEnumerator WaitForCoroutine()
    {
        yield return StartCoroutine(CRUD.GetList(CategOfCustomer.urlGetCategory, FillDroPDown.CategList));

        for (int i = 0; i < dropdownsInput.Count(); i++)
        {
            Lists[i] = new List<string>();
        }
        Lists[3] = FillDroPDown.CategList;
        CRUD.FillDay(Lists[0], 1, 32);
        CRUD.FillDay(Lists[1], 1, 13);
        CRUD.FillDay(Lists[2], 1946, 2018);
        Lists[2].Reverse();
        for (int i = 0; i < dropdownsInput.Count(); i++)
        {
            if (dropdownsInput[i] != null & Lists[i] != null)
            {
                CRUD.FillDropDawn(dropdownsInput[i], Lists[i]);
            }
        }
        for (int i = 0; i < dropdownsUpdate.Count(); i++)
        {
            if (dropdownsUpdate[i] != null & Lists[i] != null)
            {
                CRUD.FillDropDawn(dropdownsUpdate[i], Lists[i]);
            }
        }
    }
    public void FillStatementsDD()
    {
        StartCoroutine(WaitForCoroutine2());
    }
    IEnumerator WaitForCoroutine2()
    {
        yield return StartCoroutine(CRUD.GetList(CustomerCRUD.urlGetID, FillDroPDown.IDList));

        for (int i = 0; i < dropdownsInput.Count(); i++)
        {
            ListsStaements[i] = new List<string>();
        }
        ListsStaements[3] = FillDroPDown.IDList;
        CRUD.FillDay(ListsStaements[0], 1, 32);
        CRUD.FillDay(ListsStaements[1], 1, 13);
        CRUD.FillDay(ListsStaements[2], 2020, 2024);
        ListsStaements[2].Reverse();
        for (int i = 0; i < dropdownsInput.Count(); i++)
        {
            if (dropdownsInput[i] != null & ListsStaements[i] != null)
            {
                CRUD.FillDropDawn(dropdownsInput[i], ListsStaements[i]);
            }
        }
        for (int i = 0; i < dropdownsUpdate.Count(); i++)
        {
            if (dropdownsUpdate[i] != null & ListsStaements[i] != null)
            {
                CRUD.FillDropDawn(dropdownsUpdate[i], ListsStaements[i]);
            }
        }
    }
}
