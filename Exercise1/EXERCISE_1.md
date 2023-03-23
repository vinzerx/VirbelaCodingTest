# Exercise 1 #

In this exercise you'll configure a Unity scene and write scripts to create an interactive experience. As you progress through the steps, feel free to add comments to the code about *why* you choose to do things a certain way. Add comments if you felt like there's a better, but more time intensive way to implement specific functionality. It's OK to be more verbose in your comments than typical, to give us a better idea of your thoughts when writing the code.

## What you need ##

* Unity 2020 (latest, or whatever you have already)
* IDE of your choice
* Git

## Instructions ##

This test is broken into multiple phases. You can implement one phase at a time or all phases at once, whatever you find to be best for you.

### Phase 1 ###

**Project setup**:

 1. Create a new Unity project inside this directory, put "Virbela" and your name in the project name.
 1. Configure the scene:
     1. Add a central object named "Player"
     1. Add 5 objects named "Item", randomly distributed around the central object
 1. Add two C# scripts named "Player" and "Item" to your project
     1. Attach the scripts to the objects in the scene according to their name, Item script goes on Item objects, Player script goes on Player object.
     1. You may use these scripts or ignore them when pursuing the Functional Goals, the choice is yours. You're free to add any additional scripts you require to meet the functional goals.

**Functional Goal 1**:

When the game is running, make the Item closest to Player turn red. One and only one Item is red at a time. Ensure that when Player is moved around in the scene manually (by dragging the object in the scene view), the closest Item is always red.

### Phase 2 ###

**Project modification**:

 1. Add 5 objects randomly distributed around the central object with the name "Bot"
 1. Add a C# script named "Bot" to your project.
 1. Attach the "Bot" script to the 5 new objects.
     1. Again, you may use this script or ignore it when pursing the Functional Goals.

**Functional Goal 2**:

When the game is running, make the Bot closest to the Player turn blue. One and only one object (Item or Bot) has its color changed at a time. Ensure that when Player is moved around in the scene manually (by dragging the object in the scene view), the closest Item is red or the closest Bot is blue.

### Phase 3 ###

**Functional Goal 3**:

Ensure the scripts can handle any number of Items and Bots.

**Functional Goal 4**:

Allow the designer to choose the base color and highlight color for Items/Bots at edit time.

## Questions ##

 1. How can your implementation be optimized? - If the end result doesn't need to be super-accurate all the time, checking if any of the movables (Item, Bot, Player) moved can probably be done at a longer period of time instead of doing it every frame. An event system could also be put in place to detect if a movable moved instead of checking a stored position on the script versus the current position of the movable in the Update loop.

 2. How much time did you spend on your implementation? - The implementation itself took around 5 hours.

 3. What was most challenging for you? - Unit testing in Unity wasn't part of the things we did in previous companies, so I had to learn this one on the fly.

 4. What else would you add to this exercise? - I would like to add a control system in play mode, mostly to make it easier to switch to the event-driven approach to know when to do the closest item/bot checking. To make it more flexible, maybe something like click on any of the movables then use some set controls to move the object so it doesn't have to be the player that could be moved with said play mode controls.

## Extra Notes ##
Controls for other functionalities are listed below:
I - Adds an Item instance
B - Adds a Bot instance
S - Saves to file
L - Loads from file (additional option - Manager instance has a loadFromFileAtStart boolean that can be toggled in the inspector to load an existing file at the start of runtime)

## Optional ##

* Find a way to make use of dependency injection when implementing the functional goals. Feel free to use an existing framework or create your own.
* Add Unit Tests
* Add XML docs
* Optimize finding nearest
* Add new Items/Bots automatically on key press
* Read/write Item/Bot/Player state to a file and restore on launch

## Next Steps ##

* Confirm you've addressed the functional goals
* Answer the questions above by adding them to this file
* Commit and push the entire repository, with your completed project, back into a repository host of your choice (bitbucket, github, gitlab, etc.)
* Share your project URL with your Virbela contact (Recruiter or Hiring Manager)

## If you have questions ##

* Reach out to your Virbela contact (Recruiter or Hiring Manager)
