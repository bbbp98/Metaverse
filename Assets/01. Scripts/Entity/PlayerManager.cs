using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private int gold;
    public int Gold { get { return gold; } private set { gold = value; } }

    private const string GoldKey = "M_Gold";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
        gold = PlayerPrefs.GetInt(GoldKey, 500);
    }

    private void Start()
    {
        UIManager.Instance.SetGold();
    }

    private void Update()
    {
        UIManager.Instance.SetGold();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        PlayerPrefs.SetInt(GoldKey, gold);
    }

    public bool SpendGold(int amount)
    {
        if (gold < amount)
        {
            Debug.Log("°ñµå ºÎÁ·");
            return false;
        }

        gold = Mathf.Max(gold - amount, 0);
        PlayerPrefs.SetInt(GoldKey, gold);

        return true;
    }
}
