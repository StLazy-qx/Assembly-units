Assembly Units – Isometric 3D Strategy
Description
Developing a 3D game with an isometric camera, allowing players to observe the entire level space. The core mechanics involve automatic resource gathering and base development through unit creation.

Implemented Mechanics:
- Starting Units – the base starts with 3 knights
- Resource Generation – pirate coins spawn randomly on the map
- Resource Scanning – the base automatically searches for available coins
- Unit Deployment – if the base has a free knight and a coin on the map, a unit is sent to collect it
- Resource Transport – the knight picks up the coin and carries it to the base
- Resource Tracking – the base keeps track of collected resources
- Unit Creation – after collecting 3 coins, the base creates a new knight, who behaves like the others

Technical Details:
Uses a spawner system with generics for efficient object management
Automated resource detection and unit behavior
