Harvest Loop - Final Check in

This project is a small-scale farming game that combines resource management, exploration, and light combat. The combat system includes enemy AI, player attacks, health management, and visual damage feedback through popup indicators. The primary focus of the project is not content quantity, but clean system design, readable code, strong documentation, and developer tooling. The game will be built with modular gameplay systems and custom in-game debugging tools to support iteration and testing.

Control:
	- WASD to move
	- Left click as primary action
		- Hoe: till dirt 
		- Pickaxe: break rocks / break planted seed
		- Axe: chop trees / break planted seed
		- Seeds: Can be placed in tilled dirt
	- Right click as the interaction
    		- Crop: harvest crops if it is harvestable
		- Bed: Skip to next day
		- Bin: Try sell the item you are hovering 
	- 'F1'
    		- Toggle Debug Menu (shows tile position, tile type, and its tile object if it have one)

Added Feature:
	- Added Bed, right click on the bed to sleep and go on next day
	- Added where if you skip a day, tilled tile that is not occupied
	- Added Money
	- Added Sell Bin, right click on the bin to sell the item you are hovering
	- Added Shop NPC, right click on the NPC to open the shop menu

Note:
	- For the sell bin and shop NPC, I kind of ran out of time, so I did an easy way out to finish them. Now that I think about it, I should not have done that.
	- Most of my time was spent bug fixing 

Feedback I would like to receive:
	- How does the game feel? As in does it feel like it have a game loop in it. 
	- Is it good or bad for the way I did for how items in hotbar talk to the tiles and tile objects?

Am I on schedule
	- I would say yes. Even though this last check in is not what I was written in my planning, I would say that I did what I could to create feature to make it feel like a game with some game loop.

Credits:
	- Game Assets: https://cupnooble.itch.io/sprout-lands-asset-pack