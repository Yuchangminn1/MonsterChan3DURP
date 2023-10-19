using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gard : MonoBehaviour
{
    public int gardDamage;
    public int gardgroggyDamage;

    // Start is called before the first frame update
    void Start()
    {
        if(gardDamage == 0 )
        {
            gardDamage = 5;
        }
        if (gardgroggyDamage == 0)
        {
            gardgroggyDamage = 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Enemy")
    //    {
    //        Debug.Log("공격 성공");
    //        collision.GetComponent<Enemy>().Hit(gardDamage);
    //    }
    //    if (collision.tag == "Boss")
    //    {
    //        Debug.Log("공격 성공");
    //        collision.GetComponent<Boss>().Hit(gardDamage);
    //    }
    //}
}
