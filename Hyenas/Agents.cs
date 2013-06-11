using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace Hyenas {
    public class Agent : WorldObject {

        #region constants

        private const int VISION_AREA = 10;

        private const int HUNGER_DERIV = 2;

        private const int LIFE_EXPECTANCY = 80;

        private const int PRESENCE_RADIUS = 4; //radius in which the agent will notice the presence of another agent

        private const int MAX_SPEED = 11; //max speed any agent can run at

        private const int RUNNING_EFFORT = 2; //extra effort related to running (moving) compared to just existing

        private const int MAX_SIZE = 10; //this-1 = maximum size possible

        private const int MAX_VISION_DISTANCE = 51; //maximum distance the agent can see

        private const int MAX_HUNGER = 100;

        private static readonly string ID_PREFIX = "Agent";

        #endregion


        private static int lastIDNumber = -1;


        //for now they are related to the species and not the individual, it might change in the future
        private static int attackProbability = 20; //probability of attacking

        private static int scareProbability = 50; //probability of trying to scare away
        //The probability of ignoring is the remainder: 100-(attack+scare)

        private static int criticalEnergy = 10;


        public static int LastIDNumber {
            get { return Agent.lastIDNumber; }
        }

        #region Member Attributes

        /// <summary>
        /// Angle it can see in front (1-179)
        /// </summary>
        private int visionAngle;
        [DescriptionAttribute("(1-179) Determines the angle the agent can see")]
        public int VisionAngle {
            get { return visionAngle; }
            set { visionAngle = value; }
        }

        /// <summary>
        /// Side of the triangle it can see in front
        /// </summary>
        private int visionDistance;
        [DescriptionAttribute("Side of the triangle it can see in front")]
        public int VisionDistance {
            get { return visionDistance; }
            set { visionDistance = value; }
        }


        private int energy;
        /// <summary>
        /// Energy it has currently
        /// </summary>
        [DescriptionAttribute("Energy the agent has. If 0 it will die")]
        public int Energy {
            get { return energy; }
            set { energy = value; }
        }

        /// <summary>
        /// Rate of change of the energy (always negative)
        /// </summary>
        private int energyDeriv;
        [DescriptionAttribute("Rate of change of the energy")]
        public int EnergyDeriv {
            get { return energyDeriv; }
            set { energyDeriv = value; }
        }


        private int hunger;
        /// <summary>
        /// Measures hunger (1-100, 100=Full) 
        /// </summary>
        [DescriptionAttribute("Measures hunger (1-100, 100=Full)")]
        public int Hunger {
            get { return hunger; }
            set { hunger = value; }
        }

        /// <summary>
        /// Level at which it starts looking for food
        /// </summary>
        private int criticalHunger;
        [DescriptionAttribute("Level at which it starts looking for food")]
        public int CriticalHunger {
            get { return criticalHunger; }
            set { criticalHunger = value; }
        }

        /// <summary>
        /// Size of the agent
        /// </summary>
        private int size;
        [DescriptionAttribute("Size of the agent (1-10)")]
        public int Size {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// Maximum speed it can run
        /// </summary>
        private int topSpeed;
        [DescriptionAttribute("Maximum speed it can run")]
        public int TopSpeed {
            get { return topSpeed; }
            set { topSpeed = value; }
        }

        /// <summary>
        /// % of topSpeed it will use when hunting
        /// </summary>
        private int speedToHunt;
        [DescriptionAttribute("% of topSpeed it will use when hunting")]
        public int SpeedToHunt {
            get { return speedToHunt; }
            set { speedToHunt = value; }
        }

        /// <summary>
        /// % of topSpeed it will use when scouting
        /// </summary>
        private int speedToScout;
        [DescriptionAttribute("% of topSpeed it will use when scouting")]
        public int SpeedToScout {
            get { return speedToScout; }
            set { speedToScout = value; }
        }

        /// <summary>
        /// % of topSpeed it will use when idling
        /// </summary>
        private int speedToIdle;
        [DescriptionAttribute("% of topSpeed it will use when idling")]
        public int SpeedToIdle {
            get { return speedToIdle; }
            set { speedToIdle = value; }
        }

        /// <summary>
        /// Willingness to get in a fight
        /// </summary>
        private int aggressiveness;
        [DescriptionAttribute("Willingness to get in a fight")]
        public int Aggressiveness {
            get { return aggressiveness; }
            set { aggressiveness = value; }
        }

        private Directions orientation;
        /// <summary>
        /// The direction the agent is facing
        /// </summary>
        [DescriptionAttribute("The direction the agent is facing")]
        public Directions Orientation {
            get { return orientation; }
            set { orientation = value; }
        }

        private States state;
        [DescriptionAttribute("State the agent is in")]
        public States State {
            get { return state; }
            set { state = value; }
        }

        private System.Collections.Generic.Dictionary<Interaction.Types, Interaction> interactionsList;


        private int age;
        /// <summary>
        /// Age of the agent (number of turns it has lived)
        /// </summary>
        [DescriptionAttribute("Age of the agent")]
        public int Age {
            get { return age; }
            set { age = value; }
        }

        private int lifeBonus;
        /// <summary>
        /// Bonus to the life expectancy
        /// </summary>
        [DescriptionAttribute("Bonus to the life expectancy")]
        public int LifeBonus {
            get { return lifeBonus; }
            set { lifeBonus = value; }
        }

        private int turningProbability;
        /// <summary>
        /// Probability of turning vs walking
        /// </summary>
        [DescriptionAttribute("Probability of turning vs walking")]
        public int TurningProbability {
            get { return turningProbability; }
            set { turningProbability = value; }
        }


        /// <summary>
        /// Vision area expressed by a polygon. The first and last points are the same (closed polygon)
        /// </summary>
        public Point[] VisionPoly {
            get {
                Point[] poly = new Point[4];

                poly[0] = this.position;



                //make vision polygon
                //int halfHypo = (int)Math.Round((this.visionDistance / Math.Cos(this.visionAngle * Math.PI / 180.0)) / 2.0);
                int halfHypo = (int) Math.Round(Math.Sqrt(2*Math.Pow(this.visionDistance,2) - 2*Math.Pow(this.visionDistance,2)*Math.Cos(this.visionAngle * Math.PI / 180.0))/2.0);
                int mediatriz = (int)Math.Round(Math.Cos(this.visionAngle / 2.0 * Math.PI / 180.0) * this.visionDistance);


                switch (this.orientation) {
                    case Directions.WEST:
                        poly[1] = new Point(poly[0].X - mediatriz, poly[0].Y - halfHypo);
                        poly[2] = new Point(poly[0].X - mediatriz, poly[0].Y + halfHypo);

                        break;
                    case Directions.SOUTH:
                        poly[1] = new Point(poly[0].X - halfHypo, poly[0].Y + mediatriz);
                        poly[2] = new Point(poly[0].X + halfHypo, poly[0].Y + mediatriz);

                        break;
                    case Directions.EAST:
                        poly[1] = new Point(poly[0].X + mediatriz, poly[0].Y + halfHypo);
                        poly[2] = new Point(poly[0].X + mediatriz, poly[0].Y - halfHypo);

                        break;
                    case Directions.NORTH:
                        poly[1] = new Point(poly[0].X - halfHypo, poly[0].Y - mediatriz);
                        poly[2] = new Point(poly[0].X + halfHypo, poly[0].Y - mediatriz);
                        break;
                }

                poly[3] = poly[0];

                double area = Math.Sqrt(2 * Math.Pow(this.visionDistance, 2) - 2 * Math.Pow(this.visionDistance, 2) * Math.Cos(this.visionAngle * Math.PI / 180.0)) / 2.0 * Math.Cos(this.visionAngle / 2.0 * Math.PI / 180.0) * this.visionDistance;
                return poly;
            }
        }

        /// <summary>
        /// Returns the current speed it runs based on the current state
        /// </summary>
        private int CurrentSpeedPercentage {
            get {
                switch (this.state) {
                    case States.HUNGRY: return speedToScout;

                    case States.HUNTING: return speedToHunt;

                    case States.IDLE: return speedToIdle;

                    case States.SCARED: return 50; //50% of the max speed

                }
                throw new WorldObjectException("Unknown speed. State=" + this.state);
            }
        }
        /// <summary>
        /// The memory of the agent. For now it only remembers 1 previous state
        /// </summary>
        private States previousState;
        [DescriptionAttribute("Probability of turning vs walking"), ReadOnlyAttribute(true)]
        public States PreviousState {
            get { return previousState; }
            set { previousState = value; }
        }

        #endregion

        public enum States {
            IDLE,
            HUNGRY,
            HUNTING,
            SLEEPING,
            EATING,
            FIGHTING,
            SCARED
        }

        /// <summary>
        /// Creates an agent with random parameters
        /// </summary>
        /// <param name="givenID">ID of the agent</param>
        /// <param name="mainWorld">Reference to the world</param>
        public Agent(World mainWorld, Point where)
            : base(ID_PREFIX + (++lastIDNumber), mainWorld, where, "agent.png") {

            this.aggressiveness = World.randomGen.Next(-20, 20);
            this.criticalHunger = World.randomGen.Next(1, 51);
            this.energy = 100;

            this.hunger = 80;
            this.orientation = (Directions)World.randomGen.Next(4);
            this.size = World.randomGen.Next(1, MAX_SIZE);
            this.speedToHunt = World.randomGen.Next(1, 100);
            this.speedToIdle = World.randomGen.Next(1, 100);
            this.speedToScout = World.randomGen.Next(1, 100);

            this.topSpeed = World.randomGen.Next(1, MAX_SPEED);
            //adjust top speed to size
            this.topSpeed -= (int)Math.Round((this.size - 5) / ((MAX_SIZE - 2) / 2.0) * 0.20 * MAX_SPEED); //size=5 is no modification; > 5 penalty; < 5 bonus

            this.visionDistance = World.randomGen.Next(1, MAX_VISION_DISTANCE);
            this.lifeBonus = World.randomGen.Next(-10, 11);
            this.turningProbability = World.randomGen.Next(1, 100);

            this.state = States.HUNGRY;

            this.age = 0;

            this.interactionsList = new Dictionary<Interaction.Types, Interaction>();

            //dependent parameters
            this.visionAngle = (int)Math.Round(((MAX_VISION_DISTANCE - (double)this.visionDistance) / MAX_VISION_DISTANCE) * 180.0);
            this.energyDeriv = Math.Max((int)Math.Round(this.size / 2.0), 1);

            owner.addRepresentation(this, where, orientation);

        }

        /// <summary>
        /// Method called to advance one turn in the agent (Do what it needs/wants to do)
        /// </summary>
        public void doStep() {
            //update attributes
            if (this.state != States.EATING) {
                if (this.state == States.SLEEPING) {
                    hunger -= (int)Math.Round(HUNGER_DERIV / 10.0);
                } else {
                    hunger -= HUNGER_DERIV;
                }
            }


            if (this.state != States.SLEEPING) {
                energy -= energyDeriv;
            }

            age++;

            if (hunger <= 0 || energy <= 0 || age >= LIFE_EXPECTANCY + this.lifeBonus) {
                owner.reportEnd(this);
            }

            switch (this.state) {
                case States.EATING:
                    eat();
                    break;
                case States.HUNGRY:
                    scout();
                    break;
                case States.HUNTING:
                    hunt();
                    break;
                case States.IDLE:
                    idle();
                    break;
                case States.SLEEPING:
                    sleep();
                    break;
                case States.FIGHTING:
                    fight();
                    break;
                case States.SCARED:
                    scared();
                    break;
            }


        }

        private void sleep() {
            energy += (MAX_SIZE - this.size);

            if (energy >= 100) {
                this.changeState(States.IDLE);
            }
        }

        /// <summary>
        /// Function called when the agent is scared. It will make it back away in the opposite direction it was
        /// </summary>
        private void scared() {
            walk();
        }

        private void fight() {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Function called when the agent is ok, it has no needs.
        /// </summary>
        private void idle() {
            //decide if scare or attack or do nothing
            int action = World.randomGen.Next(100);
            if (action <= attackProbability / 2 + this.aggressiveness) {
                //attack
                LinkedList<Agent> agentsFound = owner.findAgentsInPoly(this.VisionPoly, this);

                if (agentsFound.Count > 0) {

                    //if more than 1, decide which one to go to
                    int minDistance = 9999;
                    Agent target = null;
                    foreach (Agent other in agentsFound) {
                        if (owner.findDistance(other.Position, this.position) < minDistance) {
                            target = other;
                            minDistance = owner.findDistance(other.Position, this.position);
                        }
                    }

                    if (walkTowardsObject(target.position)) {
                        this.interactionsList.Add(Interaction.Types.FIGHT, new FightInteraction(this, target, owner));
                        target.changeState(States.FIGHTING);
                        this.changeState(States.FIGHTING);
                    }

                } else {
                    //no agent in sight, move
                    int decision = World.randomGen.Next(100);
                    if (decision < turningProbability) {
                        turn();
                    } else {
                        walk();
                    }
                }
            } else if (action <= attackProbability / 2 + this.aggressiveness + scareProbability / 2) {
                //scare 
                LinkedList<Agent> agentsFound = owner.findAgentsInPoly(this.VisionPoly, this);

                if (agentsFound.Count > 0) {

                    //if more than 1, decide which one to scare
                    int minDistance = 9999;
                    Agent target = null;
                    foreach (Agent other in agentsFound) {
                        if (owner.findDistance(other.Position, this.position) < minDistance) {
                            target = other;
                            minDistance = owner.findDistance(other.Position, this.position);
                        }
                    }

                    scare(target);

                } else {
                    //no agent in sight, move
                    int decision = World.randomGen.Next(100);
                    if (decision < turningProbability) {
                        turn();
                    } else {
                        walk();
                    }
                }
            } else {
                //nothing to do, just move
                int decision = World.randomGen.Next(100);
                if (decision < turningProbability) {
                    turn();
                } else {
                    walk();
                }
            }

            if (this.energy <= criticalEnergy) {
                this.changeState(States.SLEEPING);
            } else if (this.hunger <= this.criticalHunger) {
                //hungry, scout for food
                this.changeState(States.HUNGRY);
            }

        }


        /// <summary>
        /// Function called when the agent has food targeted
        /// </summary>
        private void hunt() {
            //hunting; therefore, course to take depends on where target is 
            HuntInteraction hunt = (HuntInteraction)this.interactionsList[Interaction.Types.HUNT];

            if (walkTowardsObject(hunt.Target.Position)) {
                //it got there
                this.interactionsList.Add(Interaction.Types.EAT, new EatInteraction(this, hunt.Target, owner));
                this.changeState(States.EATING);
            }

        }

        /// <summary>
        /// Called when eating
        /// </summary>
        private void eat() {
            if (((EatInteraction)this.interactionsList[Interaction.Types.EAT]).Meal.decrease() == 0) {
                //food is over, remove interaction
                this.interactionsList.Remove(Interaction.Types.EAT);
                this.changeState(States.IDLE);
            }
            hunger++;

            if (this.hunger >= MAX_HUNGER) {
                //full
                this.changeState(States.IDLE);
            }

            //find if there are agents closeby
            foreach (Agent thief in this.owner.findAgentsAround(this, PRESENCE_RADIUS)) {
                int action = World.randomGen.Next(100);
                if (action <= attackProbability + this.aggressiveness) {
                    //attack with no warning
                    this.interactionsList.Add(Interaction.Types.FIGHT, new FightInteraction(this, thief, owner));
                    this.changeState(States.FIGHTING);
                    thief.changeState(States.FIGHTING);
                } else if (action <= attackProbability + this.aggressiveness + scareProbability) {
                    //scare
                    scare(thief);


                } else {
                    //ignore
                }
            }


        }

        /// <summary>
        /// Function called when an agent is trying to scare away another agent
        /// </summary>
        /// <param name="other">Agent who is being scared</param>
        private void scare(Agent other) {
            int bonusProbWinning = (int)Math.Round((this.size - other.size) / 10.0 * 60 + (this.aggressiveness - other.aggressiveness) / 40.0 * 20);
            bonusProbWinning = (int)Math.Round(bonusProbWinning * 0.5); //because its a bonus of 50% max

            int action = World.randomGen.Next(101);

            if (action <= 50 + bonusProbWinning) { //each agent has 50% base probability of winning, modified by its skills
                //current agent wins, the other agent gets scared
                other.changeState(States.SCARED);
                other.orientation = this.orientation; //run away
            } else {
                //the thief won
                this.changeState(States.SCARED);
                this.orientation = other.orientation; //run away
            }
        }

        /// <summary>
        /// Changes the state of the agent and REMOVES the interaction related to the previous state
        /// </summary>
        /// <param name="newState"></param>
        private void changeState(States newState) {
            this.previousState = this.state;
            this.state = newState;

            //remove interactions related to the last state
            switch (newState) {
                case States.EATING: this.interactionsList.Remove(Interaction.Types.HUNT);
                    break;
                case States.SCARED:
                    if (this.interactionsList.ContainsKey(Interaction.Types.EAT)) {
                        //stop eating, got scared
                        this.interactionsList.Remove(Interaction.Types.EAT);
                    }
                    break;
            }

            owner.reportMsg(this.ID + " changed status to " + newState);
        }

        /// <summary>
        /// Called when looking for food
        /// </summary>
        private void scout() {
            LinkedList<Food> foodFound = owner.findFoodInPoly(this.VisionPoly);
            if (foodFound.Count > 0) {

                //if more than 1, decide which one to go to
                int minDistance = 9999;
                Food target = null;
                foreach (Food piece in foodFound) {
                    if (owner.findDistance(piece.Position, this.position) < minDistance) {
                        target = piece;
                        minDistance = owner.findDistance(piece.Position, this.position);
                    }
                }

                this.interactionsList.Add(Interaction.Types.HUNT, new HuntInteraction(this, target, owner)); //add interaction with closest
                this.changeState(States.HUNTING);
                owner.reportMsg(ID + " found food: " + target.ID);
            } else {
                //no food in sight, move
                int decision = World.randomGen.Next(100);
                if (decision < turningProbability) {
                    turn();
                } else {
                    walk();
                }
            }
        }




        /// <summary>
        /// Function to make the agent move forward
        /// </summary>
        private void walk() {
            if (this.position.X <= 0 || this.position.Y <= 0 || this.position.X >= owner.Width || this.position.Y >= owner.Height) {
                //border of the world, turn 
                turn();
                //walk();
            } else {
                switch (this.orientation) {
                    case Directions.NORTH: this.position.Y = (int)Math.Round(Math.Max(0, this.Position.Y - this.topSpeed * CurrentSpeedPercentage / 100.0));
                        break;
                    case Directions.EAST: this.position.X = (int)Math.Round(Math.Min(owner.Width, this.Position.X + this.topSpeed * CurrentSpeedPercentage / 100.0));
                        break;
                    case Directions.SOUTH: this.position.Y = (int)Math.Round(Math.Min(owner.Height, this.Position.Y + this.topSpeed * CurrentSpeedPercentage / 100.0));
                        break;
                    case Directions.WEST: this.position.X = (int)Math.Round(Math.Max(0, this.Position.X - this.topSpeed * CurrentSpeedPercentage / 100.0));
                        break;
                }
            }
            //decrease energy according to speed
            energy -= (int)Math.Round(RUNNING_EFFORT * energyDeriv * CurrentSpeedPercentage / 100.0);




        }

        /// <summary>
        /// Function to make the object move towards a specified position
        /// </summary>
        /// <param name="target">Point where it wants to get</param>
        /// <returns>True if the agent got to the desired position; false otherwise</returns>
        private bool walkTowardsObject(Point target) {

            int stepSize; //start with the 

            int xOffset = this.position.X - target.X;
            int yOffset = this.position.Y - target.Y;
            owner.reportMsg("Walking distance: " + xOffset + ", " + yOffset);
            if (Math.Abs(xOffset) > Math.Abs(yOffset)) {
                //move in X
                stepSize = Math.Min((int)Math.Round(CurrentSpeedPercentage / 100.0 * this.topSpeed), Math.Abs(xOffset) - 1); //if close to the target, just move enough
                if (xOffset > 0) { //target is in the left of the agent
                    stepSize *= -1;
                }
                this.position.X += stepSize;
            } else {
                //move in Y
                stepSize = Math.Min((int)Math.Round(CurrentSpeedPercentage / 100.0 * this.topSpeed), Math.Abs(yOffset) - 1); //if close to the target, just move enough
                if (yOffset > 0) { //target is above the agent
                    stepSize *= -1;
                }
                this.position.Y += stepSize;
            }

            //decrease energy according to speed
            energy -= (int)Math.Round(RUNNING_EFFORT * energyDeriv * CurrentSpeedPercentage / 100.0);

            if (Math.Abs(this.position.X - target.X) <= 1 && Math.Abs(this.position.Y - target.Y) <= 1) {
                return true;
            } else { return false; }


        }

        /// <summary>
        /// Function to make the agent turn. The direction is decided randomly with 50-50 chances
        /// </summary>
        private void turn() {
            if (World.randomGen.Next(100) < 50) {
                //turn left
                if (this.orientation == 0) { this.orientation = (Directions)3; } else { this.orientation = this.orientation - 1; }
                owner.refreshOrientation(this, false);
            } else {
                //turn right
                this.orientation = (Directions)((int)(this.orientation + 1) % 4);
                owner.refreshOrientation(this, true);
            }
        }





    }


}
