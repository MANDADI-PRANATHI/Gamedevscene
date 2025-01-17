using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEditor.Animations;
using TMPro;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;

    public List<string> inventoryItemList = new List<string>();

    //Category Buttons
    Button toolsBTN;

    //Craft Buttons
    Button craftAxeBTN;

    //Requirement text
    TextMeshProUGUI AxeReq1, AxeReq2;

    public bool isOpen;

    //All Blueprint
    public Blueprint AxeBLP = new Blueprint("Axe" ,2 , "Stone" , 3 , "Stick" , 3);




    public static CraftingSystem Instance{get;set;}



    private void Awake()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }
    }




    //start is called before the first frame update
    void Start()
    {
        isOpen = false;
        

        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });
        //axe
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<TextMeshProUGUI>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<TextMeshProUGUI>();


        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

        if (AxeReq1 == null || AxeReq2 == null || craftAxeBTN == null)
        {
            Debug.LogError("One or more UI components for the Axe are missing.");
        }

    }
    //void Start()
    //{
    //    isOpen = false;

    //    toolsBTN = craftingScreenUI.transform.Find("ToolsButton")?.GetComponent<Button>();
    //    if (toolsBTN != null)
    //    {
    //        toolsBTN.onClick.AddListener(OpenToolsCategory);
    //    }
    //    else
    //    {
    //        Debug.LogError("ToolsButton is missing or not assigned.");
    //    }

    //    AxeReq1 = toolsScreenUI.transform.Find("Axe/req1")?.GetComponent<Text>();
    //    AxeReq2 = toolsScreenUI.transform.Find("Axe/req2")?.GetComponent<Text>();
    //    craftAxeBTN = toolsScreenUI.transform.Find("Axe/Button")?.GetComponent<Button>();

    //    if (AxeReq1 == null || AxeReq2 == null || craftAxeBTN == null)
    //    {
    //        Debug.LogError("One or more UI components for the Axe are missing.");
    //        return;
    //    }

    //    craftAxeBTN.onClick.AddListener(() => CraftAnyItem(AxeBLP));
    //}




    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }


    void CraftAnyItem(Blueprint blueprintToCraft)
    {

        //add item into inventory
        InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);


        //remove resources from inventory 
        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }
        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }
       //similar way to do for req 3 also

        //refresh list
        InventorySystem.Instance.ReCalculateList();

        RefreshNeededItems();




    }



    //update is called once per frame
     void Update()
    {
        RefreshNeededItems();

        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {

            Debug.Log("i is pressed");
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            if (!InventorySystem.Instance.isOpen)

            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            
            isOpen = false;
        }
    }

    private void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;

                case "Stick":
                    stick_count += 1;
                    break;

            }
        }

        //---AXE----

        if (AxeReq1 != null)
        {
            AxeReq1.text = "3 Stone [" + stone_count + "]";
        }
        if (AxeReq2 != null)
        {
            AxeReq2.text = "3 Stick [" + stick_count + "]";
        }

        if (stone_count>=3 && stick_count>=3)
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }



    }



}