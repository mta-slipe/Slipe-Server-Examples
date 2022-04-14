function init()
	outputChatBox("Hello TDM!")
	createLoginUi()
	createMatchesUi()
	createMatchUi()
	createMapsUi()
end
addEventHandler("onClientResourceStart", resourceRoot, init)