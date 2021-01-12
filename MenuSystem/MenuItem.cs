using System;
using GameEntities;

namespace MenuSystem
{
    public sealed class MenuItem
    {
        public string OptionLabelToChoose { get; set; }
        public string OptionValue { get; set; }
        public Action OptionToExecute { get; set; } = null!;
        public Func<Cell> OptionFuncToExecute { get; set; } = null!;
        public FuncToExecute FunctionToExectue = null!;
        public PlaceShipDelegate PlaceShipFunction = null!;
        public delegate void FuncToExecute(Cell[][] cell);

        public delegate void PlaceShipDelegate(Ship.ShipType shipType);
        public MenuItem(string optionLabelToChoose, string optionValue, Action optionToExecute)
        {
            OptionLabelToChoose = optionLabelToChoose.Trim();
            OptionValue = optionValue.Trim();
            OptionToExecute = optionToExecute;
        }
        public MenuItem(string optionLabelToChoose, string optionValue, FuncToExecute func)
        {
            OptionLabelToChoose = optionLabelToChoose.Trim();
            OptionValue = optionValue.Trim();
            FunctionToExectue = func;
        }
        
        public MenuItem(string optionLabelToChoose, string optionValue, PlaceShipDelegate shipDelegate)
        {
            OptionLabelToChoose = optionLabelToChoose.Trim();
            OptionValue = optionValue.Trim();
            PlaceShipFunction = shipDelegate;
        }

        public MenuItem(string optionLabelToChoose)
        {
            throw new NotImplementedException();
        }
        

        public override string ToString()
        {
            return OptionLabelToChoose + ") " + OptionValue;
        }
    }
}