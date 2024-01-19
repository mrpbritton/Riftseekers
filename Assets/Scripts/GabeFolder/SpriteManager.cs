using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public Animator character;
    private CardinalDirection cachedDir;

    #region Sprites

    public bool UpdateSpriteToDash(Vector3 direction)
    {
        bool isDefDash = false;
        if (direction.x > 0)
        {
            if (direction.z < 0) // SOUTHEAST
            {
                //characterSprite.sprite = southEast;
                character.SetTrigger("DashSE");
            }
            else if (direction.z == 0) // EAST
            {
                character.SetTrigger("DashE");
            }
            else // direction.z == 1 *** NORTHEAST
            {
                character.SetTrigger("DashNE");
            }
        }
        else if (direction.x < 0)
        {
            if (direction.z < 0) // SOUTHWEST
            {
                //characterSprite.sprite = southWest;
                character.SetTrigger("DashSW");
            }
            else if (direction.z == 0) // WEST
            {
                character.SetTrigger("DashW");
            }
            else // direction.z == 1 *** NORTHWEST
            {
                //characterSprite.sprite = northWest;
                character.SetTrigger("DashNW");
            }
        }
        else //direction.x == 0
        {
            if (direction.z < 0) // SOUTH
            {
                character.SetTrigger("DashS");
            }
            else if (direction.z == 0) // NO INPUT
            {
                //EAST BY DEFAULT
                character.SetTrigger("DashDef");
                isDefDash = true;
            }
            else // direction.z == 1 *** NORTH
            {
                character.SetTrigger("DashN");
            }
        }

        return isDefDash;
    }

    public void UpdateSpriteToWalk(Vector3 direction)
    {
        if (direction.x > 0)
        {
            if (direction.z < 0) // SOUTHEAST
            {
                //characterSprite.sprite = southEast;
                character.SetTrigger("WalkSE");
                cachedDir = CardinalDirection.southEast;
            }
            else if (direction.z == 0) // EAST
            {
                character.SetTrigger("WalkE");
                cachedDir = CardinalDirection.east;
            }
            else // direction.z == 1 *** NORTHEAST
            {
                character.SetTrigger("WalkNE");
                cachedDir = CardinalDirection.northEast;
            }
        }
        else if (direction.x < 0)
        {
            if (direction.z < 0) // SOUTHWEST
            {
                //characterSprite.sprite = southWest;
                character.SetTrigger("WalkSW");
                cachedDir = CardinalDirection.southWest;
            }
            else if (direction.z == 0) // WEST
            {
                character.SetTrigger("WalkW");
                cachedDir = CardinalDirection.west;
            }
            else // direction.z == 1 *** NORTHWEST
            {
                //characterSprite.sprite = northWest;
                character.SetTrigger("WalkNW");
                cachedDir = CardinalDirection.northWest;
            }
        }
        else //direction.x == 0
        {
            if (direction.z < 0) // SOUTH
            {
                character.SetTrigger("WalkS");
                cachedDir = CardinalDirection.south;
            }
            else if (direction.z == 0) // NO INPUT
            {
                switch (cachedDir.ToString())
                {
                    case "north":
                        character.SetTrigger("WalkNStop");
                        break;
                    case "northEast":
                        character.SetTrigger("WalkNEStop");
                        break;
                    case "northWest":
                        character.SetTrigger("WalkNWStop");
                        break;
                    case "south":
                        character.SetTrigger("WalkSStop");
                        break;
                    case "southEast":
                        character.SetTrigger("WalkSEStop");
                        break;
                    case "southWest":
                        character.SetTrigger("WalkSWStop");
                        break;
                    case "east":
                        character.SetTrigger("WalkEStop");
                        break;
                    case "west":
                        character.SetTrigger("WalkWStop");
                        break;
                    default:
                        break;
                }
            }
            else // direction.z == 1 *** NORTH
            {
                character.SetTrigger("WalkN");
                cachedDir = CardinalDirection.north;
            }
        }
    }

    /// <summary>
    /// Only use this for the default direction.
    /// </summary>
    /// <param name="cachedDir">Should be CardinalDirection.east</param>
    public void UpdateSpriteToIdle(CardinalDirection cachedDir)
    {
        switch (cachedDir.ToString())
        {
            case "north":
                character.SetTrigger("WalkNStop");
                break;
            case "northEast":
                character.SetTrigger("WalkNEStop");
                break;
            case "northWest":
                character.SetTrigger("WalkNWStop");
                break;
            case "south":
                character.SetTrigger("WalkSStop");
                break;
            case "southEast":
                character.SetTrigger("WalkSEStop");
                break;
            case "southWest":
                character.SetTrigger("WalkSWStop");
                break;
            case "east":
                character.SetTrigger("WalkEStop");
                break;
            case "west":
                character.SetTrigger("WalkWStop");
                break;
            default:
                break;
        }
    }
    #endregion
}
