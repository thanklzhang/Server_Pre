using System;

public class PrimaryKeyAttribute : Attribute
{
    public PrimaryKeyAttribute(string Descrition_in)
    {
        this.description = Descrition_in;
    }
    public PrimaryKeyAttribute() { }
    protected string description;
    public string Description
    {
        get
        {
            return this.description;
        }
    }
}
