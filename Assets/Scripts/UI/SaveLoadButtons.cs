using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadButtons : MonoBehaviour
{
    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] Button loadButton;

    private void Start()
    {
        if (!SaveLoadData.HasStoredData())
            loadButton.interactable = false;
    }

    public void Save()
    {
        SaveLoadData.SaveData();

        loadButton.interactable = true;
    }

    public void Load()
    {
        SaveLoadData.LoadData(()=>
        {
            StoredData storedData = SaveLoadData.GetStoredData();
            mapGenerator.GetGrid(storedData.seed);
            Events.Load();
        });
    }
}
