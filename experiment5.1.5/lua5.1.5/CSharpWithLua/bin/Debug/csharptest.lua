print('hello lua')

local a = true;
print(a)
local b = nil;
print(b)

test = {}
test.a = 1
test.b = 2
test.c = { d = 3 }
print(test)

LogTable2(test)

function TestCSharpCallLua()
	print("call lua test");
end

--Helper.LogTable(test)

--Helper.LogTable(test)