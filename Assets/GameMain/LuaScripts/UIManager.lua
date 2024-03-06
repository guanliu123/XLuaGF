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
--资源销毁的时候调用，lua层实例也要销毁
function  UIManager:OnDestroy(UIForm)
	local logic=self.uiformDict[UIForm]
	if not logic then return end

	self.uiformDict[UIForm]=nil
	Destroy(logic)
end

