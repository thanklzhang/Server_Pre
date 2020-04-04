using System;

public class IgnoreSerializeAttribute : Attribute
{
    public IgnoreSerializeAttribute(string Descrition_in)
    {
        this.description = Descrition_in;
    }
    public IgnoreSerializeAttribute() { }
    protected string description;
    public string Description
    {
        get
        {
            return this.description;
        }
    }
}
