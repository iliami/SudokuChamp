using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuChamp.API.SudokuGame
{
    internal class Cell
    {
        public int Value { get; private set; }
        public bool IsEditable { get; private set; }
        public Cell(int value, bool isEditable)
        {
            ValidateValue(value);
            Value = value;
            IsEditable = isEditable;
        }
        public void SetValue(int value)
        {
            if (IsEditable)
            {
                Value = value;
            }
        }
        public void ChangeEditableToFalse()
        {
            IsEditable = false;
        }
        private void ValidateValue(int value)
        {
            if (value < 1 || value > 9)
                throw new ArgumentOutOfRangeException(nameof(value), "value должно быть положительным значением от 1 до 9 включительно");
        }
    }
}
