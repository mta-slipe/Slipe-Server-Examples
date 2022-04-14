local ui = {}
local screenX, screenY = guiGetScreenSize()
local centerX, centerY = screenX * 0.5, screenY * 0.5

local function handleSelectClick()
    local row = guiGridListGetSelectedItem(ui.mapList)
    if (row == -1) then
        createErrorUi("You have to select a map.")
        return
    end

    local map = guiGridListGetItemText(ui.mapList, row, ui.mapListColumns.name)
    selectMap(map)

    setMapsUiVisible(false)
    setMatchUiVisible(true)
end

local function handleCancelClick()
    setMapsUiVisible(false)
    setMatchUiVisible(true)
end

function createMapsUi()
	ui.mapsWindow = guiCreateWindow(centerX - 150, centerY - 200, 300, 400, "Slipe Team Deathmatch : Maps", false)
    ui.mapList = guiCreateGridList(25, 25, 250, 300, false, ui.mapsWindow)
    ui.mapListColumns = {
        name = guiGridListAddColumn(ui.mapList, "Name", 0.85),
    }

    ui.cancelButton = guiCreateButton(25, 335, 115, 40, "Cancel", false, ui.mapsWindow)
    addEventHandler("onClientGUIClick", ui.cancelButton, handleCancelClick, false)
    ui.selectButton = guiCreateButton(160, 335, 115, 40, "Select", false, ui.mapsWindow)
    addEventHandler("onClientGUIClick", ui.selectButton, handleSelectClick, false)

    setMapsUiVisible(false)
end

function populateMapList()
    guiGridListClear(ui.mapList)
    for _, map in ipairs(maps) do
        local row = guiGridListAddRow(ui.mapList)
        guiGridListSetItemText(ui.mapList, row, ui.mapListColumns.name, map, false, false)
    end
end

function setMapsUiVisible(visible)
    guiSetVisible(ui.mapsWindow, visible)
    showCursor(visible)
end