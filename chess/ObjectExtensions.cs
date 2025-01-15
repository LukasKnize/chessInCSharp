namespace chess
{
    public static class ObjectExtensions
    {
        //funkce na deep copy pole Field
        public static Field[,] DeepCopyFieldArray(Field[,] originalArray)
        {
            int rows = originalArray.GetLength(0);
            int columns = originalArray.GetLength(1);

            Field[,] copiedArray = new Field[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    copiedArray[row, col] = originalArray[row, col].DeepCopy();
                }
            }

            return copiedArray;
        }
    }

}