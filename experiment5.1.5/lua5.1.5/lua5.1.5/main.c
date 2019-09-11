
#include "lua.h"
#include "lauxlib.h"
#include "lualib.h"

static int LogTable(lua_State *L)
{
	if (lua_istable(L, -1))
	{
		//int tableIndex = lua_gettop(L);
		// push the first key
		//lua_pushnil(L);
		lua_pushnil(L);
		while (lua_next(L, -2)) 
		{
			/* 此时栈上 -1 处为 value, -2 处为 key */
			const char * keyStr = lua_tostring(L, -2);
			const char * valueStr = lua_tostring(L, -1);
			printf("%s %s\n", keyStr, valueStr);
			lua_pop(L, 1);
		}
	}
	return 0;
}

static const luaL_Reg helperlib[] = {
  {"LogTable",   LogTable},
  {NULL, NULL}
};

void RegisterHelperLib(lua_State *L)
{
	luaL_register(L, "Helper", helperlib);
}

int main(int argc, char **argv)
{
	//int status;
	//struct Smain s;
	lua_State *L = lua_open();  /* create state */
	if (L == NULL) {
		printf("cannot create state: not enough memory");
		return 1;
	}
	luaL_openlibs(L);  /* open libraries */
	RegisterHelperLib(L);
	luaL_dofile(L, argv[1]);
	lua_close(L);
	return 0;
}