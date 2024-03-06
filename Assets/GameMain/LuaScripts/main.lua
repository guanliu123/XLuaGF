GameMain={}

function GameMain.Startup()
	Log.Startup()

	print("---------------------->Lua Startup.")

	--实例化一个定时器管理类，不使用单例是因为可能有多个针对不同情况的定时器管理者对象
	UpdateTimer=TimerManager()
	
	--事件全局监听实例
	GEvent = Event()

	SceneManager=LogicSceneManager()--单例，逻辑场景管理器

	SceneManager:Switch(LoginScene,true)
end

---逻辑驱动
-- @param elapseSceconds 逻辑流失时间，以秒为单位
-- @param realElapseSeconds 真实流失时间，以秒为单位
function GameMain.Update(elapseSceconds,realElapseSeconds)

	UpdateTimer:Update(elapseSceconds,realElapseSeconds)

end
