using Robocode;
using Robocode.Util;
using System;

/*
 * References:
 * http://mark.random-article.com/weber/java/robocode/lesson5.html (where I got some ideas for movement strategies)
 * https://www.youtube.com/watch?v=4TUwwjP4KNk&t=14s (where I got ideas for how to function the radar and target enemies)
 */

namespace CAP4053.Student
{  
    enum RobotState
    {
        Roam, //wandering around, has not scanned a robot that is 'in range'
        Flee, //gets too close to a robot, flees for a bit and returns to roam RobotState
        Attack //if a robot is range, it will start circling it while shooting at it with different strengths depending on distance to target
    }

    public class Smathers : TeamRobot
    {
        RobotState state = RobotState.Roam; //the current RobotState, default RobotState is Roam

        Random rand = new Random(); //random number used for changing the heading randomly in Roam RobotState
        int direction = 1; //indicates direction of travel

        public override void Run()
        {
            SetColors(System.Drawing.Color.Yellow, System.Drawing.Color.Yellow, System.Drawing.Color.Yellow); //team color

            SetAhead(2000); //initially drive towards the wall to start random roam RobotState

            do //continually spin radar during roam RobotState (this constantly spins vs. just spinning once as in the OnScannedRobot event)
            {
                if (RadarTurnRemaining == 0.0)
                    SetTurnRadarRightRadians(Double.PositiveInfinity);
                Execute();
            } while (true);
        }

        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {
            Out.WriteLine("Distance to scanned robot: " + evnt.Distance + ", Current state: " + state);

            if (!IsTeammate(evnt.Name)) //only attack and start following a robot if it is an enemy
            { 
                switch (state)
                {
                    case RobotState.Roam when (evnt.Distance < 400): //if a scanned bot is inside "attack" range, switch mode to attack
                        {
                            state = RobotState.Attack;
                            break;
                        }
                    case RobotState.Roam when (evnt.Distance >= 400): //continue in roam state and keep spinning radar
                        {

                            if (RadarTurnRemaining == 0.0)
                                SetTurnRadarRightRadians(Double.PositiveInfinity);
                            break;

                        }
                    case RobotState.Attack when (evnt.Distance >= 400): //if the state is attack but the enemy falls out of range, spin the radar around once and then change back to roam state
                        {
                            if (RadarTurnRemaining == 0.0)
                                SetTurnRadarRightRadians(Double.PositiveInfinity);
                            state = RobotState.Roam;
                            break;
                        }
                    case RobotState.Attack when (evnt.Distance < 400):
                        {


                            if (Time % 20 == 0) //strafing functionality
                            {
                                direction *= -1;
                            }

                            //once in attack mode, make the robot face the enemy at a 90 degree angle to make strafing very easy (less likely to get hit by enemy bullets)
                            SetTurnRight(evnt.Bearing + 90 - (35 * direction));
                            SetAhead(2000 * direction);

                            double enemy = HeadingRadians + evnt.BearingRadians; //direction to the enemy
                            double radar = Utils.NormalRelativeAngle(enemy - RadarHeadingRadians); //direction that the radar needs to be set
                            double gun = Utils.NormalRelativeAngle(enemy - GunHeadingRadians); //direction that the gun needs to be set
                            double scanWidth = Math.Min(Math.Atan(36.0 / evnt.Distance), Rules.RADAR_TURN_RATE_RADIANS);

                            if (radar < 0)
                                radar += (-scanWidth);
                            else
                                radar += scanWidth;

                            //actually turing the gun and radar
                            SetTurnRadarRightRadians(radar);
                            SetTurnGunRightRadians(gun);

                            //firing strategy for bullet strength depending on distance to target
                            if (evnt.Distance > 250)
                                SetFire(1);
                            if (evnt.Distance > 150)
                                SetFire(2);
                            else
                                SetFire(3);

                            break;
                        }
                    case RobotState.Attack when (evnt.Distance < 50):
                        {
                            state = RobotState.Flee;
                            break;
                        }
                }
            }
        }

        public override void OnHitWall(HitWallEvent evnt)
        {
            SetTurnLeft(evnt.Bearing + rand.Next(91));
            direction *= -1;
            SetAhead(2000 * direction);
        }

        public override void OnHitRobot(HitRobotEvent evnt)
        {
            switch (state)
            {
                //in theory, no other states should trigger a HitRobotEvent due to the transitions between states above
                case RobotState.Flee:
                    {
                        Out.WriteLine("Hit robot in flee mode");
                        SetTurnLeft(evnt.Bearing + rand.Next(90,181));
                        SetAhead(2000 * direction);
                        state = RobotState.Attack;
                        break;
                    }
            }
        }
    }
}
