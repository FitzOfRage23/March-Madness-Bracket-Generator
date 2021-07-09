using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BracketGenerator
{
    public partial class Form1 : Form
    {
        private Team[][] teamList = new Team[][] { new Team[64], new Team[32], new Team[16], new Team[8], new Team[4], new Team[2], new Team[1] };
        private Random gen;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i;
            String[] teams = System.IO.File.ReadAllLines("teams.txt");
            gen = new Random();
            for(i = 0; i < 32; i++)
            {
                teamList[1][i] = new Team("",-1);
            }
            for(i = 0; i < 16; i++)
            {
                teamList[2][i] = new Team("",-1);
            }
            for(i = 0; i < 8; i++)
            {
                teamList[3][i] = new Team("",-1);
            }
            for(i = 0; i < 4; i++)
            {
                teamList[4][i] = new Team("",-1);
            }
            for(i = 0; i < 2; i++)
            {
                teamList[5][i] = new Team("", -1);
            }
            for (i = 0; i < 1; i++)
            {
                teamList[6][i] = new Team("", -1);
            }
            i = 0;
            foreach(String teamName in teams)
            {
                teamList[0][i] = new Team(teamName, i);
                i++;
            }
            foreach (Control control in this.Controls)
            {
                string name = control.Name;
                int index;
                if (name.Contains("R64"))
                {
                    index = int.Parse(name.Substring(name.Length - 2).Replace("_",""));
                    control.Text = teamList[0][index - 1].getName();
                }
                else if(name.Contains("R32"))
                {
                    index = int.Parse(name.Substring(name.Length - 2).Replace("_",""));
                    populateComboBox((ComboBox)control, 1, index - 1);
                }
                else if (name.Contains("S16"))
                {
                    index = int.Parse(name.Substring(name.Length - 2).Replace("_", ""));
                    populateComboBox((ComboBox)control, 2, index - 1);
                }
                else if (name.Contains("E8"))
                {
                    index = int.Parse(name.Substring(name.Length - 1));
                    populateComboBox((ComboBox)control, 3, index - 1);
                }
                else if (name.Contains("F4"))
                {
                    index = int.Parse(name.Substring(name.Length - 1));
                    populateComboBox((ComboBox)control, 4, index - 1);
                }
                else if (name.Contains("Champ"))
                {
                    populateComboBox((ComboBox)control, 6, 0);
                }
                else if (name.Contains("C"))
                {
                    index = int.Parse(name.Substring(name.Length - 1));
                    populateComboBox((ComboBox)control, 5, index - 1);
                }
            }
        }

        private void populateComboBox(ComboBox box, int round, int index)
        {
            int start = (int)(index * Math.Pow(2, round));
            int end = (int)((index + 1) * Math.Pow(2, round));
            for(int i = start; i < end; i++)
            {
                box.Items.Add(teamList[0][i].getDisplayName());
            }
            
        }

        private void SelectTeam(int round, int index)
        {
            int tempIndex = index;
            for (int i = 0; i < round - 1; i++)
            {
                tempIndex /= 2;
            }
            teamList[round - 1][tempIndex] = teamList[0][index];
            tempIndex /= 2;
            int oldIndex = teamList[round][tempIndex].getIndex();
            teamList[round][tempIndex] = teamList[0][index];
            if (oldIndex != index)
            {
                for (int i = round + 1; i < 7; i++)
                {
                    tempIndex /= 2;
                    if (teamList[i][tempIndex].getIndex() == oldIndex)
                    {
                        teamList[i][tempIndex] = new Team("", -1);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            refresh();
        }

        private void refresh()
        {
            foreach (Control control in this.Controls)
            {
                string name = control.Name;
                int index;
                if (name.Contains("R32"))
                {
                    index = int.Parse(name.Substring(name.Length - 2).Replace("_", ""));
                    control.Text = teamList[1][index - 1].getDisplayName();
                }
                else if (name.Contains("S16"))
                {
                    index = int.Parse(name.Substring(name.Length - 2).Replace("_", ""));
                    control.Text = teamList[2][index - 1].getDisplayName();
                }
                else if (name.Contains("E8"))
                {
                    index = int.Parse(name.Substring(name.Length - 1));
                    control.Text = teamList[3][index - 1].getDisplayName();
                }
                else if (name.Contains("F4"))
                {
                    index = int.Parse(name.Substring(name.Length - 1));
                    control.Text = teamList[4][index - 1].getDisplayName();
                }
                else if (name.Contains("Champ"))
                {
                    control.Text = teamList[6][0].getDisplayName();
                }
                else if (name.Contains("C"))
                {
                    index = int.Parse(name.Substring(name.Length - 1));
                    control.Text = teamList[5][index - 1].getDisplayName();
                }
            }
        }

        private void randomTeam(Team team1, Team team2, int round)
        {
            int index = team1.getIndex();
            for(int i = 0; i < round; i++)
            {
                index /= 2;
            }
            if (teamList[round][index].getName() == "")
            {
                int rand = gen.Next(team1.getChance() + team2.getChance()) + 1;

                if (team1.getChance() < rand)
                    teamList[round][index] = team1;
                else
                    teamList[round][index] = team2;
            }
        }

        private void FillRandom_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < 32; i++)
            {
                randomTeam(teamList[0][i * 2], teamList[0][i * 2 + 1], 1);
            }
            for(int i = 0; i < 16; i++)
            {
                randomTeam(teamList[1][i * 2], teamList[1][i * 2 + 1], 2);
            }
            for(int i = 0; i < 8; i++)
            {
                randomTeam(teamList[2][i * 2], teamList[2][i * 2 + 1], 3);
            }
            for(int i = 0; i < 4; i++)
            {
                randomTeam(teamList[3][i * 2], teamList[3][i * 2 + 1], 4);
            }
            for(int i = 0; i < 2; i++)
            {
                randomTeam(teamList[4][i * 2], teamList[4][i * 2 + 1], 5);
            }
            randomTeam(teamList[5][0], teamList[5][1], 6);
            refresh();
        }

        private void clear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 32; i++)
            {
                teamList[1][i] = new Team("", -1);
            }
            for (int i = 0; i < 16; i++)
            {
                teamList[2][i] = new Team("", -1);
            }
            for (int i = 0; i < 8; i++)
            {
                teamList[3][i] = new Team("", -1);
            }
            for (int i = 0; i < 4; i++)
            {
                teamList[4][i] = new Team("", -1);
            }
            for (int i = 0; i < 2; i++)
            {
                teamList[5][i] = new Team("", -1);
            }
            teamList[6][0] = new Team("", -1);
            refresh();
        }

        private void clearNextRounds(int round, int index)
        {
            //teamList[round][index] = new Team("", -1);
            //for (int i = round + 1; i < 7; i++)
            //{
            //    index /= 2;
            //    teamList[i][index] = new Team("", -1);
            //}
            int oldIndex = teamList[round][index].getIndex();
            teamList[round][index] = new Team("", -1);
            for (int i = round + 1; i < 7; i++)
            {
                index /= 2;
                if (teamList[i][index].getIndex() == oldIndex)
                {
                    teamList[i][index] = new Team("", -1);
                }
                else
                {
                    break;
                }
            }
            refresh();
        }

        private void Champ_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(6, index);
            }
        }

        private void C_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(5, index);
            }
            else
            {
                clearNextRounds(5, 0);
            }
        }

        private void C_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(5, index);
            }
            else
            {
                clearNextRounds(5, 1);
            }
        }

        private void F4_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(4, index);
            }
            else
            {
                clearNextRounds(4, 0);
            }
        }

        private void F4_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(4, index);
            }
            else
            {
                clearNextRounds(4, 1);
            }
        }

        private void F4_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(4, index);
            }
            else
            {
                clearNextRounds(4, 2);
            }
        }

        private void F4_4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(4, index);
            }
            else
            {
                clearNextRounds(4, 3);
            }
        }

        private void E8_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(3, index);
            }
            else
            {
                clearNextRounds(3, 0);
            }
        }

        private void E8_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(3, index);
            }
            else
            {
                clearNextRounds(3, 1);
            }
        }

        private void E8_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(3, index);
            }
            else
            {
                clearNextRounds(3, 2);
            }
        }

        private void E8_4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(3, index);
            }
            else
            {
                clearNextRounds(3, 3);
            }
        }

        private void E8_5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(3, index);
            }
            else
            {
                clearNextRounds(3, 4);
            }
        }

        private void E8_6_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(3, index);
            }
            else
            {
                clearNextRounds(3, 5);
            }
        }

        private void E8_7_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(3, index);
            }
            else
            {
                clearNextRounds(3, 6);
            }
        }

        private void E8_8_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(3, index);
            }
            else
            {
                clearNextRounds(3, 7);
            }
        }

        private void S16_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 0);
            }
        }

        private void S16_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 1);
            }
        }

        private void S16_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 2);
            }
        }

        private void S16_4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 3);
            }
        }

        private void S16_5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 4);
            }
        }

        private void S16_6_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 5);
            }
        }

        private void S16_7_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 6);
            }
        }

        private void S16_8_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 7);
            }
        }

        private void S16_9_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 8);
            }
        }

        private void S16_10_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 9);
            }
        }

        private void S16_11_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 10);
            }
        }

        private void S16_12_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 11);
            }
        }

        private void S16_13_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 12);
            }
        }

        private void S16_14_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 13);
            }
        }

        private void S16_15_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 14);
            }
        }

        private void S16_16_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(2, index);
            }
            else
            {
                clearNextRounds(2, 15);
            }
        }

        private void R32_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 0);
            }

        }

        private void R32_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 1);
            }
        }

        private void R32_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 2);
            }
        }

        private void R32_4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 3);
            }
        }

        private void R32_5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 4);
            }
        }

        private void R32_6_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 5);
            }
        }

        private void R32_7_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 6);
            }
        }

        private void R32_8_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 7);
            }
        }

        private void R32_9_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 8);
            }
        }

        private void R32_10_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 9);
            }
        }

        private void R32_11_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 10);
            }
        }

        private void R32_12_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 11);
            }
        }

        private void R32_13_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 12);
            }
        }

        private void R32_14_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 13);
            }
        }

        private void R32_15_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 14);
            }
        }

        private void R32_16_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 15);
            }
        }

        private void R32_17_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 16);
            }
        }

        private void R32_18_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 17);
            }
        }

        private void R32_19_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 18);
            }
        }

        private void R32_20_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 19);
            }
        }

        private void R32_21_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 20);
            }
        }

        private void R32_22_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 21);
            }
        }

        private void R32_23_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 22);
            }
        }

        private void R32_24_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 23);
            }
        }

        private void R32_25_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 24);
            }
        }

        private void R32_26_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 25);
            }
        }

        private void R32_27_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 26);
            }
        }

        private void R32_28_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 27);
            }
        }

        private void R32_29_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 28);
            }
        }

        private void R32_30_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 29);
            }
        }

        private void R32_31_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 30);
            }
        }

        private void R32_32_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Text != "")
            {
                int index = teamList[0].Where(t => t.getDisplayName() == box.Text).First().getIndex();
                SelectTeam(1, index);
            }
            else
            {
                clearNextRounds(1, 31);
            }
        }
    }
}
