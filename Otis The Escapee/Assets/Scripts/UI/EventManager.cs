public static class EventManager
{
    public delegate void VoidObj (object[] values);
    public delegate void VoidSound(Sound[] values);
    public delegate void VoidStr(string[] values);

    /// <summary>
    /// Holds all events that will be passed objects when calling ObjectEvent()
    /// DO NOT CALL DIRECTLY only use += or -=
    /// </summary>
    public static event VoidObj PassObjects;

    /// <summary>
    /// Holds all events that will be passed objects when calling SoundEvent()
    /// DO NOT CALL DIRECTLY only use += or -=
    /// </summary>
    public static event VoidSound PassSounds;

    /// <summary>
    /// Holds all events that will be passed objects when calling StringEvent()
    /// DO NOT CALL DIRECTLY only use += or -=
    /// </summary>
    public static event VoidStr PassStrings;

    /// <summary>
    /// Runs the PassObjects event
    /// </summary>
    /// <param name="values">The objects to be passed along to all who are subscribed.</param>
    public static void ObjectEvent(object[] values)
    {
        if (PassObjects != null)
            PassObjects(values);
    }

    /// <summary>
    /// Runs the PassSounds event.
    /// </summary>
    /// <param name="values">The sounds to be passed along to all who are subscribed.</param>
    public static void SoundEvent(Sound[] values)
    {
        if (PassSounds != null)
            PassSounds(values);
    }

    /// <summary>
    /// Runs the PassStrings event.
    /// </summary>
    /// <param name="values">The strings to be passed along to all who are subscribed.</param>
    public static void StringEvent(string[] values)
    {
        if (PassStrings != null)
            PassStrings(values);
    }
}
