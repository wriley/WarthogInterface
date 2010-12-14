-- Data export script for Lock On version 1.2.
-- Copyright (C) 2006, Eagle Dynamics.
-- See http://www.lua.org for Lua script system info 
-- We recommend to use the LuaSocket addon (http://www.tecgraf.puc-rio.br/luasocket) 
-- to use standard network protocols in Lua scripts.
-- LuaSocket 2.0 files (*.dll and *.lua) are supplied in the Scripts/LuaSocket folder
-- and in the installation folder of the Lock On version 1.2. 

-- Please, set EnableExportScript = true in the Config/Export/Config.lua file
-- to activate this script!

-- Expand the functionality of following functions for your external application needs.
-- Look into ./Temp/Error.log for this script errors, please.

-- Uncomment if using Vector class from the Config/Export/Vector.lua file 
--[[	
LUA_PATH = "?;?.lua;./Config/Export/?.lua"
require 'Vector'
-- See the Config/Export/Vector.lua file for Vector class details, please.
--]]

 host = "127.0.0.1"
 port = 9089
 gArguments = {[76]="%.4f", [77]="%.4f", [300]="%.1f"}

-- Simulation id
gSimID = string.format("%08x",os.time())

-- State data for export
gSendStrings = {gSimID, '*'}
gLastData = {}

-- Helper Functions
function StrSplit(str, delim, maxNb)
    -- Eliminate bad cases...
    if string.find(str, delim) == nil then
        return { str }
    end
    if maxNb == nil or maxNb < 1 then
        maxNb = 0    -- No limit
    end
    local result = {}
    local pat = "(.-)" .. delim .. "()"
    local nb = 0
    local lastPos
    for part, pos in string.gfind(str, pat) do
        nb = nb + 1
        result[nb] = part
        lastPos = pos
        if nb == maxNb then break end
    end
    -- Handle the last field
    if nb ~= maxNb then
        result[nb + 1] = string.sub(str, lastPos)
    end
    return result
end

function round(num, idp)
  local mult = 10^(idp or 0)
  return math.floor(num * mult + 0.5) / mult
end

-- Status Gathering Functions
function ProcessMainPanel()
		local lArgument , lFormat , lArgumentValue
		local HSI    = LoGetControlPanel_HSI()
		local lDevice = GetDevice(0)
		lDevice:update_arguments()
		
		for lArgument, lFormat in pairs(gArguments) do 
			lArgumentValue = string.format(lFormat,lDevice:get_argument_value(lArgument))
			SendData(lArgument, lArgumentValue)
		end
end

-- Network Functions

function SendData(id, value)
	
	if string.len(value) > 3 and value == string.sub("-0.00000000",1, string.len(value)) then
		value = value:sub(2)
	end
	
	if gLastData[id] ~= value then
		table.insert(gSendStrings, id .. "=" .. value)
		gLastData[id] = value
		
		if #gSendStrings > 140 then
			socket.try(c:send(table.concat(gSendStrings, ":").."\n"))
			gSendStrings = {gSimID, '*'}
		end		
	end	
end

function FlushData()
	if #gSendStrings > 0 then
		socket.try(c:send(table.concat(gSendStrings, ":").."\n"))
		gSendStrings = {gSimID, '*'}
	end
end

function ProcessInput()
    local lInput = c:receive()
    local lCommand, lCommandArgs, lDevice, lArgument, lLastValue
    
    if lInput then
	
        lCommand = string.sub(lInput,1,1)
        
		if lCommand == "R" then
			ResetChangeValues()
		end
	
		if (lCommand == "C") then
			lCommandArgs = StrSplit(string.sub(lInput,2),",")
			lDevice = GetDevice(lCommandArgs[1])
			if type(lDevice) == "table" then
				lDevice:performClickableAction(lCommandArgs[2] + 3000,lCommandArgs[3])
			else
				logfile:write("Device ", lCommandArgs[1], " is not a table\n");
			end
		end
    end
end

function ResetChangeValues()
	logfile:write("Sending all values.", "\n")
	for lArgument, lFormat in pairs(gArguments) do
		gLastData[lArgument] = "99999" 
	end
end

function ProcessOutput()
	ProcessMainPanel()
	FlushData()
end

function LuaExportStart()
-- Works once just before mission start.

    -- Open log file for export
    logfile = io.open("./Temp/Export.log", "w")

    if logfile then
        logfile:write("Log: Start", "\n")
        logfile:flush()
    end        	
	
    -- 2) Setup udp sockets to talk to touchpal
    package.path  = package.path..";.\\LuaSocket\\?.lua"
    package.cpath = package.cpath..";.\\LuaSocket\\?.dll"
   
    socket = require("socket")
    
    c = socket.udp()
    c:setsockname(host, port)
    c:settimeout(.01) -- set the timeout for reading the socket 
end


function LuaExportBeforeNextFrame()
	ProcessInput()  
end

function LuaExportAfterNextFrame()	
	ProcessOutput()
end

function LuaExportStop()
-- Works once just after mission stop.
    
	logfile:write("Log: End", "\n")
    logfile:flush()    
    logfile:close()

    c:close()
end

function LuaExportActivityNextEvent(t)
	local tNext = t
	
	return tNext
end