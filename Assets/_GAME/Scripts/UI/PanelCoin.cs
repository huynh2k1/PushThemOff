using System.Collections;
using System.Collections.Generic;
using H_Utils;
using TMPro;
using UnityEngine;

public class PanelCoin : MonoBehaviour
{
    [SerializeField] TMP_Text coinText;

    private void OnEnable()
    {
        UpdateText(GameDatas.Coin);
        GameController.OnCoinChanged += UpdateUI;
    }

    private void OnDisable()
    {
        GameController.OnCoinChanged -= UpdateUI;
    }

    void UpdateUI()
    {
        UpdateText(GameDatas.Coin);
    }

    public void UpdateText(int coin)
    {
        coinText.text = coin.ToString();
    }
}
