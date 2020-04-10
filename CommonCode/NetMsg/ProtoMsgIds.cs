//gen by tool
public enum ProtoMsgIds
{
	//GC2CS
	GC2CS_EnterGameService = 13001,//登录成功后 需要请求进入真正的游戏服务 也就是通过 发送到 GS 转发到 CS 验证后即可进入真正的游戏服务
	GC2CS_HeroList = 13002,
	GC2CS_AddHeroLevel = 13003,
	GC2CS_NotifyUpdateHeroes = 13004,
	//GC2CS_StartMateCombat = 13500,

	//GC2LS
	GC2LS_AskLogin = 11001,

	//GS2CS
	GS2CS_UserLogin = 15001,
	GS2CS_UserExit = 15002,
	//GS2CS_FromSS_CreateCombatFinish = 15500,

	//GS2GC
	GS2GC_FromCS_SyncPlayerBaseInfo = 14005,
	//GS2GC_FromCS_StartCombat = 14010,

	//LS2GS
	LS2GS_VerificationAskLogin = 12001,

	//NetCommon
	HeartBeatHandshake = 101,
	HeartBeatSend = 102,
	HeartBeatBack = 103,

	//ResultCode

}
