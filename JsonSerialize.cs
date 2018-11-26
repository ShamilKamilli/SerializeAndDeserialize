using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SerializeAndDeserialize
{
    public class JsonSerialize
    {
        private const string _systemName = "System";

        #region Serialize
        private StringBuilder _builder { get; set; }

        public string Serialize(object data)
        {
            _builder = new StringBuilder();
            _serialize(data, null);
            return _builder.ToString();
        }

        private void _serialize(object data, PropertyInfo info)
        {
            Type dataType = data.GetType();

            ///check system type or not
            if (!_checkObjectSystemType(dataType))
            {
                _builder.Append("{");
                _loopAllPropertiesAndAppend(dataType.GetProperties(), data);
            }
            else
                _addData(data, info);
        }

        private bool _checkObjectSystemType(Type type)
        {
            if (type.Namespace == _systemName)
                return true;
            else
                return false;
        }

        private void _loopAllPropertiesAndAppend(PropertyInfo[] propertyInfos, object data)
        {
            int count = 0;
            foreach (PropertyInfo item in propertyInfos)
            {
                count++;
                var val = item.GetValue(data);
                if (val is IEnumerable && !(val is string))
                    _loopCollection(val);
                else
                    _serialize(val, item);

                if (!(count == propertyInfos.Length))
                    _builder.Append(",");
            }

            _builder.Append("}");
        }

        private void _loopCollection(object data)
        {
            var loopData = data as IEnumerable;
            _builder.Append("{");
            foreach (object item in loopData)
            {
                _serialize(item, null);
            }
            _builder.Append("}");
        }

        private void _addData(object data, PropertyInfo prop)
        {
            _builder.Append($"{prop?.Name}: {data}");
        }
        #endregion

        #region DeSerialize



        #endregion
    }
}
