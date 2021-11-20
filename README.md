# RoboCode Implementation
My implementation of a basic RoboCode bot written in C# utilizing a 3-state finite state machine architecture. 

My robot utilized a simple Finite State Machine AI architecture in which my robot alternated between three unique states depending on a couple different inputs. The three unique states of my robot are Roam, Flee, and Attack.

# States

In the Roam state, my robot will head in the direction that it is spawned at towards a wall
and upon hitting that wall will turn around off that wall at a random angle between 0 and 90
degrees. In others words the movement of my robot when roaming is random and mimics that of
those old DVD screens that bounces around but infamously never touches the corner.

In the Flee state, my robot (if hit by another robot or teammate robot) will turn itself
directly around and move 100 pixels, after that is done it will return itself back to the Attack
state. The reason for it setting the state back to Attack after Fleeing is because if it hits another
robot and moves a maximum of 100 pixels, then after fleeing it will always be in the Attack state
and never in the Roam state.

The last and most complicated state of my robot is the Attack state. The Attack state is
triggered when an enemy bot comes with 400 pixels of the spinning radar. From there the radars
locks on to the bot and follows it around and the movement pattern changes. Instead of moving
randomly, my robot will begin to shimmy towards the enemy in little half crescent shapes
switching direction every certain number of ticks. Essentially my robot strafes towards the
enemy instead of driving directly toward the enemy to avoid getting hit by less bullets. My
inspiration for this strategy came from reading multiple sources online as well as my experience
in multiplayer tactical fps games such as Counter Strike and Valorant. In those games, when
taking a gun fight, it is essential to strafe back and forth while taking shots at the enemy to make
it harder for you opponent to hit you. This same principal is applied to my robot and it works
surprisingly well at not getting hit by enemy bullets. My robots one downfall, that I find, is that 
although it makes the robot harder to hit, it is also harder for my robot to hit its shots against the
enemy due to that movement.

# Summary

All of these aspects provide a robust and mobile robot that can face off against a large
number of bots with pretty good success. Its unpredictable movement and fleeing tactics are
what really let my robot excel in both 1v1 and melee battles.
