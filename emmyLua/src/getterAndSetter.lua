print('getterAndSetter.lua')

MyClass = {}
MyClass.__index = MyClass;

function MyClass.New()
	local ret = {};
	setmetatable(ret, MyClass)
	return ret;
end


function MyClass:init()
    -- We'll store members in an internal table
    self.members = {}
end

function MyClass:__newindex( index, value )
    if index == "testMember" then
        self.members[index] = value
        print( "Set member " .. index .. " to " .. value )
    else
        rawset( self, index, value )
    end
end

function MyClass:__index( index )
    if index == "testMember" then
        print( "Getting " .. index )
        return self.members[index]
    else
        return rawget( self, index )
    end
end

function test()
    foo = MyClass.New();

    foo.testMember = 5
    foo.testMember = 2

    print( foo.testMember )
end

test()