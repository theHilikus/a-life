using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Timers;
using System.ComponentModel;


namespace Hyenas {
    public class World {
        public static Random randomGen = new Random();

        
        private Dictionary<string, Food> foods = null;

        private System.Collections.Specialized.ListDictionary population = null;

        public System.Collections.Specialized.ListDictionary Population {
            get { return population; }
            
        }

        private Queue<Agent> corps;

        
        private System.Windows.Forms.Timer worldTimer;

        private int age; //number of turns since the beginning

        public int Age {
            get { return age; }
            set { age = value; }
        }

        private bool running = false;

        public bool Running {
            get { return running; }
            
        }

       

        private GUI mainGUI = null;

        internal void addRepresentation(WorldObject obj, Point relativePoint, Directions orientation) {
            mainGUI.addRepresentation(obj.Representation, (double)relativePoint.X / this.width, (double)relativePoint.Y / this.height, orientation);
        }

        internal void addRepresentation(WorldObject obj, Point relativePoint) {
            mainGUI.addRepresentation(obj.Representation, (double)relativePoint.X / this.width, (double)relativePoint.Y / this.height, Directions.NORTH);
        }

        /// <summary>
        /// Buffer to store stuff, used as returning bag in some methods
        /// </summary>
        //private LinkedList<WorldObject> buffer;

        #region world external parameters
        private bool keepFoodConstant = true;

        private int stepsPerSecond = 2;


        private int foodQuantity = 30;

        private int populationSize = 2;

        /// <summary>
        /// X Size of the world
        /// </summary>
        private int width = 100;

        public int Width {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Y Size of the world
        /// </summary>
        private int height = 100;

        public int Height {
            get { return height; }
            set { height = value; }
        }
        #endregion

        /// <summary>
        /// Genesis
        /// </summary>
        public World(GUI theGUI) {
            running = false;

            mainGUI = theGUI;

            //buffer = new LinkedList<WorldObject>();
            

            population = new System.Collections.Specialized.ListDictionary();
            int count = 0;
            while (count < populationSize) {
                Agent newAgent = new Agent(this, new Point(randomGen.Next(width), randomGen.Next(height)));
                population.Add(newAgent.ID, newAgent);
                count++;
            }

            count = 0;
            foods = new Dictionary<string, Food>();
            while (count < foodQuantity) {
                Food newFood = new Food(randomGen.Next(1, 10), new Point(randomGen.Next(width), randomGen.Next(height)), this);
                foods.Add(newFood.ID, newFood);
                count++;
            }

            corps = new Queue<Agent>();

            worldTimer = new System.Windows.Forms.Timer();
            worldTimer.Tick += new EventHandler(OnStep);
            //worldTimer.Elapsed += new ElapsedEventHandler(OnStep);
            worldTimer.Interval = 1000 / stepsPerSecond;

            //start the world
            age = 0;
            //worldTimer.Start();

        }

        void OnStep(object sender, EventArgs eArgs) {
            update();
            
        }

        /// <summary>
        /// Function to update the world
        /// </summary>
        internal void update() {
            this.age++;

            foreach (Agent oneAgent in population.Values) {
                
                oneAgent.doStep();
                mainGUI.refreshPosition(oneAgent.Representation, (double)oneAgent.Position.X / this.width, (double)oneAgent.Position.Y / this.height);
                if (population.Count <= 0) { break; }

            }

            //delete the dead agents
            foreach (Agent deadAgent in corps) {
                population.Remove(deadAgent.ID);
                mainGUI.remove(deadAgent.Representation);
            }
            corps.Clear();
        }

        /// <summary>
        /// Called when an agent dies
        /// </summary>
        /// <param name="deadAgent">Refernce to the dead agent</param>
        internal void reportEnd(Agent deadAgent) {

            mainGUI.report(deadAgent.ID + " died. Hunger: " + deadAgent.Hunger + " Energy: " + deadAgent.Energy + " Age: " + deadAgent.Age);
            corps.Enqueue(deadAgent);
            //population.Remove(deadAgent.ID);
            //mainGUI.remove(deadAgent.Representation);

            if (population.Count <= 0) {
                mainGUI.report("Population extinguished");
                stop();
            }
        }


        internal void reportEnd(Food foodOver) {
            foods.Remove(foodOver.ID);

            if (keepFoodConstant) {
                Food newFood = new Food(randomGen.Next(1, 10), new Point(randomGen.Next(width), randomGen.Next(height)), this);
                foods.Add(newFood.ID, newFood);
            }

        }

        /// <summary>
        /// Function to search for agents in a given circunference
        /// </summary>
        /// <param name="position">Center of the circunference</param>
        /// <param name="radius">Radius of the circunference</param>
        /// <returns>An array with all the agents found in the circunference</returns>
        internal LinkedList<Agent> findAgentsAround(Agent body, int radius) {
            int distance;
            LinkedList<Agent> returnBag = new LinkedList<Agent>();
            foreach (Agent other in population.Values) {
                distance = findDistance(body.Position, other.Position);
                if (distance <= radius) {
                //found one
                    returnBag.AddLast(other);
                }
            }
            returnBag.Remove(body); //exclude itself

            return returnBag;
        }

        /// <summary>
        /// Method to find the distance (Taxicab Norm) in the world of two points
        /// </summary>
        /// <param name="position1">Point 1</param>
        /// <param name="position2">Point 2</param>
        /// <returns></returns>
        internal int findDistance(Point position1, Point position2) {
            return Math.Abs(position1.X - position2.X) + Math.Abs(position1.Y - position2.Y);
        }

        internal LinkedList<WorldObject> findAllObjectsInPoly(Point[] poly) {

            LinkedList<WorldObject> result = new LinkedList<WorldObject>();

            foreach (Food piece in foods.Values) {
                if (isInside(poly, piece.Position)) {
                    result.AddLast(piece);
                }
            }

            foreach (Agent agent in population) {
                if (isInside(poly, agent.Position)) {
                    result.AddLast(agent);
                }
            }


            return result;
        }

        /// <summary>
        /// Finds all the food elements in the given polygon
        /// </summary>
        /// <param name="poly">Polygon to look into</param>
        /// <returns></returns>
        internal LinkedList<Food> findFoodInPoly(Point[] poly) {

            LinkedList<Food> result = new LinkedList<Food>();

            foreach (Food piece in foods.Values) {
                if (isInside(poly, piece.Position)) {
                    result.AddLast(piece);
                }
            }

            return result;
        }

        /// <summary>
        /// Function to check if a point is inside a polygon
        /// </summary>
        /// <param name="polygon">Array of points where the last and the first point must be the same</param>
        /// <param name="point">Point to check</param>
        /// <returns>True if the point is inside the polygon</returns>
        /// <remarks>By Dan Sunday</remarks>
        /// <see cref="http://softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm"/>
        private bool isInside(Point[] polygon, Point point) {
            int cn = 0;    // the crossing number counter
            // loop through all edges of the polygon
            for (int i = 0; i < polygon.Length-1; i++) {    // edge from V[i] to V[i+1]
                if (((polygon[i].Y <= point.Y) && (polygon[i + 1].Y > point.Y))    // an upward crossing
                 || ((polygon[i].Y > point.Y) && (polygon[i + 1].Y <= point.Y))) { // a downward crossing
                    // compute the actual edge-ray intersect x-coordinate
                    float vt = (float)(point.Y - polygon[i].Y) / (polygon[i + 1].Y - polygon[i].Y);
                    if (point.X < polygon[i].X + vt * (polygon[i + 1].X - polygon[i].X)) // P.x < intersect
                        ++cn;   // a valid crossing of y=P.y right of P.x
                }
            }

            if (cn % 2 == 1) { return true; } else { return false; }
        }

        internal LinkedList<Agent> findAgentsInPoly(Point[] poly, Agent body) {
            LinkedList<Agent> result = new LinkedList<Agent>();

            foreach (Agent entity in population.Values) {
                if (isInside(poly, entity.Position)) {
                    result.AddLast(entity);
                }
            }

            result.Remove(body); //exclude itself

            return result;
        }

        /// <summary>
        /// Starts the world
        /// </summary>
        internal void start() {
            worldTimer.Start();
            running = true;
        }

        internal void stop() {
            worldTimer.Stop();
            running = false;
        }

        internal void refreshOrientation(Agent agent, bool right) {
            mainGUI.flipRepresentation(agent.Representation, right);
        }

        internal void reportMsg(string msg) {
            mainGUI.report(msg);
        }

        internal PointF normalizePoint(Point point) {
            return new PointF((float)point.X / this.width, (float)point.Y / this.height);
        }
    }

