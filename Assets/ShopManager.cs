using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Oracle;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Color OwnedColor;
    [SerializeField] private Color UnOwnedColor;

    [SerializeField] private Button skrimpSelector;
    [SerializeField] private TMP_Text skrimpSelectorText;
    [Space(10)] [SerializeField] private Button rainSelector;
    [SerializeField] private Button rainPurchase;
    [SerializeField] private Image rainBackground;
    [SerializeField] private TMP_Text rainSelectorText;
    [SerializeField] private Text rainPurchaseText;
    [Space(10)] [SerializeField] private Button AvoSelector;
    [SerializeField] private Button AvoPurchase;
    [SerializeField] private Image AvoBackground;
    [SerializeField] private TMP_Text AvoSelectorText;
    [SerializeField] private Text AvoPurchaseText;
    [Space(10)] [SerializeField] private Button catSelector;
    [SerializeField] private Button catPurchase;
    [SerializeField] private Image catBackground;
    [SerializeField] private TMP_Text catSelectorText;
    [SerializeField] private Text catPurchaseText;

    private Preferences prefs => oracle.saveData.preferences;

    private void Start()
    {
        skrimpSelector.onClick.AddListener(SelectSkrimp);
        rainSelector.onClick.AddListener(SelectRain);
        AvoSelector.onClick.AddListener(SelectAvo);
        catSelector.onClick.AddListener(SelectCat);
    }

    // Update is called once per frame
    private void Update()
    {
        skrimpSelectorText.text = prefs.skinSelection == SkinSelection.Skrimp ? "Selected" : "";

        rainPurchase.gameObject.SetActive(!prefs.rainSkinOwned);
        rainBackground.color = prefs.rainSkinOwned ? OwnedColor : UnOwnedColor;
        rainSelectorText.text = prefs.rainSkinOwned
            ? prefs.skinSelection == SkinSelection.Rain ? "Selected" : ""
            : rainPurchaseText.text;

        AvoPurchase.gameObject.SetActive(!prefs.avoSkinOwned);
        AvoBackground.color = prefs.avoSkinOwned ? OwnedColor : UnOwnedColor;
        AvoSelectorText.text = prefs.avoSkinOwned
            ? prefs.skinSelection == SkinSelection.Avocado ? "Selected" : ""
            : AvoPurchaseText.text;

        catPurchase.gameObject.SetActive(!prefs.catSkinOwned);
        catBackground.color = prefs.catSkinOwned ? OwnedColor : UnOwnedColor;
        catSelectorText.text = prefs.catSkinOwned
            ? prefs.skinSelection == SkinSelection.Cat ? "Selected" : ""
            : catPurchaseText.text;
    }


    public void SelectSkrimp()
    {
        prefs.skinSelection = SkinSelection.Skrimp;
    }

    public void SelectRain()
    {
        prefs.skinSelection = SkinSelection.Rain;
    }

    public void SelectAvo()
    {
        prefs.skinSelection = SkinSelection.Avocado;
    }

    public void SelectCat()
    {
        prefs.skinSelection = SkinSelection.Cat;
    }

    public void PurchaseRain()
    {
        prefs.rainSkinOwned = true;
        prefs.skinSelection = SkinSelection.Rain;
    }

    public void PurchaseAvo()
    {
        prefs.avoSkinOwned = true;
        prefs.skinSelection = SkinSelection.Avocado;
    }

    public void PurchaseCat()
    {
        prefs.catSkinOwned = true;
        prefs.skinSelection = SkinSelection.Cat;
    }
}