# Slipe Freeroam
This gamemode is a port of the default freeroam gamemode.  

The purpose of this sample gamemode is to show how a simple gamemode can be implemented in Slipe Server, and allows you to directly compare Slipe Server code to Lua code.  

The Controllers directory houses most of the code responsible for handling Lua events coming from the client.  
The client-sided Lua code is only very slightly modified, and as such we were stuck to using data-structures and event definitions as are used in the original gamemode. This resulted in some additional complexity for handling clothing and upgrades. Note that there are more elegant solutions available if you use Slipe Server.