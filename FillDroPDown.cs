using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


public class FillDroPDown : MonoBehaviour
{

    [SerializeField]private TMP_Dropdown[] dropdowns;

    public static TMP_Dropdown[] Staticdropdowns = new TMP_Dropdown[5];

    public TextMeshProUGUI textShowActiveUser;
    //public static string urlGetID = "http://localhost/legalclinic/GetID.php";
    //string urlActiveCase = "http://localhost/legalclinic/ActiveCase.php";
    string urlActiveCase = "http://89.148.237.22/legalclinic/ActiveCase.php";

    public static List<string> CategList = new List<string> { };
    public static List<string> IDList = new List<string> { };

    private List<string> [] Lists = new List<string>[5]; 

    private void Awake()
    {
        StartCoroutine(CRUD.GetList(CategOfCustomer.urlGetCategory, CategList));
        StartCoroutine(CRUD.GetList(CustomerCRUD.urlGetID, IDList));
    }
    void Start()
    {
        Array.Copy(dropdowns, Staticdropdowns, 5);

        for (int i = 0; i < dropdowns.Count(); i++)
        {
            Lists[i] = new List<string>();
        }
        Lists[0] = CategList;
        Lists[1] = IDList;
        CRUD.FillDay(Lists[2], 1, 32);
        CRUD.FillDay(Lists[3], 1, 13);
        CRUD.FillDay(Lists[4], 1946, 2018);
        Lists[4].Reverse();
        for (int i =0; i < dropdowns.Count(); i++)
        {
            if (dropdowns[i] != null & Lists[i] != null)
            {
                CRUD.FillDropDawn(dropdowns[i], Lists[i]);
            }
        }

        StartCoroutine(ShowActiveUser(urlActiveCase));
    }
   
    IEnumerator ShowActiveUser(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;


            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    textShowActiveUser.text = webRequest.downloadHandler.text;
                    break;
                default:
                    textShowActiveUser.text = "0";
                    break;

            }
        }
    }
}

