using System;
using System.Collections.Generic;
using System.Reflection;
using UCS.Files.CSV;

namespace UCS.Files.Logic
{
    internal class Data
    {
        readonly int m_globalID;

        public Data(CSVRow row, DataTable dt)
        {
            m_row = row;
            m_dtTable = dt;
            m_globalID = GlobalID.CreateGlobalID(dt.GetTableIndex() + 1, dt.GetItemCount());
        }

        protected CSVRow m_row;
        protected DataTable m_dtTable;

        public static void LoadData(Data obj, Type objectType, CSVRow row)
        {
            foreach (var prop in objectType.GetProperties())
            {
                if (prop.PropertyType.IsGenericType)
                {
                    var listType = typeof(List<>);
                    var genericArgs = prop.PropertyType.GetGenericArguments();
                    var concreteType = listType.MakeGenericType(genericArgs);
                    var newList = Activator.CreateInstance(concreteType);

                    var add = concreteType.GetMethod("Add");

                    var indexerName =
                        ((DefaultMemberAttribute)
                            newList.GetType().GetCustomAttributes(typeof(DefaultMemberAttribute), true)[0]).MemberName;
                    var indexerProp = newList.GetType().GetProperty(indexerName);

                    for (var i = row.GetRowOffset(); i < row.GetRowOffset() + row.GetArraySize(prop.Name); i++)
                    {
                        var v = row.GetValue(prop.Name, i - row.GetRowOffset());

                        if (v == string.Empty && i != row.GetRowOffset())
                            v = indexerProp.GetValue(newList, new object[] { i - row.GetRowOffset() - 1 }).ToString();

                        if (v == string.Empty)
                        {
                            var o = genericArgs[0].IsValueType ? Activator.CreateInstance(genericArgs[0]) : "";
                            add.Invoke(newList, new[] { o });
                        }

                        else
                            add.Invoke(newList, new[] { Convert.ChangeType(v, genericArgs[0]) });
                    }

                    prop.SetValue(obj, newList);
                }

                else
                {
                    if (row.GetValue(prop.Name, 0) == string.Empty)
                        prop.SetValue(obj, null, null);
                    else
                        prop.SetValue(obj, Convert.ChangeType(row.GetValue(prop.Name, 0), prop.PropertyType), null);
                }
            }
        }

        public int GetDataType()
        {
            return m_dtTable.GetTableIndex();
        }

        public int GetGlobalID()
        {
            return this.m_globalID;
        }

        public int GetInstanceID()
        {
            return GlobalID.GetInstanceID(this.m_globalID);
        }

        public string GetName()
        {
            return this.m_row.GetName();
        }
    }
}
