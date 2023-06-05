using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AddStartStates : MonoBehaviour
{
    public Button startButton;

    public Toggle myToggle;

    public TMP_InputField[] Input;

    public static string urlAddNewStatements = "http://localhost/legalclinic/AddNewStatements.php";
    //string urlAddNewCustomer = "http://localhost/legalclinic/AddNewCustomer.php";
    public static string urlAddNewCustomerAndState = "http://localhost/legalclinic/AddNewCustomerAndState.php";
    //string urlGetOneID = "http://localhost/legalclinic/GetOneID.php";
    

    void Update()
    {
        if (Input.Length > 0)
        {
            if (myToggle.isOn)
            {
                startButton.interactable = false;
                bool y = true;
                for (int i = 0; i < Input.Length; i++)
                {
                    if (Input[i].text == "")
                    {
                        y = false;
                    }
                }
                if (y == true)
                {
                    startButton.interactable = true;
                }

            }
            else
            {
                if (Input[0].text == "")
                {
                    startButton.interactable = false;
                }
                else
                {
                    startButton.interactable = true;
                }

            }
        }
        
    }
     void AddNewStatements()
    {

        if (myToggle.isOn)
        {
            string[] strings = new string[9];
            for (int i =0; i < 5;i++)
            {
                strings[i] = Input[i + 1].text;
            }

            strings[5] = FillDroPDown.Staticdropdowns[4].options[FillDroPDown.Staticdropdowns[4].value].text+
                "-"+ FillDroPDown.Staticdropdowns[3].options[FillDroPDown.Staticdropdowns[3].value].text + 
                "-" + FillDroPDown.Staticdropdowns[2].options[FillDroPDown.Staticdropdowns[2].value].text;
            strings[6] = (FillDroPDown.Staticdropdowns[0].value + 1).ToString();
            strings[7]= DateTime.Now.ToString("yyyy-MM-dd"); ;
            strings[8] = Input[0].text;
            Debug.Log(" strings[0]: " + strings[0] + " strings[1]: " + strings[1] + " strings[2]: " + strings[2] + " strings[3]: " + strings[3] + " strings[4]: " + strings[4] + " strings[5]: " + strings[5] + " strings[6]: "+ strings[6]);
            StartCoroutine(IAddNewStrToTable(urlAddNewCustomerAndState, strings));           
            MyFunction();
            for (int i = 0; i < 6; i++)
            {
                Input[i].text = "";
            }
        }
        else
        {           
            string[] strings = new string[3];
            strings[0] = FillDroPDown.Staticdropdowns[1].options[FillDroPDown.Staticdropdowns[1].value].text;
            strings[1] = DateTime.Now.ToString("yyyy-MM-dd");
            strings[2] = Input[0].text;
            StartCoroutine(IAddNewStrToTable(urlAddNewStatements,strings));
            Input[0].text = "";
        }
    }
    //public void GetId(string uri, string[] strings, string u)
    //{
    //    StartCoroutine(IGetId(uri, strings, u));
    //}
    IEnumerator IGetId(string uri, string[] strings, string u)
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
                u = www.downloadHandler.text;
            }
        }
    }

    void MyFunction()
    {
        StartCoroutine(CRUD.GetList("http://localhost/legalclinic/GetID.php", FillDroPDown.IDList)); // Запуск корутины
        StartCoroutine(WaitForCoroutine()); // Запуск второй корутины
    }

    IEnumerator WaitForCoroutine()
    {
        yield return StartCoroutine(CRUD.GetList("http://localhost/legalclinic/GetID.php", FillDroPDown.IDList)); // Ожидание завершения первой корутины
        CRUD.FillDropDawn(FillDroPDown.Staticdropdowns[1], FillDroPDown.IDList);
    }

    public static IEnumerator IAddNewStrToTable(string uri,string[] strings)
    {
        WWWForm form = new WWWForm();

        for (int i = 0; i < strings.Length;i++)
        {
            form.AddField("NewForm"+i, strings[i]);
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

}
