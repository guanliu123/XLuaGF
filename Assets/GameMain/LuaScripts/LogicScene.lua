local This=class("LogicScene")

function  This:__init( ... )
	print("------------------>LogicScene __init")

	self.res_ui={}
	--ÒÑ¾­¼ÓÔØÍêµÄ×ÊÔ´ÊýÁ¿
	self.load_count=0
	--×Ü¹²ÐèÒª¼ÓÔØµÄÊýÁ¿
	self.load_tatal=0
end

--ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ô´
function This:LoadRes()
	print("------------------>LogicScene LoadRes")

	--ï¿½ï¿½Â¼ï¿½ï¿½Òªï¿½ò¿ªµï¿½UIï¿½ï¿½ï¿½æ£¬ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Â¼ï¿½ÏµÍ³ï¿½Ú´ò¿ªºï¿½Ö´ï¿½ï¿½ï¿½ï¿½Ó¦ï¿½ß¼ï¿½
	
	GEvent:Add("EVENT_UI_OPEN",self.OnUIOpen,self)

	self:LoadRes_()
	for k,v in pairs(self.res_ui) do
		self.load_tatal=self.load_tatal+1
	end
end

--ï¿½ï¿½ï¿½ï¿½Êµï¿½Ö¾ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
function This:LoadRes_() end

function This:CheckLoadResComplete()
	--Ã¿ï¿½ï¿½Ò»ï¿½ï¿½uiï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ç·ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ò»ï¿½ï¿½ï¿½ï¿½Òªï¿½ò¿ªµÄ£ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ëµï¿½ï¿½ï¿½ï¿½ï¿½ï¿½uiï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
	CS.GameMain.GameEntry.Loading:SetLoading(self.load_count/self.load_tatal)
	if next(self.res_ui) then return end

	GEvent:Remove("EVENT_UI_OPEN",self.OnUIOpen)

	SceneManager:OnLoadComplete()
end

--ï¿½ï¿½ï¿½ï¿½ï¿½Ç´ò¿ªµï¿½uiÊµï¿½ï¿½
function This:OnUIOpen(ui_logic)
	self.res_ui[ui_logic]=nil
	self.load_count=self.load_count+1
	self:CheckLoadResComplete()
end

--ï¿½ï¿½ï¿½ë³¡ï¿½ï¿½
function This:OnEnter()
		print("------------------>LogicScene OnEnter")
end

function This:__release()

end

return This
