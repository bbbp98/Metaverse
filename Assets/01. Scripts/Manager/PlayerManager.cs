using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public class PlayerData
    {
        [SerializeField] public int Gold;
    }

    public PlayerData playerData = new PlayerData();

    private const string GoldKey = "M_Gold";

    public event Action<int> OnGoldChanged;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        playerData.Gold = PlayerPrefs.GetInt(GoldKey, 500);
    }

    private void Start()
    {
        OnGoldChanged?.Invoke(playerData.Gold);
    }

    public void AddGold(int amount)
    {
        playerData.Gold += amount;
        PlayerPrefs.SetInt(GoldKey, playerData.Gold);
        OnGoldChanged?.Invoke(playerData.Gold);
    }

    public bool SpendGold(int amount)
    {
        if (playerData.Gold < amount)
        {
            Debug.Log("°ñµå ºÎÁ·");
            return false;
        }

        playerData.Gold -= amount;
        PlayerPrefs.SetInt(GoldKey, playerData.Gold);
        OnGoldChanged?.Invoke(playerData.Gold);
        return true;
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }
}
