using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util_TransformManipulation {

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
        yield return null;
    }
}
