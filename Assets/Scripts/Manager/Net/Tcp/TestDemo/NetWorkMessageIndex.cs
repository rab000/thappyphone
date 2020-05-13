//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//
//
//    public class FID
//    {
//        
//        public const short BEGINTAG_MSG = 127;
//        public const int COMMAND_SYS_MESSAGE = 3;
//        public const int LOGIN_HTTPREQUEST_BACK = 6;	//登录回复
//        #region 测试网络延迟消息
//        public const int CLIENT_UP_DEALY = 10;          //客户端上行延迟时间
//        public const int SEVER_DOWN_DEALY = 11;         //服务器返回客户端上行时间
//
//        public const int SEVER_DOWN_DEALY1 = 20;        //服务器下行延迟时间
//        public const int CLIENT_UP_DEALY1 = 21;         //客户端上行延迟时间
//        #endregion
//        //public const int ANONYMITY_LOGIN = 111;		//匿名登陸
//        public const int GAME_LOGIN_KEY = 112;		    //服务器返回匿名登陸串
//        public const int ANONYMITY_LOGIN_CREATE = 113;	//匿名登陸创建账户
//        public const int REQUEST_LOGIN = 114;		    //请求登陆
//        public const int LOGIN_SUCCEED = 115;		    //登陆成功
//        public const int LOGIN_10015 = 10015;           //创建角色
//        public const int LOGIN_10007 = 10007;           //进入场景
//        public const int LOGIN_ACCOUNT_BACK = 10002;	//登录回复
//        public const int HEART_BEAT_BACK = 40302;       //心跳回复
//        public const int GET_VLINE_SERVER_LIST_BACK = 10004;	//获得分线列表回复
//        public const int ROLE_ENTER_GAME_BACK = 10008;	//角色进入游戏回复
//        public const int ROLE_SCENE_INFO = 10009;	    //角色场景信息请求
//        public const int ROLE_SCENE_INFO_BACK = 10010;	//角色场景信息回复
//        public const int ROLE_STATE = 15111;            //玩家切磋  更新玩家PK状态
//        public const int GET_PLAYER_DATA = 10011;	    //获得Player数据请求
//        public const int GET_PLAYER_DATA_BACK = 10022;	//获得Player数据回复
//
//
//        #region 错误提示信息
//        public const int ERROR_TIP_MSG = 10000;         //错误信息提示消息
//        #endregion
//
//        #region 场景消息相关
//        public const int TELEPART_SENCE_BACK = 11007;	        //传送场景回复
//        public const int SERVER_SENCE_RIGISTER_ROLE = 11011;	//服务器场景注册角色协议(在切换场景的时候需要向服务器注册位置)
//        public const int TELEPART_SENCE = 11015;	            //传送场景请求
//        public const int SECONDCONFIRM_SCENE_AND_LINE = 11016;	//传送场景(与切线)之前二次确认提示信息
//        public const int INGOTTELEPART_SENCE = 11017;	        //道具传送场景请求
//
//        public const int CHANGE_LINE = 10019;  //切线请求
//        public const int CHANGE_LINE_BACK = 10110;  //切换线路状态 返回信息
//
//        public const int CS_FASTPOS_MOVE_RIGHTMOUSE = 110219;   //右键快捷传送
//        public const int CS_FASTPOS_WILLSENDPOS = 110217;       //请求发送快捷坐标
//        public const int SC_FASTPOS_WILLSENDPOS_BACK = 110218;  //右键请求发送快捷坐标返回
//        #endregion
//
//        #region 机关消息
//        //===============机关==================
//        public const int MECHANISM_ID_MEG_SEND_BACK = 27001;     	//机关ID 机关方向 发给服务器
//        public const int MECHANISM_ACTIVA = 27002;                  //激活机关
//        public const int MECHANISM_USESKILL_BACK = 27003;           //机关被使用技能后下发
//        public const int MECHANISM_ADDBLOCK_BACK = 27004;           //机关添加阻挡点
//        public const int MECHANISM_PARAM_BACK = 27005;              //机关属性信息
//        #endregion
//
//        #region 聊天消息
//        public const int PLAYER_TALK_MASSAGE = 12004;      //发送聊天协议上行
//        public const int PRIVATE_TALK_MASSAGE = 12006;      //发送私聊协议上行
//        #endregion
//
//        #region 视野消息
//        public const int PLAYER_ENTER_VIEW = 11201;	        //玩家进入视野消息
//        public const int NPC_ENTER_VIEW = 11202;	        //NPC进入视野消息
//        public const int MONSTER_ENTER_VIEW = 11203;	    //怪物进入视野消息
//        public const int DROP_ITEM_ENTER_VIEW = 11205;	    //掉落物品进入视野消息
//        public const int SKILL_ENTER_VIEW = 11206;	        //特殊技能进入视野消息
//        public const int GATHER_ITEM_ENTER_VIEW = 11207;    //采集物品进入视野消息
//        public const int SPIRIT_LEAVE_VIEW = 11220; 	    //离开视野消息
//        public const int FENSHEN_ENTER_VIEW = 11208;	    //分身进入视野
//        public const int FLAG_ENTER_VIEW = 11216;	        //旗帜进入视野
//        public const int MECHANISM_ENTER_VIEW = 27000;      //机关进入视野
//        public const int VIEWMSG_OPEN = 41000;	            //打开关闭视野消息
//
//#endregion
//
//        
//        #region 移动消息
//        public const int PLAYER_MOVE = 11105;	            //Player移动请求
//        public const int SERVER_SPIRIT_MOVE = 11106;	    //服务器主动移动精灵消息;
//        public const int PLAYER_MOVE_DIR_STOP = 11107;      //请求移动方向停止的消息
//        public const int SERVER_SPRITE_STOP = 11012;        //服务器下发精灵停止的消息
//        public const int PLAYER_JUMP_OVER = 11013;          //player跳点动作结束，同步服务器
//        public const int PLAYER_MOVE_BACK = 11112;	        //Player移动请求,有阻挡的时候服务器下行消息
//        #endregion
//        #region 技能消息
//        public const int USE_SKILL = 17000;	        //使用技能消息
//        public const int USE_SKILL_RESULT = 17001;	//使用技能结果
//        public const int SKILL_USER_BACK = 17007;	//使用技能消息回复
//        public const int SKILL_LIST_INIT = 17002;	//技能列表初始化
//        public const int ACTIVATE_SKILL = 17018;    //激活技能
//        public const int ACTIVATE_SKILL_BACK = 17004;//激活技能消息回复
//        public const int SKILLS_GOODS_POSBACK = 16013;	// 技能和物品CD时间
//
//        #endregion
//
//		#region buff
//		public const int BUFF_17005 = 17005;//广播玩家增加buff
//		public const int BUFF_17006 = 17006;//广播玩家移除buff
//		#endregion
//
//        #region 心跳
//        public const int SEND_HEARTBEAT = 40301;	        //发送心跳协议
//        public const int HEARTBEAT = 40302;                 //服务器返回心跳协议
//        #endregion
//
//        #region 死亡复活相关
//
//        public const int INIT_SPIRIT_ATTRIBUTE = 14000;	    //创建场景精灵属性消息（包括hero）
//        public const int SHOW_RELIVE_WINDOW = 14010;        //显示复活面板
//        public const int SPIRIT_DEATH = 14001;	            //精灵死亡
//        public const int PLAYER_RELIVE = 14003;	            //玩家复活
//
//        #endregion
//
//        #region VIP信息相关
//        public const int VIP_INFO_BACK = 110001;		//服务器下发VIP信息
//        public const int VIP_PURCHASE_VIP_GIFT = 110002;		//购买VIP礼包
//        #endregion
//        #region 任务协议
//        public const int ACCEPT_QUEST_BACK = 65000;	//接受任务服务器回复消息
//        public const int ACCEPT_QUEST = 65001;	//接受任务
//        public const int GET_ROLE_QUEST_LIST = 65002;	//获取角色任务列表协议
//        public const int TASK_DELIVER = 65003;	//交付任务
//        public const int TASK_DELETE_ABLE = 65004;	//删除可接任务回复
//        public const int TASK_ACTION = 65005;	//对任务操作后下发
//        public const int GET_ROLE_QUEST_LIST_BACK = 65006;	//获取角色任务列表回复
//        public const int ROLE_ENTER_SEARCH_AREA = 65007;	//角色进入搜索区域
//        public const int TASK_GIVE_UP = 65008;	//放弃任务
//        public const int QUEST_OPERATE_PROMPT = 65009;	//任务操作提示
//        public const int TASK_ADD_ABLE = 65010;	//添加可接任务消息回复
//        public const int NPC_DIALOGUE_QUEST = 65011;	//NPC谈话任务请求
//        public const int TASK_PROGRESS = 65012;	//任务进度
//        public const int TASK_GATHER_UP = 65013;	//采集精灵上行
//        public const int TASK_GATHER_BACK = 65014;	//采集精灵下发
//        public const int TASK_ANSWER_UP = 65015;	//答题任务上行
//        public const int TASK_ANSWER_BACK = 65016;	//答题任务下发
//        public const int TASK_CONDITION_UP = 65017;	//按键限制条件任务上行
//        public const int GATHER_PUT_OUT = 65018;	//人物采集精灵广播
//        public const int GATHER_STOP = 65019;	//取消采集
//
//        public const int TASK_GAIN_EQUIPMENT = 65022;    //获得好装备，物品  弹出指引框
//        public const int TASK_GAIN_SKILL = 65024;   //通过任务获取技能  弹出指引框
//        public const int THE_FIRST_TASK = 65027;   //新建角色 教学后的第一个任务
//        public const int SC_COPYGUIDE = 24075;				//进入副本指引
//        #endregion
//        #region 斗魂相关
//
//        public const int DOUHUN_INIT = 69001; 				    //斗魂初始化
//        public const int DOUHUN_INIT_BACK = 69002; 				//斗魂初始化消息返回
//        public const int DOUHUN_NODE_LEVELUP = 69003;			//斗魂节点升级或突破
//        public const int DOUHUN_NODE_LEVELUP_BACK = 69004;		//节点升级返回
//        public const int DOUHUN_JIASU = 69005;				    //清除节点升级或突破冷却
//        public const int DOUHUN_REALM_BREACH_BACK = 69007;		//斗魂境界升级返回
//        public const int DOUHUN_JIASU_BACK = 69008;				//加速返回
//        public const int DOUQI_OUTPUT_UPDATE = 69009;			//斗气产出更新
//        public const int DOUHUN_IF_HAVE_OPENED_BACK = 69010;	//斗魂开启状态
//        public const int DOUHUN_TUPOEFFECT = 69011;				//斗魂突破特效广播
//
//        #endregion
//
//        #region 运镖相关
//        public const int YUNBIAO_REQ_YA_BIAO_INFO = 10030;                   //请求押镖信息
//        public const int YUNBIAO_RSP_YA_BIAO_INFO = 10031;                   //响应押镖信息
//
//        public const int YUNBIAO_REQ_REFRESH_STORE = 10034;                   //请求刷新货栈货物
//
//        public const int YUNBIAO_REQ_UNLOCK_CARGO = 10042;                   //请求解封货栈货物
//
//        public const int YUNBIAO_REQ_GET_REWARD = 10044;                   //请求领奖
//
//        public const int YUNBIAO_REQ_YUN_BIAO_EVENT = 10046;         //请求获取运镖事件信息
//        public const int YUNBIAO_RSP_YUN_BIAO_EVENT = 10047;         //响应获取运镖事件信息
//
//        public const int YUNBIAO_REQ_YUN_BIAO_INSURE = 10040;         //请求获取运镖保险
//        public const int YUNBIAO_RSP_YUN_BIAO_INSURE = 10041;         //响应获取运镖保险
//
//        public const int YUNBIAO_REQ_YA_BIAO = 10038;                   //请求开始押镖
//        public const int YUNBIAO_RSP_YA_BIAO = 10039;                   //响应押镖
//
//        public const int YUNBIAO_REQ_OPEN_STORE = 10032;                   //请求打开货栈
//        public const int YUNBIAO_RSP_OPEN_STORE = 10033;                   //响应打开货栈
//
//        #endregion
//
//        #region 野外争夺
//        public const int FIELD_FIGHT_REQ_BATTLE_INFO = 91001;         //请求野外争夺战况信息
//        public const int FIELD_FIGHT_REQ_JIAZU_RESULT = 91003;         //请求野外争夺家族战绩
//        public const int FIELD_FIGHT_REQ_CALC_RANKING = 91007;         //请求野外争夺结算排行榜
//        public const int FIELD_FIGHT_REQ_GATHER_INFO = 91009;         //请求野外争夺采集物位置信息 或者取消标记
//
//
//        #endregion
//
//        #region 家族相关
//        public const int JIAZU_REQ_JIA_ZU_INFO = 32010;         //请求获取家族信息
//        public const int JIAZU_RSP_JIA_ZU_INFO = 32011;         //响应获取家族信息
//
//        public const int JIAZU_REQ_JIA_ZU_LIST_INFO = 32015;    //请求获取家族列表信息
//        public const int JIAZU_RSP_JIA_ZU_LIST_INFO = 32016;    //响应获取家族列表信息
//
//        public const int JIAZU_REQ_SEARCH_INFO = 32056;         //请求搜索家族信息（响应信息是在32016协议里）
//
//        public const int JIAZU_REQ_CREATE_JIAZU = 32000;        //请求创建家族
//        public const int JIAZU_RSP_CREATE_JIAZU = 32001;        //响应创建家族
//
//        public const int JIAZU_RSP_STATE_NOTICE = 32002;        //1.成功加入家族 2.退出家族 3.被族长踢出 --》刷新角色家族名字
//
//        public const int JIAZU_REQ_UPDATE_INFO = 32047;         //请求修改家族信息
//        public const int JIAZU_RSP_UPDATE_INFO = 32048;         //响应修改家族信息
//
//        public const int JIAZU_REQ_INVITE_INFO = 32018;         //请求发布招募信息
//        public const int JIAZU_RSP_INVITE_INFO = 32019;         //响应发布招募信息
//
//        public const int JIAZU_REQ_APPLY_FOR_JIAZU = 32003;     //请求加入家族
//        public const int JIAZU_RSP_APPLY_FOR_JIAZU = 32058;     //响应请求加入家族
//
//        public const int JIAZU_REQ_APPLY_MEMBER_INFO = 32004;   //请求申请加入家族名单
//        public const int JIAZU_RSP_APPLY_MEMBER_INFO = 32005;   //响应申请加入家族名单
//
//        public const int JIAZU_REQ_VERIFY_MEMBER = 32006;       //请求同意加入家族
//        public const int JIAZU_RSP_VERIFY_MEMBER = 32023;       //响应同意加入家族
//
//        public const int JIAZU_REQ_MANAGER_POST = 32012;       //请求管理家族成员职位
//        public const int JIAZU_RSP_MANAGER_POST = 32013;       //响应管理家族成员职位
//
//        public const int JIAZU_REQ_LEAVE_JIAZU = 32008;       //请求退出家族或者族长踢出家族
//        //public const int JIAZU_RSP_LEAVE_JIAZU = 32013;       //响应退出家族或者族长踢出家族
//
//        public const int JIAZU_REQ_SHAIKH_FIRESELF = 32014;     //请求族长退位让贤
//
//        public const int JIAZU_REQ_CAGNG_BAO_GE_ITEM_LIST = 32027;//请求藏宝阁物品列表
//        public const int JIAZU_RSP_CAGNG_BAO_GE_ITEM_LIST = 32025;//请求藏宝阁物品列表
//
//        public const int JIAZU_REQ_REFRESH_CAGNG_BAO_GE_ITEM_LIST = 32024;//请求刷新藏宝阁物品列表
//
//        public const int JIAZU_REQ_REDEEM_ITEM = 32017;//请求兑换物品
//        public const int JIAZU_RSP_REDEEM_ITEM = 32057;//请求兑换物品
//
//        public const int JIAZU_REQ_TASK_LIST = 32028;//请求任务列表
//        public const int JIAZU_RSP_TASK_LIST = 32029;//相应任务列表
//
//		public const int JIAZU_RSP_OPEN_BOX = 32035;            //响应打开箱子
//        public const int JIAZU_REQ_OPEN_BOX = 32036;            //请求打开箱子
//		public const int JIAZU_RSP_REFRESH_CONTRIBUTION = 32037;//刷新家族贡献
//
//        public const int JIAZU_REQ_REFRESH_TASK_LIST = 32030;   //请求刷新家族任务列表
//
//        public const int JIAZU_REQ_UPGRADE_LEVEL = 32054;   //请求家族升级
//
//        public const int JIAZU_REQ_UPGRADE_SKILL = 32060;   //请求家族技能升级
//        public const int JIAZU_REQ_JUAN_XIAN = 32020;   //请求家族捐献
//
//        #endregion
//
//        #region 邮件相关
//        public const int MAIL_REQ_CHECK_EMAIL = 80004;         //请求查看邮件
//        public const int MAIL_RSP_CHECK_EMAIL = 80012;         //响应查看邮件
//        public const int MAIL_REQ_DEL_EMAIL = 80002;           //请求删除邮件
//        public const int MAIL_RSP_DEL_EMAIL = 80011;           //响应删除邮件
//        public const int MAIL_REQ_EXTRACT_MAIL = 80003;         //请求提取附件
//        public const int MAIL_REQ_SEND_MAIL = 80000;            //请求发送邮件
//        public const int MAIL_RSP_SEND_MAIL = 80014;            //响应发送邮件
//        
//        #endregion
//
//        #region 背包相关协议
//        public const int BACKPACK_OPENSIZE_BACK = 15016;   //背包和仓库开放格子数目
//        public const int PLAYER_WEAR_EQUIP_BACK = 16000;   //人物身上穿的装备
//        public const int BACKPACK_ITEMS_BACK = 16001;      //背包物品下行消息（登陆时下发）
//		public const int STORAGEPACK_ITEMS_BACK = 16002;      //仓库物品下行消息（登陆时下发）
//        public const int BACKPACK_UPDATE_ITEM_BACK = 16004;     //背包物品添加(获得物品下行)
//        public const int BACKPACK_SORTITEM_BACK = 16014;         //整理物品返回消息
//        public const int DELETE_ITEMS = 16005;             //背包物品删除（上行）
//        public const int DELETE_ITEMS_BACK = 16006;        //背包物品删除（下行）
//        public const int UPDATE_ITEMS_POS = 16007;         //背包物品更换位置（上行）
//        public const int UPDATE_ITEMS_POS_BACK = 16008;    //背包物品位置更换（下行）
//        public const int UPDATE_ITEMS_NUMBER_BACK = 16009; //更新物品数量（下行）
//        public const int PLAY_ITEMS_USE = 16012;           //使用物品请求（上行）
//        public const int BACKPACK_SORT_OUT = 16011;        //上行消息整理背包
//        public const int TIPS_16044 = 16044;//请求没有拥有者物品tips
//        public const int TIPS_16045 = 16045;//请求其他角色物品tips
//        public const int TIPS_16071 = 16071;//没有拥有者物品tips返回
//        public const int TIPS_16072 = 16072;//其他角色物品tips返回
//        public const int BACKPACK_EXPAND_CELL = 20016;    //请求扩充背包格子
//        public const int BACKPACK_EXPAND_CELLBACK = 20017;  //请求扩充背包格子返回
//		 public const int BACKPACK_MOHE_REQ = 16083;     //魔盒请求合成
//        //public const int BACKPACK_MOHE_RSP = 16083;     //魔盒响应合成
//
//         public const int SHOP_GOODS_LIST_BACK = 100402; //商店商品列表返回
//         public const int SHOP_GOODS_LIST_REQUIRE = 100400; //请求商品列表。1普通商店，2vip商店
//         public const int SHOP_GOODS_REFRESH_REQUIRE = 100401; // 请求刷新商店商品。1刷新普通商店  2刷新vip商店
//         public const int SHOP_GOODS_PURCHASE_REQUIRE = 100404;//购买对应格子的物品请求
//         public const int SHOP_GOODS_PURCHASE_SUCCESS_BACK = 100405;//购买成功返回提示
//
//
//        #endregion
//
//        #region 异火系统协议列表
//        public const int FIREPACK_ITEMS_INIT = 71001;             //异火界面初始化上行消息
//        public const int FIREPACK_ITEMS_BACK = 71002;             //异火界面初始化获得身上异火背包格子信息（下行）
//        public const int FIRE_ATTRIBUTE_BACK = 71010;             //异火界面属性信息下发（下行）
//        public const int UPDATE_FIREPACK_ITEMS = 71014;           //更新异火格子（下行）
//        public const int DEVOUR_FIRE_ITEMS = 71006;               //吞吃异火（上行）
//        public const int DEVOUR_FIREITEM_BACK = 71007;            //吞吃异火返回（下行）
//        public const int ONEKEY_DEVOUR_FIRE = 71008;              //一键吞噬异火（上行）只发5
//        public const int ONEKEY_DEVOURFIRE_BACK = 71009;          //一键吞吃异火返回（下行）
//        public const int FIREINIT_MANAGER_BACK = 71021;           //服务器下发异火初始化信息
//        public const int FIREITEM_ARRANGEMENT_CONT = 71030;       //整理异火背包物品（上行）
//        public const int FIRE_GROWUP_LINE = 71013;                //选择异火成长线(上行)
//        public const int FIREGROWUP_LINE_BACK = 71015;            //选择异火成长线(下行)
//        public const int FIRE_CHANGE_CHARACTER = 71016;           //切换异火性格(上行)
//        #endregion
//
//        #region 装备
//        public const int BACKPACK_PRODUCT_INFO_BACK = 33016;      //商城物品下行消息 供计算装备消失数量使用
//        public const int INTENSIFY_EQUIP = 16016;                 //强化装备
//        public const int INTENSIFY_EQUIP_BACK = 16025;            //强化、精炼装备返回数据
//        public const int JINGLIAN_EQUIP = 16059;                  //精炼装备
//        public const int CHUANCHENG_EQUIP = 16073;                //传承装备
//        public const int XIANGQIAN_EQUIP = 16019;                 //镶嵌装备
//        public const int BA_CHU_MO_HE_EQUIP = 16020;              //拔除魔核
//        public const int LIANHUA_SHENGJI_MOHE = 16065;            //炼化、升级魔核
//
//
//        public const int ITEM_INSPERCTOR_20005 = 20005;      //查看物品属性
//        #endregion
//
//        #region 纳戒
//        public const int NAJIE_LOTTERY = 82001;                 //请求抽奖
//        public const int NAJIE_LOTTERY_BACK = 82002;                 //请求抽奖返回
//        public const int NAJIE_REQUEST_AWARD = 82004;
//        
//        public const int NAJIE_VIEW_BACK = 82003;               //
//        public const int NAJIE_GET_REWARD = 82011;              //兑奖
//        public const int NAJIE_GET_TIME_REWARD = 82008;              //请求计时奖品列表
//        public const int NAJIE_GET_COUNT_DOWNREWARD = 82007;    //领取倒计时奖品
//        #endregion
//
//        #region 副本结算
//        public const int FUBEN_CALC_BACK = 24076;               //副本结算服务器返回信息
//        public const int FUBEN_TAO_LUN_LIST_C2S = 24081;        //请求获取副本讨论列表
//        public const int FUBEN_TAO_LUN_CLICK_GOOD_C2S = 24079;  //发送副本评论点赞
//        public const int FUBEN_TAO_LUN_SEND_MSG = 24077;        //发表评论消息
//        public const int FUBEN_LEAVE_C2S = 24003;               //离开副本
//        #endregion
//
//        #region 风云榜
//        public const int FENGYUNBANG_CHALLENGE = 11211;         //挑战玩家
//        public const int FENGYUNBANG_CHALLENGE_UI_INFO = 11212; //挑战界面请求
//        public const int FENGYUNBANG_GET_REWARD = 11214;        //请求风云榜奖品
//        public const int FENGYUNSHOP_OPEN_REQUEST = 100499;     //打开风云商店请求商店信息
//        public const int FENGYUNSHOP_GOODS_LIST_BACK = 100500;  //风云商店商品列表下发
//        public const int FENGYUNSHOP_PURCHASE_REQUEST = 100501; //风云商店购买商品请求
//        public const int FENGYUNSHOP_PURCHASE_SUCCESS_BACK = 10502; //风云商店购买成功下发
//        public const int FENGYUNSHOP_REFRESH_REQUEST = 100503;  //风云商店刷新请求
//        
//        #endregion
//        #region 购买相关次数
//        public const int PURCHASE_COUNT = 110007;               //购买相关次数
//        #endregion
//
//        #region 聊天协议
//        public const int CHAT_RECEIVE = 12010;	//接收聊天
//        #endregion
//        #region 拾取物品相关协议
//        public const int PICKUP_ITEM_DATA = 15008;          //拾取物品协议上行
//        public const int PICKUP_ITEM_DATA_BACK = 16054;     //物品拾取成功返回
//        public const int PICKUP_ITEM_LOSE_BACK = 16051;     //物品拾取失败协议
//        #endregion
//
//        #region 组队
//        //c2s
//        public const int TEAM_C2S_68000 = 68000;        //创建队伍
//        public const int TEAM_C2S_68002 = 68002;        //邀请加入队伍
//        public const int TEAM_C2S_68006 = 68006;        //被邀请或被加入队伍玩家确认
//        public const int TEAM_C2S_68007 = 68007;        //请离队伍
//        public const int TEAM_C2S_68008 = 68008;        //离开队伍
//        public const int TEAM_C2S_68009 = 68009;        //设置队长
//        public const int TEAM_C2S_68015 = 68015;        //附近玩家
//        public const int TEAM_C2S_68016 = 68016;        //附近队伍
//        public const int TEAM_C2S_68018 = 68018;        //申请加入队伍
//        public const int TEAM_C2S_68019 = 68019;        //申请加入队伍响应信息
//        public const int TEAM_C2S_68021 = 68021;        //解散队伍
//        public const int TEAM_C2S_68020 = 68020;        //设置自动接受组队邀请
//        //s2c
//        public const int TEAM_S2C_68001 = 68001;        //解散队伍
//        public const int TEAM_S2C_68003 = 68003;        //增加队员信息
//        public const int TEAM_S2C_68004 = 68004;        //删除成员
//        public const int TEAM_S2C_68005 = 68005;        //有人邀请你加入队伍或者组成队伍时，收到此消息
//        public const int TEAM_S2C_68011 = 68011;        //队友更新
//        public const int TEAM_S2C_68012 = 68012;        //设置队长
//        public const int TEAM_S2C_68014 = 68014;        //附近玩家
//        public const int TEAM_S2C_68017 = 68017;        //附近队伍列表
//        public const int TEAM_S2C_68023 = 68023;        //队友血和魔更新
//        public const int TEAM_S2C_68033 = 68033;        //通知队伍成员的位置
//        public const int TEAM_S2C_68034 = 68034;        //更新成员状态
//        #endregion
//
//        #region 好友
//        //c2s
//        public const int Friend_C2S_21024 = 21024;        //发送加好友申请
//        public const int Friend_C2S_21002 = 21002;        //客户端请求添加某（好友、仇人）
//        public const int Friend_C2S_21006 = 21006;        //客户端请求删除某（好友、仇人）
//        public const int Friend_C2S_21009 = 21009;        //客户端请求修改签名
//        public const int Friend_C2S_21011 = 21011;        //客户端请求模糊查询角色
//        public const int Friend_C2S_21016 = 21016;        //客户端发送聊天信息，转发给某好友
//        public const int Friend_C2S_21031 = 21031;        //玩家变更星座、签名信息
//        public const int Friend_C2S_21034 = 21034;        //查询好友的地图，分线，战力和家族名字
//        public const int Friend_C2S_21036 = 21036;        //分页查看好友信息数据
//        public const int Friend_C2S_21037 = 21037;			//赠送好友斗气请求
//        public const int Friend_C2S_21039 = 21039;			//领取斗气请求
//
//        //s2c
//        public const int Friend_S2C_21000 = 21000;        //角色上下线通知
//        public const int Friend_S2C_21001 = 21001;        //玩家上线时，获取好友列表
//        public const int Friend_S2C_21007 = 21007;        //成功删除某（好友、仇人）
//        public const int Friend_S2C_21030 = 21030;        //返回收到好友申请
//        public const int Friend_S2C_21005 = 21005;        //服务器返回添加好友的信息
//        public const int Friend_S2C_21010 = 21010;        //服务器广播签名给这个玩家的好友，刷新签名
//        public const int Friend_S2C_21012 = 21012;        //服务器返回模糊查询角色信息
//        public const int Friend_S2C_21017 = 21017;        //服务器将客户端来的消息转发给指定玩家
//        public const int Friend_S2C_21033 = 21033;        //玩家变更签名
//        public const int Friend_S2C_21032 = 21032;        //玩家变更星座
//        public const int Friend_S2C_21035 = 21035;        //好友的地图、分线、家族名字、战力信息下发
//        public const int Friend_S2C_21038 = 21038;			//赠送斗气的好友ID列表
//        public const int Friend_S2C_21040 = 21040;          //领取斗气成功提示
//        public const int Friend_S2C_21041 = 21041;			//更新可领取和赠送斗气的剩余次数
//       
//        #endregion
//
//        #region 活动和副本
//        //s2c
//        public const int Activity_S2C_24001 = 24001;        //成功进入某副本
//        public const int Activity_S2C_28002 = 28002;        //获得活动列表
//        public const int Activity_S2C_24019 = 24019;        //获得副本进度及最高纪录列表信息
//        public const int Activity_S2C_24055 = 24055;        //获得副本未领取的扫荡和找回奖励
//        public const int Activity_S2C_24020 = 24020;        //更新副本最高纪录列表信息
//        public const int Activity_S2C_24022 = 24022;        //更新副本进度
//        public const int Activity_S2C_24027 = 24027;        //是否成功进入副本
//        public const int Activity_S2C_24028 = 24028;        //退出副本消息返回
//        //c2s
//        public const int Activity_C2S_28001 = 28001;        //请求活动列表
//        public const int Activity_C2S_24000 = 24000;        //请求进入副本
//        public const int Activity_C2S_24052 = 24052;        //请求扫荡或找回
//        public const int Activity_C2S_24053 = 24053;        //请求领取奖励
//        public const int Activity_C2S_24044 = 24044;        //重置副本
//
//        //Y
//        public const int Activity_C2S_24007 = 24007;        //客户端答应进入副本  上行
//        public const int Activity_C2S_24110 = 24110;        //客户端回应组队进入副本 上行
//        public const int Activity_S2C_24107 = 24107;        //组队进入副本时，队伍里成员的准备状态  下行
//        public const int Activity_S2C_24109 = 24109;        //队伍中所有队友都准备好了进入副本，通知客户端开始倒计时 下行
//        public const int Activity_S2C_24043 = 24043;        //审查是否通过 下行
//        public const int Activity_S2C_24040 = 24040;        //队长申请成功，告诉其他队员成功消息 下行
//        public const int Activity_C2S_24041 = 24041;        //客户端回应组队进入副本
//        public const int Activity_S2C_24042 = 24042;        //队长收到队员拒绝进入副本的消息  下行
//
//        public const int MatchTeam_C2S_24100 = 24100;       //副本请求匹配队伍
//        public const int MatchTeam_S2C_24101 = 24101;       //副本请求自动匹配队伍回复
//        public const int MatchConfirmTeam_C2S_24102 = 24102; //客户端确认进入副本
//        public const int CancelMatchTeam_C2S_24103 = 24103;  //客户端取消进入某副本
//        public const int CancelMatchTeam_S2C_24104 = 24104;  //服务器返回成功取消某副本的信息
//
//        //囚魔虚境
//        public const int QiuMoXuJing_CurrInfo_24083 = 24083;  //本次平台怪物信息
//        public const int QiuMoXuJing_GetItem_24084 = 24084;   //已获得的奖励元素信息
//        public const int QiuMoXuJing_GetMoney_24085 = 24085;  //每个平台过关奖励的金币数
//        public const int QiuMoXuJing_GetReward_24086 = 24086; //每大关的奖励内容
//        public const int QiuMoXuJing_PlayNextPass_24087 = 24087; //进入下一关卡
//        public const int QiuMoXuJing_ShowHiddenBoss_24170 = 24170;  //隐藏BOSS出现
//        public const int QiuMoXuJing_MaxBloodUp_24088 = 24088;//上行满血
//        public const int QiuMoXuJing_MaxBloodDown_24089 = 24089;//下行满血
//        public const int QiuMoXuJing_OpenBoxPrice_24090 = 24090;//开宝箱提示和价格 服务器下行
//        public const int QiuMoXuJing_RequestOpenBox_24091 = 24091;//客户端申请开宝箱 客户端上行
//        public const int QiuMoXuJing_BoxResult_24092 = 24092;//宝箱结果 服务器下行
//
//        //Y
//
//        public const int TurnWheel_C2S_24033 = 24033;       //转动轮盘
//        public const int TurnWheelResult_S2C_24034 = 24034;         //进入轮盘副本后，收到当前转动的信息
//        public const int TurnWheelMosnterNum_S2C_24035 = 24035;     //当前副本内，当前事件里被杀死的怪物数量
//        public const int TurnWheel_S2C_24029 = 24029;   //一次事件的怪全部死亡，进行下一次转动轮
//        #endregion
//
//        #region 世界boss
//        public const int WorldBossInfo_S2C_100200 = 100200;       //世界boss活动信息
//        public const int EnterBossScene_C2S_100201 = 100201;       //进入地图
//        public const int BossDisapperTime_S2C_100202 = 100202;       //世界boss消失时间
//        public const int PersonHurtInfo_S2C_100203 = 100203;       //个人伤害数据
//        public const int ClickWishSkill_C2S_100210 = 100210;       //玩家点击祝福时发送
//        public const int AcceptWishInfo_S2C_100204 = 100204;       //接收祝福信息
//        public const int WorldTopFiveInfo_S2C_100205 = 100205;       //世界前5排名信息
//        public const int MatchConfirmTeam_C2S_100206 = 100206;       //请求个人伤害信息排名
//        public const int PersonHurtRank_S2C_100207 = 100207;       //个人伤害排名
//        public const int BossBloodUpdate_S2C_100208 = 100208;       //世界boss血量更新
//        public const int BossRewardInfo_S2C_100209 = 100209;       //结束后奖励消息
//        public const int UseExperienceItem_C2S_100213 = 100213;       //使用翻倍请求
//        public const int ExitBossScene_C2S_100214 = 100214;       //退出地图
//        #endregion
//
//        #region 提示信息（例如错误提示之类的）
//
//        public const int TIP_MESSAGE = 12013;	//提示消息(错误提示之类)
//
//        #endregion
//
//        #region 货币
//        public const int RECEIVE_CURRENCY_INFO = 20002;
//        #endregion
//
//        #region 商店
//        public const int SHOP_C2S_25000 = 25000;  //请求商店数据
//        public const int SHOP_C2S_25002 = 25002;  //请求商店交易
//        public const int SHOP_C2S_25105 = 25105;  //请求商店回购
//        public const int SHOP_S2C_25003 = 25003;  //返回商店数据
//        public const int SHOP_S2C_25104 = 25104;  //返回商店可回购物品列表
//        #endregion
//
//        #region 精灵状态
//
//        public const int SPIRIT_CHANGE_SHEEP = 14007;	//精灵变羊
//
//        #endregion
//
//        #region 其他的分散，不是系统里面的数据
//
//        public const int CHANGE_AVATARID = 20001;   //换装消息
//        public const int PLAY_FIGHT_MUSIC = 16200;  //播放战斗音乐
//
//
//        #endregion
//
//
//        #region 跨服PK
//
//        public const int CS_KUAFU_MATCH = 130001;		    //请求匹配
//        public const int CS_KUAFU_GOTO = 130003;			//请求跨服,这时GameServer拷贝角色数据到BattleServer
//        public const int CS_KUAFU_GETDATA = 130006;			//跨服3v3界面数据
//        public const int CS_KUAFU_GETREWARD = 130009;		//领取连胜奖励
//        public const int CS_KUAFU_GETEXP = 130011;			//领取经验奖励
//        public const int CS_KUAFU_BUYTIMES = 130012;		//购买次数
//
//        public const int SC_KUAFU_COUNTTIME = 16070;        //跨服倒计时
//        public const int SC_KUAFU_MATCH_RESULT = 130002;	//匹配结果，如果成功，会有BattleServer的IP
//        public const int SC_KUAFU_GOTO_RESULT = 130004;		//请求跨服结果
//        public const int SC_KUAFU_BATTLE_OVER = 130005;		//3v3 战斗结果
//        public const int SC_KUAFU_DATA = 130007;			//界面数据
//        public const int SC_KUAFU_OVERTIME = 130008;		//匹配结束
//
//        public const int CC_KUAFU_AUTOLOGIN = 1543211;		//自动重连
//        public const int CC_ERROR_CLOSE_TCP = 1543212;		//掉线
//
//        #endregion
//
//        #region 天墓
//
//        public const int CS_TIANMU_GET_PANELDATA = 24064;			//获取报名界面数据
//        public const int SC_TIANMU_PANELDATA = 24065;		        //报名界面数据
//
//        public const int SC_TIANMU_RIGHT_PANELDATA = 24066;	        //右侧数据
//
//        public const int CS_TIANMU_GET_MONEY = 24072;	            //领取金币
//
//        public const int CS_TIANMU_APPLY = 24061;		            //请求匹配
//        public const int CS_TIANMU_GOTO = 24062;		            //进入
//        public const int CS_TIANMU_ATHOME = 24063;		            //回家 提交采集物		
//
//        public const int CS_TIANMU_GET_RANKING = 24067;	            //获取排行
//        public const int SC_TIANMU_RANKING = 24068;		            //排行
//
//        public const int SC_TIANMU_GOOUT = 24060;		            //被服务器请离战场，收到后返回游戏服务器
//        public const int SC_TIANMU_FIRSTBLOOD = 24057;	            //第一滴血
//        public const int SC_TIANMU_LINKKILL = 24058;	            //连杀
//        public const int SC_TIANMU_GODLIKE = 24056;		            //连杀并且不死
//        public const int SC_TIANMU_ENDKILL = 24059;		            //终结
//
//        public const int SC_TIANMU_REWARD_PANELDATA = 24069;        //奖励界面
//        public const int SC_TIANMU_MODEL_COLOR = 24070;	            //设置模型颜色
//
//        public const int CS_TIANMU_BUY_SHIQI = 24071;
//        public const int SC_TIANMU_REFURBISH_SHIQI = 24073;         //刷新士气
//        public const int SC_TIANMU_TEAMDATA = 24074;                //成员数据
//
//        #endregion
//
//
//        #region 排行榜
//        public const int RANKING_C2S_DATA = 26000;                    //根据页数请求排行数据
//        #endregion
//
//		#region 设置界面
//		public const int SET_GUAIJI_DATA=15012;                        //挂机设置消息（上行）
//		public const int SET_GUAIJI_DATA_BACK=15109;                   //服务器挂机消息（下行）
//		public const int SET_EFFECT_DATA=20041;                        //效果设置（上行）
//		public const int SET_EFFECT_DATA_BACK=20042;                   //效果设置（下行）
//		#endregion
//
//        #region 查看他人
//        public const int LOOK_OTHER_PLAYER_20005 = 20005;                 //查看其他人（上行）
//        public const int LOOK_OTHER_PLAYER_20006 = 20006;                  //查看其他人（下行）          
//        #endregion
//
//        #region 动画播放
//        public const int MOVIE_S2C_100215 = 100215;//下行已经播放过的动画列表
//
//        public const int MOVIE_S2C_100216 = 100216;//上行播放过的动画列表
//
//        public const int MOVIE_S2C_100217 = 100217;//下行播放动画
//        #endregion
//
//        #region 更改名字颜色
//
//        public const int FACTION_UPDATE_NAME_COLOR = 32021;		        //刷新名字颜色
//
//        #endregion
//
//        #region PK规则更改
//
//        public const int CHANGE_PKMODEL = 20003;	    //更改PK模式
//        public const int CHANGE_PKMODEL_BACK = 20004;	//更改PK模式返回
//        public const int SHALUZHI_BACK = 20007;	        //杀戮值广播
//
//        #endregion
//
//        #region 坐骑相关
//        public const int MOUNT_CULTURERESULTS_BACK = 19003;  //坐骑培养结果下发
//        public const int MOUNT_ADVANCEDRESULTS_BACK = 19004;  //坐骑进阶结果下发
//        public const int MOUNT_RIDINGTYPE_BACK = 19101;       //骑乘状态下发
//        public const int MOUNT_HUANHUA_INFOBACK = 19104;      //坐骑幻化信息
//        public const int MOUNT_ATTRIBUT_BACK = 19102;         //单个坐骑属性下发
//        public const int MOUNT_RIDINGTYPE = 19000;            //坐骑骑乘上行   1：上坐骑 2：下坐骑
//        public const int MOUNT_CULTURE_OR_ADVANCED = 19002;   //坐骑培养或进阶 上行
//        public const int MOUNT_HUANHUA_INFO = 19005;          //坐骑幻化上行
//        public const int MOUNT_HUANHUA_BACK = 19006;          //坐骑幻化下行
//        #endregion
//
//        #region 打坐
//
//        public const int SIT_DOWN_STATE = 15004;                //打坐消息 客户端主动上行
//        public const int UPDATE_SIT_DOWN_STATE = 15100;         //打坐消息 服务器广播
//
//        #endregion
//
//        #region 斗翼相关
//        public const int DOU_YI_XIULIAN = 18001;                 //客户端请求修炼斗翼
//        public const int DOU_YI_REV_XIULIAN = 18002;             //服务器下发斗翼修炼界面信息 
//        public const int DOU_YI_NINGYU = 18003;                  //客户端请求凝羽操作
//        public const int DOU_YI_REV_NINGYU = 18004;              //服务器下发凝羽界面信息 
//        public const int DOU_YI_HUANHUA = 18005;                 //客户端请求幻化 
//        public const int DOU_YI_REV_HUANHUA = 18006;             //服务器下发幻化结果 
//        public const int DOU_YI_HUANHUA_DATA = 18007;            //客户端请求幻化数据 
//        public const int DOU_YI_REV_ALL_HUANHUA = 18008;         //服务器下发可以用于幻化的斗翼的ID 
//        public const int DOU_YI_REV_CHANGE_HUANHUA = 18010;      //服务器下发广播玩家现在幻化的斗翼信息（广播）
//        #endregion
//
//        #region 摆摊相关
//        public const int BAI_TAN_S_100003 = 100003;              //客户端请求摊位列表
//        public const int BAI_TAN_R_100004 = 100004;              //客户端收到摊位列表 
//        public const int BAI_TAN_S_100005 = 100005;              //客户端请求进入别人摊位
//        public const int BAI_TAN_S_100007 = 100007;              //客户端请求进入自己摊位
//        public const int BAI_TAN_R_100006 = 100006;              //返回摊位信息 
//        public const int BAI_TAN_S_100009 = 100009;              //客户端请求摆摊
//        public const int BAI_TAN_R_100010 = 100010;              //客户端请求摆摊/物品上架失败
//        public const int BAI_TAN_S_100011 = 100011;              //客户端请求增加摆摊时限 
//        public const int BAI_TAN_S_100013 = 100013;              //客户端请求取消摆摊
//        public const int BAI_TAN_S_100015 = 100015;              //客户端请求物品上架
//        public const int BAI_TAN_S_100016 = 100016;              //客户端请求物品下架
//        public const int BAI_TAN_S_100017 = 100017;              //客户端请求提取金额数据
//        public const int BAI_TAN_R_100018 = 100018;              //返回提取金额
//        public const int BAI_TAN_S_100019 = 100019;              //客户端请求提取金额
//        public const int BAI_TAN_S_100020 = 100020;              //客户端请求购买物品
//        public const int BAI_TAN_S_100021 = 100021;              //客户端请求物品tips
//        public const int BAI_TAN_S_100022 = 100022;              //客户端请求关店
//        public const int BAI_TAN_S_100023 = 100023;              //客户端请求发摊位连接（聊天）
//        public const int BAI_TAN_R_100024 = 100024;              //返回摊位连接（聊天）
//        public const int BAI_TAN_S_100025 = 100025;              //客户端请求留言
//        public const int BAI_TAN_S_100026 = 100026;              //客户端请求出售晶钻信息
//        public const int BAI_TAN_R_100027 = 100027;              //返回出售晶钻信息
//        public const int BAI_TAN_S_100028 = 100028;              //客户端请求出售金币信息
//        public const int BAI_TAN_R_100029 = 100029;              //返回出售金币信息
//        public const int BAI_TAN_S_100030 = 100030;              //客户端请求摊位查询
//        public const int BAI_TAN_R_100031 = 100031;              //返回摊位查询
//        public const int BAI_TAN_R_100038 = 100038;              //返回出售金币/晶钻删除的条目
//        #endregion
//
//        #region   奖励相关
//        public const int PACKAGEONLINECOOLTIME_S2C_66001 = 66001;             //在线礼包冷去时间
//        public const int PACKAGEINFO_C2S_66002 = 66002;        //倒计时时间到后，客户端请求对礼包操作。查看或者领取
//        public const int PACKAGEONLINEINFO_S2C_66003 = 66003;   //在线奖励物品信息
//        public const int PACKAGENUM_S2C_66017 = 66017;
//        public const int SIGNINFO_S2C_66019 = 66019;            //签到奖励信息
//        public const int SIGNINPERSONINFO_S2C_66020 = 66020;            //人物签到信息
//        public const int SIGNINRESULT_S2C_66021 = 66021;            //每日5点的刷新签到信息
//
//        public const int ACCEPT_LEVEL_AWARD_REQUET = 66002;         //请求领取等级礼包
//
//        public const int SevenDayInfo_S2C_66006 = 66006;      //七日礼包信息
//        public const int GetSevenDayPack_S2C_66007 = 66007;      //获取七日礼包结果下发 
//        public const int SendGetPack_C2S_66008 = 66008;      //获取七日礼包结果下发 
//
//        #endregion
//
//        #region 各个模块战斗力
//        public const int ZHAN_LI = 51001;                        //各个模块战斗力（没引用，直接NetLoginBack赋值）
//        #endregion
//
//        #region 返回选择角色界面数据
//
//        public const int REBACKMENU = 10020;	    //请求退出返回登录界面
//        public const int REBACKMENU_BACK = 10021;	//获取退出返回登录界面消息
//        #endregion
//
//        #region 悬赏相关
//        public const int XUAN_SHANG_S_65039 = 65039;
//        public const int XUAN_SHANG_S_65040 = 65040;
//        public const int XUAN_SHANG_S_65042 = 65042;
//        public const int XUAN_SHANG_S_65043 = 65043;
//        public const int XUAN_SHANG_S_65044 = 65044;
//        public const int XUAN_SHANG_R_65041 = 65041;
//        public const int XUAN_SHANG_R_65045 = 65045;
//        public const int XUAN_SHANG_R_65046 = 65046;
//        public const int XUAN_SHANG_R_65047 = 65047;
//        public const int XUAN_SHANG_R_65048 = 65048;
//        public const int XUAN_SHANG_R_65049 = 65049;
//        public const int XUAN_SHANG_R_65050 = 65050;
//        public const int XUAN_SHANG_R_65051 = 65051;
//        #endregion
//
//        #region 红颜相关
//        public const int HONGYAN_S_69100 = 69100;
//        public const int HONGYAN_R_69101 = 69101;
//        public const int HONGYAN_S_69102 = 69102;
//        public const int HONGYAN_S_69201 = 69201;
//        #endregion
//
//        #region 奇遇相关
//        public const int QIYU_S_69103 = 69103;
//        public const int QIYU_R_69104 = 69104;
//        public const int QIYU_S_69105 = 69105;
//        public const int QIYU_R_69106 = 69106;
//        public const int QIYU_S_69107 = 69107;
//        public const int QIYU_R_69108 = 69108;
//        public const int QIYU_S_69109 = 69109;
//        public const int QIYU_R_69110 = 69110;
//        public const int QIYU_S_69111 = 69111;
//        public const int QIYU_R_69112 = 69112;
//        public const int QIYU_S_69113 = 69113;
//        public const int QIYU_R_69114 = 69114;
//        public const int QIYU_S_69115 = 69115;
//        public const int QIYU_R_69116 = 69116;
//        public const int QIYU_S_69117 = 69117;
//        public const int QIYU_R_69118 = 69118;
//        public const int QIYU_S_69119 = 69119;
//        public const int QIYU_R_69120 = 69120;
//        public const int QIYU_R_69122 = 69122;
//        public const int QIYU_S_69123 = 69123;
//        public const int QIYU_R_69124 = 69124;
//        public const int QIYU_S_69125 = 69125;
//        public const int QIYU_R_69126 = 69126;
//        public const int QIYU_S_69127 = 69127;
//        
//        public const int QIYU_S_69129 = 69129;
//        public const int QIYU_R_69130 = 69130;
//        public const int QIYU_S_69131 = 69131;
//        public const int QIYU_R_69132 = 69132;
//        public const int QIYU_S_69133 = 69133;
//        public const int QIYU_R_69134 = 69134;
//        public const int QIYU_S_69135 = 69135;
//        public const int QIYU_R_69136 = 69136;
//        public const int QIYU_S_69137 = 69137;
//        public const int QIYU_R_69138 = 69138;
//        public const int QIYU_S_69139 = 69139;
//        public const int QIYU_R_69128 = 69128;
//
//        public const int QIYU_S_69141 = 69141;
//        public const int QIYU_R_69142 = 69142;
//        public const int QIYU_S_69143 = 69143;
//        public const int QIYU_R_69144 = 69144;
//        #endregion
//
//        #region  称号
//        public const int ChengHao_C2S_50002 = 50002;    //开启关闭称号
//
//        public const int ChengHao_S2C_50000 = 50000;    //称号列表
//        public const int ChengHao_S2C_50001 = 50001;    //获得称号
//        public const int ChengHao_S2C_50003 = 50003;    //移除称号
//        public const int ChengHao_S2C_50004 = 50004;    //开启关闭称号结果
//        #endregion
//        #region 精彩活动
//        /// <summary>
//        /// 活动界面消息上行
//        /// </summary>
//        public const int C2S11030 = 11030;
//        /// <summary>
//        /// 活动界面消息下行
//        /// </summary>
//        public const int S2C11031 = 11031;
//        /// <summary>
//        /// 领奖上行
//        /// </summary>
//        public const int C2S11032 = 11032;
//        /// <summary>
//        /// 领奖下行
//        /// </summary>
//        public const int S2C11033 = 11033;
//
//        #endregion
//    }
//
