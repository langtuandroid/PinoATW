using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerNew : MonoBehaviour
{
    float _healtsPoints, _staminaPoints, _darkEnergyPoints;
    float maxHP = 100f, minHP = 0f;
    float maxSTA = 100f, minSTA = 0f, overSTA = -25;
    float maxDE = 100f, minDE = 0f;
    float timerInvulnerable;
    [SerializeField] bool recoverySTA, exhaustState, deadState, vulnerable;
    public bool DANNO;

    PlayerMovement playerM;
    Warrior warrior;

    bool lvl1, lvl2, lvl3, lvl4, lvl5; //Prove


    public bool Lvl1 { get { return lvl1; } }
    public bool Lvl2 { get { return lvl2; } }
    public bool Lvl3 { get { return lvl3; } }
    public bool Lel4 { get { return lvl4; } }
    public bool Lvl5 { get { return lvl5; } }

    float HealtsPoints
    {
        get
        {
            return _healtsPoints;
        }
        set
        {
            if (value > maxHP)
            {
                _healtsPoints = maxHP;
            }
            else if (value < minHP)
            {
                _healtsPoints = minHP;
            }
            else
            {
                _healtsPoints = value;
            }
        }
    }

    float StaminaPoints
    {
        get
        {
            return _staminaPoints;
        }
        set
        {
            if(value > maxSTA)
            { 
                _staminaPoints = maxSTA;
                exhaustState = false;
            }
            else if(value < minSTA)
            {
                _staminaPoints = value;
                exhaustState = true;
                if(value < overSTA)
                {
                    _staminaPoints = overSTA;
                }
            }
            else
            {
                _staminaPoints = value;
            }
        }
    }

    public bool ExhaustState
    {
        get
        {
            return exhaustState;
        }
    }

    float DarkEnergyPoints
    {
        get 
        { 
            return _darkEnergyPoints;
        }
        set
        { 
            if(value < maxDE)
            {
                _darkEnergyPoints = maxDE;
            }
            else if(value < minDE)
            {
                _darkEnergyPoints = minDE;
            }
            else
            {
                _darkEnergyPoints = value;
            }
        }
    }

    private void Awake()
    {
        if(File.Exists(Application.persistentDataPath + "/Slot1Data")) LoadLvl();
    }

    private void Start()
    {
        recoverySTA = true;
        HealtsPoints = maxHP;
        StaminaPoints = maxSTA;
        vulnerable = true;
        playerM = GetComponent<PlayerMovement>();
        warrior = GetComponentInChildren<Warrior>();
    }

    private void Update()
    {
        UpdateSTA(0f);
        IsDead();
        Timer();
        Debug.Log(StaminaPoints);

        if (DANNO) //per fare test
        {
            UpdateHP(5, true); 
            DANNO = false;
        }       
    }

    public void UpdateSTA(float riduzioneStamina) //Funzione per creare una meccanica di stamina
    {
        if (recoverySTA && !exhaustState) StaminaPoints += Time.deltaTime * 25f;
        else if (recoverySTA && exhaustState) StaminaPoints += Time.deltaTime * 17.5f;

        if (playerM.consumeRun)
        {
            recoverySTA = false;
            StaminaPoints -= Time.deltaTime * 15f;
        }

        if(playerM.consumeDash)
        {
            playerM.consumeDash = false;
            StaminaPoints -= 22.5f;
        }

        if (warrior.isAttacking) recoverySTA = false;

        if (!warrior.isAttacking && !playerM.consumeRun) recoverySTA = true;

        StaminaPoints -= riduzioneStamina;
    }

    public float UpdateHP(float value, bool damage) //Funzione per gestire la vita del player
    {
        if (vulnerable)
        {
            if (damage)
            {
                playerM.anim.SetTrigger("TakeDamage");
                timerInvulnerable = 1.5f;
                InvulnerableStatus(true);
                return HealtsPoints -= value;
            }
        }
        return HealtsPoints += value;
    }

    bool IsDead() //Funzione per gestire lo stato morto del player
    {
        playerM.anim.SetBool("isDead", deadState);
        if (HealtsPoints <= 0)
        {
            return deadState = true;
        }           
        return deadState = false;
    }

    bool InvulnerableStatus(bool invulnerable) //Funzione per invulnerabilitā
    {
        if (invulnerable) return vulnerable = false;
        return vulnerable = true;
    }

    void Timer() //Timer per invulenrabilitā
    {
        timerInvulnerable -= Time.deltaTime;
        if (timerInvulnerable < 0) InvulnerableStatus(false);
    }

    void LoadLvl()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/Slot1Data");

        Game game = JsonUtility.FromJson<Game>(json);

        lvl1 = game.level1;
        lvl2 = game.level2;
        lvl3 = game.level3;
        lvl4 = game.level4;
        lvl5 = game.level5;
    }
}
