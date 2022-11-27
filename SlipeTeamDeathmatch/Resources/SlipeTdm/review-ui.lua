local ui = {}
local screenX, screenY = guiGetScreenSize()
local centerX, centerY = screenX * 0.5, screenY * 0.5



function createReviewUi()
	ui.reviewWindow = guiCreateWindow(centerX - 300, centerY - 300, 600, 450, "Slipe Team Deathmatch : Match Review", false)

    ui.winnerLabel = guiCreateLabel(25, 25, 550, 75, "Foo has won!" , false, ui.reviewWindow)
    guiLabelSetHorizontalAlign(ui.winnerLabel, "center")
    guiLabelSetVerticalAlign(ui.winnerLabel, "center")

    ui.playerList = guiCreateGridList(25, 100, 550, 175, false, ui.reviewWindow)
    ui.playerListColumns = {
        name = guiGridListAddColumn(ui.playerList, "Name", 0.45),
        killCount = guiGridListAddColumn(ui.playerList, "# Kills", 0.225),
        deathCount = guiGridListAddColumn(ui.playerList, "# Deaths", 0.225),
    }

    ui.killList = guiCreateGridList(25, 300, 550, 150, false, ui.reviewWindow)
    ui.killListColumns = {
        killer = guiGridListAddColumn(ui.killList, "Killer", 0.45),
        victim = guiGridListAddColumn(ui.killList, "Victim", 0.225),
        weapon = guiGridListAddColumn(ui.killList, "Weapon", 0.225),
    }

    setReviewUiVisible(false)
end

function populateReviewUi()
    local killsPerPlayer = {}
    local deathsPerPlayer = {}
    local winner

    guiGridListClear(ui.killList)
    guiGridListClear(ui.playerList)

    for _, kill in ipairs(match.deaths) do
        local row = guiGridListAddRow(ui.killList)
        if (kill.killer) then
            guiGridListSetItemText(ui.killList, row, ui.killListColumns.killer, getPlayerName(kill.killer), false, false)
            killsPerPlayer[kill.killer] = (killsPerPlayer[kill.killer] or 0) + 1
        end

        guiGridListSetItemText(ui.killList, row, ui.killListColumns.victim, getPlayerName(kill.victim), false, true)
        guiGridListSetItemText(ui.killList, row, ui.killListColumns.weapon, kill.weapon, false, true)

        deathsPerPlayer[kill.victim] = (deathsPerPlayer[kill.victim] or 0) + 1
    end

    for _, player in ipairs(match.players) do
        local row = guiGridListAddRow(ui.playerList)
        guiGridListSetItemText(ui.playerList, row, ui.playerListColumns.name, player.name, false, false)
        guiGridListSetItemText(ui.playerList, row, ui.playerListColumns.killCount, killsPerPlayer[player.element] or 0, false, true)
        guiGridListSetItemText(ui.playerList, row, ui.playerListColumns.deathCount, deathsPerPlayer[player.element] or 0, false, true)

        if (not isPedDead(player.element)) then
            winner = player.team
        end
    end
    
    guiSetText(ui.winnerLabel, tostring(winner) .. " has won!")
end

function setReviewUiVisible(visible)
    guiSetVisible(ui.reviewWindow, visible)
    showCursor(visible)
end