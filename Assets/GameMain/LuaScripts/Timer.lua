local This=class("Timer")

---初始化参数
-- @param delay 延时的时间
-- @param f 回调函数
-- @param isLoop 是否是循环定时器
-- @param isFrame 是否是帧计时器
function  This:Init(delay,f,isLoop,isFrame,obj)
	self.delay=delay
	self.f=f
	self.obj=obj
	self.isLoop=isLoop
	self.isFrame=isFrame

	--这两个另外加的参数用于逻辑计算
	self.left=delay
	self.isover=false
end

function This:Reset()
	self.delay=nil
	self.f=nil
	self.obj=nil
	self.isLoop=nil
	self.isFrame=nil

	--这两个另外加的参数用于逻辑计算
	self.left=nil
	self.isover=nil
end


function  This:Update(elapseSeconds,realElapseSeconds)
	--根据帧的计时器，帧-1
	if self.isFrame then
		self.left=self.left-1
	else
		self.left=self.left-elapseSeconds
	end

	local timeout=self.left<=0
	if not timeout then return end;

	--计时结束
	if self.isLoop then
		self.left=self.delay
	else
		self.isover=true;
	end

	--使用保护模式运行，因为可能会有多个计时器，如果其中某个计时器回调错误会影响其他后面的计时器执行
	--让主循环的update不中断

	--判断实例是否被销毁了（可能是实例传入的回调方法）,如果是则立刻停止计时器，哪怕是循环计时器
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
