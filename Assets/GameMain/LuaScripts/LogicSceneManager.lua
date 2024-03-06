local This=singleton("LogicSceneManager")

function This:__init()
	self.busing=false
	self.current_scene=nil
	--��ǰ���ڼ��صĳ���
	self.loading_s=nil
end

---�л������߼�
-- @param logic_scene ���л����߼�������
-- @param force_reset �Ƿ�ǿ���л������統ǰ�����Ƿ����л�����ǰ����
function This:Switch(logic_scene,force_reset)
	--�Ƿ������л�����
	if self.busing then return end
	--�Ƿ��Ѿ��ڵ�ǰ������
	--instanceof�����ж�ǰ���Ƿ��Ǻ������ʵ��
	if self.current_scene then
		if not force_reset and instanceof(self.current_scene,logic_scene) then
			return
		end
	end

	self.busing=true

	--��ʾ����������㣬Ҫ��ס����
	CS.GameMain.GameEntry.Loading:Show()
	CS.GameMain.GameEntry.Loading:SetDesc("Loading...")

	--�ͷžɳ���
	if self.current_scene then
		Destory(self.current_scene)
		self.current_scene=nil
	end

	--�����³���
	local s=logic_scene()
	self.loading_s=s

	--������Դ���������첽����
	s:LoadRes()
end

--���س����ɹ�����첽�ص�
function This:OnLoadComplete()
	local s	= self.loading_s
	self.loading_s=nil
	self.current_scene=s
	--�رս�����
	CS.GameMain.GameEntry.Loading:SetDesc("LoadingComplete")
	CS.GameMain.GameEntry.Loading:Hide()
	self.busing=false
	--���뵽�³������߼�
	s:OnEnter()
end

return This