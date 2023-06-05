using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstOrNoTog : MonoBehaviour
{
    public GameObject[] Tabs;
    public void ToggleOpen()
    {
        if (Tabs.Length > 0)
        {
            for (int i = 0; i < Tabs.Length; i++)
            {
                if (Tabs[i] != null)
                {
                    bool isActive = Tabs[i].activeSelf;
                    Tabs[i].SetActive(!isActive);
                }
            }          
        }
    }
    public void testFill()
    {
        if (Tabs[7] != null & Tabs[7].activeSelf)
        {
            CRUD.FillDropDawn(FillDroPDown.Staticdropdowns[1], FillDroPDown.IDList);
        }
        if (Tabs[7] != null & !Tabs[7].activeSelf)
        {
            CRUD.FillDropDawn(FillDroPDown.Staticdropdowns[0], FillDroPDown.CategList);
        }
    }



}
