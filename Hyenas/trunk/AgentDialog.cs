using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Hyenas {
    public partial class AgentDialog : Form {
        public AgentDialog() {
            InitializeComponent();
        }
        
        public AgentDialog(Agent agentClicked, System.Collections.Specialized.ListDictionary population, GUI mainGUI) {
            InitializeComponent();

            this.Owner = mainGUI;

            agentPropertyGrid.SelectedObject = agentClicked;

            this.Text = "Agent Parameters: " + agentClicked.ID;
        }

        private void refreshButton_Click(object sender, EventArgs e) {
            agentPropertyGrid.Refresh();
        }
    }
}