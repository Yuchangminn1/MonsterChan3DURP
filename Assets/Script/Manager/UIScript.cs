using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public static UIScript instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIScript>();
            }

            return m_instance;
        }
    }
    //싱글턴이 할당될 변수
    [SerializeField] GameObject[] uiObject;
    private static UIScript m_instance;
    [SerializeField] Slider hpBar;
    [SerializeField] Image hpBarImage;
    [SerializeField] Image[] hpHealNumImage;
    [SerializeField] ParticleSystem[] playerParticle;

    public int[] playerAttackDagame;
    public int[] playerGroggyDagame;

    public int[] enemyAttackDagame;
    public int[] bossAttackDagame;

    // Start is called before the first frame update
    void Awake()
    {
        playerAttackDagame = new int[1];
        playerAttackDagame[0] = 20;
        enemyAttackDagame = new int[1];
        enemyAttackDagame[0] = 10;
        bossAttackDagame = new int[3];
        bossAttackDagame[0] = 20;
        bossAttackDagame = new int[1];
        bossAttackDagame[0] = 60;
        bossAttackDagame = new int[1];
        bossAttackDagame[0] = 40;
    }
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UIOnOff(int _index,bool _tf)
    {
        if (uiObject[_index] != null)
        {
            uiObject[_index].SetActive(_tf);
        }
    }
    public void HpBarValue(int _hp, int _hpMax)
    {
        PlayerEffect(0);
        hpBar.value = (float)_hp/ _hpMax;
        if (hpBar.value <= 0.01)
        {
            hpBarImage.enabled = false;
        }
    }
    public void PlayerEffect(int _index)
    {
        if(playerParticle[_index] != null)
        {
            playerParticle[_index].Play();
        }

    }
    public void HpBarReset(int _hp,int _maxHp)
    {
        hpBarImage.enabled = true;
        float hpv =(float) _hp / (float)_maxHp;
        if (hpv > 1f)
        {
            hpv = 1f;
        }
        StartCoroutine(ResetHp(hpv));
    }

    public void HPHealNumIcon(int _index)
    {
        if (_index < 0)
            return;
        if (hpHealNumImage[_index] != null ) 
        {
            hpHealNumImage[_index].enabled = false;
        }
    }
    public int ResetHPHealNumIcon(int _index)
    {
        int i = 0;
        foreach (Image _hpImage in hpHealNumImage)
        {
            if (_index <= 0)
                return i;
            if (_hpImage.enabled == false)
            {
                _hpImage.enabled = true;
                _index -= 1;
            }
            ++i;
        }
        //return i;
        return hpHealNumImage.Length;
    }
    IEnumerator ResetHp(float _hp)
    {
        Debug.Log("_hp = " + _hp);
        while (hpBar.value < _hp)
        {
            int count = 0;
            while (count < 2)
            {
                hpBar.value += 0.01f;
                ++count;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    //public void HpBarReset()
    //{
    //    hpBarImage.enabled = true;
    //    StartCoroutine(ResetHp());
    //}
}
