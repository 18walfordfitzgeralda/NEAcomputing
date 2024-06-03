using System;
using System.Data;
using System.Data.OleDb;
using System.Xml;

namespace NEAcomputingForm
{

    public partial class Form1 : Form
    {


        string gamestate = "Loading";// a string that can be accessed anywhere in form 1
        MenuNavigation CurrentMenu = new MenuNavigation("Loading", "0");//creates the most basic menu to let the player know the game is loading and also sets the menu that can be accessed from anywhere in form 1
        List<MenuNavigation> menuList = new List<MenuNavigation>();// a list of menus that can be accessed from anywhere in form 1

        Squad team = new Squad("My Team");//same as above but with a squad (default name is My Team
        SecretBase Secretbase = new SecretBase();//same as above but with a secretbase
        WeaponShop weaponshop = new WeaponShop();//...weaponshop
        List<Levelset> levels = new List<Levelset>();//a list to contain all the levels ( can be accessed from anywhere in form 1)
        DatabaseConnector databaseConnector = new DatabaseConnector();// a database connector (it does what it says on the tin {it allows connection to a database})
        NumpadButtons form2;
        public Form1() 
        {
            InitializeComponent();
        }
        public void Form2Access(int buttonNumber) //allows form2 to call the functions inside form1 without making all of them public
        {
            txtCurrentInput.Text = buttonNumber.ToString();
            if (CurrentMenu.GetMenuNumberWhichOptionLeadsTo()[buttonNumber] == "0")
            {
                Output("Menu remain same due to input of: " + buttonNumber + " which is option " + CurrentMenu.GetMenuNumberWhichOptionLeadsTo()[buttonNumber]);
            }
            else
            {
                Output("Problem? input was" + buttonNumber + " which is option " + CurrentMenu.GetMenuNumberWhichOptionLeadsTo()[buttonNumber]);
                string[] nextMenuNumbersTemp = CurrentMenu.GetMenuNumberWhichOptionLeadsTo();
                LoadNextMenu(nextMenuNumbersTemp[buttonNumber]);

            }
        }
        private void Runtime2_Tick(object sender, EventArgs e)
        {
            labelTime.Text = Convert.ToString(Convert.ToInt16(labelTime.Text) + 1);//increments the time by 1 every second
        }
        private void DisplayCurrentMenu() //this displays the info for the current menu 
        {
            Output(CurrentMenu.GetMenuName());
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    if (CurrentMenu.GetMenuNumberWhichOptionLeadsTo()[i] == "0")
                    {
                        Output(i.ToString() + " leads nowhere ");
                    }
                    else
                    {
                        Output(i.ToString() + " " + Menu.MenuList[Convert.ToInt16(CurrentMenu.GetMenuNumberWhichOptionLeadsTo()[i])].GetMenuName());
                    }
                }
                catch (Exception ex)
                {
                    Output(i.ToString() + " does not lead to another menu, it leads to " + CurrentMenu.GetMenuNumberWhichOptionLeadsTo()[i]);
                }
            }
        }
        private void Runtime_Tick(object sender, EventArgs e) //does various things every second to attempt to prevent certain bugs 
        {

            gamestate = CurrentMenu.GetMenuName();//every second he ensures that the current gamestate is set to the name of the current menu
            labelGamestate.Text = gamestate;//sets the visible label to the gamestate

            foreach (Weapon temp in weaponshop.getShopInventory()) //Checks every weapon for if it is in the base inventory already and if it is not but it is owned it adds it
            {
                bool foundItem = false;
                foreach (Weapon temp2 in Secretbase.GetWeapons())
                {
                    if ((temp.getName().Equals(temp2.getName())))
                    {
                        foundItem = true;
                    }

                }
                if ((!foundItem) && temp.checkIfOwned()) { Secretbase.addWeapon(temp); }


            }
        }
        private void Output(string output) // a custom text based output that I will use instead of the console
        {
            txtOut.Text += "\n" + output;//adds the text that is input to this function to a new line on txtOut
        }
        private void buttonClear_Click(object sender, EventArgs e) //clears the output
        {
            txtOut.Clear();
        }
        private void buttonContinue_Click(object sender, EventArgs e)
        {
            //LoadNextMenu("1");
            Output(CurrentMenu.GetMenuName());
            Output(CurrentMenu.GetMenuNumber().ToString());
            DisplayCurrentMenu();
            Output("ShopInventory:");
            foreach (Weapon weapon in weaponshop.getShopInventory())
            {
                Output("Name:" + weapon.getName() + " Type 1 damage:" + weapon.getType1Damage() + " Type 2 damage:" + weapon.getType2Damage() + " Action cost:" + weapon.getActionsConsumed() + " Cost:" + weapon.getCost() + " IsOwned:" + weapon.checkIfOwned());
            }
            Output("UnlockedWeapons");
            foreach (Weapon weapon in Secretbase.GetWeapons())
            {
                Output("Name:" + weapon.getName() + " Type 1 damage:" + weapon.getType1Damage() + " Type 2 damage:" + weapon.getType2Damage() + " Action cost:" + weapon.getActionsConsumed() + " Cost:" + weapon.getCost() + " IsOwned:" + weapon.checkIfOwned());
            }

        }//among us (this is a temporary debug line

        private void LoadNextMenu(string nextMenuNumber) //loads in the next menu 
        {
            Output("Going to: " + nextMenuNumber);
            CurrentMenu = CurrentMenu.GoToNextMenu(nextMenuNumber, CurrentMenu);
            Output("Current Menu name: " + CurrentMenu.GetMenuName());
            Output("Current Menu number: " + CurrentMenu.GetMenuNumber());
            DisplayCurrentMenu();
        }


        //Here are all of the subroutines that load things in as soon as the code is run
        private void Form1_Load(object sender, EventArgs e) //this is called when form1 initially loads in
        {
            menuList.Add(CurrentMenu);

            form2 = new NumpadButtons();

            form2.Show();//Makes the numpad visible
            Output("Hello");
            LoadThingsIn();
            CurrentMenu.SetMenuList(menuList);

        }
        private void LoadThingsIn() //calls on all of the different load subroutines to ensure everything is loaded in 
        {

            LoadTutorial();//loads in the tutorial
            LoadWeapons();//loads in all the weapons
            LoadMenus("Menus.txt");//loads in all the menus from specified file
            loadLevels(1);//loads in the levels in the level set specified


            CurrentMenu = menuList[1];//Sets the current menu to the main menu now that loading is done
        }
        private void LoadWeapons() //loads in all the weapons 
        {
            try // file handling is present therefore it is a good idea to use a try catch
            {
                //loads in the players starting weapons
                Weapon knife = new Weapon("Knife", "Sharp", "None", 10, 0, 1, 0);
                knife.setOwned();
                Secretbase.addWeapon(knife);
                Weapon Club = new Weapon("Club", "Blunt", "None", 20, 0, 2, 0);
                Club.setOwned();
                Secretbase.addWeapon(Club);


                //loads in the rest of the weapons which will be seen in the shop from the txt file
                Weapon tempweapon;
                string line;
                string[] weaponTemp = new string[6];
                using (StreamReader sr = new StreamReader("Weapons.txt"))
                {
                    line = sr.ReadLine();
                    while (line != null)//this reads the file one line at a time until all of the line have been read, this allows more items to be added onto the end of the file without causing any problems
                    {
                        weaponTemp = line.Split(',');
                        tempweapon = new Weapon(weaponTemp[0], weaponTemp[1], weaponTemp[2], Convert.ToInt16(weaponTemp[3]), Convert.ToInt16(weaponTemp[4]), Convert.ToInt16(weaponTemp[5]), Convert.ToInt16(weaponTemp[6]));
                        weaponshop.addWeapon(tempweapon);
                        line = sr.ReadLine();
                    }

                }
                weaponshop.getShopInventory()[0].setOwned();
            }
            catch (Exception ex) { Output("Unable to load weapons:" + ex.Message); }

        }
        private void LoadMenus(string filename) //loads in all of the menus from the input file 
        {
            try
            {
                string line;
                MenuNavigation tempMenu;
                using (StreamReader sr = new StreamReader(filename))
                {
                    line = sr.ReadLine();
                    while (line != null)// this is almost the same as when the weapons are loaded in but it instead loads in the menus
                    {
                        string[] temp = line.Split(",");
                        string[] optionNumbers = new string[] { temp[3], temp[4], temp[5], temp[6], (temp[7]), (temp[8]), (temp[9]), (temp[10]), temp[11], (temp[12]) };
                        tempMenu = new MenuNavigation(temp[0], temp[1], optionNumbers);
                        menuList.Add(tempMenu);
                        Output(line);
                        line = sr.ReadLine();
                    }

                }
            }
            catch (Exception ex) { Output("Unable to load menus:" + ex.Message); }

        }
        private DataSet loadDataSet(string SQL) //loads the data from the database into the dataset 
        {
            DataSet database = new DataSet();
            try //file handling therefore try catch
            {
                database = DatabaseConnector.dataConnect("Enemies", SQL);// Uses the database connector and the SQL statement to return the table from the database as a dataset
                return database;
            }
            catch (Exception ex)
            {
                Output("Error loading database");
                return null;
            }
        }
        private void loadLevels(int levelSetNum) //loads the enemies from the database into the levels in the relevant levelset 
        {
            try
            {
                DataSet database = new DataSet();

                string SQL = "SELECT Enemies.*, Levels.LevelSetNum "
                             + "FROM LevelsAndSetLink, LevelSets INNER JOIN(Levels INNER JOIN Enemies ON Levels.LevelNum = Enemies.LevelNum) ON LevelSets.LevelSetNum = Levels.LevelSetNum "
                             + "WHERE(((Levels.LevelSetNum) = " + levelSetNum + 1 + "));";//defines the SQL statement that gets the enemy data from the table
                database = loadDataSet(SQL);//retrieves the information needed from the table using the input SQL statement
                Level[] templv = new Level[5];
                for (int i = 0; i < 5; i++)
                {
                    templv[i] = new Level(i + 1, levelSetNum + 1);//adds all of the levels to templv
                }

                foreach (DataRow row in database.Tables[0].Rows)//iterates through all of the rows in the database (I only call upon one table)
                {
                    int enemyID = row.Field<int>(0);//these lines put all of the info into temporary variables
                    string enemyName = row.Field<string>(1);
                    string enemyWeapon = row.Field<string>(2);
                    int enemyDodgeChance = row.Field<int>(3);
                    int enemyAim = row.Field<int>(4);
                    int enemyLevel = row.Field<int>(5);
                    Enemy tempE = new Enemy(enemyName, enemyWeapon, enemyDodgeChance, enemyAim);//This bit of code loads all of the information provided by the data base into an enemy
                    templv[enemyLevel - 1].AddEnemy(tempE);//The enemy is then added into the level which is currently stored in templv. The -1 is to convert the index which starts at one which the database uses into ther index 0 starting point arrays use
                }
                Levelset levelset = new Levelset(levelSetNum + 1);
                for (int i = 0; i < 5; i++)
                {
                    levelset.AddLevel(templv[i]);//loads all of the levels in this level set into a single levelset
                }
                levels.Add(levelset);//Adds the levelset to the list levels so it can be accessed from anywhere on this form
            }
            catch (Exception e)
            {
                Output("There was an error loading the database of enemies: " + e.Message);
            }
        }
        private void LoadTutorial() //loads the tutorial in from the txt document 
        {

            try
            {
                string line;
                
                using (StreamReader sr = new StreamReader("TutorialLines.txt"))
                {
                    line = sr.ReadLine();
                    while (line != null)// this is almost the same as when the weapons are loaded in but it instead loads in the tutorial
                    {
                        Output(line);
                        line = sr.ReadLine();
                    }

                }
            }
            catch (Exception ex) { Output("Unable to load tutorial:" + ex.Message); }





        }

        private void LoadProgress(string documentName) //when it is done it should load the progress from a document
        {
            try
            {
                string line;
                
                using (StreamReader sr = new StreamReader(documentName))
                {
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        //here it will put the data into the correct class


                        line = sr.ReadLine();
                    }

                }
            }
            catch (Exception ex) { Output("Unable to load progress:" + ex.Message); }
        }

    }

    //From here starts the classes which I use in Form1
    public class DatabaseConnector //the class which allows the program to connect to the database, to access the data using SQL statements 
    {
        public static DataSet dataConnect(string dbasename, string SQL)
        {
            try
            {
                // SQL = "SELECT * FROM Enemies;";
                OleDbConnection conn = connection(dbasename);//Matheo says: glue :P
                OleDbDataAdapter adapter = new OleDbDataAdapter(SQL, conn);//the line above connects to the database by making a temporary copy of the required data
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "DATA");//this puts the data into a dataset with the tag DATA
                return dataSet;
            }
            catch
            (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }

        }
        public static OleDbConnection connection(string database)
        {
            string conStr = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" + database + ".accdb";//provides the connection string to allow the program to access the database
            OleDbConnection conn = new OleDbConnection(conStr);
            try
            {
                conn.Open();//opens the connection
                return conn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }

    class Squad //contains all of the specialists as well as other info 
    {
        string name;

        List<Specialist> party = new List<Specialist>();//stores all of the specialists that
        public Squad(string name)
        {
            this.name = name;
        }
        public void AddToSquad(Specialist temp)
        {
            this.party.Add(temp);//adds the specialist to the squad
        }
        public List<Specialist> GetSquad()
        {
            return this.party;//returns the entire squad of specialists
        }

    }
    class SecretBase //the class that stores the base info 
    {
        int level = 0;
        int money = 0;
        int trainingTokens = 0;
        int economy = 0;
        int income = 0;
        int outcome = 0;
        List<Weapon> weapons = new List<Weapon>();//A list of weapons the player has access to/ have bought
        public SecretBase() { }
        public int getLevel()//the getters for the info
        {
            return this.level;
        }
        public int getMoney()
        {
            return this.money;
        }
        public int getTrainingTokens()
        {
            return this.trainingTokens;
        }
        public int getEconomy()
        {
            return (this.economy);
        }
        public List<Weapon> GetWeapons()
        {
            return this.weapons;
        }
        public void addWeapon(Weapon Weapon)//adds the item to the list of weapons
        {
            this.weapons.Add(Weapon);
        }
        public void payment(int cost)//removes money
        {
            this.money -= cost;
        }



    }
    class Weapon //template for a weapon 
    {
        string name = "None";
        string damageType1 = "None";
        string damageType2 = "None";
        int Type1Damage = 0;
        int Type2Damage = 0;
        int actionsConsumed;
        bool owned = false;
        int cost;
        public Weapon(string name, string damageType1, string damageType2, int Type1Damage, int Type2Damage, int actionsConsumed, int cost)
        {
            this.name = name;
            this.damageType1 = damageType1;
            this.damageType2 = damageType2;
            this.Type1Damage = Type1Damage;
            this.Type2Damage = Type2Damage;
            this.actionsConsumed = actionsConsumed;
            this.cost = cost;

        }
        public string getName()
        {
            return this.name;
        }
        public string getDamageType1()
        {
            return this.damageType1;
        }
        public string getDamageType2()
        {
            return this.damageType2;
        }
        public int getType2Damage()
        {
            return this.Type2Damage;
        }
        public int getType1Damage()
        {
            return this.Type1Damage;
        }
        public int getActionsConsumed()
        {
            return (this.actionsConsumed);
        }
        public int getCost() { return this.cost; }
        public bool checkIfOwned()
        {
            return this.owned;
        }
        public void setOwned() { this.owned = true; }


    }
    class WeaponShop // used to make the list of weapons in the shop global without having a list which can easily be messed up by mistake 
    {
        public WeaponShop()
        { }
        List<Weapon> shopWeapons = new List<Weapon>();


        public void addWeapon(Weapon Weapon)
        {
            this.shopWeapons.Add(Weapon);
        }
        public List<Weapon> getShopInventory()
        {
            return this.shopWeapons;
        }

    }
    class Specialist //template for a specialist 
    {
        string name;
        int strength = 0;
        int perception = 0;
        int endurance = 0;
        //charisma would go here but there is no need for it in this game
        int intelligence = 0;
        int agility = 0;
        int luck = 0;
        int maxHealth = 100;
        int currentHealth;
        int stamina;
        Weapon equippedWeapon;
        Weapon[] baggedWeapons = new Weapon[5];//Each specialist can have up to 5 weapons equipped

        public Specialist(string name)
        {
            this.name = name;
        }
        public string getName()
        {
            return this.name;
        }

        //stat control
        public int GetStrength()
        {
            return this.strength;
        }
        public int GetPerception()
        {
            return this.perception;
        }
        public int GetEndurance()
        {
            return this.endurance;
        }

        //no charisma
        public int GetIntelligence()
        {
            return this.intelligence;
        }
        public int GetAgility()
        {
            return this.agility;
        }
        public int GetLuck()
        {
            return this.luck;
        }


        public void SetStrength(int strength)
        {
            this.strength = strength;
        }
        public void SetPerception(int Perception)
        {
            this.perception = Perception;
        }
        public void SetEndurance(int endurance)
        {
            this.endurance = endurance;
        }

        //no need for charisma
        public void SetIntelligence(int intelligence)
        {
            this.intelligence = intelligence;
        }
        public void SetAgility(int Agility)
        {
            this.agility = Agility;
        }
        public void SetLuck(int Luck)
        {
            this.luck = Luck;
        }

        //weapon control
        public Weapon GetWeapon() //returns the equipped weapon when called 
        {
            return this.equippedWeapon;
        }
        public Weapon[] GetWeaponBag() //returns all weapons in the weapon bag when called 
        {
            return this.baggedWeapons;
        }
        public void SetWeapon(int index, int i, SecretBase Secretbase) //sets the weapon in a specific slot to the input weapon 
        {
            this.baggedWeapons[index] = Secretbase.GetWeapons()[i];

        }
        public void EmptyWeaponBag(Weapon weapon) //sets all of the weapons in the weapon bag to the input weapon, intended for use with the "None" weapon
        {
            for (int i = 0; i < this.baggedWeapons.Length; i++)
            {
                baggedWeapons[i] = weapon;

            }
            equippedWeapon = weapon;
        }
    }

    class Enemy //template for the enemies 
    {
        string name;
        Weapon EnemyWeapon;
        int dodgeChance;
        int aim;
        int health = 100;
        public Enemy(/*number 1*/string name, string EnemyWeapon, int dodgechance, int aim)
        {
            this.aim = aim;
            this.name = name;
            //this.EnemyWeapon = EnemyWeapon;    //For future me to deal with when he has time (convert string to weapon using lookup table) (make subroutine for it)
            this.dodgeChance = dodgechance;
        }
    }
    class Level //template for levels 
    {
        List<Enemy> enemyList = new List<Enemy>();//each of the levels has a list of enemies that must be defeated
        int levelNum;
        int levelSetNum;
        public Level(int levelNum, int levelSetNum)
        {
            this.levelNum = levelNum;
            this.levelSetNum = levelSetNum;
        }
        public void AddEnemy(Enemy enemy)
        {
            this.enemyList.Add(enemy);
        }
    }
    class Levelset //a template for a set of levels 
    {
        List<Level> Levels = new List<Level>();//each levelset has multiple levels in it
        string setName;
        string difficulty;
        string description;

        int levelSetNum;
        public Levelset(int levelSetNum)
        {
            this.levelSetNum = levelSetNum;
        }
        public void AddLevel(Level level)
        {
            this.Levels.Add(level);
        }
    }

    class Menu  //The base template for the menus 
    {
        public static List<MenuNavigation> MenuList = new List<MenuNavigation>();
        string MenuName;
        string MenuNumber;
        string[] MenuNumberWhichOptionLeadsTo = new string[10] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" }; //Each menu can lead up to 10 other menus with the option numbered 0-9     
        public Menu(string MenuName, string MenuNumber)
        {
            this.MenuName = MenuName;
            this.MenuNumber = MenuNumber;
        }
        public Menu(string MenuName, string MenuNumber, string[] menuNumberWhichOptionLeadsTo)
        {
            this.MenuNumberWhichOptionLeadsTo = menuNumberWhichOptionLeadsTo;
            this.MenuName = MenuName;
            this.MenuNumber = MenuNumber;
        }
        public void SetMenuList(List<MenuNavigation> temp)
        {
            MenuList = temp;
        }

        public string GetMenuNumber() { return this.MenuNumber; }
        public string GetMenuName() { return this.MenuName; }
        public string[] GetMenuNumberWhichOptionLeadsTo() { return this.MenuNumberWhichOptionLeadsTo; }
    }
    class MenuNavigation : Menu
    {
        public MenuNavigation(string MenuName, string MenuNumber) : base(MenuName, MenuNumber) { }
        public MenuNavigation(string MenuName, string MenuNumber, string[] menuNumberWhichOptionLeadsTo) : base(MenuName, MenuNumber, menuNumberWhichOptionLeadsTo) { }
        public MenuNavigation GoToNextMenu(string nextMenuNumber, MenuNavigation currentMenu)//Returns the next Menu when its number its input otherwise it keeps it on the same menu
        {

            foreach (MenuNavigation menu in MenuList)
            {

                //if (nextMenuNumber == 0) { return currentMenu; }

                if (menu.GetMenuNumber() == nextMenuNumber) { return menu; }

            }
            return currentMenu;
        }
        public MenuNavigation GoToNextMenuFromName(string nextMenuName, MenuNavigation currentMenu)
        {
            foreach (MenuNavigation menu in MenuList)
            {

                if (menu.GetMenuName().Equals(nextMenuName)) { return menu; }

            }
            return currentMenu;
        }
    } //the template for the menus outside of combat which inherits from the menu class
    class CombatMenu : Menu
    {
        //List<CombatOption> options = new List<CombatOptions>();//this list will hold the different combat options the player can select from this menu
        public CombatMenu(string MenuName, string MenuNumber) : base(MenuName, MenuNumber) { }

        Specialist currentSpecialist;

        public void selectNextSpecialist(Squad squad, int specialistPos)
        {
            this.currentSpecialist = squad.GetSquad()[specialistPos];
        }
        public Specialist getCurrentSpecialist() { return this.currentSpecialist; }

        //Here shall be the code that allows the user to make decisions in combat

    } //a template for the menus in combat which inherits from the menu class
    class CombatOption //this will store all of the different combat options which the player can use during this round of combat
    {
        string CombatOptionName;
        string CombatOptionDamageType1;
        string CombatOptionDamageType2;
        int CombatOptionDamage1;
        int CombatOptionDamage2;
        int StaminaCost;
        public CombatOption(string CombatOptionName, string CombatOptionDamageType1, string CombatOptionDamageType2, int CombatOptionDamage1, int CombatOptionDamage2, int staminaCost)
        {
            this.CombatOptionName = CombatOptionName;
            this.CombatOptionDamage1 = CombatOptionDamage1;
            this.CombatOptionDamage2 = CombatOptionDamage2;
            this.CombatOptionDamageType1 = CombatOptionDamageType1;
            this.CombatOptionDamageType2 = CombatOptionDamageType2;
            this.StaminaCost = staminaCost;
        }
        public string getOptionName()
        {
            return this.CombatOptionName;
        } //lots of getters
        public string getOptionDamageType1()
        {
            return this.CombatOptionDamageType1;
        }
        public string getOptionDamageType2()
        {
            return this.CombatOptionDamageType2;
        }
        public int getOptionDamage2()
        {
            return this.CombatOptionDamage2;
        }
        public int getOptionDamage1()
        {
            return this.CombatOptionDamage1;
        }
        public int getStaminaCost()
        {
            return this.StaminaCost;
        }
    }
}