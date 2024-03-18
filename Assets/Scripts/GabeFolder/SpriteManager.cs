using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : Singleton<SpriteManager>
{
    public Animator character;
    public CardinalDirection CachedDir { get; private set; }

    #region Helper Functions
    public Vector3 CachedDirToVector3()
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

    public CardinalDirection Vector3ToCardinal(Vector3 direction)
    {
        if (direction.x > 0)
        {
            if (direction.z < 0) // SOUTHEAST
            {
                return CardinalDirection.southEast;
            }
            else if (direction.z == 0) // EAST
            {
                return CardinalDirection.east;
            }
            else // direction.z == 1 *** NORTHEAST
            {
                return CardinalDirection.northEast;
            }
        }
        else if (direction.x < 0)
        {
            if (direction.z < 0) // SOUTHWEST
            {
                return CardinalDirection.southWest;
            }
            else if (direction.z == 0) // WEST
            {
                return CardinalDirection.west;
            }
            else // direction.z == 1 *** NORTHWEST
            {
                return CardinalDirection.northWest;
            }
        }
        else //direction.x == 0
        {
            if (direction.z < 0) // SOUTH
            {
                return CardinalDirection.south;
            }
            else if (direction.z == 0) // NO INPUT
            {
                return CardinalDirection.east;
            }
            else // direction.z == 1 *** NORTH
            {
                return CardinalDirection.north;
            }
        }
    }
    #endregion

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
        switch (CachedDir)
        {
            case CardinalDirection.north:
                character.SetTrigger("WalkNStop");
                break;
            case CardinalDirection.northEast:
                character.SetTrigger("WalkNEStop");
                break;
            case CardinalDirection.northWest:
                character.SetTrigger("WalkNWStop");
                break;
            case CardinalDirection.south:
                character.SetTrigger("WalkSStop");
                break;
            case CardinalDirection.southEast:
                character.SetTrigger("WalkSEStop");
                break;
            case CardinalDirection.southWest:
                character.SetTrigger("WalkSWStop");
                break;
            case CardinalDirection.east:
                character.SetTrigger("WalkEStop");
                break;
            case CardinalDirection.west:
                character.SetTrigger("WalkWStop");
                break;
            default:
                character.SetTrigger("WalkEStop");
                break;
        }
    }

    public void UpdateSpriteToIdle(Vector3 direction)
    {
        switch (Vector3ToCardinal(direction))
        {
            case CardinalDirection.north:
                character.SetTrigger("WalkNStop");
                break;
            case CardinalDirection.northEast:
                character.SetTrigger("WalkNEStop");
                break;
            case CardinalDirection.northWest:
                character.SetTrigger("WalkNWStop");
                break;
            case CardinalDirection.south:
                character.SetTrigger("WalkSStop");
                break;
            case CardinalDirection.southEast:
                character.SetTrigger("WalkSEStop");
                break;
            case CardinalDirection.southWest:
                character.SetTrigger("WalkSWStop");
                break;
            case CardinalDirection.east:
                character.SetTrigger("WalkEStop");
                break;
            case CardinalDirection.west:
                character.SetTrigger("WalkWStop");
                break;
            default:
                character.SetTrigger("WalkEStop");
                break;
        }
    }
    #endregion
}
