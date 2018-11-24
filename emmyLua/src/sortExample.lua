print('sortExample.lua')

local data1 = { num = 1, data = "a" }
local data2 = { num = 2, data = "b" }
local data3 = { num = 3, data = "c" }

local dataSet = { data3, data2, data1 }

for key,value in pairs(dataSet) do
    print(key, value.num, value.data)
end

function compareData(a, b)
    return a.num < b.num
end

table.sort(dataSet, compareData)

print('after sort')

for key,value in pairs(dataSet) do
    print(key, value.num, value.data)
end