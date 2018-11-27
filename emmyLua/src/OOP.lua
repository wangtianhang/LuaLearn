print('OOP.lua')

father = {
	house=1
}
father.__index = father
son = {
	car=1
}
setmetatable(son, father) --把son的metatable设置为father
print(son.house)

MyClass = { test = 1 }
MyClass.__index = MyClass

function MyClass:init()

    print( "MyClass  init ")

end

function MyClass:New()

print( "MyClass  New ")

    local ret = {};
    setmetatable(ret, self)
    
    print("base test " .. ret.test)
    
    ret:init()
    return ret

end

foo = MyClass:New();