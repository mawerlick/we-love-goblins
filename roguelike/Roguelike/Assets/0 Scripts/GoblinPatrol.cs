using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinPatrol : MonoBehaviour
{
    public int index;
    public DungeonGenerator dG;
    public Vector2Int gridLoc;
    public bool moveable;
    public int direction;
    public int playerSeen = 0;
    public Vector2Int playerDistance;
    private int moveFrame = 0;
    public Animator animator;
    public int health = 20;
    bool attack = false;


    public void Move()
    {
        if (moveFrame != 64)
        {
            if (moveFrame == 0)
            {
                attack = false;
                moveable = false; 
                PlayerSeenCheck();
                direction = RandomDirection();
                if ((gridLoc.y == dG.player.gridLoc.y || gridLoc.y == dG.player.gridLoc.y + 1) && Mathf.Abs(dG.player.gridLoc.x - gridLoc.x) == 1) attack = true;
                if (!attack) { dG.grid[gridLoc.x, gridLoc.y] = 0; animator.SetBool("Moving", true); }
            }
            if (attack) Attack();
            else
            {
                moveFrame++;
                switch (direction)
                {
                    case 2:
                        gameObject.transform.position += new Vector3(0, 1, 0);
                        if (moveFrame == 1) gridLoc.y++;
                        break;
                    case -1:
                        gameObject.transform.position += new Vector3(-1, 0, 0);
                        if (moveFrame == 1) gridLoc.x--;
                        break;
                    case -2:
                        gameObject.transform.position += new Vector3(0, -1, 0);
                        if (moveFrame == 1) gridLoc.y--;
                        break;
                    case 1:
                        gameObject.transform.position += new Vector3(1, 0, 0);
                        if (moveFrame == 1) gridLoc.x++;
                        break;
                }
                if (moveFrame == 1) dG.grid[gridLoc.x, gridLoc.y] = 4 + index;

                Invoke("Move", (float)(Time.deltaTime / dG.player.speed));
            }
            
        }
        else { moveFrame = 0; moveable = true; animator.SetBool("Moving", false); }
    }

    public void Hit()
    {
        CalculatePlayerDistance();
        PlayerSeenCheck();
        if ((playerDistance.y == 1 && playerDistance.x == 0) || (playerDistance.y == 0 && playerDistance.x == dG.player.transform.localScale.x))
        {
            moveable = false;
            health -= 10;
            if (health <= 0)
            {
                animator.Play("Death");
                dG.grid[gridLoc.x, gridLoc.y] = 0;
                dG.goblins.Remove(gameObject);
                Invoke("Die", Time.deltaTime * 240);
                dG.player.point += 10;
                if (Random.Range(0, 6) == 0) dG.player.portalpoint++;
                dG.player.UpdateCurrency();
            }
            else animator.SetTrigger("Hit");
        }
    }

    private void Attack()
    {
        moveable = false;
        animator.SetTrigger("Attack");
        gameObject.transform.localScale = new Vector3(-playerDistance.x, 1, 1);
        dG.player.Hit(playerDistance);
        Invoke("EndAttack", Time.deltaTime * 64);
    }

    public void EndAttack() { moveable = true; }

    private void Die()
    {
        if (!dG.Progressable()) dG.GameOver();
        Destroy(gameObject);
    }

    public int TowardsPlayer()
    {
        Debug.Log("Runn");
        Vector2Int desiredloc = new Vector2Int(dG.player.gridLoc.x, dG.player.gridLoc.y + 1);
        int xDist = desiredloc.x - gridLoc.x;
        int yDist = desiredloc.y - gridLoc.y;
        if (xDist > yDist && xDist < 0) return 1;
        if (xDist > yDist && xDist > 0) return -1;
        if (xDist < yDist && yDist < 0) return 2;
        if (xDist < yDist && yDist > 0) return -2;
        return 0;
    }

    public int RandomDirection()
    {
        List<int> dirs = new List<int>();
        if(gridLoc.x < dG.mapDims.x-1)if (dG.grid[gridLoc.x+1, gridLoc.y] == 0) dirs.Add(1);
        if(gridLoc.x > 0)if (dG.grid[gridLoc.x-1, gridLoc.y] == 0) dirs.Add(-1);
        if(gridLoc.y < dG.mapDims.y-1)if (dG.grid[gridLoc.x, gridLoc.y+1] == 0) dirs.Add(2);
        if(gridLoc.y > 0)if (dG.grid[gridLoc.x, gridLoc.y-1] == 0) dirs.Add(-2);
        if (dirs.Count > 0) return dirs[Random.Range(0, dirs.Count)];
        return 0;
    }

    private void PlayerSeenCheck()
    {
        CalculatePlayerDistance();
        int distX = Mathf.Abs(playerDistance.x);
        int distY = Mathf.Abs(playerDistance.y);
        if ( (distX == 1 && distY == 0) || (distY == 1 && distX == 0)) playerSeen = 2;
        else if (distX <= 5 || distY <= 5) playerSeen = 1;
        else playerSeen = 0;
    }

    private void CalculatePlayerDistance()
    {
        int x = dG.player.gridLoc.x - gridLoc.x;
        int y = dG.player.gridLoc.y - gridLoc.y;
        playerDistance = new Vector2Int(x, y);
    }

    private bool Moveable()
    {
        if (!moveable) return false;
        Vector2Int desiredPos = (direction == 1 || direction == -1) ?
            new Vector2Int(gridLoc.x + direction, gridLoc.y) :
            new Vector2Int(gridLoc.x, gridLoc.y + direction / 2);
        if (desiredPos.x < 0 || desiredPos.x >= dG.mapDims.x || desiredPos.y < 0 || desiredPos.y >= dG.mapDims.y) return false;
        if (dG.grid[desiredPos.x, desiredPos.y] == 2) dG.GenerateDungeon();
        else if (dG.grid[desiredPos.x, desiredPos.y] == 0) return true;
        return false;
    }
}
