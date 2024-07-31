using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extensions
{

    public static T GetRandom<T>(this T[] array)
	{
		return array[Random.Range(0, array.Length)];
	}

	public static void AddEventTrigger(this Transform transform, EventTriggerType triggerType,
	   UnityEngine.Events.UnityAction<BaseEventData> function)
	{
		var onEnterTrigger = new EventTrigger.Entry();
		EventTrigger trigger = transform.gameObject.AddComponent<EventTrigger>();
		onEnterTrigger.eventID = triggerType;
		onEnterTrigger.callback.AddListener(function);
		trigger.triggers.Add(onEnterTrigger);
	}

	public static bool Contains<T>(this T[] array, System.Func <T,bool> p)
	{
		return array.Where(p).Any();
	}

}
