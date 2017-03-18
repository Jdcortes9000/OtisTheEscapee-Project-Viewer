using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class EventScheduler : MonoBehaviour
{

    private enum EventType
    {
        String,
        Sound,
        Object
    }

    private class scheduledEvent
    {
        public object[] passedValues;
        public EventType ValueType;
        public float timeToSend;

        public scheduledEvent(object[] values, EventType type, float TimeToSend)
        {
            passedValues = values;
            ValueType = type;
            timeToSend = TimeToSend;
        }
    }

    System.Collections.Generic.List<scheduledEvent> scheduledEvents = new List<scheduledEvent>();
    
	void Update ()
    {
	    foreach (var e in scheduledEvents)
	    {
	        e.timeToSend -= Time.deltaTime;
	        if (!(e.timeToSend <= 0)) continue;
	        switch (e.ValueType)
	        {
	            case EventType.String:
	                EventManager.StringEvent(e.passedValues as string[]);
	                break;
	            case EventType.Sound:
	                EventManager.SoundEvent(e.passedValues as Sound[]);
	                break;
	            case EventType.Object:
	                EventManager.ObjectEvent(e.passedValues);
	                break;
	            default:
	                throw new ArgumentOutOfRangeException();
	        }
	        scheduledEvents.Remove(e);
	    }
    }

    public void ScheduleStringEvent(string[] values, float timeToEvent)
    {
        scheduledEvents.Add(new scheduledEvent(values, EventType.String, timeToEvent));
    }
    public void ScheduleObjectEvent(object[] values, float timeToEvent)
    {
        scheduledEvents.Add(new scheduledEvent(values, EventType.Object, timeToEvent));
    }
    public void ScheduleSoundEvent(Sound[] values, float timeToEvent)
    {
        scheduledEvents.Add(new scheduledEvent(values, EventType.Sound, timeToEvent));
    }
}
