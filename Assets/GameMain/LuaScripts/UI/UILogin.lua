local This=class("UILogin",UIFormLogic)

This.assetName="Assets/GameMain/Prefabs/UI/LoginPanel.prefab"--��Դ·��
This.UIGroupName="normal"--����

function This:OnInit(UFormLogic,tf,go)
	local button = tf:Find("Button"):GetComponent(typeof(CS.UnityEngine.UI.Button))
	button.onClick:AddListener(function()
		print("----------------->UILogin button click")

        self:Close()
		UIManager.Open(UIPlayerInfo)
	end)

	local button_text=button.transform:Find("name"):GetComponent(typeof(CS.TMPro.TextMeshProUGUI))
	button_text.text="Start Game"
end

return This