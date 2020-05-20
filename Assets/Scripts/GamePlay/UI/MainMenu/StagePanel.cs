using ntools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StagePanel : MonoBehaviour
{
    [SerializeField] Button ExitBtn;

    private void OnEnable()
    {
        ExitBtn.onClick.AddListener(ExitClick);
    }
    private void OnDisable()
    {
        ExitBtn.onClick.RemoveAllListeners();
    }

    public void ExitClick()
    {       
        Messenger.Broadcast<string>(GameEvent.switch_menu_state, "login");
    }

    public void TempEnterStageClick()
    {
        //NTODO 这里数据应该列表，列表数据来自server
        StageScnMgr.scnInfo = new StageScnInfoStruct();
        StageScnMgr.scnInfo.ScnName = "stage1";
        StageScnMgr.scnInfo.roleList = new List<PlayerInfoStruct>();

        PlayerInfoStruct r1 = new PlayerInfoStruct();
        r1.ID = "isabella";
        r1.BodyModelResID = "mdl_isabella_stagea_hairb";
        StageScnMgr.scnInfo.roleList.Add(r1);

        PlayerInfoStruct r2 = new PlayerInfoStruct();
        r2.ID = "katya";
        r2.BodyModelResID = "mdl_katya_stagea";
        StageScnMgr.scnInfo.roleList.Add(r2);

        PlayerInfoStruct r3 = new PlayerInfoStruct();
        r3.ID = "moxi";
        r3.BodyModelResID = "mdl_moxi_stagea";
        StageScnMgr.scnInfo.roleList.Add(r3);

        PlayerInfoStruct r4 = new PlayerInfoStruct();
        r4.ID = "rose";
        r4.BodyModelResID = "mdl_rose_stagea";
        StageScnMgr.scnInfo.roleList.Add(r4);

        PlayerInfoStruct r5 = new PlayerInfoStruct();
        r5.ID = "tamamo";
        r5.BodyModelResID = "mdl_tamamo_stagea";
        StageScnMgr.scnInfo.roleList.Add(r5);

        PlayerInfoStruct r6 = new PlayerInfoStruct();
        r6.ID = "yueling";
        r6.BodyModelResID = "mdl_yueling_stagea_low_addblendshape_migicacloth_variant";
        StageScnMgr.scnInfo.roleList.Add(r6);

        AppMgr.GetIns().SetState(AppMgr.AppState.loading);

        StageScnMgr.Create();

    }


}
