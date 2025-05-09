using System;

public class ReminderUIUpdate : IEvent
{
    public Boolean isShow=false;

    public ReminderUIUpdate(Boolean isShow)
    {
        this.isShow=isShow;
    }

    public void OnEvent()
    {
    }
}