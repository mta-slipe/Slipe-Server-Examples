maps = {}
matches = {}

function createMatch(name)
	triggerServerEvent("Slipe.TeamDeathMatch.CreateMatch", localPlayer, {
		Name = name
	})
end

function leaveMatch()
	triggerServerEvent("Slipe.TeamDeathMatch.LeaveMatch", localPlayer)
end

function joinMatch(match)
	triggerServerEvent("Slipe.TeamDeathMatch.JoinMatch", localPlayer, {
		match.id
	})
end

function selectMap(map)
	triggerServerEvent("Slipe.TeamDeathMatch.SetMap", localPlayer, {
		Name = map
	})
end

function startMatch()
	triggerServerEvent("Slipe.TeamDeathMatch.StartMatch", localPlayer)
end

function requestMaps()
	triggerServerEvent("Slipe.TeamDeathMatch.RequestMaps", localPlayer)
end

function requestMatches()
	triggerServerEvent("Slipe.TeamDeathMatch.RequestMatches", localPlayer)
end

addEvent("Slipe.TeamDeathMatch.Matches", true)
addEventHandler("Slipe.TeamDeathMatch.Matches", root, function(data)
	matches = data
	populateMatchList()
end)

addEvent("Slipe.TeamDeathMatch.Maps", true)
addEventHandler("Slipe.TeamDeathMatch.Maps", root, function(data)
	maps = data
	populateMapList()
end)

addEvent("Slipe.TeamDeathMatch.Error", true)
addEventHandler("Slipe.TeamDeathMatch.Error", root, function(error)
	createErrorUi(error)
end)

addCommandHandler("crun", function(command, ...)
	outputChatBox("Running code")
	local code = table.concat({ ... }, " ")
	loadstring(code)()
end)
