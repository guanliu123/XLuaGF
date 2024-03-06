local This=class("Timer")

---��ʼ������
-- @param delay ��ʱ��ʱ��
-- @param f �ص�����
-- @param isLoop �Ƿ���ѭ����ʱ��
-- @param isFrame �Ƿ���֡��ʱ��
function  This:Init(delay,f,isLoop,isFrame,obj)
	self.delay=delay
	self.f=f
	self.obj=obj
	self.isLoop=isLoop
	self.isFrame=isFrame

	--����������ӵĲ��������߼�����
	self.left=delay
	self.isover=false
end

function This:Reset()
	self.delay=nil
	self.f=nil
	self.obj=nil
	self.isLoop=nil
	self.isFrame=nil

	--����������ӵĲ��������߼�����
	self.left=nil
	self.isover=nil
end


function  This:Update(elapseSeconds,realElapseSeconds)
	--����֡�ļ�ʱ����֡-1
	if self.isFrame then
		self.left=self.left-1
	else
		self.left=self.left-elapseSeconds
	end

	local timeout=self.left<=0
	if not timeout then return end;

	--��ʱ����
	if self.isLoop then
		self.left=self.delay
	else
		self.isover=true;
	end

	--ʹ�ñ���ģʽ���У���Ϊ���ܻ��ж����ʱ�����������ĳ����ʱ���ص������Ӱ����������ļ�ʱ��ִ��
	--����ѭ����update���ж�

	--�ж�ʵ���Ƿ������ˣ�������ʵ������Ļص�������,�����������ֹͣ��ʱ����������ѭ����ʱ��
	if self.obj then
		if not self.obj.__released then
			local ok,err=xpcall(self.f,debug.traceback,self.obj)
			if not ok then
				print("-------------->timer err:",err);
			end
		else
			self.isover=true
		end
	else
		local ok,err=xpcall(self.f,debug.traceback)
		if not ok then
			print("-------------->timer err:",err);
		end
	end
	

end
