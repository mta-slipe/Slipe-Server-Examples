function init()
	createLoginUi()
	createMatchesUi()
	createMatchUi()
	createMapsUi()
	createReviewUi()
end
addEventHandler("onClientResourceStart", resourceRoot, init)

bindKey("u", "down", "chatbox", "matchsay")