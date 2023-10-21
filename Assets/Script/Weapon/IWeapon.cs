using UnityEngine;

public class IGreatSword : IWeapon
{
    public int equipNum { get { return 0; } }
    public int Damage { get { return 5; } set { Damage = value; } }
    public void Equip()
    {
        Debug.Log("1");

    }
    public void ChangeHand()
    {
        Debug.Log("1");

    }

    public void Attack()
    {
        Debug.Log("1");
    }
    
    
}
public interface IWeapon
{
    public int equipNum { get;}

    public int Damage { get; set;}
    public void Equip();
    public void ChangeHand();

    public void Attack();
}