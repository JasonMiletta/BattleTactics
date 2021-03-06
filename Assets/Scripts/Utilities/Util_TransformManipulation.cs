﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util_TransformManipulation {

    #region EVENTS
    public delegate void LerpEvent();
    public static event LerpEvent OnLerpBegin;
    public static event LerpEvent OnLerpComplete;
    #endregion

	public static IEnumerator lerpObjToScale(GameObject obj, Vector3 targetScale, float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, targetScale, duration);
            yield return null;
        }
        obj.transform.localScale = targetScale;
        if(targetScale == Vector3.zero)
        {
            obj.SetActive(false);
        }
        if(OnLerpComplete != null){
            OnLerpComplete();
        }
    }

    public static IEnumerator smoothMovement(GameObject obj, Vector3 source, Vector3 destination, float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            obj.transform.position = Vector3.Lerp(source, destination, (Time.time - startTime) / duration);
            yield return null;
        }
        obj.transform.position = destination;
        yield return null;
    }
}
