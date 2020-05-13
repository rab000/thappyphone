using NLog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 排序
/// 
/// 算法常用策略
/// 1 分治，二分法
/// 2 时间换空间
/// </summary>
public class SortHelper{

	private static bool BeShowLog = true;

	/// <summary>
	/// 起泡排序
	/// 1 从第0个元素开始，相邻元素比较，前面>后面就互换，这个过程能让整个数组中的最大的数到达数组末尾
	/// 2 从第1个元素开始，重复1过程，让整个数组中第二大的数到达数组倒数第二位置
	/// 3 从第2个元素开始，重复1过程，让整个数组中第三大的数到达数组倒数第三位置
	/// 4 用exChangePos来记录最后一次发生互换的位置，如果123执行中的某个位置开始一直没发生互换，就说明从这个位置开始
	///   后面的数据都是排布好的，exChangePos就用来记录这个位置
	/// 5 从第3个元素开始，重复1过程，这一次也是相邻依次比较，但只比较到exChangePos所指示的位置
	/// 
	/// O(N2)
	/// </summary>
	/// <param name="arrayInt">Array int.</param>
	public static void BubbleSort(int[] arrayInt){
		int n = arrayInt.Length;
		//Debug.Log ("n=>"+n);
		int i = n-2;//这个变量用来记录最后发生交换排序的位置，初始值是倒数第二个数据的位置
		while(i>0){
			int exChangePos = 0;
			for (int j = 0; j < i; j++) {
				if (arrayInt [j] > arrayInt [j + 1]) {
					int tmp = arrayInt [j];
					arrayInt [j] = arrayInt [j + 1];
					arrayInt [j + 1] = tmp;
					exChangePos = j;
				}
			}
			i = exChangePos;
		}
			
		if (BeShowLog) {
			for (int k = 0; k < n; k++) {
				LogMgr.I("SortHelper","BubbleSort","[k"+k+"]=>"+arrayInt[k],BeShowLog);
			}
		}
	}

	/// <summary>
	/// 快速排序
	/// 快排体现分治思想
	/// QuickSort说明
	/// 使用QuickSortSplit(array,0,array.len-1)，把array 1分为2(其中基准值在中间，大于基准值的值在右，小于基准值的在做)，返回分割位置pos
	/// 递归处理已经分割好的两半数组，知道不满足条件(low < hight)
	/// 
	/// QuickSortSplit说明
	/// 1 在数组中随意选取一个值(基准值，可以选取第一个数)
	/// 2 按基准值切割数组，返回基准值在数组中的位置 
	///   具体思路:把>=基准值得放数组右边，<=基准值的放基准值左 2个index(low,hight)分别从数组两边向中间靠近
	///   具体做法:
	///   2个index(low,hight)分别从数组两边向中间走
	///   当hight的遇到<基准值 时就暂停，跟low所在位置交换值  
	///   当low的遇到>基准值 时就暂停，跟hight所在位置交换值 
	///   最后返回low最后停止的位置
	/// 
	/// O(logN)
	/// </summary>
	/// <param name="arrayInt">Array int.</param>
	public static void QuickSort(int[] arrayInt,int low,int hight){
		if (low < hight) {
			int splitPos = QuickSortSplit (arrayInt,low,hight);//计算出切割数组的位置
			QuickSort(arrayInt,low,splitPos-1);//递归
			QuickSort(arrayInt,splitPos+1,hight);//递归
		}
	}
	/// <summary>
	/// 按基准值(在一个array中随意选取，一般选第一个)，分割数组，计算并返回分割点位置
	/// </summary>
	/// <returns>The sort split.</returns>
	/// <param name="arrayInt">Array int.</param>
	/// <param name="low">Low.</param>
	/// <param name="hight">Hight.</param>
	private static int QuickSortSplit(int[] arrayInt, int low,int hight){
		
		int privotValue = arrayInt [low];//随意选取基准值，一般选取low这个位置上的，就是数组最低位(这里不一定0，因为数组会多次分割成子数组)
		int tmp = 0;
		while (low < hight) {//从表的两端交替地向中间扫描  
			while (low < hight && arrayInt [hight] >= privotValue) {
				--hight;
			}
			tmp = arrayInt [low];
			arrayInt [low] = arrayInt [hight];
			arrayInt[hight] = tmp;

			while (low < hight && arrayInt [low] <= privotValue) {
				++low;
			}
			tmp = arrayInt [low];
			arrayInt [low] = arrayInt [hight];
			arrayInt[hight] = tmp;
		}

		return low;
	}

}
