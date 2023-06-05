using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Login : MonoBehaviour
{
    [HideInInspector]
    public bool UserIsAuthorized = false;
    private string JobTitle = string.Empty;
    private string UserName = string.Empty;

    public static List<string> ListAuthorization = new List<string> { };

    [SerializeField] private GameObject[] Tabs;

    [SerializeField] private GameObject[] FirstOrNotTabs;

    [SerializeField] private ScrollRect scrollRect;

    [SerializeField] private TextMeshProUGUI TablePost;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI SavePost;


    [SerializeField] private TextMeshProUGUI UserNameForLogWindow;
    [SerializeField] private TextMeshProUGUI UserMailForLogWindow;

    [SerializeField]
    private Toggle SaveMeToggle;
    [SerializeField]
    private Image imageError;
    [SerializeField]
    private Image imageEmail;
    [SerializeField]
    private Image imagePassword;
    [SerializeField]
    private TMP_InputField InputEmail;
    [SerializeField]
    private TMP_InputField InputPassword;
    [SerializeField]
    private Color newColorRed;

    private void Start()
    {
        UpdateUserInformation();
        //if (PlayerPrefs.GetInt("UserIsAuthorized") == 1) ChoosePost();
    }

    private void UpdateUserInformation()
    {
        TablePost.text = PlayerPrefs.GetString("JobTitle");
        Name.text = PlayerPrefs.GetString("Surname") + " " + PlayerPrefs.GetString("Name");
        UserNameForLogWindow.text = PlayerPrefs.GetString("Surname") + " " + PlayerPrefs.GetString("Name");
        UserMailForLogWindow.text = PlayerPrefs.GetString("Mail");
    }

    public void Authorization()
    {
        string tablename = "";

        if (string.IsNullOrEmpty(InputEmail.text)) 
        {
            imageEmail.color = newColorRed;
            return;
        }
        if (string.IsNullOrEmpty(InputPassword.text))
        {
            imagePassword.color = newColorRed;
            return;
        }

        if (!string.IsNullOrEmpty(SavePost.text))
        {
            if (SavePost.text == "Консультант" || SavePost.text == "Администратор")
            {
                tablename = "consultants";
            }
            if (SavePost.text == "Куратор")
            {
                tablename = "teacher";
            }
        }
        Debug.Log("Хэш: "+CRUD.HashPassword(InputPassword.text)+"\nПароль:"+InputPassword.text);

        string[] strings = new string[2];

        strings[0] = InputEmail.text;
        strings[1] = CRUD.HashPassword(InputPassword.text);

        StartCoroutine(WaitForCoroutineAuthorization(tablename, strings));
       
    }

    IEnumerator WaitForCoroutineAuthorization(string tablename, string[] strings)
    {
        yield return StartCoroutine(CRUD.IUpdateStrToTable(CRUD.urlAuthorization, tablename, strings, ListAuthorization)); // Ожидание завершения первой корутины      
        
        if (ListAuthorization.Count > 0)
        {
            string[] pages = ListAuthorization[0].Split('!');

            JobTitle = SavePost.text;
            ToggleOpen(Tabs);
            scrollRect.vertical = true;
            UserIsAuthorized = true;
            
            PlayerPrefs.SetInt("IDUser", Int32.Parse(pages[0]));
            PlayerPrefs.SetString("Surname", pages[1]);
            PlayerPrefs.SetString("Name", pages[2]);
            PlayerPrefs.SetString("Patronymic", pages[3]);
            PlayerPrefs.SetString("PhoneNumber", pages[4]);
            PlayerPrefs.SetString("Mail", pages[5]);
            PlayerPrefs.SetString("JobTitle", JobTitle);
            PlayerPrefs.SetInt("UserIsAuthorized", 1);
            UpdateUserInformation();
            ChoosePost();
            if (SaveMeToggle.isOn)
            {
                Debug.Log("+");
            }
            else
            {
                Debug.Log("-");
            }
        }
        else
        {
            imageEmail.color = newColorRed;
            imagePassword.color = newColorRed;
            imageError.enabled = true;
            UserIsAuthorized = false;
        }
    }
    public void FirstOrNot()
    {
        int CheckAuthorization = PlayerPrefs.GetInt("UserIsAuthorized");
        if (CheckAuthorization == 0)
        {
            FirstOrNotTabs[0].SetActive(true);
            FirstOrNotTabs[1].SetActive(false);
        }
        if (CheckAuthorization == 1)
        {
            FirstOrNotTabs[0].SetActive(false);
            FirstOrNotTabs[1].SetActive(true);
        }
    }
    public void ChangeUser()
    {
        PlayerPrefs.DeleteAll();
        UserIsAuthorized = false;
        TablePost.text = "";
        Name.text = "";
    }
    public void UserExit()
    {
        ChangeUser();
        ToggleOpen(Tabs);
        scrollRect.vertical = true;
    }
    public void ChoosePost()
    {
        string userJob = PlayerPrefs.GetString("JobTitle");
        if (userJob == "Консультант")
        {
            Newstart.LoadScene(10);
        }
        if (userJob == "Куратор")
        {
            Newstart.LoadScene(11);
        }
        if (userJob == "Администратор")
        {
            Newstart.LoadScene(9);
        }
    }

    public void ToggleOpen(GameObject[] Tabs)
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
}
