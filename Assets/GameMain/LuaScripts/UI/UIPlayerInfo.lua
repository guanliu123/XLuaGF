local This=class("UIPlayerInfo",UIFormLogic)

This.assetName="Assets/GameMain/Prefabs/UI/PlayerInfoPanel.prefab"--资源路径
This.UIGroupName="normal"--组名

function This:OnInit(UFormLogic,tf,go)
	--在lua中可以调用unity的方法来获取组件，调用方法是一样的，不过要注意：调用
	local button = tf:Find("Button"):GetComponent(typeof(CS.UnityEngine.UI.Button))
	button.onClick:AddListener(function()
		print("----------------->UIPlayerInfo button click")
	end)

	local button_text=button.transform:Find("name"):GetComponent(typeof(CS.TMPro.TextMeshProUGUI))
	button_text.text="Player Information"
end

return This