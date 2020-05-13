using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Nafio 数据缓冲区
/// 优点，二进制体积最小
/// 缺点，数据本身并不能说明自身数据意义，数据生成代码和数据解析代码必须对等
/// 如果byts里面是多条数据，并且数据长度不同，那么就要一次性把所有数据解析完，
/// 而不可以先存数据，然后用到时再解析
/// 
/// 注意事项，使用iobuffer存数据，对于数据制作要求，一般格式如下,要保证数据有长度，否则不方便解析
/// (做技能数据时尝试过在数据结尾加结束字符串来判断结束，但是这样解析就需要嵌套循环，容易死循环，所以最好加入数据长度的概念)
/// dataSize=3
/// data1
/// 	subDataSize = 2
/// 	subData1
///     subDtaa2
/// data2
/// ...
/// data3
/// ....
/// 
/// 暂时不能异步读大文件(貌似目标不是读大文件用的，网络和数据存储用)
/// </summary>
public class IoBuffer{
	
	byte[] buffer;
	int putIndex = 0;
	int getIndex = 0;

    /// <summary>
    /// 输入输出是否字节序相反
    /// 如果是不同的字节序，那么存入的时候将字节序倒置
    /// </summary>
    bool beDiffEndian = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="num"></param>
    /// <param name="bBigEndian"></param>
	public IoBuffer(int num = 1024000,bool beDiffEndian = false)
    {
        this.beDiffEndian = beDiffEndian;
        buffer = new byte[num];
	}
	
	public void PutBool(bool b){
		if(b)buffer[putIndex] = (byte)1;
		else buffer[putIndex] = (byte)0;
		putIndex++;

	}
	public void PutByte(byte b){
		buffer[putIndex] = b;
		putIndex++;
	}
	
	public void PutShort(short value){
		byte[] bytes = flip(BitConverter.GetBytes(value));
		Array.Copy(bytes, 0, buffer, putIndex, bytes.Length);
		putIndex = putIndex + bytes.Length;
	}

	public void PutInt(int value){
		byte[] bytes = flip(BitConverter.GetBytes(value));
		Array.Copy(bytes, 0, buffer, putIndex, bytes.Length);
		putIndex = putIndex + bytes.Length;
	}

    public void PutLong(long value)
    {
        byte[] bytes = flip(BitConverter.GetBytes(value));
        Array.Copy(bytes, 0, buffer, putIndex, bytes.Length);
        putIndex = putIndex + bytes.Length;
    }

    public void PutFloat(float value){
		byte[] bytes = flip(BitConverter.GetBytes(value));
		Array.Copy(bytes, 0, buffer, putIndex, bytes.Length);
		putIndex = putIndex + bytes.Length;
	}
	
	public void PutString(string value){
	
		byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(value);
		bytes = flip(bytes);

		PutInt(bytes.Length);

		Array.Copy(bytes, 0, buffer, putIndex, bytes.Length);
		putIndex = putIndex + bytes.Length;

	}


	public void PutBytes(byte[] bytes){
		if(null==bytes){
			Debug.LogError("IoBuffer.PutBytes:bytes=null!!! 操作失败");
			return;
		}
		Array.Copy(bytes, 0, buffer, putIndex, bytes.Length);
		putIndex = putIndex + bytes.Length;
	}



	public bool GetBool(){
		byte value = buffer[getIndex];
		if(value == 0){
			getIndex++;
			return false;
		}
		else {
			if(value!=1)Debug.Log("GetBool error");
			getIndex++;
			return true;
		}

	}

	public byte GetByte(){
		byte value = buffer[getIndex];
		getIndex++;
		return value;
	}

	public short GetShort()
	{
		return BitConverter.ToInt16(GetBytes(2), 0);
	}

	public int GetInt(){
		return BitConverter.ToInt32(GetBytes(4), 0);
	}

    public long GetLong()
    {
        return BitConverter.ToInt64(GetBytes(8), 0);
    }

    public float GetFloat(){
		return BitConverter.ToSingle(GetBytes(4), 0);
	}

	public string GetString(){
		int size = GetInt();
		return System.Text.UTF8Encoding.UTF8.GetString(GetBytes(size));
		//return BitConverter.ToSingle(Get(4), 0);
	}

	byte[] flip(byte[] bytes){
        if (beDiffEndian)
        {
            Array.Reverse(bytes);
        }
        return bytes;
	}



	public byte[] GetBytes(int len){
		byte[] bytes = new byte[len];
		Array.Copy(buffer, getIndex, bytes, 0, len);
        //if (BitConverter.IsLittleEndian)
        //{
        //    Array.Reverse(bytes);
        //}
        getIndex += len;
		return bytes;
	}

    public int GetLength()
    {
        return putIndex;
    }

	public void Clear(){
		putIndex = 0;
		getIndex = 0;
		Array.Clear(buffer,0,buffer.Length);
		//buffer = null;
	}

	public byte[] ToArray(){
		byte[] bytes = new byte[putIndex];
		Array.Copy(buffer, 0, bytes, 0, bytes.Length);
		return bytes;
	}

    /// <summary>
    /// 判定buffer数据读取是否已经达到尾部
    /// 返回true是代表没到达尾部，还有数据
    /// </summary>
    /// <returns></returns>
    public bool HasData()
    {

        if (getIndex < putIndex) return true;

        return false;

    }
}
