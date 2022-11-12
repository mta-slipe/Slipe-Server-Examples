function init()
	createLoginUi()
	createMatchesUi()
	createMatchUi()
	createMapsUi()
end
addEventHandler("onClientResourceStart", resourceRoot, init)

bindKey("u", "down", "chatbox", "matchsay")