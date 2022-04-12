function init()
	outputChatBox("Hello TDM!")
	createMatchesUi()
	createMatchUi()
	createMapsUi()
end
addEventHandler("onClientResourceStart", resourceRoot, init)