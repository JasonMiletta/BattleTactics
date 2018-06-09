using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Anim_OpenCloseForm : MonoBehaviour {
	#region PARAMETERS
    //Animator State and Transition names we need to check against.
    public const string animatorParameterName_Open = "Open";
    public const string animatorStateName_Closed = "Closed";
	#endregion

    //Currently Open Screen
    public Animator actionPanelAnimator;

    //Hash of the parameter we use to control the transitions.
    private int animatorParameter_Open;

    public void OnEnable()
    {
        //We cache the Hash to the "Open" Parameter, so we can feed to Animator.SetBool.
        animatorParameter_Open = Animator.StringToHash (animatorParameterName_Open);
    }

    //Closes the currently open panel and opens the provided one.
    //It also takes care of handling the navigation, setting the new Selected element.
    public void OpenPanel ()
    {
		if(actionPanelAnimator == null){
			Debug.LogError("Anim_OpenCloseForm: Cant open form, actionPanelAnimator is null");
			return;
		}
		//Activate the new Screen hierarchy so we can animate it.
		actionPanelAnimator.gameObject.SetActive(true);

		//Start the open animation
		actionPanelAnimator.SetBool(animatorParameter_Open, true);
	}
    //Closes the currently open Screen
    //It also takes care of navigation.
    //Reverting selection to the Selectable used before opening the current screen.
    public void ClosePanel()
    {
		if(actionPanelAnimator == null){
			Debug.LogError("Anim_OpenCloseForm: Cant close form, actionPanelAnimator is null");
			return;
		}

        //Start the close animation.
        actionPanelAnimator.SetBool(animatorParameter_Open, false);

        //Start Coroutine to disable the hierarchy when closing animation finishes.
        StartCoroutine(DisablePanelDelayed(actionPanelAnimator));
    }

    //Coroutine that will detect when the Closing animation is finished and it will deactivate the
    //hierarchy.
    IEnumerator DisablePanelDelayed(Animator anim)
    {
        bool closedStateReached = false;
        bool wantToClose = true;
        while (!closedStateReached && wantToClose)
        {
            if (!anim.IsInTransition(0))
                closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(animatorStateName_Closed);

            wantToClose = !anim.GetBool(animatorParameter_Open);

            yield return new WaitForEndOfFrame();
        }

        if (wantToClose)
            anim.gameObject.SetActive(false);
    }
}
