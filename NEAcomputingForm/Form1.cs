using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using Newtonsoft.Json;
namespace NEAcomputingForm
{

    public partial class Form1 : Form
    {
        //defining the initial of all the variables accessible anywhere in form 1

        //all of the "global" variables using my own classes
        MenuNavigation CurrentMenu = new MenuNavigation("Loading", "0");//creates the most basic menu to let the player know the game is loading and also sets the menu that can be accessed from anywhere in form 1
        List<MenuNavigation> menuList = new List<MenuNavigation>();// a list of menus that can be accessed from anywhere in form 1
        Squad team = new Squad("My Team");//same as above but with a squad (default name is My Team
        SecretBase Secretbase = new SecretBase();//same as above but with a secretbase
        WeaponShop weaponshop = new WeaponShop();//...weaponshop
        List<Levelset> levels = new List<Levelset>();//a list to contain all the levels ( can be accessed from anywhere in form 1)
        Level currentlevel;
        Specialist currentSpecialist;

        //all of the "global" varibles using standard or semistandard classes
        DatabaseConnector databaseConnector = new DatabaseConnector();// a database connector (it does what it says on the tin {it allows connection to a database})
        NumpadButtons form2;
        DebugMenu debug;
        CombatMenu combatTurn;
        int selectedLevelset = 999;
        int selectedLevel = 999;
        string gamestate = "Loading";// a string that can be accessed anywhere in form 1
        Weapon selectedWeapon = new Weapon("SelectedWeaponPlaceHolder", "0", "0", 0, 0, 0, 0);
        int selectedSlot = 999;
        string selectedType = "";

        //all of the boollean values that make combat work
        bool inCombat = false;
        bool playerTurn = false;
        bool enemyTurn = false;
        bool startingCombat = false;
        bool optionsOnDisplay = false;
        bool playerTurnNext = false;
        public string debugStringAccess = "";


        //dunno
        Dictionary<int, (string, int)> tierBuffs = new Dictionary<int, (string, int)>();




        //clocks timers and ticks
        private void Runtime2_Tick(object sender, EventArgs e)
        {
            labelTime.Text = Convert.ToString(Convert.ToInt16(labelTime.Text) + 1);//increments the time by 1 every second
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
                if ((!foundItem) && temp.checkIfOwned()) { Secretbase.addWeapon(temp, tierBuffs); }

            }

            if (inCombat) { CombatTick(); }

        }
        //see also CombatTick() under combat logic under combat subroutines

        //stuff that doesnt fit into other categories
        private void DisplayCurrentMenu() //this displays the info for the current menu 
        {
            txtOut.Text = "";
            Output(CurrentMenu.GetMenuName());
            for (int i = 0; i < 10; i++)
            {
                string temp = CurrentMenu.GetMenuNumberWhichOptionLeadsTo()[i];
                try
                {
                    if (temp == "0")
                    {
                        Output(i.ToString() + " leads nowhere ");
                    }
                    else
                    {
                        Output(i.ToString() + " " + menuList[Convert.ToInt16(temp)].GetMenuName());
                    }
                }
                catch (Exception ex)
                {
                    if (temp.Contains("Levelset"))
                    {
                        string temp3 = temp.Substring(8);
                        if (Convert.ToInt16(temp3) > levels.Count)
                        {
                            Output(i.ToString() + " " + temp + " does not currently exist.");
                        }
                        else
                        {
                            Levelset temp2 = levels[Convert.ToInt16(temp3) - 1];
                            Output(i.ToString() + " " + temp + " Name: " + temp2.getName() + " Description: " + temp2.getDescription() + " Diffictulty: " + temp2.getDiff());
                        }

                    }
                    else { Output(i.ToString() + " " + temp); }
                }
            }
        }

        private void Output(string output) // a custom text based output that I will use instead of the console
        {
            txtOut.Text += "\n" + output;//adds the text that is input to this function to a new line on txtOut
        }
        public void Form2Access(int buttonNumber) //allows form2 to call the functions inside form1 without making all of them public
        {
            txtCurrentInput.Text = buttonNumber.ToString();
            if (!inCombat)
            {
                if (CurrentMenu.GetMenuNumberWhichOptionLeadsTo()[buttonNumber] == "0")
                {
                    Output("Menu remain same due to input of: " + buttonNumber + " which is option " + CurrentMenu.GetMenuNumberWhichOptionLeadsTo()[buttonNumber]);
                }
                else
                {

                    string[] nextMenuNumbersTemp = CurrentMenu.GetMenuNumberWhichOptionLeadsTo();
                    try
                    {
                        int i = Convert.ToInt32(nextMenuNumbersTemp[buttonNumber]);
                        LoadNextMenu(i.ToString());
                    }
                    catch
                    {
                        handleThings(nextMenuNumbersTemp[buttonNumber]);
                    }


                }
            }
            else
            {
                playerTurnInput(buttonNumber);
            }
        }
        public Form1()
        {
            InitializeComponent();
        }


