UIManager={
	uiformDict={}
}

function UIManager:Open(UIForm)
	local logic=self.uiformDict[UIForm]
	if not logic then
		logic=UIForm()
	end

	logic:Open()
	return logic
end
--��Դ���ٵ�ʱ����ã�lua��ʵ��ҲҪ����
function  UIManager:OnDestroy(UIForm)
	local logic=self.uiformDict[UIForm]
	if not logic then return end

	self.uiformDict[UIForm]=nil
	Destroy(logic)
end

