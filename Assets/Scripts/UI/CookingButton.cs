using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingButton : MonoBehaviour
{
    private bool cookingIsActive;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        Events.CookingDeactivation += CookingDeactivation;
    }

    private void OnClick()
    {
        cookingIsActive = !cookingIsActive;
        if (cookingIsActive)
        {
            GetComponent<Image>().color = Color.green;
            Events.CookingActivation();
        }
        else
        {
            Events.CookingDeactivation();
        }
    }

    private void CookingDeactivation()
    {
        GetComponent<Image>().color = Color.white;
    }
}
