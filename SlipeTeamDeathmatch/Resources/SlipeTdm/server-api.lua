maps = {}
matches = {}
isLoggedIn = false

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
		Id = match.id
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

function login(username, password)
	triggerServerEvent("Slipe.TeamDeathMatch.Login", localPlayer, {
		Username = username,
		Password = password
	})
end

function register(username, password, passwordConfirmation)
	triggerServerEvent("Slipe.TeamDeathMatch.Register", localPlayer, {
		Username = username,
		Password = password,
		PasswordConfirmation = passwordConfirmation
	})
end

function logout()
	triggerServerEvent("Slipe.TeamDeathMatch.Logout", localPlayer)
end

addEvent("Slipe.TeamDeathMatch.Matches", true)
addEventHandler("Slipe.TeamDeathMatch.Matches", root, function(data)
	matches = data
	populateMatchList()
    setMatchUiVisible(false)
    setMatchesUiVisible(true)
end)

addEvent("Slipe.TeamDeathMatch.Match", true)
addEventHandler("Slipe.TeamDeathMatch.Match", root, function(data)
	match = data

	if (match.state == "Lobby") then
		populateMatchUi()
		setMatchesUiVisible(false)
		setMatchUiVisible(true)
	elseif (match.state == "Review") then
		
	end
end)

addEvent("Slipe.TeamDeathMatch.Maps", true)
addEventHandler("Slipe.TeamDeathMatch.Maps", root, function(data)
	maps = data
	populateMapList()
    setMatchUiVisible(false)
    setMapsUiVisible(true)
end)

addEvent("Slipe.TeamDeathMatch.Start", true)
addEventHandler("Slipe.TeamDeathMatch.Start", root, function()
    setMatchesUiVisible(false)
    setMatchUiVisible(false)
    setMapsUiVisible(false)
end)

addEvent("Slipe.TeamDeathMatch.Error", true)
addEventHandler("Slipe.TeamDeathMatch.Error", root, function(error)
	createErrorUi(error)
end)

addEvent("Slipe.TeamDeathMatch.LoggedIn", true)
addEventHandler("Slipe.TeamDeathMatch.LoggedIn", root, function()
    setLoginVisible(false)	
	isLoggedIn = true
	setMatchesUiLoginButtonState()
end)

addEvent("Slipe.TeamDeathMatch.LoggedOut", true)
addEventHandler("Slipe.TeamDeathMatch.LoggedOut", root, function()
	isLoggedIn = false
	setMatchesUiLoginButtonState()
end)

addCommandHandler("crun", function(command, ...)
	outputChatBox("Running code")
	local code = table.concat({ ... }, " ")
	loadstring(code)()
end)
