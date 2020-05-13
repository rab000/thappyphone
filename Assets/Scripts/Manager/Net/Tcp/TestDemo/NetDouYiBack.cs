using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetDouYiBack :  AbsNetBack{
    public NetDouYiBack(): base(){

    }
    public override int[] RegistMsg(){
        return new int[]{
            18002,
            18004,
            18006,
            18008,
            18010
         };
    }
    /// <summary>
    /// 服务器下发斗翼修炼界面信息 
    /// </summary>
    public void Recevie18002(){
        Command_18002 cmd = new Command_18002();
//        cmd.cmd = FID.DOU_YI_REV_XIULIAN;
//        cmd.info.isAdd = ReadByte();
//        if (cmd.info.isAdd == 1)
//        {
//            cmd.info.isOpen = ReadByte();
//            if (cmd.info.isOpen >= 1)
//            {
//                cmd.info.lv = ReadByte();
//                cmd.info.shuLian = ReadByte();
//                cmd.info.sLong = ReadString();
//                cmd.info.sNum = ReadString();
//            }
//        }
		RegisterCommand((Cmd4Rec)cmd);
    }
    /// <summary>
    /// 服务器下发凝羽界面信息 
    /// </summary>
    public void Recevie18004(){
        Command_18004 cmd = new Command_18004();
//        cmd.cmd = FID.DOU_YI_REV_NINGYU;
//        cmd.info.isAdd = ReadByte();
//        if (cmd.info.isAdd == 1)
//        {
//            cmd.info.isOpen = ReadByte();
//            cmd.info.douYiID = ReadByte();
//            cmd.info.luckNum = ReadInt();
//            cmd.info.wid = ReadInt();
//            cmd.info.baoJi = ReadByte();
//        }
		RegisterCommand((Cmd4Rec)cmd);
    }
    /// <summary>
    /// 服务器下发斗翼幻化结果
    /// </summary>
    public void Recevie18006(){
        Command_18006 cmd = new Command_18006();
//        cmd.cmd = FID.DOU_YI_REV_HUANHUA;
//        cmd.isOk = ReadByte();
//        cmd.wID = ReadInt();
//        cmd.mID = ReadInt();
		RegisterCommand((Cmd4Rec)cmd);
    }
    /// <summary>
    /// 服务器下发可以用于幻化的斗翼的ID
    /// </summary>
    public void Recevie18008(){
        Command_18008 cmd = new Command_18008();
//        cmd.cmd = FID.DOU_YI_REV_ALL_HUANHUA;
//        int len = ReadInt();
//        for (int i = 0; i < len; i++ )
//        {
//            var n = ReadInt();
//            ////资源全了就把n<=2去掉
//            //if (n <= 2)
//            //{
//            //    cmd.info.huanHuaList.Add(n);
//            //}
//            cmd.info.huanHuaList.Add(n);
//            cmd.info.showTime.Add(ReadLong());
//        }
		RegisterCommand((Cmd4Rec)cmd);
    }
    /// <summary>
    /// 服务器广播玩家现在幻化的斗翼信息
    /// </summary>
    public void Recevie18010(){
        Command_18010 cmd = new Command_18010();
        //cmd.cmd = FID.DOU_YI_REV_CHANGE_HUANHUA;
        //cmd.roleID = ReadInt();
        //cmd.wID = ReadInt();
		RegisterCommand((Cmd4Rec)cmd);
    }
    
}
public class Command_18002 : Cmd4Rec
{
    //public DouYiXiuLianMsgInfo info = new DouYiXiuLianMsgInfo();
}
public class Command_18004 : Cmd4Rec
{
    //public DouYiNingYuMsgInfo info = new DouYiNingYuMsgInfo();
}
public class Command_18006 : Cmd4Rec
{
    public int isOk;
    public int wID;
    public int mID;
}
public class Command_18008 : Cmd4Rec
{
    //public DouYiHuanHuaInfo info = new DouYiHuanHuaInfo();
}
public class Command_18010 : Cmd4Rec
{
    public int roleID;
    public int wID;

}