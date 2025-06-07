There were three main tasks in this interview for a considerably short time, so I focused on accomplishing a basic but working and stable version of each of the main systems before adding more features and polish. 

I decided to begin with task 1 because it's the one I'm less used to and I thought it would consume a valuable time. Then I focused on both tasks 2 and 3 because they're related. Finally, I worked on task 4. I chose this order considering how much time each task would consume, so I could start with the more complex and time-consuming ones and let the fast ones to the end.

The gameplay feel demands a few tweaks to work properly, so I focused on making the system fully adjustable via inspector, so I could experiment with the values while developing it. The system controls the player based on where the camera is looking. When you rotate the camera, the character also rotates. It can move forward, backward and laterally.

The interaction system is constantly looking for objects that implement an IInteractable interface. If it finds one, it allows the player to interact. If the player interacts with an item on the ground, the item is added to the inventory.

The inventory system holds X slots and each slot can contain an inventory item. Inventory items can be dragged and dropped. If an item is dropped outside the inventory, it is returned to the world. If it is dropped on an empty slot, it is moved to it. If it is dropped on an occupied slot, the items switch places.

The save system is a binary serialization of a struct that holds all inventory data. The struct is constructed using inventory data when the game is closed and the inventory is reconstructed using the data stored on the struct when the game starts. This is possible because the "Item" is just an identifier that holds reference to it's world (3D) and inventory (2D) versions. The item identifier can be easily retrieved at any time using the item's ID.

I know I'm capable of more than this, but to be honest, the most challenging task was to deliver all of this in 48h. I'm not exactly happy with the final result after making everything that fast and under time pressure, but I'm happy with my performance. I think I managed it well.