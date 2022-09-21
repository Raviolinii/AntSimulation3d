using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntWarrior : Ant
{
    // Fight
    float anthillAttackSpeed = 2f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //SetOwner(Owner.AI);
        hp = 10;
        dmg = 5;
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            previousTile = currentTile;
            currentTile = other.transform.position;

            tileScript = other.GetComponent<Tile>();
            tileScript.AddWarriorPheromone(pheromoneLeaveAmount);
            surroundings = tileScript.GetSurroundingsNulls(chosenMoveIndex);

            GoToNextTile();
        }

        if (other.CompareTag("AntWorker") || other.CompareTag("AntWarrior"))
        {
            var antScript = other.GetComponent<Ant>();
            if (antScript.GetOwner() != _owner)
                AntDetected(antScript);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("AntWorker") || other.CompareTag("AntWarrior"))
        {
            var antScript = other.GetComponent<Ant>();
            if (antScript.GetOwner() != _owner)
                AntDetected(antScript);
        }
        if (other.CompareTag("Anthill"))
        {
            var anthillScript = other.GetComponent<Anthill>();
            if (anthillScript.GetOwner() != _owner)
                AnthillDetected(anthillScript);
        }
    }


    // Move
    protected override void ChoseMoveIndex()
    {
        int?[] pheromoneValues = new int?[8];
        int sum = 0;
        int rand;
        for (int i = 0; i < 8; i++)
        {
            if (surroundings[i] != null)
            {
                if (surroundings[i].GetSpawnedObjectType() == SpawnedObject.anthill)
                {
                    Anthill anthillScript = surroundings[i].GetSpawnedObject().GetComponent<Anthill>();
                    if (anthillScript.GetOwner() != _owner)
                    {
                        chosenMoveIndex = i;
                        return;
                    }
                }
                else if (surroundings[i].GetSpawnedObjectType() == SpawnedObject.no)
                {
                    pheromoneValues[i] = surroundings[i].GetWarriorPheromoneValue();
                    sum += (int)pheromoneValues[i] + 1;
                    pheromoneValues[i] = sum;
                }
            }
        }

        rand = Random.Range(1, sum + 1);
        int index = FindIndex(pheromoneValues, rand);
        ChosenIndexValidation(index);
    }

    protected override void ChosenIndexValidation(int index)
    {
        if (index != -1)
            chosenMoveIndex = index;
        else
        {
            surroundings = tileScript.GetSurroundings();
            ChoseMoveIndex();
        }
    }


    // Fight
    void AntDetected(Ant ant)
    {
        Fight(ant);
        ant.Fight(this);
    }

    void AnthillDetected(Anthill anthill)
    {
        anthill.Alarm();
        anthill.Fight(this);
        Fight(anthill);
    }

    void Fight(Anthill anthill)
    {
        if (!inFight)
        {
            //Debug.Log("InFight");
            inFight = true;
            animator.SetBool(isMoving, false);
            agent.isStopped = true;
            attackCoroutine = StartCoroutine(AttackAnthill(anthill));
        }
    }

    IEnumerator AttackAnthill(Anthill anthill)
    {
        animator.SetBool(isAttacking, true);
        yield return new WaitForSeconds(attackSpeed);
        animator.SetBool(isAttacking, false);

        inFight = false;
        agent.isStopped = false;
        if (anthill != null)
        {
            anthill.DecreseHp(dmg);
            if (anthill.GetHp() > 0)
                Fight(anthill);
        }
    }
}
