print('====================================getterAndSetter.lua')

MyClass = { };
--MyClass.__index = MyClass;

--function MyClass:Test()
--    print('MyClass Test')
--end

function MyClass:New()
    ret = { members = {} };
    setmetatable(ret, MyClass)
    return ret;
end

function MyClass:__newindex( index, value )
    print('MyClass __newindex ' .. index)
    if index == "testMember" then
        self.members[index] = value
        print( "Set member " .. index .. " to " .. value )
    else
        rawset( self, index, value )
    end
end

function MyClass:__index( index )
    print('MyClass __index ' .. index)
    if index == "testMember" then
        print( "Getting " .. index )
        return self.members[index]
    else
        return rawget( self, index )
        --return self.index
    end
end

function test()

    foo = MyClass:New()
    
    foo.testMember = 5
    foo.testMember = 2
    
    --foo.Test()

    print( foo.testMember )
end

test()