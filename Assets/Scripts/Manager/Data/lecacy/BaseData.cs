using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// by nafio 18.12.6
/// </summary>
public abstract class BaseData {

	public string Key;

	public string SubKey;

	public BaseData ()
	{
		
	}

	public BaseData (string key) : this (key,DataMgr.DEFAULT_SUBKEY)
	{
		
	}

	protected virtual string GetClassName (){
		return null;
	}


	public BaseData(string key,string subkey)
	{
		
		this.Key = key;

		this.SubKey = subkey;

		DataMgr.PutData (key, this, subkey);

	}

	public virtual byte[] Serialize()
	{
		return null;
	}

	public virtual void Clone()
	{
	
	}

}
