GameMain={}

function GameMain.Startup()
	Log.Startup()

	print("---------------------->Lua Startup.")

	--ʵ����һ����ʱ�������࣬��ʹ�õ�������Ϊ�����ж����Բ�ͬ����Ķ�ʱ�������߶���
	UpdateTimer=TimerManager()
	
	--�¼�ȫ�ּ���ʵ��
	GEvent = Event()

	SceneManager=LogicSceneManager()--�������߼�����������

	SceneManager:Switch(LoginScene,true)
end

---�߼�����
-- @param elapseSceconds �߼���ʧʱ�䣬����Ϊ��λ
-- @param realElapseSeconds ��ʵ��ʧʱ�䣬����Ϊ��λ
function GameMain.Update(elapseSceconds,realElapseSeconds)

	UpdateTimer:Update(elapseSceconds,realElapseSeconds)

end
