print('luaversion' .. _VERSION)

dofile('./sortAndSearchExample.lua')
dofile('./exceptionExample.lua')

test = vector3.New()
test.SetX(100)
print("GetX" .. test.GetX())