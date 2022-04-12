local ui = {}
local screenX, screenY = guiGetScreenSize()
local centerX, centerY = screenX * 0.5, screenY * 0.5

function createMatchesUi()
	ui.matchesWindow = guiCreateWindow(centerX - 400, centerY - 300, 800, 600, "Slipe Team Deathmatch : Matches", false)
    ui.matchList = guiCreateGridList(25, 25, 500, 550, false, ui.matchesWindow)
    ui.matchListColumns = {
        name = guiGridListAddColumn(ui.matchList, "Name", 0.25),
        map = guiGridListAddColumn(ui.matchList, "Map", 0.25),
        state = guiGridListAddColumn(ui.matchList, "State", 0.25),
        playerCount = guiGridListAddColumn(ui.matchList, "Players", 0.15),
    }
    
    ui.matchNameLabel = guiCreateLabel(550, 25, 225, 40, "Name: ", false, ui.matchesWindow)
    ui.matchName = guiCreateEdit(550, 45, 225, 40, "", false, ui.matchesWindow)
    ui.createMatchButton = guiCreateButton(550, 95, 225, 40, "Create match", false, ui.matchesWindow)
    addEventHandler("onClientGUIClick", ui.createMatchButton, handleCreateMatchClick, false)
    ui.joinMatchButton = guiCreateButton(550, 175, 225, 40, "Join match", false, ui.matchesWindow)
    addEventHandler("onClientGUIClick", ui.joinMatchButton, handleJoinMatchClick, false)
    ui.refreshButton = guiCreateButton(550, 225, 225, 40, "Refresh matches", false, ui.matchesWindow)
    addEventHandler("onClientGUIClick", ui.refreshButton, handleRefreshClick, false)

    setMatchesUiVisible(false)
end

function populateMatchList()
    guiGridListClear(ui.matchList)
    for _, match in ipairs(matches) do
        local row = guiGridListAddRow(ui.matchList)
        guiGridListSetItemText(ui.matchList, row, ui.matchListColumns.name, match.name, false, false)
        guiGridListSetItemText(ui.matchList, row, ui.matchListColumns.map, match.mapName, false, false)
        guiGridListSetItemText(ui.matchList, row, ui.matchListColumns.state, match.state, false, false)
        guiGridListSetItemText(ui.matchList, row, ui.matchListColumns.playerCount, #match.players, false, true)
        guiGridListSetItemData(ui.matchList, row, ui.matchListColumns.name, match)
    end
end

function handleJoinMatchClick()
    local row = guiGridListGetSelectedItem(ui.matchList)
    if (row == -1) then
        createErrorUi("You have to select a match to join.")
        return
    end

    local match = guiGridListGetItemData(ui.matchList, row, ui.matchListColumns.name)
    joinMatch(match)
end

function handleCreateMatchClick()
    createMatch(guiGetText(ui.matchName))
end

function handleRefreshClick()
    requestMatches()
end

function setMatchesUiVisible(visible)
    guiSetVisible(ui.matchesWindow, visible)
    showCursor(visible)
end