using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class EnemyAnimation : MonoBehaviour
{
    public Animator character;
    public SpriteRenderer characterSprite;
    private CardinalDirection cachedDir;
    [SerializeField]
    public NavMeshAgent agent;
    [SerializeField]
    private GameObject model;

    private void Update()
    {
        model.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);

        float x = transform.rotation.eulerAngles.y;
        if (x > 337.5 & x <= 360 | x >= 0 & x <= 22.5)
        {
            cachedDir = CardinalDirection.north;
        }
        else if (x > 22.5 & x <= 67.5)
        {
            cachedDir = CardinalDirection.northEast;
        }
        else if (x > 67.5 & x <= 112.5)
        {
            cachedDir = CardinalDirection.east;
        }
        else if (x > 112.5 & x <= 157.5)
        {
            cachedDir = CardinalDirection.southEast;
        }
        else if (x > 157.5 & x <= 202.5)
        {
            cachedDir = CardinalDirection.south;
        }
        else if (x > 202.5 & x <= 247.5)
        {
            cachedDir = CardinalDirection.southWest;
        }
        else if (x > 247.5 & x <= 292.5)
        {
            cachedDir = CardinalDirection.west;
        }
        else if (x > 292.5 & x <= 337.5)
        {
            cachedDir = CardinalDirection.northWest;
        }


        switch (cachedDir.ToString())
        {
            case "north":
                if(agent.velocity.magnitude < 0.5 || !GetComponent<EnemyMovement>().target)
                {
                    character.SetTrigger("StandN");
                    break;
                }

                if (!GetComponent<EnemyMelee>() && GetComponentInChildren<EnemyFiring>().firing == true)
                {
                    character.SetTrigger("StandN");
                    break;
                }

                character.SetTrigger("WalkN");
                break;

            case "northEast":
                if (agent.velocity.magnitude < 0.5)
                {
                    character.SetTrigger("StandNE");
                    break;
                }

                if (!GetComponent<EnemyMelee>() && GetComponentInChildren<EnemyFiring>().firing == true)
                {
                    character.SetTrigger("StandNE");
                    break;
                }

                character.SetTrigger("WalkNE");
                break;

            case "east":
                if (agent.velocity.magnitude < 0.5)
                {
                    character.SetTrigger("StandE");
                    break;
                }

                if (!GetComponent<EnemyMelee>() && GetComponentInChildren<EnemyFiring>().firing == true)
                {
                    character.SetTrigger("StandE");
                    break;
                }

                character.SetTrigger("WalkE");
                break;

            case "southEast":
                if (agent.velocity.magnitude < 0.5)
                {
                    character.SetTrigger("StandSE");
                    break;
                }

                if (!GetComponent<EnemyMelee>() && GetComponentInChildren<EnemyFiring>().firing == true)
                {
                    character.SetTrigger("StandSE");
                    break;
                }

                character.SetTrigger("WalkSE");
                break;

            case "south":
                if (agent.velocity.magnitude < 0.5)
                {
                    character.SetTrigger("StandS");
                    break;
                }

                if (!GetComponent<EnemyMelee>() && GetComponentInChildren<EnemyFiring>().firing == true)
                {
                    character.SetTrigger("StandS");
                    break;
                }

                character.SetTrigger("WalkS");
                break;

            case "southWest":
                if (agent.velocity.magnitude < 0.5)
                {
                    character.SetTrigger("StandSW");
                    break;
                }

                if (!GetComponent<EnemyMelee>() && GetComponentInChildren<EnemyFiring>().firing == true)
                {
                    character.SetTrigger("StandSW");
                    break;
                }

                character.SetTrigger("WalkSW");
                break;

            case "west":
                if (agent.velocity.magnitude < 0.5)
                {
                    character.SetTrigger("StandW");
                    break;
                }

                if (!GetComponent<EnemyMelee>() && GetComponentInChildren<EnemyFiring>().firing == true)
                {
                    character.SetTrigger("StandW");
                    break;
                }

                character.SetTrigger("WalkW");
                break;

            case "northWest":
                if (agent.velocity.magnitude < 0.5)
                {
                    character.SetTrigger("StandNW");
                    break;
                }

                if (!GetComponent<EnemyMelee>() && GetComponentInChildren<EnemyFiring>().firing == true)
                {
                    character.SetTrigger("StandNW");
                    break;
                }

                character.SetTrigger("WalkNW");
                break;

            default:
                break;
        }

    }
}
