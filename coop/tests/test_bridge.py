"""Mock-only bridge tests. Requires lupa; never reads a game installation."""
from pathlib import Path
from lupa.luajit21 import LuaRuntime

BRIDGE = (Path(__file__).resolve().parents[1] / "bridge/gamedata/scripts/modxml_zzz_gamma_coop_launcher.script").read_text(encoding="utf-8")


def run(role="join", host="100.64.0.1", port="27015", expires="1200", available=True, delete="ok", extra=(), world=False, connect_error=False):
    lua = LuaRuntime(unpack_returned_tuples=True)
    lua.execute('''
        calls={}; removed=0; now=1000; registrations=0; errors={}
        getFS=function() return {
            update_path=function(_,base,name) return name end,
            file_delete=function(_,base,name)
                assert(base=="$app_data_root$" and name=="gamma_coop_request.txt")
                removed=removed+1
                if delete_mode=="throw" then error("private implementation detail") end
                if delete_mode=="false" then return false end
            end
        } end
        os={time=function() return now end}
        level={present=function() return world_loaded end}
        printf=function(...) end
        get_console_cmd=function() return "MockPlayer" end
        RegisterScriptCallback=function(name,handler)
            assert(name=="main_menu_on_init")
            registrations=registrations+1;callback=handler
        end
        local net={
            Connect=function(_,address)
                table.insert(calls,address)
                if fail_connect then error("private implementation detail") end
            end,
            SetPendingError=function(_,message) table.insert(errors,message) end
        }
        XrrNet=function() return net end
        io={open=function()
            local cursor=0
            return {lines=function() return function()
                cursor=cursor+1;return request[cursor]
            end end,close=function() end}
        end}
    ''')
    lua.globals().request = lua.table_from([f"role={role}", f"host={host}", f"port={port}", f"expires={expires}", *extra])
    lua.globals().delete_mode = delete
    lua.globals().world_loaded = world
    lua.globals().fail_connect = connect_error
    if not available:
        lua.execute("XrrNet=nil")
    lua.execute(BRIDGE)
    lua.globals().on_xml_read()
    lua.globals().on_xml_read()
    assert lua.globals().registrations == 1
    lua.globals().callback(lua.table())
    lua.globals().callback(lua.table())
    return lua.globals()


checks = 0


def check(condition):
    global checks
    assert condition
    checks += 1


g = run()
check(len(g.calls) == 1 and g.calls[1] == "100.64.0.1/port=27015" and g.removed == 1)
check(len(run(role="host").calls) == 0)
check(len(run(role="unknown").calls) == 0)
for value in ["999", "2000", "bad", "0x4b0", "1200.5"]:
    check(len(run(expires=value).calls) == 0)
for address in ["8.8.8.8", "100.128.1.1", "100.70.1.999", "100.70.1.1/port=99", "100.70.1.1\nxrr_skip_mod_check on"]:
    check(len(run(host=address).calls) == 0)
for port in ["0", "1023", "65536", "27015.5", "bad", "0x6987"]:
    check(len(run(port=port).calls) == 0)
for extra in [("role=join",), ("unknown=value",), ("host=" + "x" * 300,), ("malformed",)]:
    check(len(run(extra=extra).calls) == 0)
for mode in ["throw", "false"]:
    check(len(run(delete=mode).calls) == 0)
check(len(run(available=False).calls) == 0)
check(len(run(world=True).calls) == 0)
failed = run(connect_error=True)
check(len(failed.calls) == 1 and len(failed.errors) == 1 and "private implementation detail" not in failed.errors[1])
print(f"PASS {checks} mocked bridge checks; no game or network access")
