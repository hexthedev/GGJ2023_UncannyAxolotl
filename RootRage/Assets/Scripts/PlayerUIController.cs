using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public Image ResourceBar;

    public float[] RequiredResourcePercentage;
    public GameObject[] Button;

    public event Action<int> OnBuildingClicked;

    public void ClickBuilding(int i) => OnBuildingClicked?.Invoke(i);

    float current = 0;
    
    public void SetResource(float percentage)
    {
        current = percentage;
        ((RectTransform) ResourceBar.transform).anchorMax = new Vector2(0, percentage);

        for (var i = 0; i < RequiredResourcePercentage.Length; i++)
        {
            if(current>= RequiredResourcePercentage[i])
                Button[i].SetActive(true);
            else
                Button[i].SetActive(false);
            
        }
    }
}