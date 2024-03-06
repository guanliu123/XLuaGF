local This=class("TimerManager")

function This:__init()
    --��ż�ʱ���б�
	self.update_timer={}
	--����б��Ǵ�������µļ�ʱ�����б�
	--��Ϊ�ص��������ܻ������µļ�ʱ��������ѭ��������ʱ������Ԫ�ػᵼ�´���
	--���������¼���ļ�ʱ������ֱ�Ӽ��뵱ǰ�����������м�ʱ�������Ƿŵ����������һ��ѭ��֮ǰһ���Լ���
	self.toAdd={}
	--����أ����洴�����������Ѿ������˵ļ�ʱ�����´��뿪�µļ�ʱ���ȴ�����������û��
	self.pool={}
end

--����ѭ���е��ø�update
function This:Update(elapseSeconds,realElapseSeconds)
	for timer in pairs(self.toAdd) do
		self.update_timer[timer]=true
		self.toAdd[timer]=nil
	end
	
	for timer in pairs(self.update_timer) do
		timer:Update(elapseSeconds,realElapseSeconds);
	end

	for timer in pairs(self.update_timer) do
		if(timer.isover) then
			self.update_timer[timer]=nil
			timer:Reset()
			table.insert(self.pool,timer)
		end
	end
end

--�Ƴ���ʱ���ķ������ڶ�������ʱ�Ƴ�����ؼ�ʱ��
function This:Remove(timer)
	self.update_timer[timer]=nil
	self.toAdd[timer]=nil

	timer:Reset()
	table.insert(self.pool,timer)
end

--����ӿڵ����þ����ṩ���ⲿ���ã�����һ����ʱ��
--����obj�Ǽ��紫��Ļص�����f�ǵ����ĺ�������Ҫ�ѵ�����ʵ������һ�£��漰�������÷�ʽ�����������־��
function This:GetTimer(delay,f,isLoop,isFrame,obj)
	local timer=table.remove(self.pool)
	if not timer then
		timer=Timer()
	end
	timer:Init(delay,f,isLoop,isFrame,obj)


	self.toAdd[timer] = true
	return timer
end

return This;