using System;
using System.Collections.Generic;
using System.Web;


public class VariableCollection
{
    private Dictionary<string, string> _collection = new Dictionary<string, string>();

    public VariableCollection() { }

    public void Add(string variable, string value)
    {
        this._collection.Add(variable, value);
    }

    public Dictionary<string, string> GetCollection()
    {
        return this._collection;
    }
}