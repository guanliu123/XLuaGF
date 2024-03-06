local This = class("UIFormLogic")

This.AllowMultiInstance=false--是否允许多开
This.PauseCoveredUIForm=true --覆盖后是否暂停

--打开
function This:Open()
	--调用C#中的方法，返回的是一个序列号（自增序列)
	local serialId=CS.GameMain.GameEntry.UI:OpenUIForm(self)
	self.serialId=serialId
	return serialId
end

function This:Close()
	self.UFormLogic:Close()
end

--在这里初始化一些不变的东西
-- @param UFormLogic unity逻辑层
-- @param tf transform
-- @param go gameobject
function This:OnUInit(UFormLogic,tf,go)
	self.UFormLogic=UFormLogic
	self.tf=tf
	self.go=go

	--调用子类的方法
	self:OnInit(UFormLogic,tf,go)
end

function This:OnInit()
	
end

--显示界面的时候会调用
function This:OnOpen()
	print("----------------->UI Open!")

	--发送事件表示ui打开完毕，虽然是异步加载，但这里有可能是同步被调用，所以延后一帧发送事件
	--如果缓存系统中已经有缓存这个资源，可能会导致同步处理，延后强迫其异步发送事件
	self.open_event_timer=UpdateTimer:GetTimer(1,self.open_event,true,true,self)
end

function This:open_event()
	self.open_event_timer=nil

	GEvent:Call("EVENT_UI_OPEN",self)
end

function This:OnClose()
	print("----------------->UI Close!")
end

function This:OnDestroy()
	UIManager:OnDestroy(classof(self))
end

function This:__release()
	self.UFormLogic=nil
	self.tf=nil
	self.go=nil

	if self.open_event_timer then
		UpdateTimer:Remove(self.open_event_timer)
		self.open_event_timer=nil
	end
end

return This