local This=class("TimerManager")

function This:__init()
    --存放计时器列表
	self.update_timer={}
	--这个列表是待加入的新的计时器的列表
	--因为回调函数可能会增加新的计时器，而在循环遍历表时增加新元素会导致错误
	--所以所有新加入的计时器都不直接加入当前遍历的运行中计时器表，而是放到这个表中下一次循环之前一次性加入
	self.toAdd={}
	--对象池，缓存创建出来并且已经用完了的计时器，下次想开新的计时器先从这里面找有没有
	self.pool={}
end

--在主循环中调用该update
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

--移除计时器的方法，在对象被销毁时移除其相关计时器
function This:Remove(timer)
	self.update_timer[timer]=nil
	self.toAdd[timer]=nil

	timer:Reset()
	table.insert(self.pool,timer)
end

--这个接口的作用就是提供给外部调用，返回一个计时器
--这里obj是假如传入的回调参数f是单例的函数，需要把单例的实例保存一下（涉及隐含调用方式，详见开发日志）
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