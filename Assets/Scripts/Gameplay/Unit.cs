using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public GameObject unitModel;

    #region UNIT_INFO
    public string unitName = "Test";
    #endregion

    #region UNIT_STATS
    public int moveDistance = 1;
    public int minAttackRange = 1;
    public int maxAttackRange = 1;
    public int totalHealth = 1;
    public int currentHealth = 1;
    public int attackStrength = 1;
    #endregion

    #region UNIT_STATE
    public int teamNumber = 0;
    private bool hasMoved = false;
    private bool hasAttacked = false;
    #endregion

    public Material selectedMaterial;

    private Material originalMaterial;

    #region EVENTS
    public delegate void UnitSelectEvent(int xCoor, int yCoor, int moveDistance);
    public static event UnitSelectEvent OnUnitSelect;
    public static event UnitSelectEvent OnUnitDeselect;

    public delegate void UnitCreateEvent(Unit unit);
    public static event UnitCreateEvent OnUnitCreate;

    public delegate void UnitDestroyEvent(Unit unit);
    public static event UnitDestroyEvent OnUnitDestroy;
    #endregion

    #region ANIMATIONS
    private Anim_Floating anim_Floating;
    #endregion

    // Use this for initialization
    void Start () {
        if(OnUnitCreate != null){
            OnUnitCreate(this);
        }
        anim_Floating = GetComponentInChildren<Anim_Floating>();
	}
	
	// Update is called once per frame
	void Update () {
    }

    void OnDestroy(){
        if(OnUnitDestroy != null){
            OnUnitDestroy(this);
        }
    }

    public void moveUnitToGridTile(GridTile tile)
    {
        float angle = Vector3.Angle(transform.forward, tile.transform.position - transform.position);
        Vector3 cross = Vector3.Cross(transform.forward, tile.transform.position - transform.position);
        if(cross.y < 0)
        {
            angle = -angle;
        }
        transform.Rotate(Vector3.up, angle);
        transform.parent = tile.transform;
        StartCoroutine(smoothMovementCoRoutine(transform.position, tile.transform.position, 0.05f));
        deselectUnit();
    }

    private IEnumerator smoothMovementCoRoutine(Vector3 source, Vector3 destination, float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            transform.position = Vector3.Lerp(source, destination, (Time.time - startTime) / duration);
            yield return null;
        }
        transform.position = destination;
        yield return null;
    }

    public void selectUnit()
    {
        toggleFloatingAnimation();
        GridTile tile = GetComponentInParent<GridTile>();
        if(tile != null && OnUnitSelect != null)
        {
            OnUnitSelect(tile.xCoor, tile.yCoor, moveDistance);
        }
    }

    public void deselectUnit()
    {
        toggleFloatingAnimation();
        GridTile tile = GetComponentInParent<GridTile>();
        if (tile != null && OnUnitDeselect != null)
        {
            OnUnitDeselect(tile.xCoor, tile.yCoor, moveDistance);
        }
    }

    //@Description - Used when inflicting damage to another unit
    public void attackTile(GridTile tile){
        GameObject poof = Resources.Load("Particle Systems/Poof") as GameObject;
        GameObject newPoofObj = Instantiate(poof, tile.transform.position, poof.transform.rotation, tile.transform);
        ParticleSystem newPoof = newPoofObj.GetComponent<ParticleSystem>();
        newPoof.Play();
        Destroy(newPoof, 2.0f);
        Debug.Log("Attacking Tile! " + tile.name);
        deselectUnit();
    }
    
    //@Description - used to inflict damage to this unit
    public void takeDamage(int incomingAttackStrength){
        currentHealth -= incomingAttackStrength;
    }

    private void toggleFloatingAnimation(){
        if(anim_Floating != null){
            anim_Floating.toggleAnimation();
        }
    }
}
