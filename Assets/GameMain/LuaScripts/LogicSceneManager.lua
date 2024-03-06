local This=singleton("LogicSceneManager")

function This:__init()
	self.busing=false
	self.current_scene=nil
	--当前正在加载的场景
	self.loading_s=nil
end

---切换场景逻辑
-- @param logic_scene 欲切换的逻辑场景类
-- @param force_reset 是否强制切换，比如当前场景是否还能切换到当前场景
function This:Switch(logic_scene,force_reset)
	--是否正在切换场景
	if self.busing then return end
	--是否已经在当前场景中
	--instanceof可以判断前者是否是后者类的实例
	if self.current_scene then
		if not force_reset and instanceof(self.current_scene,logic_scene) then
			return
		end
	end

	self.busing=true

	--显示进度条，最顶层，要盖住所有
	CS.GameMain.GameEntry.Loading:Show()
	CS.GameMain.GameEntry.Loading:SetDesc("Loading...")

	--释放旧场景
	if self.current_scene then
		Destory(self.current_scene)
		self.current_scene=nil
	end

	--创建新场景
	local s=logic_scene()
	self.loading_s=s

	--加载资源，这里是异步加载
	s:LoadRes()
end

--加载场景成功后的异步回调
function This:OnLoadComplete()
	local s	= self.loading_s
	self.loading_s=nil
	self.current_scene=s
	--关闭进度条
	CS.GameMain.GameEntry.Loading:SetDesc("LoadingComplete")
	CS.GameMain.GameEntry.Loading:Hide()
	self.busing=false
	--进入到新场景的逻辑
	s:OnEnter()
end

return This