using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int enemyID;
    public int reward = 3;
    public int damage = 5;

    public int GetEnemyID(){
        return enemyID;
    }

    public void SetEnemyID(int _id){
        enemyID = _id;
    }

    public int GetReward(){
        return reward;
    }

    public void SetReward(int _reward){
        reward = _reward;
    }

    public int GetDamage(){
        return damage;
    }

    public void SetDamage(int _damage){
        damage = _damage;
    }
}
