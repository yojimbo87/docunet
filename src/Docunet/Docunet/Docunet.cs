using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Docunet
{
    public class Docunet : Dictionary<string, object>
    {
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
                return (bool)fieldValue;
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
                return (byte)fieldValue;
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
                return (short)fieldValue;
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
                return (int)fieldValue;
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
                return (long)fieldValue;
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
                return (float)fieldValue;
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
                return (double)fieldValue;
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
                return (decimal)fieldValue;
            }
        }
        
        public string String(string fieldPath)
        {
            return (string)GetField(fieldPath);
        }
        
        public DateTime DateTime(string fieldPath)
        {
            return (DateTime)GetField(fieldPath);
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
            return ((IEnumerable)GetField(fieldPath)).Cast<T>().ToList();
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
            SetField(fieldPath, value);

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
        
        /*public bool Has(string fieldPath)
        {
            
        }*/
        
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
    }
}