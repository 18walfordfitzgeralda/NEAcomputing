using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NEAcomputingForm.Form1;

namespace NEAcomputingForm
{
    public partial class NumpadButtons : Form
    {
        List<Button> buttons = new List<Button>();
        Form1 form1;
        public NumpadButtons()
        {
            InitializeComponent();
        }

        private void NumpadButtons_Load(object sender, EventArgs e)
        {
            form1 = (Form1)Application.OpenForms["Form1"];
            CreateInput();
        }


        private void button1Clicked(object sender,EventArgs e,int buttonNumber) //I started doing this because the iteration didn't work (see comments below) 
        {
            
            form1.Form2Access(buttonNumber);
        }
        private void button2Clicked(object sender, EventArgs e, int buttonNumber) 
        {
            
            form1.Form2Access(buttonNumber);
        }
        private void button3Clicked(object sender, EventArgs e, int buttonNumber) 
        {

            form1.Form2Access(buttonNumber);
        }
        private void button4Clicked(object sender, EventArgs e, int buttonNumber) 
        {

            form1.Form2Access(buttonNumber);
        }
        private void button5Clicked(object sender, EventArgs e, int buttonNumber) 
        {

            form1.Form2Access(buttonNumber);
        }
        private void button6Clicked(object sender, EventArgs e, int buttonNumber) //Sorry 
        {

            form1.Form2Access(buttonNumber);
        } 
        private void button7Clicked(object sender, EventArgs e, int buttonNumber) 
        {

            form1.Form2Access(buttonNumber);
        }
        private void button8Clicked(object sender, EventArgs e, int buttonNumber) 
        {

            form1.Form2Access(buttonNumber);
        }
        private void button9Clicked(object sender, EventArgs e, int buttonNumber) 
        {

            form1.Form2Access(buttonNumber);
        }
        private void button0Clicked(object sender, EventArgs e, int buttonNumber) //the reason for these functions is because events override each other sometimes if the same function is used
        {

            form1.Form2Access(buttonNumber);
        }







        private void CreateInput()//this creates the number pad of buttons 
        {
            Size buttonSize = new Size(50, 50);
            Point buttonPoint = new Point(0, 0);
            int numObuttons = 10;
            
            for (int i = 0; i < numObuttons; i++)
            {

                buttons.Add(new Button());
                buttons[i].Size = buttonSize;
                buttonPoint.X = buttonPoint.X + buttonSize.Width;
                buttons[i].Text = (i + 1).ToString();
                //buttons[i].Tag = i + 1;
                

                if (buttonPoint.X > 150)
                {
                    buttonPoint.X = 50;
                    buttonPoint.Y = buttonPoint.Y + buttonSize.Height;
                }

                
                if (i == 9) 
                { 
                    buttonPoint.X = buttonPoint.X + buttonSize.Width; 
                    buttons[i].Text = "0";
                    //buttonNumber = "10";
                    //buttons[i].Tag = 0;
                }
                
                buttons[i].Location = buttonPoint;
                buttons[i].Visible = true;
                buttons[i].Enabled = true;
                Controls.Add(buttons[i]);
                
                
            }

            buttons[0].Click += (sender, EventArgs) => //this seemed like the easiest way to do this, do to the events overriding each other if they all use the same function
            {
                button1Clicked(sender, EventArgs, 1); 
            };
            buttons[1].Click += (sender, EventArgs) =>
            {
                button2Clicked(sender, EventArgs, 2);
            };
            buttons[2].Click += (sender, EventArgs) =>
            {
                button3Clicked(sender, EventArgs, 3);
            };
            buttons[3].Click += (sender, EventArgs) =>
            {
                button4Clicked(sender, EventArgs, 4);
            };
            buttons[4].Click += (sender, EventArgs) =>
            {
                button5Clicked(sender, EventArgs, 5);
            };
            buttons[5].Click += (sender, EventArgs) =>
            {
                button6Clicked(sender, EventArgs, 6);
            };
            buttons[6].Click += (sender, EventArgs) =>
            {
                button7Clicked(sender, EventArgs, 7);
            };
            buttons[7].Click += (sender, EventArgs) =>
            {
                button8Clicked(sender, EventArgs, 8);
            };
            buttons[8].Click += (sender, EventArgs) =>
            {
                button9Clicked(sender, EventArgs, 9);
            };
            buttons[9].Click += (sender, EventArgs) =>
            {
                button0Clicked(sender, EventArgs, 0);
            };

        }
    }
    
}