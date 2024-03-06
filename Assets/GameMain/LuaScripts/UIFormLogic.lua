local This = class("UIFormLogic")

This.AllowMultiInstance=false--�Ƿ�����࿪
This.PauseCoveredUIForm=true --���Ǻ��Ƿ���ͣ

--��
function This:Open()
	--����C#�еķ��������ص���һ�����кţ���������)
	local serialId=CS.GameMain.GameEntry.UI:OpenUIForm(self)
	self.serialId=serialId
	return serialId
end

function This:Close()
	self.UFormLogic:Close()
end

--�������ʼ��һЩ����Ķ���
-- @param UFormLogic unity�߼���
-- @param tf transform
-- @param go gameobject
function This:OnUInit(UFormLogic,tf,go)
	self.UFormLogic=UFormLogic
	self.tf=tf
	self.go=go

	--��������ķ���
	self:OnInit(UFormLogic,tf,go)
end

function This:OnInit()
	
end

--��ʾ�����ʱ������
function This:OnOpen()
	print("----------------->UI Open!")

	--�����¼���ʾui����ϣ���Ȼ���첽���أ��������п�����ͬ�������ã������Ӻ�һ֡�����¼�
	--�������ϵͳ���Ѿ��л��������Դ�����ܻᵼ��ͬ�������Ӻ�ǿ�����첽�����¼�
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