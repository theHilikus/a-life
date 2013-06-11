using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Hyenas {


    /// <summary>
    /// Base class for interactions between objects in the world
    /// </summary>
    public abstract class Interaction {

        private static readonly string ID_PREFIX = "Intera";

        private static int lastIDNumber = -1;

        protected Types type;

        World owner;

        string ID;

        /// <summary>
        /// Turn where the interaction started
        /// </summary>
        protected int startTurn;

        public enum Types {
            EAT = 0,
            FIGHT = 10,
            HUNT = 20,
        }

        public Interaction(Types type, World itsWorld) {
            owner = itsWorld;
            ID = ID_PREFIX + ++lastIDNumber;
            //: base(ID_PREFIX + (++lastIDNumber), itsWorld) {
            
            startTurn = itsWorld.Age;
        }

    }

    /// <summary>
    /// Encapsulates the eating interaction
    /// </summary>
    public class EatInteraction : Interaction {


        /// <summary>
        /// Main actor of the action
        /// </summary>
        public Agent Actor {
            get { return actor; }
            set { actor = value; }
        }
        private Agent actor;


        /// <summary>
        /// Secondary actor involved in the interaction
        /// </summary>
        public Food Meal {
            get { return meal; }
            set { meal = value; }
        }
        private Food meal;

        public EatInteraction(Agent one, Food two, World itsWorld)
            : base(Interaction.Types.EAT, itsWorld) {
            actor = one;
            meal = two;


        }
    }


    public class HuntInteraction : Interaction {
        private Agent actor;

        public Agent Actor {
            get { return actor; }
            set { actor = value; }
        }

        private Food target;

        public Food Target {
            get { return target; }
            set { target = value; }
        }

        
        //internal Point twoDDistance;
        ///// <summary>
        ///// This is not a point, but its represented in two-dimensional system. Distance in X and distance in Y
        ///// </summary>
        //public Point TwoDDistance {
        //    get { return twoDDistance; }
        //    set { twoDDistance = value; }
        //}

        public HuntInteraction(Agent one, Food two, World itsWorld)
            : base(Interaction.Types.HUNT, itsWorld) {
            actor = one;
            target = two;

            //twoDDistance = new Point(actor.Position.X - target.Position.X, actor.Position.Y - target.Position.Y);
        }
    }

    public class FightInteraction : Interaction {
        private Agent agentOne;

        private Agent agentTwo;

        public FightInteraction(Agent one, Agent two, World itsWorld) : base(Types.FIGHT, itsWorld) {
            agentOne = one;
            agentTwo = two;
        
        }
    }
}