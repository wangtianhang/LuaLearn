---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by bulan.
--- DateTime: 2018/11/24 13:59
---

print('=====================================exceptionExample.lua')

function exceptionFunc()

    local dataSet = {}
    print(dataSet[1].value)

end

--exceptionFunc();

if(pcall(exceptionFunc)) then
    print("pcall execute func no exception")
else
    print("pcall execute func trigger exception")
    --error()
end

function exceptionHandle(msg)
    print("------------")
    print("LUA ERROR: " .. tostring(msg) .. "\n")
    print(debug.traceback())
    print("------------")
end

if(xpcall(exceptionFunc, exceptionHandle)) then
    print("xpcall execute func no exception")
else
    print("xpcall execute func trigger exception")
end