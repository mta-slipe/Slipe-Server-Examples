# Slipe Team Deathmatch
This gamemode is a team deathmatch sample gamemode. A classic gamemode where two (or more) teams fight each other, last team standing wins.

The purpose of this sample gamemode is to show how a relatively simple gamemode can be enhanced with some more advanced features that Slipe Server offers.

These include:
- Custom sync middleware  
  The [`MatchMiddleware`](https://github.com/mta-slipe/Slipe-Server-Examples/blob/main/SlipeTeamDeathmatch/Middleware/MatchMiddleware.cs) class is used to relay (sync) packets only to other players in your match. Since players outside of your match do not need to know of your player's sync data
- Custom element ID assigning  
  Custom element ID assigning allows for multiple elements to share the same ID space, as long as they are sent to different players.  
  For example every single match has its own range of element IDs, which would allow for far more matches to be played simultaneously without running into element ID constraints.
- Lua Controllers
  The [`AuthenticationController`](https://github.com/mta-slipe/Slipe-Server-Examples/blob/main/SlipeTeamDeathmatch/Controllers/AuthenticationController.cs) is one of the classes used to handle lua events triggered from the client.  
  This works in a way similar to how ASP.NET controllers work, and by just defining methods in these controllers event will be handled.
- More to be added

