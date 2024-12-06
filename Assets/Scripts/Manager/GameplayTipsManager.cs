using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayTipsManager : MonoBehaviour
{
    public static GameplayTipsManager Instance { get; private set; }
    public GameObject[] Tips;
    public int currentTipIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateActiveTipUI();
    }

    private void Update()
    {
        if (UIManager.Instance.menuActivated && UIManager.Instance.tipsMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PreviousTip();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                NextTip();
            }
        }
    }

    private void UpdateActiveTipUI()
    {
        for (int i = 0; i < Tips.Length; i++)
        {
            Tips[i].SetActive(i == currentTipIndex);
        }
    }

    #region Change Inventories

    public void PreviousTip()
    {
        Tips[currentTipIndex].SetActive(false);
        currentTipIndex = (currentTipIndex - 1 + Tips.Length) % Tips.Length;
        UpdateActiveTipUI();
        SoundFXManager.Instance.PlayChangeTabSound();
    }

    public void NextTip()
    {
        Tips[currentTipIndex].SetActive(false);
        currentTipIndex = (currentTipIndex + 1) % Tips.Length;
        UpdateActiveTipUI();
        SoundFXManager.Instance.PlayChangeTabSound();
    }

    #endregion
}
