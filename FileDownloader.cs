using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections;
using TMPro;
using Unity.Notifications.Android;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;
using System.Net;
using System.Text.RegularExpressions;

public class FileDownloader : MonoBehaviour
{
    [SerializeField] private Animation AnimationLoad;
    [SerializeField] private RawImage ImageLoad;

    public Button NewAppealButton;

    public Toggle myToggle;
    public Toggle FirstToggle;

    public TMP_InputField[] Input;

    [SerializeField] private TMP_Dropdown[] dropdowns;
    private static List<string> CategoryList = new List<string> { };
    private List<string>[] Lists = new List<string>[4];

    private void Awake()
    {
        StartCoroutine(CRUD.GetList(CategOfCustomer.urlGetCategory, CategoryList));
    }
    void Start()
    {
        CreateNotificationChannel();

        for (int i = 0; i < dropdowns.Count(); i++)
        {
            Lists[i] = new List<string>();
        }
        Lists[0] = CategoryList;
        CRUD.FillDay(Lists[1], 1, 32);
        CRUD.FillDay(Lists[2], 1, 13);
        CRUD.FillDay(Lists[3], 1946, 2018);
        Lists[3].Reverse();
        for (int i = 0; i < dropdowns.Count(); i++)
        {
            if (dropdowns[i] != null & Lists[i] != null)
            {
                CRUD.FillDropDawn(dropdowns[i], Lists[i]);
            }
        }
    }
    void Update()
    {
        NewAppealButton.interactable = false;
        if (Input.Length > 0)
        {
            if (myToggle.isOn)
            {
                
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
                    NewAppealButton.interactable = true;
                }

            }          
        }
    }
    public void AddNewStatements()
    {
        if (myToggle.isOn)
        {
            string inputDate = dropdowns[1].options[dropdowns[1].value].text + "." + dropdowns[2].options[dropdowns[2].value].text + "." + dropdowns[3].options[dropdowns[3].value].text;
            if (CRUD.IsValidDate(inputDate))
            {
                Debug.Log("Дата является реальной.");
                string[] SQLParGetIdCat = new string[2] { dropdowns[0].options[dropdowns[0].value].text, "categoriesofcustomers" };
                StartCoroutine(WaitForCoroutineReturnIdCatCreate(SQLParGetIdCat));
            }
            else
            {
                Debug.Log("Дата не является реальной.");
            }            
        }
    }
    IEnumerator WaitForCoroutineReturnIdCatCreate(string[] SQLParGetIdCat)
    {
        yield return StartCoroutine(CRUD.IUpdateStrToTable(CustomerCRUD.urlGetIdCategor, SQLParGetIdCat, CustomerCRUD.IDCategList)); // Ожидание завершения первой корутины      
        string[] strings = new string[9];
        for (int i = 0; i < 5; i++)
        {
            strings[i] = CRUD.RemoveSpaces(Input[i + 1].text);
        }

        strings[5] = dropdowns[3].options[dropdowns[3].value].text +
                "-" + dropdowns[2].options[dropdowns[2].value].text +
                "-" + dropdowns[1].options[dropdowns[1].value].text;
        strings[6] = CustomerCRUD.IDCategList[0];
        strings[7] = DateTime.Now.ToString("yyyy-MM-dd"); ;
        strings[8] = Input[0].text;

        if (FirstToggle.isOn)
        {
            StartCoroutine(CRUD.IAddNewStrToTable(CRUD.urllAddingaNewTrusteeandStatement, strings));
        }
        else
        {
             List<string> ListId = new List<string> { };
             StartCoroutine(WaitForCoroutineReturnIdCustomer(strings, ListId));
        }

        for (int i = 0; i < dropdowns.Count(); i++)
        {
            List<TMP_Dropdown.OptionData> dropdownOptions = dropdowns[i].options;
            TMP_Dropdown.OptionData selectedOption = dropdownOptions.Find(option => option.text == Lists[i][0]);
            dropdowns[i].value = dropdownOptions.IndexOf(selectedOption);
        }
        for (int i = 0; i < 6; i++)
        {
            Input[i].text = "";
        }
    }
    IEnumerator WaitForCoroutineReturnIdCustomer(string[] strings, List<string> ListId)
    {
        yield return StartCoroutine(CRUD.IUpdateStrToTable(CRUD.urllFindIdCustomer, strings, ListId)); // Ожидание завершения первой корутины      
        
        if (ListId.Count > 0)
        {
            if (CRUD.IsNumber(ListId[0]))
            {
                string[] stringsnonew = new string[3];
                stringsnonew[0] = ListId[0];
                stringsnonew[1] = strings[7];
                stringsnonew[2] = strings[8];
                StartCoroutine(CRUD.IAddNewStrToTable(AddStartStates.urlAddNewStatements, stringsnonew));
            }        
        }
        else
        {
            StartCoroutine(CRUD.IAddNewStrToTable(CRUD.urllAddingaNewTrusteeandStatement, strings));
        }
    }
    public void DownloadFile(string Url)
    {
        StartCoroutine(DownloadFileRoutine(Url));
    }
    public void DownloadFileRegulationLegalClinic()
    {
        StartCoroutine(DownloadFileRoutine(CRUD.fileURLRegulationLegalClinic));
    }
    public void DownloadFileProcedureForCarryingOutActivities()
    {
        StartCoroutine(DownloadFileRoutine(CRUD.fileURLProcedureForCarryingOutActivities));
    }
    public void DownloadFileRegulationPersonalData()
    {
        StartCoroutine(DownloadFileRoutine(CRUD.fileURLRegulationPersonalData));
    }
    private IEnumerator DownloadFileRoutine(string Url)
    {
        ImageLoad.enabled = true;
        AnimationLoad.Play();
        string savePath = GetDownloadFolderPath();

        if (string.IsNullOrEmpty(savePath))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                savePath = Path.Combine(Application.persistentDataPath, "DownloadedFile");
            }
            else
            {
                savePath = Path.Combine(Application.dataPath, "DownloadedFile");
            }
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Get(Url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Ошибка загрузки файла: " + webRequest.error);
            }
            else
            {
                string fileName = "";

                if (Url == CRUD.fileURLRegulationLegalClinic)
                {
                    fileName = "6.18.1-01_230621-5_Приложение_2.docx";
                }
                if (Url == CRUD.fileURLProcedureForCarryingOutActivities)
                {
                    fileName = "6.18.1-01_230621-5_Приложение_3.docx";
                }
                if (Url == CRUD.fileURLRegulationPersonalData)
                {
                    fileName = "Положение_об_обработке_персональных_данных_Национальным_исследовательским_университетом_«Высшая_школа_экономики».docx";
                }

                string baseFileName = Path.GetFileNameWithoutExtension(fileName);
                string fileExtension = Path.GetExtension(fileName);
                int counter = 0;
                string finalFileName = fileName;
                string filePath = Path.Combine(savePath, finalFileName);
                while (File.Exists(filePath))
                {
                    counter++;
                    finalFileName = $"{baseFileName} ({counter}){fileExtension}";
                    filePath = Path.Combine(savePath, finalFileName);
                }
                System.IO.File.WriteAllBytes(filePath, webRequest.downloadHandler.data);
                SendNotification("Скачивание завершено", finalFileName, 0);
            }
        }
        ImageLoad.enabled = false;
        AnimationLoad.Stop();
    }

    public static string GetDownloadFolderPath()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaObject environment = new AndroidJavaClass("android.os.Environment"))
            {
                // Получаем путь к папке Downloads
                AndroidJavaObject downloadsDir = environment.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", environment.GetStatic<string>("DIRECTORY_DOWNLOADS"));
                if (downloadsDir != null)
                {
                    string downloadDirPath = downloadsDir.Call<string>("getAbsolutePath");
                    return downloadDirPath;
                }
            }
        }
        return "";
    }

    public void CreateNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "24",Name = "Download", Importance = Importance.High,Description = "Download notifications"
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);

    }

    public void SendNotification(string TitleNotification, string textNotification, int time)
    {
        var notification = new AndroidNotification();

        notification.Title = TitleNotification;
        notification.Text = textNotification;
        //notification.LargeIcon = "icon_1";
        //notification.SmallIcon = "icon_0";
        notification.FireTime = System.DateTime.Now.AddSeconds(time);
        AndroidNotificationCenter.SendNotification(notification, "24");
    }


}