        //Load subroutines
        private void Form1_Load(object sender, EventArgs e) //this is called when form1 initially loads in
        {
            menuList.Add(CurrentMenu);
            debug = new DebugMenu();
            form2 = new NumpadButtons();
            form2.Show();//Makes the numpad visible
            LoadThingsIn();
            CurrentMenu.SetMenuList(menuList);

        }
        private void LoadThingsIn() //calls on all of the different load subroutines to ensure everything is loaded in 
        {
            tierBuffs.Add(0, ("Bad", 0));
            tierBuffs.Add(1, ("Poor", 0));
            tierBuffs.Add(2, ("Basic", 0));
            tierBuffs.Add(3, ("Standard", 0));
            tierBuffs.Add(4, ("Augmented", 0));
            tierBuffs.Add(5, ("Militarised", 0));
            tierBuffs.Add(6, ("Unimpressionable", 0));
            
            LoadTutorial();//loads in the tutorial
            LoadWeapons();//loads in all the weapons
            LoadMenus("Menus.txt");//loads in all the menus from specified file
            loadLevels(1);//loads in the levels in the level set specified
            loadLevelsets();//loads in the name difficulty etc for all of the level sets
            team.AddToSquad(new Specialist("Debug man"));
            if (currentSpecialist == null) { currentSpecialist = team.GetSquad().First();  }
            try
            {
                CurrentMenu = menuList[1];//Sets the current menu to the main menu now that loading is done
            }
            catch (Exception ex) { Output(ex.ToString() + " the first menu is not loading in please fix"); }
            Output("To begin press 0, you will be put into the base. I would suggest you equip some weapons in the specialist menu");

        }
        private void LoadWeapons() //loads in all the weapons 
        {
            try // file handling is present therefore it is a good idea to use a try catch
            {
                //loads in the players starting weapons
                Weapon knife = new Weapon("Knife", "Sharp", "None", 10, 0, 1, 0);
                knife.setOwned();
                Secretbase.addWeapon(knife,tierBuffs);
                Weapon Club = new Weapon("Club", "Blunt", "None", 20, 0, 2, 0);
                Club.setOwned();
                Secretbase.addWeapon(Club, tierBuffs);


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
                        //Output(line);
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
        private void loadLevelsets()
        {
            DataSet database = new DataSet();

            string SQL = "SELECT LevelSets.*"
                + "FROM LevelSets;";
            //defines the SQL statement that gets the levelset data from the table


            database = loadDataSet(SQL);//retrieves the information needed from the table using the input SQL statement
            for (int i = 0; i < database.Tables[0].Rows.Count; i++)
            {
                levels[i].setTheName(database.Tables[0].Rows[i].Field<string>(1));
                levels[i].setDifficulty(database.Tables[0].Rows[i].Field<string>(3));
                levels[i].setDesc(database.Tables[0].Rows[i].Field<string>(2));
            }
        }
        private void loadLevels(int levelSetNum) //loads the enemies from the database into the levels in the relevant levelset 
        {
            try
            {
                DataSet database = new DataSet();

                string SQL = "SELECT Enemies.*, Levels.LevelSetNum "
                             + "FROM LevelsAndSetLink, LevelSets INNER JOIN(Levels INNER JOIN Enemies ON Levels.LevelNum = Enemies.LevelNum) ON LevelSets.LevelSetNum = Levels.LevelSetNum "
                             + "WHERE(((Levels.LevelSetNum) = " + levelSetNum + "));";//defines the SQL statement that gets the enemy data from the table

                //SQL = "SELECT Enemies.*, Levels.LevelSetNum\r\nFROM LevelsAndSetLink, LevelSets INNER JOIN (Levels INNER JOIN Enemies ON Levels.LevelNum = Enemies.LevelNum) ON LevelSets.LevelSetNum = Levels.LevelSetNum\r\nWHERE (((Levels.LevelSetNum)=1));";
                database = loadDataSet(SQL);//retrieves the information needed from the table using the input SQL statement


                Level[] templv = new Level[5];
                for (int i = 0; i < 5; i++)
                {
                    templv[i] = new Level(i + 1, levelSetNum + 1);//adds all of the levels to templv
                    templv[i].levelID = Convert.ToChar(i + 65) + "" + Convert.ToChar(levelSetNum + 65);
                }

                foreach (DataRow row in database.Tables[0].Rows)//iterates through all of the rows in the database (I only call upon one table)
                {

                    //(DEBUG) THIS IS NOT RUNNING, THIS NEEDS TO BE FIXED BEFORE I CAN TEST THE COMBAT SYSTEM *****************

                    int enemyID = row.Field<int>(0);//these lines put all of the info into temporary variables
                    string enemyName = row.Field<string>(1);
                    string enemyWeapon = row.Field<string>(2);
                    int enemyDodgeChance = row.Field<int>(3);
                    int enemyAim = row.Field<int>(4);
                    int enemyLevel = row.Field<int>(5);
                    Enemy tempE = new Enemy(enemyName, enemyWeapon, enemyDodgeChance, enemyAim, weaponshop);//This bit of code loads all of the information provided by the data base into an enemy
                    templv[enemyLevel - 1].AddEnemy(tempE);//The enemy is then added into the level which is currently stored in templv. The -1 is to convert the index which starts at one which the database uses into ther index 0 starting point arrays use
                }
                /*
                //(DEBUG) due to the above bit not running this allows me to debug the combat system until I get it working
                int enemyID = 0;
                string enemyName = "Test4";
                string enemyWeapon = "Handgun";
                int enemyDodgeChance = 0;
                int enemyAim = 0;
                int enemyLevel = 1;
                Enemy tempE = new Enemy(enemyName, enemyWeapon, enemyDodgeChance, enemyAim, weaponshop);
                templv[0].AddEnemy(tempE);
                //(END DEBUG)
                */
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
        private void LoadNextMenu(string nextMenuNumber) //loads in the next menu 
        {
            Output("Going to: " + nextMenuNumber);
            CurrentMenu = CurrentMenu.GoToNextMenu(nextMenuNumber, CurrentMenu);
            Output("Current Menu name: " + CurrentMenu.GetMenuName());
            Output("Current Menu number: " + CurrentMenu.GetMenuNumber());
            DisplayCurrentMenu();
        }


        //Handle save/load system
        private SaveLoad createSaveLoadFile()
        {
            SaveLoad saveLoad = new SaveLoad();

            saveLoad.saveName = team.getName();

            List<string> unlockedWeapons = new List<string>();
            foreach (Weapon weapon in Secretbase.GetWeapons())
            {
                unlockedWeapons.Add(weapon.getName());
            }
            saveLoad.unlockedWeapons = unlockedWeapons;

            List<string> unlockedLevels = new List<string>();
            foreach (Levelset levelset in levels)
            {
                foreach (Level level in levelset.GetLevels())
                {
                    if (level.unlocked)
                    {
                        unlockedLevels.Add(level.levelID);
                    }
                }
            }
            saveLoad.unlockedLevels = unlockedLevels;

            List<string> specialistNames = new List<string>();
            List<int> strengths = new List<int>();
            List<int> perceptions = new List<int>();
            List<int> Endurances = new List<int>();
            List<int> Intelligences = new List<int>();
            List<int> Agilities = new List<int>();
            List<int> Lucks = new List<int>();

            foreach (Specialist specialist in team.GetSquad())
            {
                specialistNames.Add(specialist.getName());
                strengths.Add(specialist.GetStrength());
                perceptions.Add(specialist.GetPerception());
                Endurances.Add(specialist.GetEndurance());
                Intelligences.Add(specialist.GetIntelligence());
                Agilities.Add(specialist.GetAgility());
                Lucks.Add(specialist.GetLuck());
            }
            saveLoad.specialistNames = specialistNames;
            saveLoad.specialistStrengths = strengths;
            saveLoad.specialistPerceptions = perceptions;
            saveLoad.specialistEndurances = Endurances;
            saveLoad.specialistIntelligences = Intelligences;
            saveLoad.specialistAgilities = Agilities;
            saveLoad.specialistLucks = Lucks;
            saveLoad.trainingTokens = Secretbase.getTrainingTokens();
            return saveLoad;
        } //loads all of the data to be saved into the saveload class
        private void SaveProgress(SaveLoad temp)
        {
            string filename = temp.saveName + ".json";
            if (!File.Exists(@"SaveFolders\" + filename))
            {
                var tempfile = File.Create(@"SavesFolder\" + filename);
                tempfile.Close();
                Output("File created");
            }
            Output("Saving file");
            string file = JsonConvert.SerializeObject(temp);

            using (StreamWriter Fred = new StreamWriter(@"SavesFolder\" + filename))
            {
                Fred.Write(file);

            }
            Output(file);

        } //uses the class to save the data into a json file
        private void LoadProgress(string documentName) //when it is done it should load the progress from a json file
        {
            try
            {
                string file = File.ReadAllText(@"SavesFolder\" + documentName + ".json");
                SaveLoad temp = new SaveLoad();
                temp = JsonConvert.DeserializeObject<SaveLoad>(file);
                int i = 0;
                team = new Squad(temp.saveName);
                foreach (string name in temp.specialistNames)
                {
                    Specialist specialist = new Specialist(name);
                    specialist.SetStrength(temp.specialistStrengths[i]);
                    specialist.SetPerception(temp.specialistPerceptions[i]);
                    specialist.SetEndurance(temp.specialistEndurances[i]);
                    specialist.SetIntelligence(temp.specialistIntelligences[i]);
                    specialist.SetAgility(temp.specialistAgilities[i]);
                    specialist.SetLuck(temp.specialistLucks[i]);
                    i++;
                    team.AddToSquad(specialist);
                }
                foreach (Levelset levelset in levels)
                {
                    foreach (Level level in levelset.GetLevels())
                    {
                        level.unlocked = false;
                        if (temp.unlockedLevels.Contains(level.levelID))
                        {
                            level.unlocked = true;
                        }
                    }
                }
                foreach (Weapon weapon in weaponshop.getShopInventory())
                {
                    if (temp.unlockedWeapons.Contains(weapon.getName()))
                    {
                        weapon.setOwned();
                    }
                    else { weapon.setUnowned(); }
                }
                Secretbase.addTrainingTokens(temp.trainingTokens - Secretbase.getTrainingTokens());
                currentSpecialist = team.GetSquad().First();
            }
            catch (Exception ex) { Output("Unable to load progress:" + ex.Message); }
        }


        //menus and their handling
        private void handleThings(string input)
        {
            switch (input)
            {
                case "Previous Specialist":
                    specialistMenuHandle(0); break;
                case "Next Specialist":
                    specialistMenuHandle(1); break;
                case "Train":
                    specialistMenuHandle(2); break;
                case "Strength":
                    specialistMenuHandle(3); break;
                case "Perception":
                    specialistMenuHandle(4); break;
                case "Endurance":
                    specialistMenuHandle(5); break;
                case "Intelligence":
                    specialistMenuHandle(6); break;
                case "Agility":
                    specialistMenuHandle(7); break;
                case "Luck":
                    specialistMenuHandle(8); break;
                case "Cancel":
                    CurrentMenu = CurrentMenu.GoToNextMenu("1", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Levelset1":
                    selectedLevelset = 1;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Levelset2":
                    selectedLevelset = 2;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Levelset3":
                    selectedLevelset = 3;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Levelset4":
                    selectedLevelset = 4;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Levelset5":
                    selectedLevelset = 5;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Levelset6":
                    selectedLevelset = 6;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Levelset7":
                    selectedLevelset = 7;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Levelset8":
                    selectedLevelset = 8;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Levelset9":
                    selectedLevelset = 9;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Level1":
                    selectedLevel = 1;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Level2":
                    selectedLevel = 2;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Level3":
                    selectedLevel = 3;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Level4":
                    selectedLevel = 4;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Level5":
                    selectedLevel = 5;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "LevelSelect":
                    levelSelect(); break;
                case "Deselect Level":
                    selectedLevel = 999;
                    selectedLevelset = 999;
                    CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Enter Combat":
                    startCombat();
                    break;
                case "Save":
                    SaveProgress(createSaveLoadFile());
                    break;
                case "Load":
                    LoadProgress(team.getName()); ;
                    break;
                case "Sharp":
                    handleWeaponChanges("Sharp");
                    break;
                case "Blunt":
                    handleWeaponChanges("Blunt");
                    break;
                case "Ballistic":
                    handleWeaponChanges("Ballistic");
                    break;
                case "Explosive":
                    
                    handleWeaponChanges("Explosive");
                    break;
                case "Melee":
                    CurrentMenu = CurrentMenu.GoToNextMenu("10", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Ranged":
                    CurrentMenu = CurrentMenu.GoToNextMenu("11", CurrentMenu);
                    DisplayCurrentMenu();
                    break;
                case "Weapon1":
                    selectWeapon(1);
                    break;
                case "Weapon2":
                    selectWeapon(2);
                    break;
                case "Weapon3":
                    selectWeapon(3);
                    break;
                case "Weapon4":
                    selectWeapon(4);
                    break;
                case "Weapon5":
                    selectWeapon(5);
                    break;
                case "Weapon6":
                    selectWeapon(6);
                    break;
                case "Weapon7":
                    selectWeapon(7);
                    break;
                case "Weapon8":
                    selectWeapon(8);
                    break;
                case "Weapon9":
                    selectWeapon(9);
                    break;
                case "Slot1":
                    selectSlot(1);
                    break;
                case "Slot2":
                    selectSlot(2);
                    break;
                case "Slot3":
                    selectSlot(3);
                    break;
                case "Slot4":
                    selectSlot(4);
                    break;
                case "Slot5":
                    selectSlot(5);
                    break;
                case "BuySharp":
                    outputWeaponListForShop("Sharp");
                    break;
                case "BuyBlunt":
                    outputWeaponListForShop("Blunt");
                    break;
                case "BuyExplosive":
                    outputWeaponListForShop("Explosive");
                    break;
                case "BuyBallistic":
                    outputWeaponListForShop("Ballistic");
                    break;
                case "Buy Weapon 1":
                    selectWeaponBuy(1);
                    break;
                case "Buy Weapon 2":
                    selectWeaponBuy(2);
                    break;
                case "Buy Weapon 3":
                    selectWeaponBuy(3);
                    break;
                case "Buy Weapon 4":
                    selectWeaponBuy(4);
                    break;
                case "Buy Weapon 5":
                    selectWeaponBuy(5);
                    break;
                case "Buy Weapon 6":
                    selectWeaponBuy(6);
                    break;
                case "Buy Weapon 7":
                    selectWeaponBuy(7);
                    break;
                case "Buy Weapon 8":
                    selectWeaponBuy(8);
                    break;
                case "Buy Weapon 9":
                    selectWeaponBuy(9);
                    break;
            } //due to the large number of cases that this is handling I have decided to use a switch case instead of a if else if else if etc which I usually prefer
        } //handles all of the non integer inputs from menus
        private void buyWeapon(Weapon purchasedweapon)
        {
            for (int i=0;i<Secretbase.GetWeapons().Count-1;i++) 
            {
                if (Secretbase.GetWeapons()[i].getName().Equals(purchasedweapon.getName()) && Secretbase.GetWeapons()[i].checkIfOwned())
                {
                    Output("The tier of the weapon has increased");
                } else if (Secretbase.GetWeapons()[i].getName().Equals(purchasedweapon.getName())) 
                {
                    Output("The weapon has been purchased");
                }
            }
            Secretbase.addWeapon(purchasedweapon, tierBuffs);
            CurrentMenu = CurrentMenu.GoToNextMenu("2", CurrentMenu);
            DisplayCurrentMenu();
        }

        private void outputWeaponListForShop(string weaponType) 
        {
            selectedType = weaponType;
            
            CurrentMenu = CurrentMenu.GoToNextMenu("15", CurrentMenu);
            DisplayCurrentMenu();
            outputWeaponNameForShop();
        }
        public void outputWeaponNameForShop() 
        {

            int count = 1;
            foreach (Weapon weapon in weaponshop.getShopInventory()) 
            {
                if (weapon.getDamageType1().Equals(selectedType))
                {
                    Output("weapon" + count.ToString() + ": " + weapon.getName());
                    count++;
                }
            }
        
        }
        private void selectWeaponBuy(int slotNumber) 
        {
            int count = 1;
            List<Weapon> weapons = new List<Weapon>();
            foreach (Weapon weapon in weaponshop.getShopInventory())
            {
                if (weapon.getDamageType1().Equals(selectedType))
                {
                    weapons.Add(weapon);
                }
                
            }
            if (slotNumber <= weapons.Count)
            {
                buyWeapon(weapons[slotNumber - 1]);
            }
            else 
            { 
                Output("That choice does not have a weapon please try again"); 
                CurrentMenu = CurrentMenu.GoToNextMenu("2", CurrentMenu);
                DisplayCurrentMenu();
            }


        }
        private void selectWeapon(int num) 
        { 
            List<Weapon> weaponsInCategory = new List<Weapon>();
            foreach (Weapon weapon in Secretbase.GetWeapons()) 
            {
                if (weapon.getDamageType1().Equals(selectedType)) 
                {
                    weaponsInCategory.Add(weapon);
                }
            
            }
            if (num > weaponsInCategory.Count)
            { }
            else 
            { 
              selectedWeapon = weaponsInCategory[num-1];
            }
            
            CurrentMenu = CurrentMenu.GoToNextMenu("13", CurrentMenu);
            DisplayCurrentMenu();
            
            

        }

        private void selectSlot(int num) 
        {
                 selectedSlot = num;
                 Output("Putting the weapon in the slot");
                 putWeaponInSlot();
        }

        private void putWeaponInSlot() 
        {
            if (selectedWeapon.getName() != "SelectedWeaponPlaceHolder")
            {
                currentSpecialist.SetWeapon(selectedSlot, selectedWeapon);
                Output($"{selectedWeapon.getName()} has been put in slot {selectedSlot.ToString()} of {currentSpecialist.getName()}");
            }
            else { Output("There was no weapon selected, please try again"); }
        }
        private void handleWeaponChanges(string weaponType)
        {
            selectedType = weaponType;
            List<Weapon> weaponsOfCorrectType = new List<Weapon>();
            foreach (Weapon weapon in Secretbase.GetWeapons())
            {
                if (weapon.getDamageType1().Equals(weaponType)) { weaponsOfCorrectType.Add(weapon); }
            }
            
            CurrentMenu = CurrentMenu.GoToNextMenu("12", CurrentMenu);
            DisplayCurrentMenu();
            int count = 0;
            foreach (Weapon weapon in weaponsOfCorrectType)
            {
                Output("Weapon " + (count + 1).ToString() + ":" + weapon.getName());
            }
        }
        private void specialistMenuHandle(int input)
        {
            if (currentSpecialist == null) { currentSpecialist = team.GetSquad()[0]; }
            if (input == 0)
            {
                int i = team.GetSquad().IndexOf(currentSpecialist);
                if ((i - 1) < 0) { i = team.GetSquad().Count() - 1; } else { i -= 1; }
                currentSpecialist = team.GetSquad()[i];
            }
            else if (input == 1)
            {
                int i = team.GetSquad().IndexOf(currentSpecialist);
                if ((i + 1) >= team.GetSquad().Count()) { i = 0; } else { i += 1; }
                currentSpecialist = team.GetSquad()[i];
            }
            else if (input == 2)
            {
                Output("You have " + Secretbase.getTrainingTokens() + " training tokens");
                CurrentMenu = CurrentMenu.GoToNextMenu("6", CurrentMenu);
                DisplayCurrentMenu();
            }
            else if (input == 3 && Secretbase.getTrainingTokens() >= 1)
            {
                int i = team.GetSquad().IndexOf(currentSpecialist);
                team.GetSquad()[i].SetStrength(team.GetSquad()[i].GetStrength() + 10);
                Secretbase.removeTrainingToken();
            }
            else if (input == 4 && Secretbase.getTrainingTokens() >= 1)
            {
                int i = team.GetSquad().IndexOf(currentSpecialist);
                team.GetSquad()[i].SetPerception(team.GetSquad()[i].GetPerception() + 10);
                Secretbase.removeTrainingToken();
            }
            else if (input == 5 && Secretbase.getTrainingTokens() >= 1)
            {
                int i = team.GetSquad().IndexOf(currentSpecialist);
                team.GetSquad()[i].SetEndurance(team.GetSquad()[i].GetEndurance() + 10);
                Secretbase.removeTrainingToken();
            }
            else if (input == 6 && Secretbase.getTrainingTokens() >= 1)
            {
                int i = team.GetSquad().IndexOf(currentSpecialist);
                team.GetSquad()[i].SetIntelligence(team.GetSquad()[i].GetIntelligence() + 10);
                Secretbase.removeTrainingToken();
            }
            else if (input == 7 && Secretbase.getTrainingTokens() >= 1)
            {
                int i = team.GetSquad().IndexOf(currentSpecialist);
                team.GetSquad()[i].SetAgility(team.GetSquad()[i].GetAgility() + 10);
                Secretbase.removeTrainingToken();
            }
            else if (input == 8 && Secretbase.getTrainingTokens() >= 1)
            {
                int i = team.GetSquad().IndexOf(currentSpecialist);
                team.GetSquad()[i].SetLuck(team.GetSquad()[i].GetLuck() + 10);
                Secretbase.removeTrainingToken();
            }

        } //handles all of the menu inputs related to the specialists
        private void levelSelect()
        {
            if (selectedLevelset == null || selectedLevelset == 999)
            {
                CurrentMenu = CurrentMenu.GoToNextMenu("7", CurrentMenu);
                DisplayCurrentMenu();
                for (int i = 1; i < 10; i++)
                {
                    try
                    {
                        Output("Level set " + i + " name:" + levels[i].getName() + "Description" + levels[i].getDescription());
                    }
                    catch (Exception e) { }
                }
            }
            else if (selectedLevel == null || selectedLevel == 999)
            {
                CurrentMenu = CurrentMenu.GoToNextMenu("8", CurrentMenu);
                DisplayCurrentMenu();
                for (int i = 1; i < 6; i++)
                {
                    try
                    {
                        Output("Level number:" + i + " Unlocked: " + levels[selectedLevelset - 1].GetLevels()[i].unlocked);
                    }
                    catch (Exception e) { }
                }
            }


        } // performs the logic that allows the player to select which levelset and which level

        //combat subroutines

        //player combat stuff
        private void playerCombatTurn() // the display side of the player turn
        {

            playerTurnNext = false;
            int numOfSpecialists = team.GetSquad().Count;
            List<Specialist> conciousSpecialists = new List<Specialist>();
            for (int i = 0; i < numOfSpecialists; i++)
            {
                if (team.GetSquad()[i].isConcious())
                {
                    conciousSpecialists.Add(team.GetSquad()[i]);
                }
            }

            combatTurn = new CombatMenu("Player Combat Turn", "MEL");


            combatTurn.setCurrentSpecialist(conciousSpecialists[0]);
            combatTurn.createCombatOptions(combatTurn.getCurrentSpecialist());
            combatTurn.setListSpecialists(conciousSpecialists);
            foreach (Enemy enemy in currentlevel.getEnemyList())
            {
                Output("Enemy health:" + enemy.getHealth().ToString());
            }

            Output("0: select next specialist");
            int count = 1;
            foreach (CombatOption option in combatTurn.GetCombatOptions())
            {
                Output(count + ":" + option.getOptionName());
                count++;
            }
            optionsOnDisplay = true;
        }
        private void playerTurnInput(int buttonNumber)//allows the player to choose their input but only on their turn
        {
            if (playerTurn)
            {
                if (buttonNumber == 0)
                {
                    combatTurn.selectNextSpecialist();
                    optionsOnDisplay = false;
                    playerCombatTurn();
                }
                int count = 1;
                foreach (CombatOption option in combatTurn.GetCombatOptions())
                {
                    if (buttonNumber == count)
                    {
                        DoOption(option);
                    }
                    count++;
                }
            }

        }
        private void DoOption(CombatOption option)// here the option the player chose will be performed
        {
            bool optionDone = false;
            if (option.getOptionDamage1() < 0)
            {
                inCombat = false;
                optionDone = true;
                playerTurn = false;
                enemyTurn = false;
                playerTurnNext = false;
                llbCombat.Text = "False";
                Output("Leaving combat");

            }
            for (int i = 0; i < currentlevel.getEnemyList().Count; i++)
            {
                if (!optionDone)
                {
                    if (currentlevel.getEnemyList()[i].getHealth() > 0)
                    {
                        int damage1resist = 1;
                        int damage2resist = 1;
                        if (currentlevel.getEnemyList()[i].CheckResistance(option.getOptionDamageType1())) { damage1resist = 999; }
                        if (currentlevel.getEnemyList()[i].CheckResistance(option.getOptionDamageType2())) { damage2resist = 999; }

                        int damage1 = option.getOptionDamage1() / damage1resist;
                        int damage2 = option.getOptionDamage2() / damage2resist;
                        currentlevel.getEnemyList()[i].setHealth(currentlevel.getEnemyList()[i].getHealth() - damage1 - damage2);

                    }



                }


            }
            playerTurn = false;
            enemyTurn = true;
        }

        private bool checkIfPlayerWin()
        {
            int count = 0;
            foreach (Enemy enemy in currentlevel.getEnemyList())
            {
                if (enemy.getHealth() <= 0) count++;
            }
            if (count == currentlevel.getEnemyList().Count) { if (selectedLevel < 5) { levels[selectedLevelset - 1].GetLevels()[selectedLevel + 1].unlocked = true; } return true; }

            return false;
        } //a function to see if the player defeated all the enemies and also unlock the next level where relevant
        private bool checkIfPlayerLost()
        {
            int count = 0;
            foreach (Specialist specialist in team.GetSquad())
            {
                if (!specialist.isConcious()) { count++; }
            }
            if (count == (team.GetSquad().Count)) { return true; }

            return false;
        }  //a function to see if the player has no specialists left

        //enemy combat stuff
        private void enemyCombatTurn()
        {
            foreach (Enemy enemy in currentlevel.getEnemyList())
            {
                if (enemy.getHealth() > 0)
                {
                    Weapon weapon = enemy.GetWeapon();
                    for (int i = 0; i < team.GetSquad().Count; i++)
                    {
                        if (team.GetSquad()[i].isConcious())
                        {
                            team.GetSquad()[i].Damage((weapon.getType1Damage() + tierBuffs[weapon.getTier()].Item2), weapon.getDamageType1());
                            Output(team.GetSquad()[i].getName() + " is now on " + team.GetSquad()[i].getCurrentHealth());
                            team.GetSquad()[i].Damage((weapon.getType2Damage() + tierBuffs[weapon.getTier()].Item2), weapon.getDamageType2());
                            i += 999;
                        }

                    }

                }

                enemyTurn = false;
                playerTurn = true;
                optionsOnDisplay = false;

            }


        } //performs the combat turn of the enemy

        //combat logic
        private void CombatTick()
        {
            llbCombat.Text = "Yes";
            if (startingCombat)
            {
                currentlevel = levels[0].GetLevels()[0]; //(Debug) loads the player into the test level
                startingCombat = false;
            }

            bool NoTurnOccuring = !(playerTurn || enemyTurn);

            if (checkIfPlayerLost())
            {
                inCombat = false;
                playerTurn = false;
                enemyTurn = false;
                llbCombat.Text = "No";
                Output("Player lost");
                CurrentMenu = CurrentMenu.GoToNextMenu("9", CurrentMenu);
            } // checks to see if the player has lost
            if (checkIfPlayerWin())
            {
                inCombat = false;
                playerTurn = false;
                enemyTurn = false;
                llbCombat.Text = "No";
                Output("Player win");
            } //checks to see if the player has won

            if (playerTurn && !optionsOnDisplay && inCombat)
            {
                Output("Player turn");
                playerCombatTurn();

            } //allows the player to actually input and displays options

            if (optionsOnDisplay && !playerTurn && inCombat)
            {
                Output("Enemy turn");
                enemyCombatTurn();
            }

        }//performs the combat logic every "tick" (1 ms)

        private void startCombat()
        {
            try
            {
                currentlevel = levels[selectedLevelset - 1].GetLevels()[selectedLevel - 1];
                if (currentlevel.unlocked || selectedLevel == 1)
                {
                    currentlevel.unlocked = true;
                    inCombat = true;
                    playerTurnNext = true;
                    playerTurn = true;
                    startingCombat = true;
                }
                else { Output("This level is currently locked"); }
            }
            catch (Exception e)
            {
                Output("Please select a levelset and a level before entering combat.");
                Output("Selected levelset:" + selectedLevelset);
                Output("Selected level:" + selectedLevel);
            }
        }

        //here are all of the debug subroutines
        private void debutton4_Click(object sender, EventArgs e)
        {
            team.GetSquad()[0].Heal(999999999, true);
        }
        public void DebugMenuAccess(int buttonNumber)
        {
            if (buttonNumber == 0) { txtOut.Clear(); }
            if (buttonNumber == 1) { team.GetSquad()[0].EmptyWeaponBag(Secretbase.GetWeapons()[1]); }
            if (buttonNumber == 2)
            {
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
            }
            if (buttonNumber == 3) { form2 = new NumpadButtons(); form2.Show(); }
            if (buttonNumber == 4) { debug = new DebugMenu(); debug.Show(); }
            if (buttonNumber == 5)
            {
                startCombat();
            }
            if (buttonNumber == 6) { try { team.GetSquad()[0].Heal(99999999, true); } catch { } }
            if (buttonNumber == 7) { team.GetSquad()[0].EmptyWeaponBag(Secretbase.GetWeapons()[0]); }
            if (buttonNumber == 8)
            {
                var temp = createSaveLoadFile();
                SaveProgress(temp);
            }
            if (buttonNumber == 9)
            {
                LoadProgress(team.getName());
            }
            if (buttonNumber == 10)
            {
                team.changeName(debugStringAccess);
            }
            if (buttonNumber == 11) { Secretbase.addTrainingTokens(10); }
            if (buttonNumber == 12) { currentlevel.unlocked = true; }
        } //the function that allows the debug menu to do stuff in the main form
        private void btnOpenDebug_Click(object sender, EventArgs e)
        {
            debug.Show();
        } //opens the debug menu
        private void button1_Click(object sender, EventArgs e) // this is a debug button to test the combat system
        {
            startCombat();
        }
        private void buttonClear_Click(object sender, EventArgs e) //clears the output
        {
            txtOut.Clear();
            team.GetSquad()[0].EmptyWeaponBag(Secretbase.GetWeapons()[1]); //sets all of the weapons in the bad to the club
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

        //shop and other logistics
        

        private void llbCombat_Click(object sender, EventArgs e)
        {

        }
    }

    //From here starts the classes which I use in Form1
    public class DatabaseConnector //the class which allows the program to connect to the database, to access the data using SQL statements 
    {
        public static DataSet dataConnect(string dbasename, string SQL)
        {
            try
            {
               
                OleDbConnection conn = connection(dbasename);

                //I have traced the problem with the enemies not being loaded in all the way back to here, the rows of the database are simply just not making it into the program, why: I have no clue *************

                OleDbDataAdapter adapter = new OleDbDataAdapter(SQL, conn);//the line above connects to the database by making a temporary copy of the required data
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);//this puts the data into a dataset
                return dataSet;
            }
            catch
            (Exception e)
            {
                //Console.WriteLine(e.ToString());
                return null;
            }

        }
        public static OleDbConnection connection(string database) // I have traced the error all the way here Problem solved: OLEDB needs 64 bit access not 32 bit access
        {
            string conStr = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" + database + ".accdb";//provides the connection string to allow the program to access the database

            

            OleDbConnection conn = new OleDbConnection(conStr);
            try
            {
                conn.Open();//opens the connection (do not actively debug this line it causes an error)
                return conn;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
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
        public string getName() { return this.name; }
        public void changeName(string newName) { this.name = newName; }
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
        public void addTrainingTokens(int num) { this.trainingTokens += num; }
        public void removeTrainingToken() { this.trainingTokens -= 1; }
        public int getEconomy()
        {
            return (this.economy);
        }
        public List<Weapon> GetWeapons()
        {
            return this.weapons;
        }
        public void addWeapon(Weapon Weapon, Dictionary<int, (string, int)> tierbuffs)//adds the item to the list of weapons
        {
            bool temp100ish = false;
            int count = 0;
            foreach (Weapon weapon in this.weapons) { if (weapon.getName().Equals(Weapon.getName())) { temp100ish = true; } ;count++; }
            if (temp100ish)
            {
                weapons[count].increaseTier(tierbuffs);
            }
            else {
                this.weapons.Add(Weapon);
            }
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
        string tier = "Poor";
        int tierNumber = 0;
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
        public void setUnowned() { this.owned = false; }
        public void increaseTier(Dictionary<int,(string,int)> tierbuffs) 
        { 
            this.tierNumber += 1;
            this.tier = tierbuffs[tierNumber].Item1;
        }
        public int getTier() { return this.tierNumber; }

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
        int strength = 100;
        int perception = 100;
        int endurance = 100;
        //charisma would go here but there is no need for it in this game
        int intelligence = 100;
        int agility = 100;
        int luck = 100;
        int maxHealth = 100;
        int currentHealth = 100;
        int maxstamina = 5;
        int currentStamina = 5;
        Weapon equippedWeapon;
        Weapon[] baggedWeapons = new Weapon[5];//Each specialist can have up to 5 weapons equipped
        List<string> damageResistances = new List<string>();
        bool conscious = true;
        public Specialist(string name)
        {
            this.name = name;
        }
        public string getName()
        {
            return this.name;
        }

        public bool TryCombatOption(CombatOption combatOption)
        {
            this.currentStamina -= combatOption.getStaminaCost();
            if (this.currentStamina <= 0)
            {
                this.currentStamina += combatOption.getStaminaCost();
                return false;
            }
            return true;
        }

        //health stuff

        public void startFight()
        {
            this.conscious = true;
            this.Heal(999999, false);

        }
        public bool isConcious()
        {
            return this.conscious;
        }
        public int getCurrentHealth()
        {
            return this.currentHealth;
        }
        public void Heal(int amountHealed, bool addsShield)
        {
            if (this.currentHealth + amountHealed > this.maxHealth && !addsShield)
            {
                currentHealth = maxHealth;
            }
            else if (this.currentHealth + amountHealed > this.maxHealth && addsShield)
            {
                currentHealth += amountHealed;
            }
            else
            {
                currentHealth += amountHealed;
            }

        }
        public void Damage(int amountDamaged, string damageType)
        {

            foreach (string resist in this.damageResistances)
            {
                if (resist.Equals(damageType)) { amountDamaged = (amountDamaged / 2) - 1; }
            }
            if (amountDamaged < 0) amountDamaged = 0;
            this.currentHealth -= amountDamaged;

            if (this.currentHealth <= 0) { this.conscious = false; }

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
        public void SetWeapon(int index, Weapon weapon) 
        {
            this.baggedWeapons[index-1] = weapon;
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
        List<string> resistances = new List<string>();
        public Enemy(/*number 1*/string name, string EnemyWeapon, int dodgechance, int aim, WeaponShop weaponshop)
        {
            this.aim = aim;
            this.name = name;
            foreach (Weapon w in weaponshop.getShopInventory())
            {
                if (w.getName() == EnemyWeapon) this.EnemyWeapon = w;

            }
            //this.EnemyWeapon = EnemyWeapon;    //For future me to deal with when he has time and willpower (convert string to weapon using lookup table) (make subroutine for it)
            this.dodgeChance = dodgechance;
        }
        public string getName()
        {
            return name;
        }
        public int getDodgeChance() { return this.dodgeChance; }
        public int getHealth() { return this.health; }
        public void setHealth(int newHealth) { this.health = newHealth; }
        public bool CheckResistance(string input)
        {

            if (resistances.Contains(input))
            {
                return true;
            }

            return false;
        }
        public Weapon GetWeapon() { return this.EnemyWeapon; }
    }
    class Level //template for levels 
    {
        public string levelID;
        List<Enemy> enemyList = new List<Enemy>();//each of the levels has a list of enemies that must be defeated
        int levelNum;
        int levelSetNum;
        public bool unlocked=false;
        public Level(int levelNum, int levelSetNum)
        {
            this.levelNum = levelNum;
            this.levelSetNum = levelSetNum;
        }
        public void AddEnemy(Enemy enemy)
        {
            this.enemyList.Add(enemy);
        }
        public List<Enemy> getEnemyList() { return this.enemyList; }
        public int getLevelNum() { return this.levelNum; }
        public int getLevelSetNum() { return this.levelSetNum; }
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
        public string getName() { return this.setName; }
        public string getDescription() { return this.description; }
        public List<Level> GetLevels() { return this.Levels; }
        public string getDiff() { return this.difficulty; }
        public void setTheName(string name) { this.setName = name; }
        public void setDifficulty(string difficulty) { this.difficulty = difficulty; }
        public void setDesc(string desc) { this.description = desc; }
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
        List<CombatOption> options = new List<CombatOption>();//this list will hold the different combat options the player can select from this menu
        public CombatMenu(string MenuName, string MenuNumber) : base(MenuName, MenuNumber) { }
        List<Specialist> specialists = new List<Specialist>();
        Specialist currentSpecialist;

        public void setListSpecialists(List<Specialist> specialists)
        {
            this.specialists = specialists;
        }
        public void setCurrentSpecialist(Specialist specialist)
        {
            this.currentSpecialist = specialist;
        }
        public void selectNextSpecialist()
        {
            int i = specialists.IndexOf(currentSpecialist);
            i++;
            if (i == specialists.Count)
            {
                i -= specialists.Count;
            }
            currentSpecialist = specialists[i];
            createCombatOptions(currentSpecialist);
        }
        public Specialist getCurrentSpecialist()
        {
            return this.currentSpecialist;
        }

        public List<CombatOption> GetCombatOptions()
        {
            return this.options;
        }
        public void createCombatOptions(Specialist person)
        {
            Weapon[] bag = person.GetWeaponBag();
            this.options = new List<CombatOption>();
            for (int i =0;i<bag.Length;i++) 
            { 
                if (bag[i] ==null) { bag[i]= new Weapon("None","None","None",0,0,1,0); }
            }
            
            foreach (Weapon weapon in bag)
            {
                
                string optionName = weapon.getName();
                string DmgType1 = weapon.getDamageType1();
                string DmgType2 = weapon.getDamageType2();
                int Type1Dmg = weapon.getType1Damage();
                int Type2Dmg = weapon.getType2Damage();
                int StamCost = weapon.getActionsConsumed();
                this.options.Add(new CombatOption(optionName, DmgType1, DmgType2, Type1Dmg, Type2Dmg, StamCost));
            }
            this.options.Add(new CombatOption("Run from battle", "Escape", "Escape", -1, -1, 0));

        } //creates most of the combat options from the weapons in the bag and also the escape option

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


    class SaveLoad //acts as the template for the json files in order to save/load progress
    {
        public string saveName;
        public List<string> unlockedWeapons;
        public List<string> specialistNames;
        public List<string> unlockedLevels;
        public List<int> specialistStrengths;
        public List<int> specialistPerceptions;
        public List<int> specialistEndurances;
        public List<int> specialistIntelligences;
        public List<int> specialistAgilities;
        public List<int> specialistLucks;
        public int trainingTokens;
    }


    /* 
     To do list:                                priority (lower higher priority)
    
    Convert Debug buttons into a dev menu                        7
    Fill out files                                               5
    Polish stuff                                                 9
    Enhance Save load system                                     6
    Add comments                                                10
    Allow user to change weapons for specialists                 3
    Improve and enhance weapon system (including above)          1
     */
}