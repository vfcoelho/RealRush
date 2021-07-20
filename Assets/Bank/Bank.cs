using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;
    [SerializeField] int currentBalance;
    void Awake(){
        currentBalance = startingBalance;
    }
    public int CurrentBalance{get{return currentBalance;}}

    public void Deposit(int amount){
        currentBalance += Mathf.Abs(amount);
    }
    public void Withdrawal(int amount){
        currentBalance -= Mathf.Abs(amount);
    }
}
