using System;
using System.Collections;
using System.Collections.Generic;

namespace yours_indie_gameDev.Plugin.DataStructures
{
    [Serializable]
    public class Array2D<T> : IEnumerable
    {
        [Serializable]
        public struct RowData<D>
        {
            public D[] row;
        }
        public RowData<T>[] rowData;

        public Array2D(int rows, int cols)
        {
            rowData = new RowData<T>[rows];
            for (int i = 0; i < rows; i++)
            {
                rowData[i].row = new T[cols];
            }
        }

        public T this[int x, int y]
        {
            get { return rowData[x].row[y]; }
            set { rowData[x].row[y] = value; }
        }
        public void CopyFrom(T[,] another2darr)
        {
            for (int y = 0; y < rowData[0].row.Length; y++)
            {
                for (int x = 0; x < rowData.Length; x++)
                {
                    this[x, y] = another2darr[x, y];
                }
            }
        }
        public void CopyTo(T[,] another2darr)
        {
            for (int y = 0; y < rowData[0].row.Length; y++)
            {
                for (int x = 0; x < rowData.Length; x++)
                {
                    another2darr[x, y] = this[x, y];
                }
            }
        }
        public void CopyFrom(List<List<T>> another2dlist)
        {
            for (int y = 0; y < rowData[0].row.Length; y++)
            {
                for (int x = 0; x < rowData.Length; x++)
                {
                    this[x, y] = another2dlist[x][y];
                }
            }
        }
        public void CopyTo(List<List<T>> another2dlist)
        {
            for (int y = 0; y < rowData[0].row.Length; y++)
            {
                for (int x = 0; x < rowData.Length; x++)
                {
                    another2dlist[x][y] = this[x, y];
                }
            }
        }
        public IEnumerator GetEnumerator()
        {
            foreach (var item in rowData) yield return item;
        }
    }

}
