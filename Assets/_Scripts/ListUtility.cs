using System.Collections.Generic;

public static class ListUtility
{
    public static void Sort<T>(ref List<T> toSort, List<float> constSortingList)
    {
        List<float> copy = new() { Capacity = constSortingList.Capacity };
        copy.AddRange(constSortingList);
        Sort(ref toSort, ref copy);
    }

    public static void Sort<T>(ref List<T> toSort, ref List<float> transSortingList)
    {
        int i = 1;
        while (i < transSortingList.Count)
        {
            float buffer = transSortingList[i];
            T valueBuffer = toSort[i];
            int j = i;
            while (j > 0 && transSortingList[j - 1] > transSortingList[j])
            {
                transSortingList[j] = transSortingList[j - 1];
                toSort[j] = toSort[j - 1];
                --j;
            }
            transSortingList[j] = buffer;
            toSort[j] = valueBuffer;
            ++i;
        }
    }
}
