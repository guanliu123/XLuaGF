print("---------------------->come on lua.")

local A = class()

function A:__init(x,y)
	print("------------------>x,y",x,y)
	self.x=x
	self.y=y
end

function A:test()
	print("---------->",self.x,self.y)
end

local a =A(1,2)
local b  = A("sss","tttt")

a:test()
b:test()
