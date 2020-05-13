using System;
using System.Reflection;
public class ReflectionHelper{

	/// <summary>
	/// 通过类名(string)创建实例类
	/// </summary>
	/// <returns>The object by class name.</returns>
	/// <param name="className">Class name.</param>
	/// <param name="args">Arguments.</param>
	/// <param name="activationAttributes">Activation attributes.</param>
	public static object CreateObjByClassName(string className,object[] args,object[] activationAttributes){
		Type t  = Type.GetType (className);
		object obj = Activator.CreateInstance(t,args,activationAttributes);
		return obj;
	}

	//public static object 

}