    /// <summary>
    /// Base class for objects in the world
    /// </summary>
    public abstract class WorldObject {


        protected string _ID;

        /// <summary>
        /// ID in the world
        /// </summary>
        public string ID {
            get { return _ID; }
            set { _ID = value; }
        }

        /// <summary>
        /// Reference to the world that it belongs to
        /// </summary>
        protected World owner;

        protected Point position;
        /// <summary>
        /// Current position of the object
        /// </summary>
        public Point Position {
            get { return position; }
            set { position = value; }
        }

        private System.Windows.Forms.PictureBox representation;

[BrowsableAttribute(false)]
        public System.Windows.Forms.PictureBox Representation {
            get { return representation; }
        }

        public WorldObject(string IDParam, World itsWorld, Point objPosition, string imageURL) {
            _ID = IDParam;
            owner = itsWorld;
            position = objPosition;

            representation = new System.Windows.Forms.PictureBox();
            representation.Name = IDParam;
            
            //representation.Location = objPosition;
            representation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            representation.Size = new Size(15, 15);
            representation.Image = Image.FromFile(imageURL);
            
            representation.Visible = true;
            representation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            representation.Name = IDParam;
            representation.BackColor = Color.FromArgb(0);

            
            
            
        }

        /// <summary>
        /// Constructor for objects with no position
        /// </summary>
        /// <param name="IDParam">ID of the object (string + number)</param>
        /// <param name="itsWorld">Reference to the world</param>
        public WorldObject(string IDParam, World itsWorld) {
            _ID = IDParam;
            owner = itsWorld;
            position = new Point(-1, -1);
        }

    }


    /// <summary>
    /// Directions, turning right when added 1
    /// </summary>
    public enum Directions {
        NORTH=0,
        EAST,
        SOUTH,
        WEST
    }


    /// <summary>
    /// Class to encapsulate food in the world
    /// </summary>
    public class Food : WorldObject {

        private static readonly string ID_PREFIX = "Food";

        private int amount;
        
        private static int lastIDNumber = -1;

        public static int LastIDNumber {
            get { return Food.lastIDNumber; }
        }

        public Food(int amountP, Point positionP, World itsWorld)
            : base(ID_PREFIX + (++lastIDNumber), itsWorld, positionP, "food.png") {
            amount = amountP;
            owner.addRepresentation(this, positionP);
        }

        /// <summary>
        /// Decreases the amount of food in the object
        /// </summary>
        /// <returns>Amount left</returns>
        internal int decrease() {
            amount--;
            if (amount == 0) {
                this.owner.reportEnd(this);
            }
            return amount;
        }
    }
}
