using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] GameObject buildingPanel;

    private bool buildingIsActive;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        Events.BuildingDeactivation += BuildingDeactivation;
    }

    private void OnClick()
    {
        buildingIsActive = !buildingIsActive;
        if (buildingIsActive)
        {
            GetComponent<Image>().color = Color.green;
            buildingPanel.SetActive(true);
            Events.BuildingActivation();
        }
        else
        {
            Events.BuildingDeactivation();
        }
    }

    private void BuildingDeactivation()
    {
        GetComponent<Image>().color = Color.white;
        buildingPanel.SetActive(false);
    }
}
