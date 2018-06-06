using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public enum UnitAction {Moving, Attacking, Action};
    public enum UnitActionLayoutType {Any, Line};

    #region UNIT_COMPONENTS
    public GameObject unitModel;
    public GameObject unitStateIndicator;
    private Material movementIndicatorMaterial;
    private Material attackIndicatorMaterial;
    #endregion
    
    #region UNIT_INFO
    public string unitName = "Test";
    #endregion

    #region UNIT_STATS
    public UnitActionLayoutType movementLayoutType = UnitActionLayoutType.Any;
    public int moveDistance = 1;
    public UnitActionLayoutType attackLayoutType = UnitActionLayoutType.Any;
    public int minAttackRange = 1;
    public int maxAttackRange = 1;
    public int totalHealth = 1;
    public int currentHealth = 1;
    public int attackStrength = 1;
    #endregion

    #region UNIT_STATE
    public int teamNumber = 0;
    public bool isEnabled = false;
    private bool hasMoved = false;
    private bool hasAttacked = false;
    #endregion

    #region EVENTS
    public delegate void UnitSelectEvent(int xCoor, int yCoor, Unit unit);
    public static event UnitSelectEvent OnUnitSelect;
    public static event UnitSelectEvent OnUnitDeselect;
    
    public delegate void UnitMoveEvent(int xCoor, int yCoor, Unit unit);
    public static event UnitMoveEvent OnUnitMoving;

    public delegate void UnitAttackEvent(int xCoor, int yCoor, Unit unit);
    public static event UnitAttackEvent OnUnitAttacking;
    
    public delegate void UnitRangeInfoEvent(int xCoor, int yCoor, Unit unit);
    public static event UnitRangeInfoEvent OnUnitRangeInfo;

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

        loadIndicatorMaterials();
        SetTeamColor(teamNumber);
	}
	
	// Update is called once per frame
	void Update () {
    }

    void OnDestroy(){
        if(OnUnitDestroy != null){
            OnUnitDestroy(this);
        }
    }

    //Sets the team color. May not seem apparent, but do note that this is network code.
    private void SetTeamColor(int colorValue) {
        Debug.Log("Setting Team Color: " + colorValue);
        int teamColorValue = colorValue;
        switch (colorValue) {
            default:
                foreach(Renderer renderer in GetComponentsInChildren<Renderer>()){
                    foreach(Material mat in renderer.materials){
                        if(mat.name.Contains("TeamColor")){
                            mat.SetColor("_TeamColor", Color.gray);
                        }
                    }
                }
                break;
            case 0:
                foreach(Renderer renderer in GetComponentsInChildren<Renderer>()){
                    foreach(Material mat in renderer.materials){
                        if(mat.name.Contains("TeamColor")){
                            mat.SetColor("_TeamColor", Color.gray);
                        }
                    }
                }
                break;
            case 1:
                foreach(Renderer renderer in GetComponentsInChildren<Renderer>()){
                    foreach(Material mat in renderer.materials){
                        if(mat.name.Contains("TeamColor")){
                            mat.SetColor("_TeamColor", new Color(0, 0.5f, 1));
                        }
                    }
                }
                break;
            case 2:
                foreach(Renderer renderer in GetComponentsInChildren<Renderer>()){
                    foreach(Material mat in renderer.materials){
                        if(mat.name.Contains("TeamColor")){
                            mat.SetColor("_TeamColor", Color.red);
                        }
                    }
                }
                break;
            case 3:
                foreach(Renderer renderer in GetComponentsInChildren<Renderer>()){
                    foreach(Material mat in renderer.materials){
                        if(mat.name.Contains("TeamColor")){
                            mat.SetColor("_TeamColor", Color.green);
                        }
                    }
                }
                break;
        }
    }

    public void moveUnitToGridTile(GridTile tile)
    {
        Vector3 tilePosition = tile.transform.position;
        Vector3 currentPosition = transform.position;

        //Since we have some floating happening and y values might differ, normalize them
        tilePosition.y = 0.0f;
        currentPosition.y = 0.0f;
        
        float angle = Vector3.Angle(transform.forward, tilePosition - currentPosition);
        Vector3 cross = Vector3.Cross(transform.forward, tilePosition - currentPosition);
        if(cross.y < 0)
        {
            angle = -angle;
        }
        transform.Rotate(Vector3.up, angle);
        transform.parent = tile.transform;
        StartCoroutine(Util_TransformManipulation.smoothMovement(gameObject, transform.position, tile.transform.position, 0.05f));
        hasMoved = true;

        unitStateIndicator.GetComponent<Renderer>().material = attackIndicatorMaterial;
        deselectUnit();
    }

    public void selectUnit()
    {
        if(isEnabled){
            enableFloatingAnimation();
            /* We're handling this with Hud actions and hotkeys now
                if(!hasMoved){
                    startMoving();
                } else if(!hasAttacked){
                    startAttacking();
                }
            */
            GridTile tile = GetComponentInParent<GridTile>();
            if(tile != null && OnUnitSelect != null)
            {
                OnUnitSelect(tile.xCoor, tile.yCoor, this);
            }
        }
    }

    public void deselectUnit()
    {
        disableFloatingAnimation();
        GridTile tile = GetComponentInParent<GridTile>();
        if (tile != null && OnUnitDeselect != null)
        {
            OnUnitDeselect(tile.xCoor, tile.yCoor, this);
        }
    }

    public void startMoving(){
        if(!hasMoved && !hasAttacked){
            GridTile tile = GetComponentInParent<GridTile>();
            if(tile != null && OnUnitMoving != null)
            {
                OnUnitMoving(tile.xCoor, tile.yCoor, this);
            }
        }
    }

    public void startAttacking(){
        if(!hasAttacked){
            GridTile tile = GetComponentInParent<GridTile>();
            if(tile != null && OnUnitAttacking != null)
            {
                OnUnitAttacking(tile.xCoor, tile.yCoor, this);
            }
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
        
        Unit tileUnit = tile.getChildUnit();
        if(tileUnit != null){
            tileUnit.takeDamage(attackStrength);
        }
        hasAttacked = true;
        disableActionIndicator();
        deselectUnit();
        GridTile parentTile = GetComponentInParent<GridTile>();
        if(parentTile != null){
            StartCoroutine(Util_TransformManipulation.smoothMovement(gameObject, this.transform.position, parentTile.transform.position, 1.0f));
        } else {
            Debug.LogError("Couldn't find the parent gridtile for this unit!" + unitName);

        }
    }
    
    //@Description - used to inflict damage to this unit
    public void takeDamage(int incomingAttackStrength){
        currentHealth -= incomingAttackStrength;
        if(this.currentHealth <= 0){
            playDestructionParticle();
            Destroy(gameObject);
        }
    }

    public void prepareUnitForTurn(){
        isEnabled = true;
        hasMoved = false;
        hasAttacked = false;
        if(movementIndicatorMaterial == null){
            loadIndicatorMaterials();
        }
        unitStateIndicator.GetComponent<Renderer>().material = movementIndicatorMaterial;
        enableActionIndicator();
    }

    public void cleanpUnitForTurn(){
        isEnabled = false;
        hasMoved = true;
        hasAttacked = true;
        disableActionIndicator();
    }

    private void enableFloatingAnimation(){
        Vector3 floatAbovePosition = transform.position + new Vector3(0, 0.5f, 0);
        StartCoroutine(Util_TransformManipulation.smoothMovement(gameObject, transform.position, floatAbovePosition, 0.05f));
        if(anim_Floating != null){
            anim_Floating.enableAnimation();
        }
    }
    private void disableFloatingAnimation(){
        if(anim_Floating != null){
            anim_Floating.disableAnimation();
        }
    }

    private void enableActionIndicator(){
        unitStateIndicator.SetActive(true);
         StartCoroutine(Util_TransformManipulation.lerpObjToScale(unitStateIndicator, new Vector3(0.25f, 0.25f, 0.25f), 0.5f));
    }

    private void disableActionIndicator(){
        StartCoroutine(Util_TransformManipulation.lerpObjToScale(unitStateIndicator, new Vector3(0.0f, 0.0f, 0.0f), 0.5f));
    }

    private void loadIndicatorMaterials(){
        if(movementIndicatorMaterial == null){
            movementIndicatorMaterial = Resources.Load("Materials/BlueToon") as Material;
        }
        if(attackIndicatorMaterial == null){
            attackIndicatorMaterial = Resources.Load("Materials/RedToon") as Material;
        }
    }

    private void playDestructionParticle(){
        GameObject poof = Resources.Load("Particle Systems/Poof") as GameObject;
        GameObject newPoofObj = Instantiate(poof, this.transform, true);
        ParticleSystem newPoof = newPoofObj.GetComponent<ParticleSystem>();
        newPoof.Play();
        Destroy(newPoof, 2.0f);
    }
}
