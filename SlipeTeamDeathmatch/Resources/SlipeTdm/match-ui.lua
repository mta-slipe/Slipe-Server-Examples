local ui = {}
local screenX, screenY = guiGetScreenSize()
local centerX, centerY = screenX * 0.5, screenY * 0.5

function createMatchUi()
	ui.matchWindow = guiCreateWindow(centerX - 350, centerY - 300, 700, 450, "Slipe Team Deathmatch : Match", false)

    ui.nameLabel = guiCreateLabel(25, 25, 50, 25, "Name: " , false, ui.matchWindow)
    ui.name = guiCreateLabel(75, 25, 100, 25, "" , false, ui.matchWindow)
    ui.hostLabel = guiCreateLabel(25, 50, 50, 25, "Host: " , false, ui.matchWindow)
    ui.host = guiCreateLabel(75, 50, 100, 25, "" , false, ui.matchWindow)
    ui.mapLabel = guiCreateLabel(25, 75, 50, 25, "Map: " , false, ui.matchWindow)
    ui.map = guiCreateLabel(75, 75, 100, 25, "" , false, ui.matchWindow)

    ui.playerListLabel = guiCreateLabel(275, 25, 200, 25, "Players:", false, ui.matchWindow)
    ui.playerList = guiCreateGridList(275, 50, 400, 375, false, ui.matchWindow)
    ui.playerListColumns = {
        name = guiGridListAddColumn(ui.playerList, "Name", 0.5),
        matchCount = guiGridListAddColumn(ui.playerList, "# Matches", 0.15),
        killCount = guiGridListAddColumn(ui.playerList, "# Kills", 0.15),
        deathCount = guiGridListAddColumn(ui.playerList, "# Deaths", 0.15),
    }

    ui.changeMapButton = guiCreateButton(25, 125, 200, 50, "Change Map", false, ui.matchWindow)
    addEventHandler("onClientGUIClick", ui.changeMapButton, handleChangeMapClick, false)
    ui.startButton = guiCreateButton(25, 185, 200, 50, "Start", false, ui.matchWindow)
    addEventHandler("onClientGUIClick", ui.startButton, handleStartClick, false)
    ui.leaveButton = guiCreateButton(25, 375, 200, 50, "Leave", false, ui.matchWindow)
    addEventHandler("onClientGUIClick", ui.leaveButton, handleLeaveClick, false)



    setMatchUiVisible(false)
end

function populateMatchUi()
    guiSetText(ui.name, match.name)
    guiSetText(ui.host, match.host.name)
    guiSetText(ui.map, match.mapName)

    local isHost = localPlayer == match.host.element
    guiSetVisible(ui.changeMapButton, isHost)
    guiSetVisible(ui.startButton, isHost)

    guiGridListClear(ui.playerList)
    for _, player in ipairs(match.players) do
        local row = guiGridListAddRow(ui.playerList)
        guiGridListSetItemText(ui.playerList, row, ui.playerListColumns.name, player.name, false, false)
        guiGridListSetItemText(ui.playerList, row, ui.playerListColumns.matchCount, player.matchCount, false, true)
        guiGridListSetItemText(ui.playerList, row, ui.playerListColumns.killCount, player.killCount, false, true)
        guiGridListSetItemText(ui.playerList, row, ui.playerListColumns.deathCount, player.deathCount, false, true)
    end
end

function handleChangeMapClick()
    requestMaps()
end

function handleStartClick()
    startMatch()
end

function handleLeaveClick()
    leaveMatch()
end

function setMatchUiVisible(visible)
    guiSetVisible(ui.matchWindow, visible)
    showCursor(visible)
end