local ui = {}
local screenX, screenY = guiGetScreenSize()
local centerX, centerY = screenX * 0.5, screenY * 0.5

local function handleCancelClick()
    setLoginVisible(false)
end

local function handleLoginClick()
    login(guiGetText(ui.loginUsernameInput), guiGetText(ui.loginPasswordInput))
    guiSetText(ui.loginPasswordInput, "")
end

local function handleRegisterClick()
    register(guiGetText(ui.registerUsernameInput), guiGetText(ui.registerPasswordInput), guiGetText(ui.registerPasswordConfirmationInput))
end

function createLoginUi()
    ui.window = guiCreateWindow(centerX - 200, centerY - 150, 300, 350, "Slipe Team Deathmatch : Login", false)
    ui.tabPanel = guiCreateTabPanel(25, 25, 250, 300, false, ui.window)

    ui.loginTab = guiCreateTab("Login", ui.tabPanel)
    ui.loginUsernameLabel = guiCreateLabel(25, 25, 200, 25, "Username:", false, ui.loginTab)
    ui.loginUsernameInput = guiCreateEdit(25, 50, 200, 25, "", false, ui.loginTab)

    ui.loginPasswordLabel = guiCreateLabel(25, 90, 200, 25, "Password:", false, ui.loginTab)
    ui.loginPasswordInput = guiCreateEdit(25, 115, 200, 25, "", false, ui.loginTab)
    guiEditSetMasked(ui.loginPasswordInput, true)

    ui.loginCancelButton = guiCreateButton(25, 225, 90, 25, "Cancel", false, ui.loginTab)
    addEventHandler("onClientGUIClick", ui.loginCancelButton, handleCancelClick, false)
    ui.loginButton = guiCreateButton(135, 225, 90, 25, "Login", false, ui.loginTab)
    addEventHandler("onClientGUIClick", ui.loginButton, handleLoginClick, false)

    ui.registerTab = guiCreateTab("Register", ui.tabPanel)
    ui.registerUsernameLabel = guiCreateLabel(25, 25, 200, 25, "Username:", false, ui.registerTab)
    ui.registerUsernameInput = guiCreateEdit(25, 50, 200, 25, "", false, ui.registerTab)

    ui.registerPasswordLabel = guiCreateLabel(25, 90, 200, 25, "Password:", false, ui.registerTab)
    ui.registerPasswordInput = guiCreateEdit(25, 115, 200, 25, "", false, ui.registerTab)
    guiEditSetMasked(ui.registerPasswordInput, true)

    ui.registerPasswordConfirmationLabel = guiCreateLabel(25, 155, 200, 25, "Confirm password:", false, ui.registerTab)
    ui.registerPasswordConfirmationInput = guiCreateEdit(25, 180, 200, 25, "", false, ui.registerTab)
    guiEditSetMasked(ui.registerPasswordConfirmationInput, true)

    ui.registerCancelButton = guiCreateButton(25, 225, 90, 25, "Cancel", false, ui.registerTab)
    addEventHandler("onClientGUIClick", ui.registerCancelButton, handleCancelClick, false)
    ui.registerButton = guiCreateButton(135, 225, 90, 25, "Register", false, ui.registerTab)
    addEventHandler("onClientGUIClick", ui.registerButton, handleRegisterClick, false)

    setLoginVisible(false)
end

function setLoginVisible(visible)
    guiSetVisible(ui.window, visible)
    guiBringToFront(ui.window)
    
    guiSetText(ui.loginUsernameInput, "")
    guiSetText(ui.loginPasswordInput, "")
    guiSetText(ui.registerUsernameInput, "")
    guiSetText(ui.registerPasswordInput, "")
    guiSetText(ui.registerPasswordConfirmationInput, "")
end