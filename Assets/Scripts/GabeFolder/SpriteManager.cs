using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public Animator character;
    public CardinalDirection CachedDir { get; private set; }

    public Vector3 CachedDirToVector()
    {
        switch (CachedDir.ToString())
        {
            case "north":
                return new Vector3(0, 0, 1);
            case "northEast":
                return new Vector3(1, 0, 1);
            case "northWest":
                return new Vector3(-1, 0, 1);
            case "south":
                return new Vector3(0, 0, -1);
            case "southEast":
                return new Vector3(1, 0, -1);
            case "southWest":
                return new Vector3(-1, 0, -1);
            case "east":
                return new Vector3(1, 0, 0);
            case "west":
                return new Vector3(-1, 0, 0);
            default:
                return new Vector3(1, 0, 0);
        }

    }

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
                CachedDir = CardinalDirection.southEast;
            }
            else if (direction.z == 0) // EAST
            {
                character.SetTrigger("WalkE");
                CachedDir = CardinalDirection.east;
            }
            else // direction.z == 1 *** NORTHEAST
            {
                character.SetTrigger("WalkNE");
                CachedDir = CardinalDirection.northEast;
            }
        }
        else if (direction.x < 0)
        {
            if (direction.z < 0) // SOUTHWEST
            {
                //characterSprite.sprite = southWest;
                character.SetTrigger("WalkSW");
                CachedDir = CardinalDirection.southWest;
            }
            else if (direction.z == 0) // WEST
            {
                character.SetTrigger("WalkW");
                CachedDir = CardinalDirection.west;
            }
            else // direction.z == 1 *** NORTHWEST
            {
                //characterSprite.sprite = northWest;
                character.SetTrigger("WalkNW");
                CachedDir = CardinalDirection.northWest;
            }
        }
        else //direction.x == 0
        {
            if (direction.z < 0) // SOUTH
            {
                character.SetTrigger("WalkS");
                CachedDir = CardinalDirection.south;
            }
            else if (direction.z == 0) // NO INPUT
            {
                switch (CachedDir.ToString())
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
                CachedDir = CardinalDirection.north;
            }
        }
    }

    public void UpdateSpriteToIdle()
    {
        switch (CachedDir.ToString())
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
