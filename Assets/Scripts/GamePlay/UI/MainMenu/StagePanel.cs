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
        StageScnMgr.scnInfo.roleList = new List<StageRoleInfoStruct>();

        StageRoleInfoStruct r1 = new StageRoleInfoStruct();
        r1.BodyResID = "mdl_isabella_stagea_hairb.ab";

        StageRoleInfoStruct r2 = new StageRoleInfoStruct();
        r2.BodyResID = "mdl_katya_stagea.ab";

        StageRoleInfoStruct r3 = new StageRoleInfoStruct();
        r3.BodyResID = "mdl_moxi_stagea.ab";

        StageRoleInfoStruct r4 = new StageRoleInfoStruct();
        r4.BodyResID = "mdl_rose_stagea.ab";

        StageRoleInfoStruct r5 = new StageRoleInfoStruct();
        r5.BodyResID = "mdl_tamamo_stagea.ab";

        StageRoleInfoStruct r6 = new StageRoleInfoStruct();
        r6.BodyResID = "mdl_yueling_stagea_low_addblendshape_migicacloth_variant.ab";

        AppMgr.GetIns().SetState(AppMgr.AppState.loading);

        StageScnMgr.Create();

    }


}
