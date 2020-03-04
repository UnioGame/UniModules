using System;

public class ResourceAttribute : Attribute
{
    public string Address;
    public ResourceAttribute(string address)
    {
        this.Address = address;
    }
}