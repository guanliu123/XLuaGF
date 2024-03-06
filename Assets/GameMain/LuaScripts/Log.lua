local oldprint = print
Log={}

--默认空方法，当没有宏的时候即使调用也什么都不输出
local print_empty=function() end
Log.Info=print_empty
Log.Debug=print_empty
Log.Error=print_empty
Log.Warning=print_empty
Log.Fatal=print_empty

--根据宏进行初始化
function Log.Startup()
	if(Log.LEVEL_DEBUG) then
		Log.Debug=oldprint
	end
	if(Log.LEVEL_INFO) then
		Log.Info=oldprint
	end
	if(Log.LEVEL_WARNING) then
		Log.Warning=oldprint
	end
	if(Log.LEVEL_ERROR) then
		Log.Error=oldprint
	end
	if(Log.LEVEL_FATAL) then
		Log.Fatal=oldprint
	end

	print=Log.Info
end
