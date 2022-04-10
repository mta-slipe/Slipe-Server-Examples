function init()
	outputChatBox("Hello TDM!")
	createMatchesUi()
    setMatchesUiVisible(true)
end
addEventHandler("onClientResourceStart", resourceRoot, init)