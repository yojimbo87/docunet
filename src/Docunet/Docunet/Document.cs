﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Docunet
{
    /// <summary> 
    /// Document JSON-like structure represented as dictionary of strings and objects.
    /// </summary>
    public class Document : Dictionary<string, object>
    {
        /// <summary> 
        /// Global settings object for documents.
        /// </summary>
        public static DocumentSettings Settings = new DocumentSettings();
        
        /// <summary> 
        /// Creates new document.
        /// </summary>
        public Document() {  }
        
        /// <summary> 
        /// Creates new document by deserializing json string.
        /// </summary>
        /// <param name="json">JSON string to be deserialized to document object instance.</param>
        public Document(string json)
        {
            foreach(KeyValuePair<string, object> field in DeserializeDocument(json))
            {
                this.Add(field.Key, field.Value);
            }
        }
        
        #region Field getters
        
        /// <summary> 
        /// Gets boolean type value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public bool Bool(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return System.Convert.ToBoolean(fieldValue);
            }
        }
        
        /// <summary> 
        /// Gets byte type value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public byte Byte(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return System.Convert.ToByte(fieldValue);
            }
        }
        
        /// <summary> 
        /// Gets short (Int16) type value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public short Short(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return System.Convert.ToInt16(fieldValue);
            }
        }
        
        /// <summary> 
        /// Gets integer (Int32) type value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public int Int(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return System.Convert.ToInt32(fieldValue);
            }
        }
        
        /// <summary> 
        /// Gets long (Int64) type value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public long Long(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return System.Convert.ToInt64(fieldValue);
            }
        }
        
        /// <summary> 
        /// Gets float type value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public float Float(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return System.Convert.ToSingle(fieldValue);
            }
        }
        
        /// <summary> 
        /// Gets double type value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public double Double(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return System.Convert.ToDouble(fieldValue);
            }
        }
        
        /// <summary> 
        /// Gets decimal type value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public decimal Decimal(string fieldPath)
        {
            var fieldValue = GetField(fieldPath);
            
            if (fieldValue == null)
            {
                throw new Exception("Value is null.");
            }
            else
            {
                return System.Convert.ToDecimal(fieldValue);
            }
        }
        
        /// <summary> 
        /// Gets string type value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public string String(string fieldPath)
        {
            return (string)GetField(fieldPath);
        }
        
        /// <summary> 
        /// Gets DateTime object value of specific field. If value is in string DateTime or unix timestamp format it's converted to DateTime object.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public DateTime DateTime(string fieldPath)
        {
            var value = GetField(fieldPath);
            
            if (value is string)
            {
                return System.DateTime.Parse((string)value, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal);
            }
            else if (value is long)
            {
                return DocumentSettings.UnixEpoch.AddSeconds((long)value);
            }
            else
            {
                return (DateTime)value;
            }
        }
        
        /// <summary> 
        /// Gets object type value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public object Object(string fieldPath)
        {
            return (object)GetField(fieldPath);
        }
        
        /// <summary> 
        /// Gets generic object value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public T Object<T>(string fieldPath)
        {
            return (T)GetField(fieldPath);
        }
        
        /// <summary> 
        /// Gets document object value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public Document Docunet(string fieldPath)
        {
            return (Document)GetField(fieldPath);
        }
        
        /// <summary> 
        /// Gets enum value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public T Enum<T>(string fieldPath)
        {
            var type = typeof(T);
            
            return (T)System.Enum.ToObject(type, GetField(fieldPath));
        }
        
        /// <summary> 
        /// Gets generic list object value of specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
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
                        collection = ((List<object>)data).Select(System.Convert.ToBoolean).ToList() as List<T>;
                        break;
                    case "Byte":
                        collection = ((List<object>)data).Select(System.Convert.ToByte).ToList() as List<T>;
                        break;
                    case "Int16":
                        collection = ((List<object>)data).Select(System.Convert.ToInt16).ToList() as List<T>;
                        break;
                    case "Int32":
                        collection = ((List<object>)data).Select(System.Convert.ToInt32).ToList() as List<T>;
                        break;
                    case "Int64":
                        collection = ((List<object>)data).Select(System.Convert.ToInt64).ToList() as List<T>;
                        break;
                    case "Single":
                        collection = ((List<object>)data).Select(System.Convert.ToSingle).ToList() as List<T>;
                        break;
                    case "Double":
                        collection = ((List<object>)data).Select(System.Convert.ToDouble).ToList() as List<T>;
                        break;
                    case "Decimal":
                        collection = ((List<object>)data).Select(System.Convert.ToDecimal).ToList() as List<T>;
                        break;
                    case "DateTime":
                        collection = ((List<object>)data).Select(System.Convert.ToDateTime).ToList() as List<T>;
                        break;
                    case "String":
                        collection = ((List<object>)data).Select(System.Convert.ToString).ToList() as List<T>;
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
                        embeddedDocument = (Document)GetFieldValue(currentField, arrayContent, embeddedDocument);
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
        
        private object GetFieldValue(string fieldName, string arrayContent, Document fieldObject)
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
        
        /// <summary> 
        /// Sets boolean type value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document Bool(string fieldPath, bool value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        /// <summary> 
        /// Sets byte type value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document Byte(string fieldPath, byte value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        /// <summary> 
        /// Sets short (Int16) type value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document Short(string fieldPath, short value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        /// <summary> 
        /// Sets integer (Int32) type value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document Int(string fieldPath, int value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        /// <summary> 
        /// Sets long (Int64) type value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document Long(string fieldPath, long value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        /// <summary> 
        /// Sets float type value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document Float(string fieldPath, float value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        /// <summary> 
        /// Sets double type value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document Double(string fieldPath, double value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        /// <summary> 
        /// Sets decimal type value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document Decimal(string fieldPath, decimal value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        /// <summary> 
        /// Sets string type value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document String(string fieldPath, string value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        /// <summary> 
        /// Sets DateTime object value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document DateTime(string fieldPath, DateTime value)
        {
            return DateTime(fieldPath, value, Settings.DateTimeFormat);
        }
        
        /// <summary> 
        /// Sets DateTime object value to specific field with given format.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        /// <param name="format">Format in which will be DateTime object stored.</param>
        public Document DateTime(string fieldPath, DateTime value, DateTimeFormat format)
        {
            switch (format)
            {
                case DateTimeFormat.Iso8601String:
                    SetField(fieldPath, value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", DateTimeFormatInfo.InvariantInfo));
                    break;
                case DateTimeFormat.UnixTimeStamp:
                    TimeSpan span = (value.ToUniversalTime() - DocumentSettings.UnixEpoch);
                    SetField(fieldPath, (long)span.TotalSeconds);
                    break;
                case DateTimeFormat.DateTimeObject:
                default:
                    SetField(fieldPath, value);
                    break;
            }

            return this;
        }

        /// <summary> 
        /// Sets object type value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document Object(string fieldPath, object value)
        {
            SetField(fieldPath, value);

            return this;
        }
        
        /// <summary> 
        /// Sets generic object value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document Object<T>(string fieldPath, T value)
        {
            SetField(fieldPath, value);
            
            return this;
        }
        
        /// <summary> 
        /// Sets document object value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document Docunet<T>(string fieldPath, T value)
        {
            if (value is Document)
            {
                SetField(fieldPath, value);
            }
            else
            {
                SetField(fieldPath, ToDocument<T>(value));
            }
            
            return this;
        }
        
        /// <summary> 
        /// Sets enum value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document Enum<T>(string fieldPath, T value)
        {
            SetField(fieldPath, value);
            
            return this;
        }

        /// <summary> 
        /// Sets generic list object value to specific field.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="value">Value to be saved in specified field.</param>
        public Document List<T>(string fieldPath, List<T> value)
        {
            SetField(fieldPath, value);

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
                        embeddedDocument = (Document)embeddedDocument[field];
                    }
                    else
                    {
                        // if document which contains the field doesn't exist create it first
                        var tempDocument = new Document();
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
        
        #region Field checkers
        
        /// <summary> 
        /// Checks for existence of specific field in document. True is returned also if field has null value.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
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
                        embeddedDocument = (Document)GetFieldValue(currentField, arrayContent, embeddedDocument);
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
        
        /// <summary> 
        /// Checks if value of specific field in document is null. If field doesn't exist true is returned.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
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
                                else
                                {
                                    return true;
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
                        else
                        {
                            return true;
                        }
                        
                        break;
                    }

                    if (embeddedDocument.ContainsKey(currentField))
                    {
                        embeddedDocument = (Document)GetFieldValue(currentField, arrayContent, embeddedDocument);
                    }
                    else
                    {
                        // if current field in path isn't present
                        return true;
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
                        else
                        {
                            return true;
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
                else
                {
                    return true;
                }
            }
            
            return false;
        }
        
        /// <summary> 
        /// Gets type of specific field in document.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
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
                        embeddedDocument = (Document)GetFieldValue(currentField, arrayContent, embeddedDocument);
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
        
        #endregion
        
        #region Field type conversion
        
        /// <summary> 
        /// Converts specific field value to given type.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        /// <param name="type">Type to which will be field value converted.</param>
        public Document Convert(string fieldPath, Type type)
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
                            embeddedDocument[currentField] = ConvertField(embeddedDocument[currentField], type);
                        }
                        
                        break;
                    }

                    if (embeddedDocument.ContainsKey(currentField))
                    {
                        embeddedDocument = (Document)GetFieldValue(currentField, arrayContent, embeddedDocument);
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
                    this[currentField] = ConvertField(this[currentField], type);
                }
            }
            
            return this;
        }
        
        private object ConvertField(object value, Type type)
        {
            var valueType = value.GetType();
            
            if ((valueType.Name == "DateTime") && (type.Name == "String"))
            {
                value = ((DateTime)value).ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", DateTimeFormatInfo.InvariantInfo);
            }
            else if ((valueType.Name == "DateTime") && (type.Name == "Int64"))
            {
                TimeSpan span = (((DateTime)value).ToUniversalTime() - DocumentSettings.UnixEpoch);
                value = (long)span.TotalSeconds;
            }
            else if ((valueType.Name == "String") && (type.Name == "DateTime"))
            {
                value = System.DateTime.Parse((string)value, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal);
            }
            else if ((valueType.Name == "Int64") && (type.Name == "DateTime"))
            {
                value = DocumentSettings.UnixEpoch.AddSeconds((long)value);
            }
            else
            {
                value = System.Convert.ChangeType(value, type);
            }
            
            return value;
        }
        
        #endregion
        
        /// <summary> 
        /// Removes specified field from document.
        /// </summary>
        /// <param name="fieldPath">Path to the field in document.</param>
        public Document Drop(string fieldPath)
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
                        embeddedDocument = (Document)GetFieldValue(currentField, arrayContent, embeddedDocument);
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
        
        /// <summary> 
        /// Clones current document and returns it's deep copy.
        /// </summary>
        public Document Clone()
        {
            return Clone(this);
        }
        
        private Document Clone(Document document)
        {
            var clonedDocument = new Document();
            
            foreach (KeyValuePair<string, object> field in document)
            {
                if (field.Value is Document)
                {
                    clonedDocument.Add(field.Key, Clone((Document)field.Value));
                }
                else
                {
                    clonedDocument.Add(field.Key, field.Value);
                }
            }
            
            return clonedDocument;
        }
        
        #endregion
        
        #region Merge
        
        /// <summary> 
        /// Merges current document with specified document object based on given merge options.
        /// </summary>
        /// <param name="document">Document object which will be merged.</param>
        /// <param name="mergeOptions">Options determining how two documents will be merged.</param>
        public void Merge(Document document, MergeOptions mergeOptions = MergeOptions.MergeFields)
        {
            var mergedDocument = Merge(this, document, mergeOptions);
            
            this.Clear();
            
            foreach (KeyValuePair<string, object> field in mergedDocument)
            {
                this.Add(field.Key, field.Value);
            }
        }
        
        /// <summary> 
        /// Merges two documents based on given merge options.
        /// </summary>
        /// <param name="document1">First document to be merged.</param>
        /// <param name="document2">Second document to be merged.</param>
        /// <param name="mergeOptions">Options determining how two documents will be merged.</param>
        public static Document Merge(Document document1, Document document2, MergeOptions mergeOptions = MergeOptions.MergeFields)
        {
            // clone first document to prevent its poisoning/injection of fields from second document
            var clonedDocument1 = document1.Clone();
            
            foreach (KeyValuePair<string, object> field in document2)
            {
                if (clonedDocument1.ContainsKey(field.Key))
                {
                    var field1Value = clonedDocument1[field.Key];
                    var field2Value = field.Value;
                    
                    if ((field1Value is Document) && (field2Value is Document))
                    {
                        if (mergeOptions == MergeOptions.MergeFields)
                        {
                            clonedDocument1.Remove(field.Key);
                            clonedDocument1.Add(field.Key, Merge((Document)field1Value, (Document)field2Value, mergeOptions));
                        }
                        else if (mergeOptions == MergeOptions.ReplaceFields)
                        {
                            clonedDocument1.Remove(field.Key);
                            clonedDocument1.Add(field.Key, (Document)field2Value);
                        }
                        else if (mergeOptions == MergeOptions.KeepFields)
                        {
                            // do nothing - keep it as it is
                        }
                    }
                    else if ((field1Value is IList) && (field2Value is IList) && (mergeOptions == MergeOptions.MergeFields))
                    {
                        var collection1 = (IList)field1Value;
                        var collection2 = (IList)field2Value;
                        
                        for (var i = 0; i < collection2.Count; i++)
                        {
                            var item = collection2[i];
                            
                            if (!collection1.Contains(item))
                            {
                                collection1.Add(item);
                            }
                        }
                    }
                    else if (mergeOptions == MergeOptions.ReplaceFields)
                    {
                        clonedDocument1.Remove(field.Key);
                        clonedDocument1.Add(field.Key, field2Value);
                    }
                    else if (mergeOptions == MergeOptions.KeepFields)
                    {
                        // do nothing - keep it as it is
                    }
                }
                else
                {
                    clonedDocument1.Add(field.Key, field.Value);
                }
            }
            
            return clonedDocument1;
        }
        
        #endregion
        
        /// <summary> 
        /// Replaces current document with specified document object.
        /// </summary>
        /// <param name="document">Document object which will replace current document.</param>
        public void Replace(Document document)
        {
            this.Clear();
            
            foreach(KeyValuePair<string, object> field in document)
            {
                this.Add(field.Key, field.Value);
            }
        }
        
        /// <summary> 
        /// Clones current document and returns new document except specified fields.
        /// </summary>
        /// <param name="fields">Fields which won't be present in new document.</param>
        public Document Except(params string[] fields)
        {
            var document = Clone();
            
            foreach (string field in fields)
            {
                document.Drop(field);
            }
            
            return document;
        }
        
        /// <summary> 
        /// Clones current document and returns new document only with specified fields.
        /// </summary>
        /// <param name="fields">Fields which will be present in new document.</param>
        public Document Only(params string[] fields)
        {
            var document = new Document();
            
            foreach (string field in fields)
            {
                document.SetField(field, GetField(field));
            }
            
            return document;
        }
        
        #region Equals
        
        /// <summary> 
        /// Compares current document with specified document to determine their equality.
        /// </summary>
        /// <param name="document">Document which will be compared for equality.</param>
        public bool Equals(Document document)
        {
            return CompareDocuments(document, this);
        }
        
        private bool CompareDocuments(Document document1, Document document2)
        {
            var iterations = 0;
            
            foreach (KeyValuePair<string, object> field in document1)
            {
                if (document2.Has(field.Key))
                {
                    var areEqual = false;
                    var obj = document2.GetField(field.Key);
                    
                    if ((field.Value is Document) && (obj is Document))
                    {
                        areEqual = CompareDocuments((Document)field.Value, (Document)obj);
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
                
                if ((item is Document) && (collection2[i] is Document))
                {
                    areEqual = CompareDocuments((Document)item, (Document)collection2[i]);
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
        /// Converts current document to generic object of specified type.
        /// </summary>
        public T ToObject<T>() where T : class, new()
        {
            T genericObject = new T();

            genericObject = (T)ToObject<T>(genericObject, this);

            return genericObject;
        }
        
        /// <summary>
        /// Converts current document to specified generic object.
        /// </summary>
        /// <param name="genericObject">Generic object to which will be current document converted.</param>
        public void ToObject<T>(T genericObject) where T : class, new()
        {
            genericObject = (T)ToObject<T>(genericObject, this);
        }
        
        private T ToObject<T>(T genericObject, Document document) where T : class, new()
        {
            var genericObjectType = genericObject.GetType();

            if (genericObject is Document)
            {
                // if generic object is arango specific class - use set field to copy data
                foreach (KeyValuePair<string, object> item in document)
                {
                    (genericObject as Document).SetField(item.Key, item.Value);
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

                        if (fieldType == typeof(Document))
                        {
                            propertyInfo.SetValue(genericObject, ToObject(instance, (Document)fieldValue), null);
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
                            propertyInfo.SetValue(genericObject, System.Convert.ChangeType(fieldValue, propertyInfo.PropertyType), null);
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
                            
                            if (elementType == typeof(Document))
                            {
                                ((IList)collectionObject).Add(ToObject(instance, (Document)collection[i]));
                            }
                            else
                            {
                                if (elementType == instance.GetType())
                                {
                                    ((IList)collectionObject).Add(collection[i]);
                                } 
                                else
                                {
                                    ((IList)collectionObject).Add(System.Convert.ChangeType(collection[i], collectionType));
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
        
        /// <summary> 
        /// Converts generic object to it's document representation.
        /// </summary>
        /// <param name="inputObject">Generic object to be converted into document.</param>
        public static Document ToDocument<T>(T inputObject)
        {
            if (inputObject is Document)
            {
                return inputObject as Document;
            }
            else if (inputObject is Dictionary<string, object>)
            {
                var document = new Document();
                
                foreach (KeyValuePair<string, object> field in inputObject as Dictionary<string, object>)
                {
                    document.Object(field.Key, field.Value);
                }
                
                return document;
            }
            else
            {
                var inputObjectType = inputObject.GetType();
                var document = new Document();
                
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
        
        /// <summary> 
        /// Converts generic collection to list of objects which are suitable for storing in document field.
        /// </summary>
        /// <param name="inputCollection">Generic collection to be converted into list of objects.</param>
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
        
        /// <summary> 
        /// Serializes current document to it's JSON string representation.
        /// </summary>
        public string Serialize()
        {
            return Serialize(this);
        }
        
        /// <summary> 
        /// Serializes specified generic object to it's JSON string representation.
        /// </summary>
        /// <param name="value">Generic object to be serialized into JSON string.</param>
        public static string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }
        
        #endregion
        
        #region Deserialization

        /// <summary> 
        /// Deserializes specified JSON string into current document.
        /// </summary>
        /// <param name="json">JSON string to be deserialized.</param>
        public void Parse(string json)
        {
            foreach(KeyValuePair<string, object> field in DeserializeDocument(json))
            {
                this.Add(field.Key, field.Value);
            }
        }
        
        /// <summary> 
        /// Deserializes specified JSON string into list of generic objects.
        /// </summary>
        /// <param name="json">JSON string to be deserialized.</param>
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
                        collection = data.Select(System.Convert.ToBoolean).ToList() as List<T>;
                        break;
                    case "Byte":
                        collection = data.Select(System.Convert.ToByte).ToList() as List<T>;
                        break;
                    case "Int16":
                        collection = data.Select(System.Convert.ToInt16).ToList() as List<T>;
                        break;
                    case "Int32":
                        collection = data.Select(System.Convert.ToInt32).ToList() as List<T>;
                        break;
                    case "Int64":
                        collection = data.Select(System.Convert.ToInt64).ToList() as List<T>;
                        break;
                    case "Single":
                        collection = data.Select(System.Convert.ToSingle).ToList() as List<T>;
                        break;
                    case "Double":
                        collection = data.Select(System.Convert.ToDouble).ToList() as List<T>;
                        break;
                    case "Decimal":
                        collection = data.Select(System.Convert.ToDecimal).ToList() as List<T>;
                        break;
                    case "DateTime":
                        collection = data.Select(System.Convert.ToDateTime).ToList() as List<T>;
                        break;
                    case "String":
                        collection = data.Select(System.Convert.ToString).ToList() as List<T>;
                        break;
                    default:
                        collection = ((IEnumerable)data).Cast<T>().ToList();
                        break;
                }
            }
            
            return collection;
        }
        
        /// <summary> 
        /// Deserializes specified JSON string into document object.
        /// </summary>
        /// <param name="json">JSON string to be deserialized.</param>
        public static Document DeserializeDocument(string json)
        {
            var document = new Document();
            var fields = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(json, DocumentSettings.SerializerSettings);
            
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
            var embedded = new Document();

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