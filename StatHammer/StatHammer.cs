using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StatHammer
{
    public partial class StatHammer : Form
    {
        public StatHammer()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            int attacks = int.Parse(txtAttacks.Text);
            int toHit = int.Parse(txtToHit.Text);
            int toWound = int.Parse(txtToWound.Text);
            int saveFail = int.Parse(txtSave.Text);
            int atLeast = int.Parse(txtAtLeast.Text);
            int lessThan = int.Parse(txtLessThan.Text);

            double totalProbability = 0;

            double pAtLeast = 0;
            double pLessThan = 0;

            chart1.Series[0].Points.Clear();
            for (int unsavedDamage = 0; unsavedDamage <= attacks; unsavedDamage++)
            {
                double pUnsavedDamage = 0;
                for (int hits = unsavedDamage; hits <= attacks; hits++)
                {
                    double pHits = d6RollProbability(attacks, hits, toHit);
                    for (int wounds = unsavedDamage; wounds <= hits; wounds++)
                    {
                        double pWounds = d6RollProbability(hits, wounds, toWound);
                        //For unsaved = unsavedDamage To wounds
                        double pUnsaved = d6RollProbability(wounds, unsavedDamage, saveFail);
                        pUnsavedDamage += pHits * pWounds * pUnsaved;
                        if (atLeast <= unsavedDamage)
                        {
                            pAtLeast += pHits * pWounds * pUnsaved;
                        }


                        if (lessThan > unsavedDamage)
                        {
                            pLessThan += pHits * pWounds * pUnsaved;
                        }
                        
                    }
            }
                chart1.Series[0].Points.AddXY(unsavedDamage, pUnsavedDamage * 100);
                totalProbability += pUnsavedDamage;
            }

            txtPAtLeast.Text = string.Format("{0:0.00}",pAtLeast * 100);
            txtPLessThan.Text = string.Format("{0:0.00}", pLessThan * 100);
            txtSumTotal.Text = string.Format("{0:0.00}", totalProbability);
        }

        public double d6RollProbability(int input, int output, int successOutOfSix)
        {
            double result = combination(output, input) * Math.Pow((Convert.ToDouble(successOutOfSix) / 6), output) * Math.Pow(1 - (Convert.ToDouble(successOutOfSix) / 6), input - output);
            return result;
        }

        public int factorial(int input)
        {
            int result = 1;
            for(int index = 1; index <= input;input ++) 
            {
                result = result * index;
            }
        return result;
        }

        public List<int> combinationCalcNumerator = new List<int> { };
        public List<int> combinationCalcDenominator = new List<int> { };
        public double combination(int success, int outOf)
        {
            combinationCalcNumerator.Clear();
            combinationCalcDenominator.Clear();

            for (int index = 1; index <= outOf; index++)
            {
                combinationCalcNumerator.Add(index);
            }

            for (int index = 1; index <= success; index ++)
            {
                combinationCalcDenominator.Add(index);
            }

            int buffer = outOf - success;
            for (int index = 1; index <= buffer; index++)
            {
                combinationCalcDenominator.Add(index);
            }

            double Result = 1;
            for (int index = 0; index <= combinationCalcDenominator.Count - 1; index++)
            {
                Result *= Convert.ToDouble(combinationCalcNumerator[index]) / Convert.ToDouble(combinationCalcDenominator[index]);
            }

        return Result;
        }

        public List<attacker> attackerList = new List<attacker> { };
        public List<defender> defenderList = new List<defender> { };

        private void btnAttackerAdd_Click(object sender, EventArgs e)
        {
            attacker newAttacker = new attacker();
            newAttacker.name = txtAttackerName.Text;
            newAttacker.ws = int.Parse(txtAttackerWS.Text);
            newAttacker.bs = int.Parse(txtAttackerBS.Text);
            newAttacker.myWeapon.strength = int.Parse(txtAttackerStrength.Text);
            newAttacker.myWeapon.ap = int.Parse(txtAttackerAP.Text);
            newAttacker.myWeapon.attacks = int.Parse(txtAttackerAttacks.Text);

            attackerList.Add(newAttacker);
            lbAttackers.Items.Add(txtAttackerName.Text);
        }

        private void btnAttackerRemove_Click(object sender, EventArgs e)
        {
            if(lbAttackers.SelectedIndex >= 0)
            {
                attackerList.RemoveAt(lbAttackers.SelectedIndex);
                lbAttackers.Items.RemoveAt(lbAttackers.SelectedIndex);
            }
        }

        private void btnAttackerSave_Click(object sender, EventArgs e)
        {
            if(lbAttackers.SelectedIndex >= 0)
            {
                attacker newAttacker = new attacker();
                newAttacker.name = txtAttackerName.Text;
                newAttacker.ws = int.Parse(txtAttackerWS.Text);
                newAttacker.bs = int.Parse(txtAttackerBS.Text);
                newAttacker.myWeapon.strength = int.Parse(txtAttackerStrength.Text);
                newAttacker.myWeapon.ap = int.Parse(txtAttackerAP.Text);
                newAttacker.myWeapon.attacks = int.Parse(txtAttackerAttacks.Text);

                attackerList[lbAttackers.SelectedIndex] = newAttacker;
                lbAttackers.Items[lbAttackers.SelectedIndex] = txtAttackerName.Text;
            }
        }

        private void lbAttackers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lbAttackers.SelectedIndex >= 0)
            {
                txtAttackerName.Text = attackerList[lbAttackers.SelectedIndex].name;
                txtAttackerWS.Text = attackerList[lbAttackers.SelectedIndex].ws.ToString();
                txtAttackerBS.Text = attackerList[lbAttackers.SelectedIndex].bs.ToString();
                txtAttackerStrength.Text = attackerList[lbAttackers.SelectedIndex].myWeapon.strength.ToString();
                txtAttackerAP.Text = attackerList[lbAttackers.SelectedIndex].myWeapon.ap.ToString();
                txtAttackerAttacks.Text = attackerList[lbAttackers.SelectedIndex].myWeapon.attacks.ToString();
            }
        }

        private void btnDefenderAdd_Click(object sender, EventArgs e)
        {
            defender newDefender = new defender();
            newDefender.name = txtDefenderName.Text;
            newDefender.ws = int.Parse(txtDefenderWS.Text);
            newDefender.toughness = int.Parse(txtDefenderToughness.Text);
            newDefender.save = int.Parse(txtDefenderSave.Text);
            newDefender.invulnerableSave = int.Parse(txtDefenderInvuln.Text);
            newDefender.feelNoPain = int.Parse(txtDefenderFNP.Text);

            defenderList.Add(newDefender);
            lbDefenders.Items.Add(txtDefenderName.Text);
        }

        private void btnDefenderRemove_Click(object sender, EventArgs e)
        {
            if (lbDefenders.SelectedIndex >= 0)
            {
                defenderList.RemoveAt(lbDefenders.SelectedIndex);
                lbDefenders.Items.RemoveAt(lbDefenders.SelectedIndex);
            }
        }

        private void btnDefenderSave_Click(object sender, EventArgs e)
        {
            if (lbDefenders.SelectedIndex >= 0)
            {
                defender newDefender = new defender();
                newDefender.name = txtDefenderName.Text;
                newDefender.ws = int.Parse(txtDefenderWS.Text);
                newDefender.toughness = int.Parse(txtDefenderToughness.Text);
                newDefender.save = int.Parse(txtDefenderSave.Text);
                newDefender.invulnerableSave = int.Parse(txtDefenderInvuln.Text);
                newDefender.feelNoPain = int.Parse(txtDefenderFNP.Text);

                defenderList[lbDefenders.SelectedIndex] = newDefender;
                lbDefenders.Items[lbDefenders.SelectedIndex] = txtDefenderName.Text;
            }
        }

        private void lbDefenders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbDefenders.SelectedIndex >= 0)
            {
                txtDefenderName.Text = defenderList[lbDefenders.SelectedIndex].name;
                txtDefenderWS.Text = defenderList[lbDefenders.SelectedIndex].ws.ToString();
                txtDefenderToughness.Text = defenderList[lbDefenders.SelectedIndex].toughness.ToString();
                txtDefenderSave.Text = defenderList[lbDefenders.SelectedIndex].save.ToString();
                txtDefenderInvuln.Text = defenderList[lbDefenders.SelectedIndex].invulnerableSave.ToString();
                txtDefenderFNP.Text = defenderList[lbDefenders.SelectedIndex].feelNoPain.ToString();
            }
        }

        public List<string> listOfAttackerSpecials;
        public List<string> listOfDefenderSpecials;

        List<double> resultWounds;

        private void StatHammer_Load(object sender, EventArgs e)
        {
            listOfAttackerSpecials = new List<string> { };
            listOfDefenderSpecials = new List<string> { };

            resultWounds = new List<double> { };

            listOfAttackerSpecials.Add("Reroll failed hits");
            listOfAttackerSpecials.Add("Reroll successful hits");
            listOfAttackerSpecials.Add("Armorbane");
            listOfAttackerSpecials.Add("Fleshbane");
            listOfAttackerSpecials.Add("Tank Hunter");
            listOfAttackerSpecials.Add("Monster Hunter");
            listOfAttackerSpecials.Add("Reroll failed wounds");
            listOfAttackerSpecials.Add("Reroll successful wounds");
            listOfAttackerSpecials.Add("Melta");
            listOfAttackerSpecials.Add("Ignores Cover");

            listOfDefenderSpecials.Add("Reroll failed hits");
            listOfDefenderSpecials.Add("Reroll successful hits");
            listOfDefenderSpecials.Add("Reroll failed wounds");
            listOfDefenderSpecials.Add("Reroll successful wounds");
            listOfDefenderSpecials.Add("Ceramite plating");
            listOfDefenderSpecials.Add("Reroll Armor Saves");
            listOfDefenderSpecials.Add("Reroll Invulnerable Saves");
            listOfDefenderSpecials.Add("Reroll FNP");


            for (int index = 0;index < listOfAttackerSpecials.Count; index ++)
            {
                lbAttackerAvailableRules.Items.Add(listOfAttackerSpecials[index]);
            }

            for (int index = 0; index < listOfDefenderSpecials.Count; index ++)
            {
                lbDefenderAvailableRules.Items.Add(listOfDefenderSpecials[index]);
            }
        }

        private void btnNewRunStats_Click(object sender, EventArgs e)
        {
            if ((lbAttackers.SelectedIndex >= 0) & (lbDefenders.SelectedIndex >= 0))
            {
                
            }
        }

        private void btnDefenderAdd_Click_1(object sender, EventArgs e)
        {
            defender newDefender;
              newDefender.name = txtDefenderName.Text;
            newDefender.ws = int.Parse(txtDefenderWS.Text);
            newDefender.toughness  = int.Parse(txtDefenderToughness.Text);
            newDefender.save = int.Parse(txtDefenderSave.Text) ;
            newDefender.invulnerableSave = int.Parse(txtDefenderInvuln.Text);
            newDefender.feelNoPain = int.Parse(txtDefenderFNP.Text);

            defenderList.Add(newDefender);
            lbDefenders.Items.Add(newDefender.name);
    }

        private void lbDefenders_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (lbDefenders.SelectedIndex >= 0)
            {
                int index = lbDefenders.SelectedIndex;

                
                txtDefenderName.Text = defenderList[index].name;
       txtDefenderWS.Text = defenderList[index].ws.ToString();
        txtDefenderToughness.Text =  defenderList[index].toughness.ToString();
        txtDefenderSave.Text = defenderList[index].save.ToString();
        txtDefenderInvuln.Text = defenderList[index].invulnerableSave.ToString();
        txtDefenderFNP.Text = defenderList[index].feelNoPain.ToString();
    }
        }

        private void btnDefenderRemove_Click_1(object sender, EventArgs e)
        {
            if (lbDefenders.SelectedIndex > 0)
            {
                int index = lbDefenders.SelectedIndex;
                lbDefenders.Items.RemoveAt(index);
                defenderList.RemoveAt(index);
            }
        }

        private void btnDefenderSave_Click_1(object sender, EventArgs e)
        {
            if (lbDefenders.SelectedIndex > 0)
            {
                defender newDefender;
                newDefender.name = txtDefenderName.Text;
                newDefender.ws = int.Parse(txtDefenderWS.Text);
                newDefender.toughness = int.Parse(txtDefenderToughness.Text);
                newDefender.save = int.Parse(txtDefenderSave.Text);
                newDefender.invulnerableSave = int.Parse(txtDefenderInvuln.Text);
                newDefender.feelNoPain = int.Parse(txtDefenderFNP.Text);

                defenderList[lbDefenders.SelectedIndex] = newDefender;
            }
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {

        }

        #region mathItems
        
        public attackProfile generateAttackProfile(attacker attack, defender defence)
        {
            attackProfile returnStructure;

            returnStructure.attacks = attack.myWeapon.attacks;
            returnStructure.toHit = attack.bs;
            if (returnStructure.toHit > 5)
            {
                returnStructure.toHit = 5;
            }
            
            if (attack.myWeapon.strength - 2 >= defence.toughness)
            {
                returnStructure.toWound = 5;
            }
            else if (attack.myWeapon.strength - 1 == defence.toughness)
            {
                returnStructure.toWound = 4;
            }
            else if (attack.myWeapon.strength == defence.toughness)
            {
                returnStructure.toWound = 3;
            }
            else if (attack.myWeapon.strength + 1 == defence.toughness)
            {
                returnStructure.toWound = 2;
            }
            else if (attack.myWeapon.strength + 4 <= defence.toughness)
            {
                returnStructure.toWound = 0;
            }
            else
            {
                returnStructure.toWound = 1;
            }

            if (defence.invulnerableSave < defence.save)
            {
                returnStructure.toSave = 7 - defence.invulnerableSave;
            }
            else if(defence.save < attack.myWeapon.ap)
            {
                returnStructure.toSave = 0;
            }
            else
            {
                returnStructure.toSave = defence.save;
            }
            return returnStructure;
        }

       
        public attackResult runSingleAttack(attackProfile input)
        {
            int attacks = input.attacks;
            int toHit = input.toHit;
            int toWound = input.toWound;
            int saveFail = 7 - input.toSave;
            int atLeast = int.Parse(txtAtLeast.Text);
            //int lessThan = int.Parse(txtLessThan.Text);

            attackResult returnStructure;

            returnStructure.pAtLeast = 0;
            returnStructure.pLessThan = 0;
            returnStructure.pSanity = 0;

            double totalProbability = 0;

            double pAtLeast = 0;
            double pLessThan = 0;

            chart1.Series[0].Points.Clear();
            for (int unsavedDamage = 0; unsavedDamage <= attacks; unsavedDamage++)
            {
                double pUnsavedDamage = 0;
                for (int hits = unsavedDamage; hits <= attacks; hits++)
                {
                    double pHits = d6RollProbability(attacks, hits, toHit);
                    for (int wounds = unsavedDamage; wounds <= hits; wounds++)
                    {
                        double pWounds = d6RollProbability(hits, wounds, toWound);
                        //For unsaved = unsavedDamage To wounds
                        double pUnsaved = d6RollProbability(wounds, unsavedDamage, saveFail);
                        pUnsavedDamage += pHits * pWounds * pUnsaved;
                        if (atLeast <= unsavedDamage)
                        {
                            pAtLeast += pHits * pWounds * pUnsaved;
                        }


                        if (lessThan > unsavedDamage)
                        {
                            pLessThan += pHits * pWounds * pUnsaved;
                        }

                    }
                }
                chart1.Series[0].Points.AddXY(unsavedDamage, pUnsavedDamage * 100);
                totalProbability += pUnsavedDamage;
            }
            
            return returnStructure;
        }

        
        #endregion
    }


    #region objects
    public struct attackResult
    {
        public double pAtLeast;
        public double pLessThan;
        public double pSanity; //Should always be 1, if it isn't we fucked up
    }
    public struct attackProfile
    {
        public int attacks;
        public int toHit;
        public int toWound;
        public int toSave;
    }

    public struct weapon
    {
        public string name;
        public int strength;
        public int ap;
        public int attacks;
    }

    public struct attacker
    {
        public string name;
        public int ws;
        public int bs;
        public weapon myWeapon;
    }

    public struct defender
    {
        public string name;
        public int ws;
        public int toughness;
        public int save;
        public int invulnerableSave;
        public int feelNoPain;
    }
    #endregion
}
