local This = class("LoginScene",LogicScene)

function This:__init()

end

--������Դ
function This:LoadRes_()
	local uilogin = UIManager:Open(UILogin)
	self.res_ui[uilogin]=true
end


return This