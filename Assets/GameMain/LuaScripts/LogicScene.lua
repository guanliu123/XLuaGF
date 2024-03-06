local This=class("LogicScene")

function  This:__init( ... )
	print("------------------>LogicScene __init")

	self.res_ui={}
	--�Ѿ����������Դ����
	self.load_count=0
	--�ܹ���Ҫ���ص�����
	self.load_tatal=0
end

--������Դ
function This:LoadRes()
	print("------------------>LogicScene LoadRes")

	--��¼��Ҫ�򿪵�UI���棬�������¼�ϵͳ�ڴ򿪺�ִ����Ӧ�߼�
	
	GEvent:Add("EVENT_UI_OPEN",self.OnUIOpen,self)

	self:LoadRes_()
	for k,v in pairs(self.res_ui) do
		self.load_tatal=self.load_tatal+1
	end
end

--����ʵ�־�������
function This:LoadRes_() end

function This:CheckLoadResComplete()
	--ÿ��һ��ui������Ƿ������һ����Ҫ�򿪵ģ������˵������ui��������
	CS.GameMain.GameEntry.Loading:SetLoading(self.load_count/self.load_tatal)
	if next(self.res_ui) then return end

	GEvent:Remove("EVENT_UI_OPEN",self.OnUIOpen)

	SceneManager:OnLoadComplete()
end

--�����Ǵ򿪵�uiʵ��
function This:OnUIOpen(ui_logic)
	self.res_ui[ui_logic]=nil
	self.load_count=self.load_count+1
	self:CheckLoadResComplete()
end

--���볡��
function This:OnEnter()
		print("------------------>LogicScene OnEnter")
end

function This:__release()

end

return This
