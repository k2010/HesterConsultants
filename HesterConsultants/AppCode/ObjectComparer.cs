using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace HesterConsultants.AppCode
{
    // this is modified from 
    // http://www.codeproject.com/KB/recipes/Sorting_with_Objects.aspx
    // I merged in dot-walking and some other things from GenericComparer 

    // to do -
    //You should consider 2 improvements:
    //1. Don't require the user to provide the multiple flag - just split the string by "," and find out for yourself if there's more than one field.
    //2. Do the string parsing in the constructor, and not in the Compare() method. The Compare method gets called quite alot, causing the parsing code to be execute alot of times. 

    [Serializable]
    public class ObjectComparer<T> : IComparer<T>
    {
        #region enums

        //public enum SortOrders
        //{
        //    Ascending,
        //    Descending
        //};

        #region Property

        private string propertyName;
        private bool multiColumn;
        //private SortOrders sortOrder;
        private ObjectSortField[] sortFields;

        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }

        public bool MultiColumn
        {
            get { return multiColumn; }
            set { multiColumn = value; }
        }

        //public SortOrders SortOrder
        //{
        //    get { return sortOrder; }
        //    set { sortOrder = value; }
        //}

        #endregion

        #endregion

        #region Constructor
        public ObjectComparer()
        {
        }

        //public ObjectComparer(string p_propertyName)
        //{
        //    //We must have a property name for this comparer to work
        //    this.PropertyName = p_propertyName;
        //    this.MultiColumn = false;
        //    this.SortOrder = ObjectComparer<ComparableObject>.SortOrders.Ascending;
        //}

        public ObjectComparer(string p_propertyName, bool p_MultiColumn)
        {
            //We must have a property name for this comparer to work
            this.PropertyName = p_propertyName;
            this.MultiColumn = p_MultiColumn;
            //this.SortOrder = ObjectComparer<ComparableObject>.SortOrders.Ascending;
        }

        //public ObjectComparer(string p_propertyName, bool p_multiColumn, SortOrders p_sortOrder)
        //{
        //    this.PropertyName = p_propertyName;
        //    this.MultiColumn = p_multiColumn;
        //    this.SortOrder = p_sortOrder;
        //}

        public ObjectComparer(ObjectSortField[] fields)
        {
            this.sortFields = fields;
        }

        #endregion


        #region IComparer<ComparableObject> Members

        //public int Compare___3(T x, T y)
        //{
        //    Type t = x.GetType();

        //    if (multiColumn) // Multi Column Sorting
        //    {
        //        string[] sortExpressions = propertyName.Trim().Split(',');

        //        for (int i = 0; i < sortExpressions.Length; i++)
        //        {
        //            //Debug.WriteLine("sortExpressions[" + i.ToString() + "]");

        //            string fieldName, direction = "ASC";

        //            if (sortExpressions[i].Trim().EndsWith(" DESC"))
        //            {
        //                fieldName = sortExpressions[i].Replace(" DESC", "").Trim();
        //                direction = "DESC";
        //            }
        //            else
        //            {
        //                fieldName = sortExpressions[i].Replace(" ASC", "").Trim();
        //            }

        //            //Get property by name
        //            PropertyInfo val = t.GetProperty(fieldName);
        //            if (val != null)
        //            {

        //                //Compare values, using IComparable interface of the property's type
        //                int iResult = Comparer.DefaultInvariant.Compare(val.GetValue(x, null), val.GetValue(y, null));
        //                if (iResult != 0)
        //                {
        //                    //Return if not equal
        //                    if (direction == "DESC")
        //                    {
        //                        //Invert order
        //                        return -iResult;
        //                    }
        //                    else
        //                    {
        //                        return iResult;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                throw new Exception(fieldName + " is not a valid property to sort on.  It doesn't exist in the Class.");
        //            }
        //        }
        //        //Objects have the same sort order
        //        return 0;
        //    }
        //    else
        //    {
        //        PropertyInfo val = t.GetProperty(this.PropertyName);
        //        if (val != null)
        //        {
        //            return Comparer.DefaultInvariant.Compare(val.GetValue(x, null), val.GetValue(y, null));
        //        }
        //        else
        //        {
        //            throw new Exception(this.PropertyName + " is not a valid property to sort on.  It doesn't exist in the Class.");
        //        }
        //    }
        //}

        public int Compare(T x, T y)
        {
            string sortExpression;
            ObjectSortField.SortOrders sortOrder;

            Type t = x.GetType();

            for (int k = 0; k < sortFields.Length; k++)
            {
                object objX = x;
                object objY = y;

                sortExpression = sortFields[k].SortExpression;
                sortOrder = sortFields[k].SortOrder;

                string[] fields = sortExpression.Split('.');
                foreach (string field in fields)
                {
                    objX = GetPropertyValue(objX, field);
                    objY = GetPropertyValue(objY, field);
                }

                int result = Comparer.DefaultInvariant.Compare(objX, objY);

                if (result != 0)
                {
                    if (sortOrder == ObjectSortField.SortOrders.Descending)
                        return -result;
                    else
                        return result;
                }
            }

            // objects compare equally
            return 0;
        }

        /// <summary>
        /// This comparer is used to sort the generic comparer
        /// The constructor sets the PropertyName that is used
        /// by reflection to access that property in the object to 
        /// object compare.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        //public int Compare___old(ComparableObject x, ComparableObject y)
        //{
        //    Type t = x.GetType();

        //    object objX = x;
        //    object objY = y;

        //    if (multiColumn) // Multi Column Sorting
        //    {
        //        string[] sortExpressions = propertyName.Trim().Split(',');

        //        for (int k = 0; k < sortExpressions.Length; k++)
        //        {
        //            string sortExpression = sortExpressions[k].Trim(); 

        //            // walk the dots
        //            string[] fields = sortExpression.Split('.');
        //            foreach (string field in fields)
        //            {
        //                objX = GetPropertyValue(objX, t, field);
        //                objY = GetPropertyValue(objY, t, field);
        //            }

        //            int iResult = Comparer.DefaultInvariant.Compare(objX, objY);

        //            if (iResult != 0)
        //            {
        //                //Return if not equal
        //                if (sortOrder == ObjectComparer<ComparableObject>.SortOrders.Descending)
        //                    return -iResult;
        //                else
        //                    return iResult;
        //            }
        //        }

        //        // objects have the same sort order
        //        return 0;
        //    }
        //    else // only one sort field
        //    {
        //        string[] fields = propertyName.Split('.');
        //        foreach (string field in fields)
        //        {
        //            objX = GetPropertyValue(objX, t, field);
        //            objY = GetPropertyValue(objY, t, field);
        //        }                
        //    }

        //    return Comparer.DefaultInvariant.Compare(objX, objY);
        //}

        private object GetPropertyValue(object o, string fieldName)
        {
            object val;

            if (o == null) 
            {
                string typeName = o.GetType().Name.ToLower();

                // try to handle null strings and numerics with default values
                if (typeName.StartsWith("string"))
                {
                    val = String.Empty;
                }
                // can't have null value type (date, int, decimal)
                //else if (typeName.StartsWith("int"))
                //{
                //    val = 0;
                //}
                else
                {
                    throw new Exception("Exception: " + fieldName + " is null.");
                }
            }
            else
            {
                PropertyInfo propertyInfo = o.GetType().GetProperty(fieldName);
                val = propertyInfo.GetValue(o, null);
            }

            return val;
        }

        #endregion
    }

    // 
    public class ObjectSortField
    {
        // enums
        public enum SortOrders
        {
            Ascending,
            Descending
        };

        // fields
        private string sortExpression; // can be complex, 
        // e.g., object.property.otherproperty
        private SortOrders sortOrder;

        public string SortExpression
        {
            get
            {
                return sortExpression;
            }
            set
            {
                sortExpression = value;
            }
        }

        public SortOrders SortOrder
        {
            get
            {
                return sortOrder;
            }
            set
            {
                sortOrder = value;
            }
        }

        // constructor
        public ObjectSortField(string sortExpression, SortOrders sortOrder)
        {
            this.sortExpression = sortExpression;
            this.sortOrder = sortOrder;
        }
    }

}