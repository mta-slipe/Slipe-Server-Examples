local screenX, screenY = guiGetScreenSize()
local centerX, centerY = screenX * 0.5, screenY * 0.5

function createErrorUi(errorMessage)    
	local errorWindow = guiCreateWindow(centerX - 200, centerY - 100, 400, 150, "Slipe Team Deathmatch : Error", false)
    local label = guiCreateLabel(25, 25, 350, 150, errorMessage, false, errorWindow) 
    local okButton = guiCreateButton(325, 95, 150, 40, "Close", false, errorWindow)
    addEventHandler("onClientGUIClick", okButton, function()
        destroyElement(label) 
        destroyElement(okButton) 
        destroyElement(errorWindow) 
    end, false)
end