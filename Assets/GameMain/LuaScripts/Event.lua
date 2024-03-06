local This=class("Event")

function This:__init()
	self.list={}
end

--���һ������
-- @param name �¼�����
-- @param f �¼��ص�
-- @param obj �����ʵ������Ļص�������obj��ʵ������
function This:Add(name,f,obj)
	local a=self.list[name]
	if not a then
		a={}
		self.list[name]=a
	end

	if obj then
		a[f]=obj
	else
		a[f]=true
	end
end

--�Ƴ�����
-- @param name �¼�����
-- @param f �¼��ص�
function This:Remove(name,f)
	local a =self.list[name]
	if not a then return end

	a[f]=nil
end

--�����¼�
-- @param name �¼�����
-- @param ...�������÷���
function This:Call(name,...)
	local a=self.list[name]
	if not a then return end
	
	local ok,err
	for f,obj in pairs(a) do
		--�ж�ֵ���ͣ������boolean����˵������ʵ���Ĵ��ص�����
		if type(obj) == "boolean" then
			ok,err=xpcall(f,debug.traceback,...)	
			if not ok then
				print("------------>Event call error",err)
			end
		else
			if not obj.__released then
				ok,err=xpcall(f,debug.traceback,obj,...)
				if not ok then
					print("------------>Event call error",err)
				end
			else
				a[f]=nil--�Ƴ�����
			end
		end
	end

end

return This
