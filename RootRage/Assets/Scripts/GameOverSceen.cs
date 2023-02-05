using TMPro;
using UnityEngine;

public class GameOverSceen : MonoBehaviour
{
    public TMP_Text Text;

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void SetText(string text) => Text.text = text;
}