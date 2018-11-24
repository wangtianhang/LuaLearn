#define ldblib_c
#define LUA_LIB

#include "luasrc/lua.h"
#include "luasrc/lauxlib.h"
#include "luasrc/lualib.h"

static struct Vector3
{
	float x;
	float y;
	float z;
};

static int NewVector3(lua_State *L)
{
	size_t size = sizeof(struct Vector3);
	struct Vector3 *pVector3;
	pVector3 = (struct Vector3 *)lua_newuserdata(L, size);
	pVector3->x = 0;
	pVector3->y = 0;
	pVector3->z = 0;

	luaL_getmetatable(L, "vector3_meta_table");

	//则把新生成的userdata的metatable设置为vector3_meta_table
	lua_setmetatable(L, -2);

	return 1;
}

static int GetX(lua_State *L)
{
	struct Vector3 *pVector3 = (struct Vector3 *)luaL_checkudata(L, 1, "vector3_meta_table");
	//luaL_argcheck(L, pVector3 != NULL, 1, "Wrong Parameter");
	lua_pushnumber(L, pVector3->x);
	return 1;
}

static int SetX(lua_State *L)
{
	struct Vector3 *pVector3 = (struct Vector3 *)luaL_checkudata(L, 1, "vector3_meta_table");
	//luaL_argcheck(L, pVector3 != NULL, 1, "Wrong Parameter");

	float x = luaL_checknumber(L, 2);
	//luaL_argcheck(L, pVector3 != NULL, 2, "Wrong Parameter");
	pVector3->x = x;
	return 0;
}

static const luaL_Reg vector3Lib[] =
{
	{ "New", NewVector3 },
	{NULL, NULL}
};

static const luaL_Reg vector3_meta[] = 
{
	{ "GetX", GetX },
	{ "SetX", SetX },
	{ NULL, NULL }
};

LUAMOD_API int luaopen_vector3(lua_State *L)
{
	luaL_newmetatable(L, "vector3_meta_table");

	lua_pushvalue(L, -1);
	lua_setfield(L, -2, "__index");
	luaL_setfuncs(L, vector3_meta, 0);

	luaL_newlib(L, vector3Lib);
	return 1;
}