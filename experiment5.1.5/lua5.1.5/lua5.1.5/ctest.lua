print('hello lua')

meta = {}

test = {}
test.a = 1
test.b = 2
test.c = { d = 3 }
test.e = function() end
setmetatable(test, meta)

print('test metatable', getmetatable(test))

for k, v in pairs(test) do
	print(tostring(k), '  ', tostring(v))
end

print('=================')

LogTable2(test)

print('=================')

Helper.LogTable(test)

--Helper.LogTable(test)