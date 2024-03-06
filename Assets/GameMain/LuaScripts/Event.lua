local This=class("Event")

function This:__init()
	self.list={}
end

--添加一个监听
-- @param name 事件名字
-- @param f 事件回调
-- @param obj 如果是实例传入的回调参数，obj是实例自身
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

--移除监听
-- @param name 事件名字
-- @param f 事件回调
function This:Remove(name,f)
	local a =self.list[name]
	if not a then return end

	a[f]=nil
end

--发送事件
-- @param name 事件名字
-- @param ...其他调用方法
function This:Call(name,...)
	local a=self.list[name]
	if not a then return end
	
	local ok,err
	for f,obj in pairs(a) do
		--判断值类型，如果是boolean类型说明不是实例的传回调参数
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
				a[f]=nil--移除监听
			end
		end
	end

end

return This
