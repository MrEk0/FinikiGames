using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text counterText;

    public void UpdateText(int count)
    {
        counterText.text = count.ToString();
    }
}
