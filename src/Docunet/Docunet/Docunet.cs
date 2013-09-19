using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Docunet
{
    public class Docunet : Dictionary<string, object>
    {
        public static DocunetSettings Settings = new DocunetSettings();
        
        public Docunet() {  }
        
        public Docunet(string json)
        {
            foreach(KeyValuePair<string, object> field in DeserializeDocument(json))
            {
                this.Add(field.Key, field.Value);
            }
        }
        
        #region Field getters
        
        public bool Bool(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return Convert.ToBoolean(fieldValue);
            }
        }
        
        public byte Byte(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return Convert.ToByte(fieldValue);
            }
        }
        
        public short Short(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return Convert.ToInt16(fieldValue);
            }
        }
        
        public int Int(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return Convert.ToInt32(fieldValue);
            }
        }
        
        public long Long(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return Convert.ToInt64(fieldValue);
            }
        }
        
        public float Float(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return Convert.ToSingle(fieldValue);
            }
        }
        
        public double Double(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return Convert.ToDouble(fieldValue);
            }
        }
        
        public decimal Decimal(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return Convert.ToDecimal(fieldValue);
            }
        }
        
        public string String(string fieldPath)
        {
            return (string)GetField(fieldPath);
        }
        
        public DateTime DateTime(string fieldPath)
        {
            var value = GetField(fieldPath);
            
            if (value is string)
            {
                return System.DateTime.Parse((string)value, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal);
            }
            else if (value is long)
            {
                return DocunetSettings.UnixEpoch.AddSeconds((long)value);
            }
            else
            {
                return (DateTime)value;
            }
        }
        
        public object Object(string fieldPath)
        {
            return (object)GetField(fieldPath);
        }
        
        public T Object<T>(string fieldPath)
        {
            return (T)GetField(fieldPath);
        }
        
        public Docunet Document(string fieldPath)
        {
            return (Docunet)GetField(fieldPath);
        }
        
        public List<T> List<T>(string fieldPath)
        {
            var collection = new List<T>();
            var type = typeof(T);
            var data = GetField(fieldPath);
            
            if (data is List<T>)
            {
                collection = ((IEnumerable)data).Cast<T>().ToList();
            }
            else
            {
                switch (type.Name)
                {
                    case "Boolean":
                        collection = ((List<object>)data).Select(Convert.ToBoolean).ToList() as List<T>;
                        break;
                    case "Byte":
                        collection = ((List<object>)data).Select(Convert.ToByte).ToList() as List<T>;
                        break;
                    case "Int16":
                        collection = ((List<object>)data).Select(Convert.ToInt16).ToList() as List<T>;
                        break;
                    case "Int32":
                        collection = ((List<object>)data).Select(Convert.ToInt32).ToList() as List<T>;
                        break;
                    case "Int64":
                        collection = ((List<object>)data).Select(Convert.ToInt64).ToList() as List<T>;
                        break;
                    case "Single":
                        collection = ((List<object>)data).Select(Convert.ToSingle).ToList() as List<T>;
                        break;
                    case "Double":
                        collection = ((List<object>)data).Select(Convert.ToDouble).ToList() as List<T>;
                        break;
                    case "Decimal":
                        collection = ((List<object>)data).Select(Convert.ToDecimal).ToList() as List<T>;
                        break;
                    case "DateTime":
                        collection = ((List<object>)data).Select(Convert.ToDateTime).ToList() as List<T>;
                        break;
                    case "String":
                        collection = ((List<object>)data).Select(Convert.ToString).ToList() as List<T>;
                        break;
                    default:
                        collection = ((IEnumerable)data).Cast<T>().ToList();
                        break;
                }
            }
            
            return collection;
        }
        
        private object GetField(string fieldPath)
        {
            var currentField = "";
            var arrayContent = "";
            
            if (fieldPath.Contains("."))
            {
                var fields = fieldPath.Split('.');
                var iteration = 1;
                var embeddedDocument = this;
                
                foreach (var field in fields)
                {
                    currentField = field;
                    arrayContent = "";
                    
                    if (field.Contains("["))
                    {
                        var firstIndex = field.IndexOf('[');
                        var lastIndex = field.IndexOf(']');
                        
                        arrayContent = field.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                        currentField = field.Substring(0, firstIndex);
                    }
                    
                    if (iteration == fields.Length)
                    {
                        if (embeddedDocument.ContainsKey(currentField))
                        {
                            return GetFieldValue(currentField, arrayContent, embeddedDocument);
                        }
                        
                        break;
                    }

                    if (embeddedDocument.ContainsKey(currentField))
                    {
                        embeddedDocument = (Docunet)GetFieldValue(currentField, arrayContent, embeddedDocument);
                    }
                    else
                    {
                        // if current field in path isn't present
                        break;
                    }

                    iteration++;
                }
            }
            else
            {
                currentField = fieldPath;
                
                if (fieldPath.Contains("["))
                {
                    var firstIndex = fieldPath.IndexOf('[');
                    var lastIndex = fieldPath.IndexOf(']');
                    
                    arrayContent = fieldPath.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                    currentField = fieldPath.Substring(0, firstIndex);
                }
                
                if (this.ContainsKey(currentField))
                {
                    return GetFieldValue(currentField, arrayContent, this);
                }
            }
            
            return null;
        }
        
        private object GetFieldValue(string fieldName, string arrayContent, Docunet fieldObject)
        {
            if (arrayContent == "")
            {
                return fieldObject[fieldName];
            }
            else
            {
                return ((IList)fieldObject[fieldName])[int.Parse(arrayContent)];
            }
        }
        
        #endregion
        
        #region Field setters
        
        public Docunet Bool(string fieldPath, bool value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Byte(string fieldPath, byte value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Short(string fieldPath, short value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Int(string fieldPath, int value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Long(string fieldPath, long value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Float(string fieldPath, float value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Double(string fieldPath, double value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet Decimal(string fieldPath, decimal value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet String(string fieldPath, string value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        public Docunet DateTime(string fieldPath, DateTime value)
        {
            return DateTime(fieldPath, value, Settings.DateTimeFormat);
        }
        
        public Docunet DateTime(string fieldPath, DateTime value, DateTimeFormat format)
        {
            switch (format)
            {
                case DateTimeFormat.Iso8601String:
                    SetField(fieldPath, value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", DateTimeFormatInfo.InvariantInfo));
                    break;
                case DateTimeFormat.UnixTimeStamp:
                    TimeSpan span = (value.ToUniversalTime() - DocunetSettings.UnixEpoch);
                    SetField(fieldPath, (long)span.TotalSeconds);
                    break;
                case DateTimeFormat.DateTimeObject:
                default:
                    SetField(fieldPath, value);
                    break;
            }

            return this;
        }
        
        // used for null inputObject
        public Docunet Object(string fieldPath, object inputObject)
        {
            SetField(fieldPath, inputObject);

            return this;
        }
        
        public Docunet Object<T>(string fieldPath, T inputObject)
        {
            SetField(fieldPath, inputObject);

            return this;
        }
        
        public Docunet Document<T>(string fieldPath, T inputObject)
        {
            if (inputObject is Docunet)
            {
                SetField(fieldPath, inputObject);
            }
            else
            {
                SetField(fieldPath, ToDocument<T>(inputObject));
            }
            
            return this;
        }
        
        public Docunet List<T>(string fieldPath, List<T> inputCollection)
        {
            SetField(fieldPath, inputCollection);

            return this;
        }
        
        private void SetField(string fieldPath, object inputObject)
        {
            if (fieldPath.Contains("."))
            {
                var fields = fieldPath.Split('.');
                var iteration = 1;
                var embeddedDocument = this;

                foreach (var field in fields)
                {
                    if (iteration == fields.Length)
                    {
                        if (embeddedDocument.ContainsKey(field))
                        {
                            embeddedDocument[field] = inputObject;
                        }
                        else
                        {
                            embeddedDocument.Add(field, inputObject);
                        }
                        
                        break;
                    }

                    if (embeddedDocument.ContainsKey(field))
                    {
                        embeddedDocument = (Docunet)embeddedDocument[field];
                    }
                    else
                    {
                        // if document which contains the field doesn't exist create it first
                        var tempDocument = new Docunet();
                        embeddedDocument.Add(field, tempDocument);
                        embeddedDocument = tempDocument;
                    }

                    iteration++;
                }
            }
            else
            {
                if (this.ContainsKey(fieldPath))
                {
                    this[fieldPath] = inputObject;
                }
                else
                {
                    this.Add(fieldPath, inputObject);
                }
            }
        }
        
        #endregion
        
        public bool Has(string fieldPath)
        {
            var currentField = "";
            var arrayContent = "";
            
            if (fieldPath.Contains("."))
            {
                var fields = fieldPath.Split('.');
                var iteration = 1;
                var embeddedDocument = this;
                
                foreach (var field in fields)
                {
                    currentField = field;
                    arrayContent = "";
                    
                    if (field.Contains("["))
                    {
                        var firstIndex = field.IndexOf('[');
                        var lastIndex = field.IndexOf(']');
                        
                        arrayContent = field.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                        currentField = field.Substring(0, firstIndex);
                    }
                    
                    if (iteration == fields.Length)
                    {
                        if (embeddedDocument.ContainsKey(currentField))
                        {
                            // it's array - should check if there is value at specific index
                            if (arrayContent != "")
                            {
                                // passed array index is less than total number of elements in the array
                                if (((IList)embeddedDocument[currentField]).Count > int.Parse(arrayContent))
                                {
                                    return true;
                                }
                            }
                            // it's single value
                            else
                            {
                                return true;
                            }
                        }
                        
                        break;
                    }

                    if (embeddedDocument.ContainsKey(currentField))
                    {
                        embeddedDocument = (Docunet)GetFieldValue(currentField, arrayContent, embeddedDocument);
                    }
                    else
                    {
                        // if current field in path isn't present
                        break;
                    }

                    iteration++;
                }
            }
            else
            {
                currentField = fieldPath;
                
                if (fieldPath.Contains("["))
                {
                    var firstIndex = fieldPath.IndexOf('[');
                    var lastIndex = fieldPath.IndexOf(']');
                    
                    arrayContent = fieldPath.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                    currentField = fieldPath.Substring(0, firstIndex);
                }
                
                if (this.ContainsKey(currentField))
                {
                    // it's array - should check if there is value at specific index
                    if (arrayContent != "")
                    {
                        // passed array index is less than total number of elements in the array
                        if (((IList)this[currentField]).Count > int.Parse(arrayContent))
                        {
                            return true;
                        }
                    }
                    // it's single value
                    else
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        public bool IsNull(string fieldPath)
        {
            var currentField = "";
            var arrayContent = "";
            
            if (fieldPath.Contains("."))
            {
                var fields = fieldPath.Split('.');
                var iteration = 1;
                var embeddedDocument = this;
                
                foreach (var field in fields)
                {
                    currentField = field;
                    arrayContent = "";
                    
                    if (field.Contains("["))
                    {
                        var firstIndex = field.IndexOf('[');
                        var lastIndex = field.IndexOf(']');
                        
                        arrayContent = field.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                        currentField = field.Substring(0, firstIndex);
                    }
                    
                    if (iteration == fields.Length)
                    {
                        if (embeddedDocument.ContainsKey(currentField))
                        {
                            // it's array - should check if there is value at specific index
                            if (arrayContent != "")
                            {
                                var collection = (IList)embeddedDocument[currentField];
                                var index = int.Parse(arrayContent);                        
                                // passed array index is less than total number of elements in the array
                                if (collection.Count > index)
                                {
                                    if (collection[index] == null)
                                    {
                                        return true;
                                    }
                                }
                            }
                            // it's single value
                            else
                            {
                                if (embeddedDocument[currentField] == null)
                                {
                                    return true;
                                }
                            }
                        }
                        
                        break;
                    }

                    if (embeddedDocument.ContainsKey(currentField))
                    {
                        embeddedDocument = (Docunet)GetFieldValue(currentField, arrayContent, embeddedDocument);
                    }
                    else
                    {
                        // if current field in path isn't present
                        break;
                    }

                    iteration++;
                }
            }
            else
            {
                currentField = fieldPath;
                
                if (fieldPath.Contains("["))
                {
                    var firstIndex = fieldPath.IndexOf('[');
                    var lastIndex = fieldPath.IndexOf(']');
                    
                    arrayContent = fieldPath.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                    currentField = fieldPath.Substring(0, firstIndex);
                }
                
                if (this.ContainsKey(currentField))
                {
                    // it's array - should check if there is value at specific index
                    if (arrayContent != "")
                    {
                        var collection = (IList)this[currentField];
                        var index = int.Parse(arrayContent);                        
                        // passed array index is less than total number of elements in the array
                        if (collection.Count > index)
                        {
                            if (collection[index] == null)
                            {
                                return true;
                            }
                        }
                    }
                    // it's single value
                    else
                    {
                        if (this[currentField] == null)
                        {
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }
        
        public Type Type(string fieldPath)
        {
            var currentField = "";
            var arrayContent = "";
            
            if (fieldPath.Contains("."))
            {
                var fields = fieldPath.Split('.');
                var iteration = 1;
                var embeddedDocument = this;
                
                foreach (var field in fields)
                {
                    currentField = field;
                    arrayContent = "";
                    
                    if (field.Contains("["))
                    {
                        var firstIndex = field.IndexOf('[');
                        var lastIndex = field.IndexOf(']');
                        
                        arrayContent = field.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                        currentField = field.Substring(0, firstIndex);
                    }
                    
                    if (iteration == fields.Length)
                    {
                        if (embeddedDocument.ContainsKey(currentField))
                        {
                            return embeddedDocument[currentField].GetType();
                        }
                        
                        break;
                    }

                    if (embeddedDocument.ContainsKey(currentField))
                    {
                        embeddedDocument = (Docunet)GetFieldValue(currentField, arrayContent, embeddedDocument);
                    }
                    else
                    {
                        // if current field in path isn't present
                        break;
                    }

                    iteration++;
                }
            }
            else
            {
                currentField = fieldPath;
                
                if (fieldPath.Contains("["))
                {
                    var firstIndex = fieldPath.IndexOf('[');
                    var lastIndex = fieldPath.IndexOf(']');
                    
                    arrayContent = fieldPath.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                    currentField = fieldPath.Substring(0, firstIndex);
                }
                
                if (this.ContainsKey(currentField))
                {
                    return this[currentField].GetType();
                }
            }
            
            return null;
        }
        
        public Docunet Drop(string fieldPath)
        {
            var currentField = "";
            var arrayContent = "";
            
            if (fieldPath.Contains("."))
            {
                var fields = fieldPath.Split('.');
                var iteration = 1;
                var embeddedDocument = this;
                
                foreach (var field in fields)
                {
                    currentField = field;
                    arrayContent = "";
                    
                    if (field.Contains("["))
                    {
                        var firstIndex = field.IndexOf('[');
                        var lastIndex = field.IndexOf(']');
                        
                        arrayContent = field.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                        currentField = field.Substring(0, firstIndex);
                    }
                    
                    if (iteration == fields.Length)
                    {
                        if (embeddedDocument.ContainsKey(currentField))
                        {
                            embeddedDocument.Remove(currentField);
                        }
                        
                        break;
                    }

                    if (embeddedDocument.ContainsKey(currentField))
                    {
                        embeddedDocument = (Docunet)GetFieldValue(currentField, arrayContent, embeddedDocument);
                    }
                    else
                    {
                        // if current field in path isn't present
                        break;
                    }

                    iteration++;
                }
            }
            else
            {
                currentField = fieldPath;
                
                if (fieldPath.Contains("["))
                {
                    var firstIndex = fieldPath.IndexOf('[');
                    var lastIndex = fieldPath.IndexOf(']');
                    
                    arrayContent = fieldPath.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                    currentField = fieldPath.Substring(0, firstIndex);
                }
                
                if (this.ContainsKey(currentField))
                {
                    this.Remove(currentField);
                }
            }
            
            return this;
        }
        
        #region Clone
        
        public Docunet Clone()
        {
            return Clone(this);
        }
        
        private Docunet Clone(Docunet document)
        {
            var clonedDocument = new Docunet();
            
            foreach (KeyValuePair<string, object> field in document)
            {
                if (field.Value is Docunet)
                {
                    clonedDocument.Add(field.Key, Clone((Docunet)field.Value));
                }
                else
                {
                    clonedDocument.Add(field.Key, field.Value);
                }
            }
            
            return clonedDocument;
        }
        
        #endregion
        
        public Docunet Except(params string[] fields)
        {
            var document = Clone();
            
            foreach (string field in fields)
            {
                document.Drop(field);
            }
            
            return document;
        }
        
        public Docunet Only(params string[] fields)
        {
            var document = new Docunet();
            
            foreach (string field in fields)
            {
                document.SetField(field, GetField(field));
            }
            
            return document;
        }
        
        #region Equals
        
        public bool Equals(Docunet document)
        {
            return CompareDocuments(document, this);
        }
        
        private bool CompareDocuments(Docunet document1, Docunet document2)
        {
            var iterations = 0;
            
            foreach (KeyValuePair<string, object> field in document1)
            {
                if (document2.Has(field.Key))
                {
                    var areEqual = false;
                    var obj = document2.GetField(field.Key);
                    
                    if ((field.Value is Docunet) && (obj is Docunet))
                    {
                        areEqual = CompareDocuments((Docunet)field.Value, (Docunet)obj);
                    }
                    else if ((field.Value is IList) && (obj is IList))
                    {
                        areEqual = CompareCollections((IList)field.Value, (IList)obj);
                    }
                    else
                    {
                        areEqual = CompareValues(field.Value, obj);
                    }
                    
                    if (!areEqual)
                    {
                        return false;
                    }
                    
                    iterations++;
                }
                else
                {
                    return false;
                }
            }
            
            if (iterations != document2.Count)
            {
                return false;
            }
            
            return true;
        }
        
        private bool CompareCollections(IList collection1, IList collection2)
        {
            if (collection1.Count != collection2.Count)
            {
                return false;
            }

            for (var i = 0; i < collection1.Count; i++)
            {
                var item = collection1[i];
                var areEqual = false;
                
                if ((item is Docunet) && (collection2[i] is Docunet))
                {
                    areEqual = CompareDocuments((Docunet)item, (Docunet)collection2[i]);
                }
                else
                {
                    areEqual = CompareValues(item, collection2[i]);
                }
                
                if (!areEqual)
                {
                    return false;
                }
            }
            
            return true;
        }
        
        private bool CompareValues(object value1, object value2)
        {
            var areEqual = false;
            
            if ((value1 != null) && (value2 != null))
            {
                areEqual = value1.Equals(value2);
            }
            else if ((value1 == null) && (value2 == null))
            {
                areEqual = true;
            }
            
            return areEqual;
        }
        
        #endregion
        
        #region Convert to generic object
        
        /// <summary>
        /// Converts and copies document fields to specified generic object.
        /// </summary>
        public T ToObject<T>() where T : class, new()
        {
            T genericObject = new T();

            genericObject = (T)ToObject<T>(genericObject, this);

            return genericObject;
        }
        
        private T ToObject<T>(T genericObject, Docunet document) where T : class, new()
        {
            var genericObjectType = genericObject.GetType();

            if (genericObject is Docunet)
            {
                // if generic object is arango specific class - use set field to copy data
                foreach (KeyValuePair<string, object> item in document)
                {
                    (genericObject as Docunet).SetField(item.Key, item.Value);
                }
            }
            else
            {
                foreach (PropertyInfo propertyInfo in genericObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    object fieldValue = null;
                    Type fieldType = null;
                    
                    if (document.Has(propertyInfo.Name))
                    {
                        fieldValue = document.GetField(propertyInfo.Name);
                        
                        if (fieldValue != null)
                        {
                            fieldType = fieldValue.GetType();
                        }
                    }
                    else
                    {
                        continue;
                    }
                    
                    // property is a collection
                    if ((propertyInfo.PropertyType.IsArray || 
                         propertyInfo.PropertyType.IsGenericType))
                    {
                        var instance = Activator.CreateInstance(propertyInfo.PropertyType);
                            
                        propertyInfo.SetValue(
                            genericObject,
                            ConvertToCollection(instance, (IList)fieldValue, propertyInfo.PropertyType),
                            null
                        );
                    }
                    // property is class except the string type since string values are parsed differently
                    else if (propertyInfo.PropertyType.IsClass && (propertyInfo.PropertyType.Name != "String"))
                    {
                        // create object instance of embedded class
                        var instance = Activator.CreateInstance(propertyInfo.PropertyType);

                        if (fieldType == typeof(Docunet))
                        {
                            propertyInfo.SetValue(genericObject, ToObject(instance, (Docunet)fieldValue), null);
                        }
                        else
                        {
                            propertyInfo.SetValue(genericObject, fieldValue, null);
                        }
                    }
                    // property is basic type
                    else
                    {
                        if ((fieldValue == null) || (propertyInfo.PropertyType == fieldType))
                        {
                            propertyInfo.SetValue(genericObject, fieldValue, null);
                        } 
                        else
                        {
                            propertyInfo.SetValue(genericObject, Convert.ChangeType(fieldValue, propertyInfo.PropertyType), null);
                        }
                    }
                }
            }

            return genericObject;
        }
        
        private object ConvertToCollection(object collectionObject, IList collection, Type collectionType)
        {
            if (collection == null)
            {
                return null;
            }
            
            //List<object> convertedCollection = new List<object>();
            
            if (collection.Count > 0)
            {
                // create instance of property type
                var collectionInstance = Activator.CreateInstance(collectionType, collection.Count);

                for (int i = 0; i < collection.Count; i++)
                {
                    var elementType = collection[i].GetType();
                    
                    // collection is simple array
                    if (collectionType.IsArray)
                    {
                        ((IList)collectionObject).Add(collection[i]);
                    }
                    // collection is generic
                    else if (collectionType.IsGenericType && (collection is IEnumerable))
                    {
                        // generic collection consists of basic types
                        if (elementType.IsPrimitive ||
                            (elementType == typeof(string)) ||
                            (elementType == typeof(DateTime)) ||
                            (elementType == typeof(decimal)))
                        {
                            ((IList)collectionObject).Add(collection[i]);
                        }
                        // generic collection consists of generic type which should be parsed
                        else
                        {
                            // create instance object based on first element of generic collection
                            var instance = Activator.CreateInstance(collectionType.GetGenericArguments().First(), null);
                            
                            if (elementType == typeof(Docunet))
                            {
                                ((IList)collectionObject).Add(ToObject(instance, (Docunet)collection[i]));
                            }
                            else
                            {
                                if (elementType == instance.GetType())
                                {
                                    ((IList)collectionObject).Add(collection[i]);
                                } 
                                else
                                {
                                    ((IList)collectionObject).Add(Convert.ChangeType(collection[i], collectionType));
                                }
                            }
                        }
                    }
                    else
                    {
                        var obj = Activator.CreateInstance(elementType, collection[i]);

                        ((IList)collectionObject).Add(obj);
                    }
                }
            }
            
            return collectionObject;
        }
        
        #endregion
        
        public static Docunet ToDocument<T>(T inputObject)
        {
            if (inputObject is Docunet)
            {
                return inputObject as Docunet;
            }
            else if (inputObject is Dictionary<string, object>)
            {
                var document = new Docunet();
                
                foreach (KeyValuePair<string, object> field in inputObject as Dictionary<string, object>)
                {
                    document.Object(field.Key, field.Value);
                }
                
                return document;
            }
            else
            {
                var inputObjectType = inputObject.GetType();
                var document = new Docunet();
                
                foreach (var propertyInfo in inputObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var propertyValue = propertyInfo.GetValue(inputObject);
                    
                    if (propertyValue == null)
                    {
                        document.SetField(propertyInfo.Name, null);
                    }
                    // property is array or collection
                    else if (propertyInfo.PropertyType.IsArray || propertyInfo.PropertyType.IsGenericType)
                    {
                        document.SetField(propertyInfo.Name, ToList(propertyValue));
                    }
                    // property is class except the string type since string values are parsed differently
                    else if (propertyInfo.PropertyType.IsClass && (propertyInfo.PropertyType.Name != "String"))
                    {
                        document.SetField(propertyInfo.Name, ToDocument(propertyValue));
                    }
                    // property is basic type
                    else
                    {
                        document.SetField(propertyInfo.Name, propertyValue);
                    }
                }
                
                return document;
            }
        }
        
        public static List<object> ToList<T>(T inputCollection)
        {
            var collectionType = inputCollection.GetType();
            var documents = new List<object>();
            
            if (collectionType.IsArray || collectionType.IsGenericType)
            {
                var collection = (IList)inputCollection;
                
                if (collection.Count > 0)
                {
                    // create instance of property type
                    var collectionInstance = Activator.CreateInstance(collectionType, collection.Count);
    
                    for (int i = 0; i < collection.Count; i++)
                    {
                        var elementType = collection[i].GetType();
                        
                        // collection is simple array
                        if (collectionType.IsArray)
                        {
                            documents.Add(collection[i]);
                        }
                        // collection is generic
                        else if (collectionType.IsGenericType && (collection is IEnumerable))
                        {
                            // generic collection consists of basic types
                            if (elementType.IsPrimitive ||
                                (elementType == typeof(string)) ||
                                (elementType == typeof(DateTime)) ||
                                (elementType == typeof(decimal)))
                            {
                                documents.Add(collection[i]);
                            }
                            // generic collection consists of generic type which should be parsed
                            else
                            {
                                // create instance object based on first element of generic collection
                                var instance = Activator.CreateInstance(collectionType.GetGenericArguments().First(), null);
                                
                                documents.Add(ToDocument(collection[i]));
                            }
                        }
                        else
                        {
                            var obj = Activator.CreateInstance(elementType, collection[i]);
    
                            documents.Add(obj);
                        }
                    }
                }
            }
            
            return documents;
        }
        
        #region Serialization
        
        public string Serialize()
        {
            return Serialize(this);
        }
        
        /// <summary>
        /// Serializes specified object to JSON string.
        /// </summary>
        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        
        #endregion
        
        #region Deserialization

        public void Parse(string json)
        {
            foreach(KeyValuePair<string, object> field in DeserializeDocument(json))
            {
                this.Add(field.Key, field.Value);
            }
        }
        
        public static List<T> DeserializeArray<T>(string json)
        {
            var collection = new List<T>();
            var type = typeof(T);
            var data = DeserializeArray(JArray.Parse(json));
            
            if (data is List<T>)
            {
                collection = ((IEnumerable)data).Cast<T>().ToList();
            }
            else
            {
                switch (type.Name)
                {
                    case "Boolean":
                        collection = data.Select(Convert.ToBoolean).ToList() as List<T>;
                        break;
                    case "Byte":
                        collection = data.Select(Convert.ToByte).ToList() as List<T>;
                        break;
                    case "Int16":
                        collection = data.Select(Convert.ToInt16).ToList() as List<T>;
                        break;
                    case "Int32":
                        collection = data.Select(Convert.ToInt32).ToList() as List<T>;
                        break;
                    case "Int64":
                        collection = data.Select(Convert.ToInt64).ToList() as List<T>;
                        break;
                    case "Single":
                        collection = data.Select(Convert.ToSingle).ToList() as List<T>;
                        break;
                    case "Double":
                        collection = data.Select(Convert.ToDouble).ToList() as List<T>;
                        break;
                    case "Decimal":
                        collection = data.Select(Convert.ToDecimal).ToList() as List<T>;
                        break;
                    case "DateTime":
                        collection = data.Select(Convert.ToDateTime).ToList() as List<T>;
                        break;
                    case "String":
                        collection = data.Select(Convert.ToString).ToList() as List<T>;
                        break;
                    default:
                        collection = ((IEnumerable)data).Cast<T>().ToList();
                        break;
                }
            }
            
            return collection;
        }
        
        /// <summary>
        /// Deserializes specified json string to document object.
        /// </summary>
        public static Docunet DeserializeDocument(string json)
        {
            var document = new Docunet();
            var fields = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(json);
            
            foreach (KeyValuePair<string, JToken> field in fields)
            {
                switch (field.Value.Type)
                {
                    case JTokenType.Array:
                        document.Add(field.Key, DeserializeArray((JArray)field.Value));
                        break;
                    case JTokenType.Object:
                        document.Add(field.Key, DeserializeEmbeddedObject((JObject)field.Value));
                        break;
                    default:
                        document.Add(field.Key, DeserializeValue(field.Value));
                        break;
                }
            }
            
            return document;
        }

        private static object DeserializeEmbeddedObject(JObject jObject)
        {
            var embedded = new Docunet();

            foreach (KeyValuePair<string, JToken> field in jObject)
            {
                switch (field.Value.Type)
                {
                    case JTokenType.Array:
                        embedded.Add(field.Key, DeserializeArray((JArray)field.Value));
                        break;
                    case JTokenType.Object:
                        embedded.Add(field.Key, DeserializeEmbeddedObject((JObject)field.Value));
                        break;
                    default:
                        embedded.Add(field.Key, DeserializeValue(field.Value));
                        break;
                }
            }

            return embedded;
        }

        private static List<object> DeserializeArray(JArray jArray)
        {
            var array = new List<object>();
            
            foreach (JToken item in jArray)
            {
                switch (item.Type)
                {
                    case JTokenType.Array:
                        array.Add(DeserializeArray((JArray)item));
                        break;
                    case JTokenType.Object:
                        array.Add(DeserializeEmbeddedObject((JObject)item));
                        break;
                    default:
                        array.Add(DeserializeValue(item));
                        break;
                }
            }
            
            return array;
        }
        
        private static object DeserializeValue(JToken token)
        {
            return token.ToObject<object>();
        }
        
        #endregion
    }
}