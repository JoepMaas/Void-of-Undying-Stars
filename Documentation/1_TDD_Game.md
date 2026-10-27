# Technical Design Document (TDD)
## Functionalities

* Player movement
* The player can move left to right, with the A & D keys, With the joystick on a controller.
* Unity Input System (Both Input System) will be used. 
* A PlayerMovement component will be attached to the player. 
* Movement input will be read as a Vector2 and raw to make it instant also be called horizontal. 
* Horizontal movement will be applied using a Rigidbody2D and LinearVelocity. 
* Physics-based movement using FixedUpdate().
* Clamp the fallspeed so i doesn't keep accelerating.
*
* Jumping
* The player can jump with the space key, or the assigned jump key on the controller.
* I will check if the player is grounded with raycast2D
* I will make a jumpPower int so that the code is more readable.
* I will make a layermask to check if the player is touching the wall and not something else.
* If the jump button is down it applies the jumpPower to the linearvelocityY.
* If the jump button is released it will apply gravity down. 
* The player gets a jumppower that reaches a maxheight so he cant jump forever.
* I will make a hasDoubleJump bool that gets set to true when the player solves a certain puzzle.
* If the player is in the air and has the hasDoubleJump bool set to true then i will apply the jump again.
* 
* Dashing
* The player can dash left to right with the shift key, or the assigned dash key on the controller.
* I will use the unity particle system to make the dash feel fast and smooth i also will boost the x as velocity of the rigidbody.
* I will make a dashPower int that is very high.
* I will apply to linearVelocity.x the horizontal * dashPower so the dash gets multiplied by -1 or 1, so that it goes the right direction
* I use the linearVelocity to make sure you dont go through walls. 
* 
* Wall Slide And Wall Jump
* The player can slide down the wall and jump of the wall.
* I will detect if there is a wall and see what direction its facing, based on that it jumps to the different direction.
* Wall detection using raycasts on both sides of the player. 
* Wall direction determined by which raycast hits. 
* Reduced gravity while sliding. 
* Layermask to make sure it ground so you cant wall jump on a enemy.
* Wall jump force applied in opposite direction of the wall normal. 
* Wall sliding limits downward velocity.
*
* Main Menu
* There should be a main menu, where you can press play, options, exit.
* I will make a new scene called:"MainMenu". I will make a menuHandler script to handle the button presses. In the script there will be
* multiple functions: "StartGame", "OpenOptions", "Exit". I will use the scenemanagement function from unity to tp the user to the main game.
* Also a ui that gets setActive(true). Exit will just quit the application. In the option menu you can change the sound, resolution, vsync.
* I would change the sound using the sound source and a slider, The resolution with a dropdown, and vsync with a button.
*
* Focus Bar
* The focus bar should fill when killing enemies.
* You can use the focus bar to use abilities.
* I will add this to the playerHandler, so when you kill a enemy it will access a function called AddFocus(int Amount).
* And it has a max Focus.
* 
* Health bar
* There should be a health bar.
* In the playerhandeler i will make the hp bar with a max and the hp,
* then make a function called takedamage(int damage) and i just takes the damage - hp bar.
* I will make the ui the image of the player head. 
* When you get hit the player head goes grey, until every single one is grey.
* You can heal by using the focus bar.
* 
* Npc Talking
* The player can interact with npc to talk and gain experiences of different areas.
* I will make a trigger hitbox and when the player gets close it 
* displays a ui above the npc where it says: "speak" or "Listen"
* When the player accepts the movement will get locked, by making a bool locked.
* I will add to every movement script, if !locked then.... So when he isnt locked than he can do whatever.
* 
* Attack Direction
* The player can attack up,down,left,right. Based on the direction he's facing.
* i will check the direction there facing by using horizontal and vertical value.
* I will store the value in a string, so it called: Left, Right, Up, Down, resting.
* Hitboxes spawned in attack direction. 
* Animation-driven attacks.
* 
* Abilities
* The player should have a abilities where he does a mega slash. or Multiples slashes, that he can learn from npc.
* I will check the input with the unity input system, and there will be particles firing of.
* I also will make a database to check if they have the ability unlocked.
* I will spawn a hitbox the size of the attack, check if their is a enemy in, and deal damage.
*
* Puzzles
* There should be multiples puzzles that the player can solve to unlock different abilities.
* I will just pre draw a lot of combination that i want in it that i can then make in multiple rooms.
* So there will be mulitple bools to check if the player has done every objective in the room.
* I Will make a trigger object on handles and other puzzle mechanics, then check if the player hit e.
* And set the bool to true.
* 
* Resting Place
* Resting places, Where you regain hp and set a spawnpoint.
* See if the player is touching the resting place hitbox, and then regain the health and set the spawnpoint
* I will use the function AddHealth(int amount), and will set the health to maxHealth.
* 
* Gravity Switches
* Gravity switches on the different biomes.
* In the movementHandeler script, i will change the gravity based on the biome.
* in the player movement i /2 the y value when he is falling. So i will make it less or more based on the biome.
* I will make a float list of gravity values
* So i can easily access them to change on the biome.
* 
* AI Enemies
* There should be a fighting AI, For example enemies, bosses.
* I will make a list of attacks the enemy can do based on the distance of the player.
* The ai will check how far the player is and based on that it will do a random attack,
* The enemy gets a big trigger hitbox, so when the player touches the trigger, then the enemy follows the player.
* I will determine the distance between the player and enemy, by storing the x and y value of the player, 
* and checking if the enemy x and y, are close enough for a certain attack.
* also will always be facing the player.
* 
* Enemies drop money
* Enemies should drop money, where you can buy stuff from.
* When the enemy dies it should instantiate the money i made a prefab off, 
* The money will randomize on the amount so i will make a max int and a min int for the values,
* then i will randomize a number between those values.
* The money will get a bigger trigger hitbox, when the PLAYER touches the trigger,
* it will add the money using addMoney(Int Amount) in the playerHandler, and update the ui.
* when trying to buy something it should check the money the player has.
* 
* Sound Based on biome
* There should be sound for multiple biomes.
* Using the unity sound system for the sounds and will make different scenes where the player hears different sounds.
* I will check the biome the player is in via the playerHandler where their is a int biome.
* I can then check which value int biome has, and then change the music based on the biome
* 
* Sound Effects
* There should be simple sound effects for hitting, dashing, walking, talking, etc...
* Using the unity sound system and storing all the sounds in a folder so i can easily access it in a script
* In the playermovement and handler will i change the sound effect based on the action. 
* On every function i will apply a sound effect.
* 