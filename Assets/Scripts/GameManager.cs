using UnityEngine;

public class GameManager : MonoBehaviour {

    #region GAME_STATE
    public int turnNumber = 0;
    #endregion
    void Start(){
        //TODO: Initialize mainmenu

    }

    void Update(){
    }

    public void loadEditor(){
        //TODO load editor scene
    }

    public void startLevel(){
        ++turnNumber;
    }
}