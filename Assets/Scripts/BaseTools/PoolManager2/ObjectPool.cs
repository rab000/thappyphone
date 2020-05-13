using UnityEngine;
using System.Collections;

/// <summary>
/// 轻量空闲对象池
/// 18.4.21发现一处缺陷，以后考虑怎么处理
/// 使用这个池的类必须继承自NodeObject，那prefab怎么办(prefab里一般有美术资源时对象池的大客户)
/// </summary>
public class ObjectPool<T> where T:NodeObject,new(){

	public int size = 0;
	public T head = null;

	/// <summary>
	/// 取出空闲对象
	/// </summary>
	/// <returns>The free node.</returns>
	public T Get(){
		if(size == 0){
			return new T();
		}
		else if(size == 1){
			size = 0;
			return head;
		}
		else{
			T tempNode  =  head;
			head = (T)head.Next;//TODO 以后检测这里的问题
			tempNode.Next  =  null;
			size--;
			return tempNode;
		}
	}

	/// <summary>
	/// 存入空闲对象
	/// </summary>
	/// <param name="node">Node.</param>
	public void PushFreeNodeToPool(T node){
		node.Reset();
		
		if(size==0){
			head=node;
		}else{
			node.Next = head;
			head = node;
		}
		size++;
	}

	/// <summary>
	/// 清空池对象
	/// </summary>
	public void Clear(){
		if(size == 0){

		}
		else if(size == 1){
			size = 0;
			head.Clear();
			head = null;
		}
		else{
			NodeObject tempNode  =  head;
			head = (T)head.Next;//TODO 这里这么搞合适么？
			tempNode.Next  =  null;
			tempNode.Clear();
			tempNode = null;
			Clear();//递归调用，直到全部清空为止
		}
	}

	//NINFO 池是用来存储，防止频繁创建的，每个节点资源一定时经过清理才入池，不需要更新每个节点资源
	/// <summary>
	/// 遍历所有节点
	/// </summary>
//	public void tUpdate()
//	{
//		NodeObject h = head;
//		while (null != h) {
//			h.tUpdate();
//			h = head.Next;
//		}
//	}
}

/// <summary>
/// 对象节点
/// </summary>
public class NodeObject{
	
	public NodeObject Next;

	public virtual void tUpdate(){}

	/// <summary>
	/// 重置到初始状态
	/// </summary>
	public virtual void Reset(){}
	
	/// <summary>
	/// 清理自身
	/// </summary>
	public virtual void Clear(){}
}

