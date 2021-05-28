using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerConroller : MonoBehaviour
{
    public DungeonGenerator dG;
    [SerializeField]
    public Vector2Int gridLoc;
    public Camera mainCam;
    public float speed = 2f;
    public Animator animator;
    private int moveFrame = 0;
    public int direction = 1;
    private bool moveable = true;
    private bool hitable = true;
    [SerializeField]
    public StatDisplay display;

    public int point;
    public int health = 50;
    public int portalpoint = 5;
    public gameover gmscreen;

    void Start()
    {
        point = 0;
        gameObject.GetComponent<CharRandomizer>().RandomizeChar();
        UpdateCurrency();

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) && moveable) { direction = 2; if (Moveable()) { Invoke("Move", Time.deltaTime / speed); moveable = false; } }
        if (Input.GetKey(KeyCode.A) && moveable) { direction = -1; gameObject.transform.localScale = new Vector3(1, 1, 1); if (Moveable()) { Invoke("Move", Time.deltaTime / speed); moveable = false; } }
        if (Input.GetKey(KeyCode.S) && moveable) { direction = -2; if (Moveable()) { Invoke("Move", Time.deltaTime / speed); moveable = false; } }
        if (Input.GetKey(KeyCode.D) && moveable) { direction = 1; gameObject.transform.localScale = new Vector3(-1, 1, 1); if (Moveable()) { Invoke("Move", Time.deltaTime / speed); moveable = false; } }
        if (Input.GetKeyDown(KeyCode.Space) && moveable) { Attack(); }
        
    }

    public void Move()
    {
        if (moveFrame != 64)
        {
            if (moveFrame == 0)
            {
                moveable = false; animator.SetBool("Moving", true);
                dG.grid[gridLoc.x, gridLoc.y] = 0;
            }
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
            if (moveFrame == 1) { dG.grid[gridLoc.x, gridLoc.y] = 1; dG.MoveGoblins(); }//Debug.Log(dG.grid[gridLoc.x, gridLoc.y - 1]);
            mainCam.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, mainCam.transform.position.z);
            Invoke("Move", (float)(Time.deltaTime / speed));
        }
        else { moveFrame = 0; if (health > 0) moveable = true; animator.SetBool("Moving", false);  }
    }


    public void Hit(Vector2Int goblinDistance)
    {
        if ((goblinDistance.y == 1 && goblinDistance.x == 0) || (goblinDistance.y == 0 && goblinDistance.x == dG.player.transform.localScale.x))
        {
            moveable = false;
            health -= 10;
            UpdateCurrency();
            if (health <= 0)
            {
                animator.Play("Death");
                Invoke("Die", Time.deltaTime * 240);
            }
            else if (hitable)
            {
                animator.SetTrigger("Hit");
            }
        }
    }

    private void Die()
    {
        if (health <= 0)
        {

            gmscreen.Setup(point);
        }
    }

    private bool Moveable()
    {
        if (!moveable) return false;
        Vector2Int desiredPos = (direction == 1 || direction == -1) ?
            new Vector2Int(gridLoc.x + direction, gridLoc.y) :
            new Vector2Int(gridLoc.x, gridLoc.y + direction / 2);
        foreach (GameObject p in dG.portals) p.GetComponent<SpriteRenderer>().sortingOrder = (p.transform.position.y > gameObject.transform.position.y + 64) ? 19 : 50;
        if (desiredPos.x < 0 || desiredPos.x >= dG.mapDims.x || desiredPos.y < 0 || desiredPos.y >= dG.mapDims.y) return false;

        if (dG.grid[desiredPos.x, desiredPos.y] == 2) { if (portalpoint > 0) { dG.GenerateDungeon(); portalpoint--; UpdateCurrency(); }
            else
            {
                return false;
            }
        }


        if (dG.grid[desiredPos.x, desiredPos.y] == 3) { dG.LootChest(desiredPos); return true; }
        else if (dG.grid[desiredPos.x, desiredPos.y] == 0) return true;
        return false;
    }

    private void Attack()
    {
        
        moveable = false;
        animator.SetTrigger("Attack"); direction = 0;
        int dir = (int)gameObject.transform.localScale.x;

        foreach (GameObject g in dG.goblins.ToArray()) { g.GetComponent<GoblinPatrol>().moveable = true;  g.GetComponent<GoblinPatrol>().Hit(); }

        dG.MoveGoblins();
        Invoke("EndAttack", Time.deltaTime*64); 
    }

    public void UpdateCurrency()
    {
        if (health < 0) health = 0;
        display.health.text = health.ToString();
        display.point.text = point.ToString();
        display.portal.text = portalpoint.ToString();
    }
    
    public void EndAttack() { moveable = true; }
}

[System.Serializable]
public struct StatDisplay
{
    public TextMeshProUGUI health;
    public TextMeshProUGUI point;
    public TextMeshProUGUI portal;

}